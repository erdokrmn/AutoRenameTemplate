# AutoRenameTemplate

🔁 Bu uygulama, bir GitHub veya yerel C# projesinin klasörünü seçerek içindeki tüm isimleri (klasör, dosya ve içerik) yeni bir proje adıyla otomatik olarak yeniden adlandırmanızı sağlar. 

## ✨ Özellikler

- 📂 Klasör, dosya ve içerik düzeyinde eski proje adını yeni isimle değiştirir
- ⚙️ `.csproj`, `.cs`, `.xaml`, `.json`, `.md`, `.cshtml`, `.sln` vb. dosya türlerinde içerik değiştirir
- 🧹 `.git` klasörünü otomatik olarak kaldırır
- 🛠️ `.sln` dosyası varsa silinir ve yenisi oluşturulur
- 🧠 Otomatik olarak tüm `.csproj` dosyalarını yeni `.sln` dosyasına ekler
- ✅ Sade ve kullanıcı dostu WPF arayüz

## ⚠️ Dikkat

> **Seçilen klasörün ismi, proje ismi ile aynı olmalıdır.**  
> Uygulama bu klasör adını eski proje ismi olarak kullanır.  
> Örnek: `C:\Projeler\EskiProje` seçildiğinde `"EskiProje"` eski isim olarak kabul edilir.


## 🚀 Nasıl Kullanılır?

1. Uygulamayı çalıştır.
2. Ana sayfadaki "📂 Proje Klasörü Seç" butonuna tıkla.
3. **Proje klasörünün adının, eski proje ismi olduğundan emin ol.**
4. Yeni proje adını gir ve "🚀 Yeniden Adlandır" butonuna tıkla.
5. Uygulama:
   - `.git` klasörünü siler
   - klasör adlarını, dosya adlarını ve içeriklerini değiştirir
   - eski `.sln` varsa siler, yeni `.sln` oluşturur
   - `.csproj` dosyalarını `.sln` içine ekler
6. Tüm işlem birkaç saniyede tamamlanır 🎉

## 📝 Gereksinimler

- .NET Core SDK (5.0+ önerilir)
- `dotnet` CLI terminalde çalışabiliyor olmalı

## 📦 Derleme

Visual Studio'da açıp doğrudan çalıştırabilirsin.  
Ya da CLI üzerinden:

```bash
dotnet build
dotnet run
