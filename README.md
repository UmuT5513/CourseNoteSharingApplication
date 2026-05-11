# Course Note Sharing System

Course Note Sharing System, öğrencilerin ders notlarını yükleyip paylaşabildiği; yöneticilerin ise kullanıcı, rol, kurs ve içerik yönetimi yapabildiği bir ASP.NET Core MVC uygulamasıdır.

## Teknolojiler

- .NET 10 (`net10.0`)
- ASP.NET Core MVC + Razor Views
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- Bootstrap

## Temel Özellikler

### Kimlik Doğrulama ve Yetkilendirme
- Kayıt ol, giriş yap ve çıkış yap
- Rol bazlı yetkilendirme: `Admin`, `User`
- Hatalı girişlerde lockout desteği
- Şifre sıfırlama akışı

### Not ve Dosya Yönetimi
- Not yükleme, görüntüleme, düzenleme ve silme
- Desteklenen dosya türleri: `.pdf`, `.doc`, `.docx`, `.txt`, `.pptx`
- Maksimum dosya boyutu: 50 MB
- Not durumları: `Pending`, `Approved`, `Rejected`
- Notlara yorum ekleme
- İndirme sayısı ve indirme kayıtlarının tutulması

### Dashboard ve Profil Yönetimi
- Kullanıcı dashboard’u ile not, yorum ve indirme özetleri
- Profil bilgilerini görüntüleme ve güncelleme
- Admin dashboard üzerinden kullanıcı, rol, kurs ve not yönetimi

## Proje Yapısı

- `CourseNoteSharingSystem/Program.cs` – uygulama başlangıcı ve middleware yapılandırması
- `CourseNoteSharingSystem/Data/CourseNoteSharingSystemContext.cs` – EF Core veritabanı bağlamı
- `CourseNoteSharingSystem/Models/` – temel veri modelleri
- `CourseNoteSharingSystem/ViewModels/` – ekran ve form modelleri
- `CourseNoteSharingSystem/Controllers/` – iş mantığı ve endpoint’ler
- `CourseNoteSharingSystem/Views/` – Razor view dosyaları
- `CourseNoteSharingSystem/wwwroot/uploads/` – yüklenen dosyalar
- `CourseNoteSharingSystem/appsettings.json` – bağlantı dizesi ve uygulama ayarları

## Kurulum

1. `CourseNoteSharingSystem/appsettings.json` içindeki bağlantı dizesini düzenleyin:

```json
"ConnectionStrings": {
  "SqlCon": "your_sql_server"
}
```

2. Bağımlılıkları geri yükleyin:

```bash
dotnet restore
```

3. Veritabanını migration’larla oluşturun/güncelleyin:

```bash
dotnet ef database update --project CourseNoteSharingSystem/CourseNoteSharingSystem.csproj
```

4. Uygulamayı çalıştırın:

```bash
dotnet run --project CourseNoteSharingSystem/CourseNoteSharingSystem.csproj
```

## Önemli Yönlendirmeler

- Varsayılan rota: `Home/Index`
- Giriş sayfası: `Home/SignIn`
- Admin paneli: `AdminDashboard/Index`
- Kullanıcı paneli: `UserDashboard/Index`

## Notlar

- Yüklenen dosyalar `CourseNoteSharingSystem/wwwroot/uploads` altında tutulur.
- Kimlik doğrulama çerezi adı: `CNSSAuthCookie`
- Cookie oturum süresi: 5 dakika
