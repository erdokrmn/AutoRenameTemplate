using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Linq;
using System.Diagnostics;


namespace AutoRenameTemplate
{
    /// <summary>
    /// MainWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class MainWindow : Window
    {
        private string selectedFolderPath = string.Empty;
        private string oldProjectName = string.Empty;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectFolderButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Bir proje klasörü seçin";
                // dialog.UseDescriptionForTitle = true; ← Bu satırı silebiliriz.

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    selectedFolderPath = dialog.SelectedPath;
                    oldProjectName = new DirectoryInfo(selectedFolderPath).Name;

                    SelectedFolderPathText.Text = selectedFolderPath;
                    OldProjectNameText.Text = oldProjectName;

                    Log($"Klasör seçildi: {selectedFolderPath}");
                    Log($"Eski proje ismi belirlendi: {oldProjectName}");
                }
            }
        }


        private void RenameButton_Click(object sender, RoutedEventArgs e)
        {
            string newProjectName = NewProjectNameBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(selectedFolderPath) || string.IsNullOrWhiteSpace(oldProjectName) || string.IsNullOrWhiteSpace(newProjectName))
            {
                Log("⚠️ Tüm alanlar dolu olmalıdır.");
                return;
            }

            try
            {
                Log("🔁 Yeniden adlandırma işlemi başlatıldı...");

                DeleteGitFolder();

                var finalDir = RenameInDirectory(new DirectoryInfo(selectedFolderPath), oldProjectName, newProjectName);
                selectedFolderPath = finalDir.FullName;

                CreateSolutionFreshly(newProjectName);
                CreateSolutionIfNotExists(newProjectName);

                Log("✅ Tüm işlemler başarıyla tamamlandı.");
            }
            catch (Exception ex)
            {
                Log($"❌ Hata: {ex.Message}");
            }
        }

        private void DeleteGitFolder()
        {
            string gitPath = Path.Combine(selectedFolderPath, ".git");
            if (Directory.Exists(gitPath))
            {
                Directory.Delete(gitPath, true);
                Log($".git klasörü silindi.");
            }
        }

        private DirectoryInfo RenameInDirectory(DirectoryInfo directory, string oldName, string newName)
        {
            // Alt klasörleri önce işle
            foreach (var subDir in directory.GetDirectories())
            {
                RenameInDirectory(subDir, oldName, newName);
            }

            // Dosyaları işle
            foreach (var file in directory.GetFiles())
            {
                string originalPath = file.FullName;
                string newFileName = file.Name.Replace(oldName, newName);
                string newFilePath = Path.Combine(file.DirectoryName, newFileName);

                if (newFileName != file.Name)
                {
                    file.MoveTo(newFilePath);
                    Log($"Dosya adı değişti: {file.Name} → {newFileName}");
                }

                ReplaceContent(new FileInfo(newFilePath), oldName, newName);
            }

            // En son klasör adını değiştir
            string newFolderName = directory.Name.Replace(oldName, newName);
            if (newFolderName != directory.Name)
            {
                string parentPath = directory.Parent?.FullName;
                if (parentPath != null)
                {
                    string newFullPath = Path.Combine(parentPath, newFolderName);
                    directory.MoveTo(newFullPath);
                    Log($"Klasör adı değişti: {directory.Name} → {newFolderName}");
                    return new DirectoryInfo(newFullPath);
                }
            }

            return directory;
        }



        private void ReplaceContent(FileInfo file, string oldName, string newName)
        {
            string[] textExtensions = new[] { ".cs", ".csproj", ".sln", ".xaml", ".json", ".md", ".txt" , ".cshtml" };
            if (!textExtensions.Contains(file.Extension.ToLower())) return;

            string content = File.ReadAllText(file.FullName);
            if (content.Contains(oldName))
            {
                content = content.Replace(oldName, newName);
                File.WriteAllText(file.FullName, content);
                Log($"Dosya içeriği değiştirildi: {file.Name}");
            }
        }


        private void CreateSolutionFreshly(string newProjectName)
        {
            // 1. Varsa mevcut sln dosyasını sil
            var slnFiles = Directory.GetFiles(selectedFolderPath, "*.sln", SearchOption.TopDirectoryOnly);
            foreach (var file in slnFiles)
            {
                File.Delete(file);
                Log($"Mevcut .sln dosyası silindi: {Path.GetFileName(file)}");
            }

            // 2. Yeni sln oluştur
            var startInfo = new ProcessStartInfo("dotnet", $"new sln -n {newProjectName}")
            {
                WorkingDirectory = selectedFolderPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = Process.Start(startInfo);
            process.WaitForExit();
            Log($".sln dosyası oluşturuldu: {newProjectName}.sln");

            // 3. Tüm .csproj dosyalarını ekle
            var csprojFiles = Directory.GetFiles(selectedFolderPath, "*.csproj", SearchOption.AllDirectories);
            foreach (var proj in csprojFiles)
            {
                string relativePath = GetRelativePath(selectedFolderPath, proj);
                var addInfo = new ProcessStartInfo("dotnet", $"sln add \"{relativePath}\"")
                {
                    WorkingDirectory = selectedFolderPath,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                var addProc = Process.Start(addInfo);
                addProc.WaitForExit();
                Log($".csproj eklendi: {relativePath}");
            }
        }




        private void CreateSolutionIfNotExists(string newProjectName)
        {
            string slnPath = Directory.GetFiles(selectedFolderPath, "*.sln", SearchOption.TopDirectoryOnly).FirstOrDefault();
            if (slnPath != null) return;

            var startInfo = new ProcessStartInfo("dotnet", $"new sln -n {newProjectName}")
            {
                WorkingDirectory = selectedFolderPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = Process.Start(startInfo);
            process.WaitForExit();
            Log($".sln dosyası oluşturuldu: {newProjectName}.sln");

            var csprojFiles = Directory.GetFiles(selectedFolderPath, "*.csproj", SearchOption.AllDirectories);
            foreach (var proj in csprojFiles)
            {
                string relativePath = GetRelativePath(selectedFolderPath, proj);
                var addInfo = new ProcessStartInfo("dotnet", $"sln add \"{relativePath}\"")
                {
                    WorkingDirectory = selectedFolderPath,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                var addProc = Process.Start(addInfo);
                addProc.WaitForExit();
                Log($".csproj eklendi: {relativePath}");
            }
        }

        private string GetRelativePath(string basePath, string fullPath)
        {
            Uri baseUri = new Uri(AppendDirectorySeparatorChar(basePath));
            Uri fullUri = new Uri(fullPath);
            return Uri.UnescapeDataString(baseUri.MakeRelativeUri(fullUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }

        private string AppendDirectorySeparatorChar(string path)
        {
            if (!path.EndsWith(Path.DirectorySeparatorChar.ToString()))
                return path + Path.DirectorySeparatorChar;
            return path;
        }

        private void Log(string message)
        {
            LogListBox.Items.Add($"[{DateTime.Now:HH:mm:ss}] {message}");
        }
    }
}
