# AutoRenameTemplate

🔁 Bu uygulama, bir GitHub veya yerel C# projesinin klasörünü seçerek içindeki tüm isimleri (klasör, dosya ve içerik) yeni bir proje adıyla otomatik olarak yeniden adlandırmanızı sağlar. 

## ✨ Özellikler

- 📂 Klasör, dosya ve içerik düzeyinde eski proje adını yeni isimle değiştirir
- ⚙️ `.csproj`, `.cs`, `.xaml`, `.json`, `.md`, `.cshtml`, `.sln` vb. dosya türlerinde içerik değiştirir
- 🧹 `.git` klasörünü otomatik olarak kaldırır
- 🛠️ `.sln` dosyası varsa silinir ve yenisi oluşturulur
- 🧠 Otomatik olarak tüm `.csproj` dosyalarını yeni `.sln` dosyasına ekler
- ✅ Sade ve kullanıcı dostu WPF arayüz

## 📸 Ekran Görüntüsü

> İsteğe bağlı: `Screenshots/auto-rename-example.png`

## 🚀 Nasıl Kullanılır?

1. Uygulamayı çalıştır.
2. Ana sayfadaki "📂 Proje Klasörü Seç" butonuna tıkla.
3. Yeniden adlandırmak istediğin projenin klasörünü seç.
4. Uygulama otomatik olarak eski proje adını klasör isminden algılar.
5. Yeni proje adını gir ve "🚀 Yeniden Adlandır" butonuna tıkla.
6. Hepsi bu kadar 🎉

## 📝 Gereksinimler

- .NET Core SDK (5.0+ önerilir)
- dotnet CLI terminalde erişilebilir olmalı

## 🛡️ Uyarılar

- Seçtiğin proje klasörünün yedeğini alman önerilir.
- `.sln` dosyası silinir ve baştan oluşturulur — önceki özel yapılandırmalar korunmaz.
- Tüm işlemler geri alınamaz.

## 📦 Derleme

Visual Studio'da açıp doğrudan çalıştırabilirsin.  
Ya da CLI üzerinden:

```bash
dotnet build
dotnet run
