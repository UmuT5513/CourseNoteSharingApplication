# Course Note Sharing System

Course Note Sharing System, öğrencilerin not yükleyip paylaşabildiği ve yöneticilerin içerik/kullanıcı yönetimi yapabildiği bir ASP.NET Core MVC uygulamasıdır.

## Teknolojiler

- .NET 10 (`net10.0`)
- ASP.NET Core MVC + Razor Views
- Entity Framework Core (SQL Server)
- ASP.NET Core Identity
- Bootstrap

## Özellikler

### Kimlik Doğrulama ve Yetkilendirme
- Kayıt ol / giriş yap / çıkış yap
- Rol bazlı yetkilendirme (`Admin`, `User`)
- Hatalı girişte lockout desteği
- Şifre sıfırlama akışı (token tabanlı)

### Not Yönetimi
- Not yükleme, görüntüleme, düzenleme, silme
- Desteklenen dosya türleri: `.pdf`, `.doc`, `.docx`, `.txt`, `.pptx`
- Maksimum dosya boyutu: 50MB
- Not onay süreci: `Pending`, `Approved`, `Rejected`
- Notlara yorum ekleme

### Takip ve Dashboard
- Not indirme ve indirme sayısı takibi
- İndirme log kayıtları
- Admin dashboard (kullanıcı, rol, kurs ve not yönetimi)
- User dashboard (kullanıcının not, yorum ve indirme özetleri)

## Proje Yapısı

- `CourseNoteSharingSystem/Program.cs` – uygulama başlangıcı ve middleware
- `CourseNoteSharingSystem/Data/CourseNoteSharingSystemContext.cs` – EF Core DbContext
- `CourseNoteSharingSystem/Models/` – varlık modelleri
- `CourseNoteSharingSystem/ViewModels/` – form ve ekran modelleri
- `CourseNoteSharingSystem/Controllers/` – iş akışı ve endpoint'ler
- `CourseNoteSharingSystem/Views/` – Razor view dosyaları
- `CourseNoteSharingSystem/appsettings.json` – bağlantı ve uygulama ayarları

## Kurulum ve Çalıştırma

1. `CourseNoteSharingSystem/appsettings.json` dosyasında bağlantı cümlesini düzenleyin:

```json
"ConnectionStrings": {
  "SqlCon": "your_sql_server"
}
```

2. Bağımlılıkları yükleyin:

```bash
dotnet restore
```

3. Veritabanını migration'larla güncelleyin:

```bash
dotnet ef database update --project CourseNoteSharingSystem/CourseNoteSharingSystem.csproj
```

4. Uygulamayı çalıştırın:

```bash
dotnet run --project CourseNoteSharingSystem/CourseNoteSharingSystem.csproj
```

## Varsayılan Rota ve Giriş Akışı

- Varsayılan rota: `Home/SignIn`
- Giriş sonrası yönlendirme:
  - `Admin` → `AdminDashboard/Index`
  - `User` → `UserDashboard/Index`

## Ek Bilgiler

- Yüklenen dosyalar `CourseNoteSharingSystem/wwwroot/uploads` altında tutulur.
- Kimlik doğrulama çerezi: `CNSSAuthCookie`
- Cookie oturum süresi: 5 dakika (Program.cs ayarına göre)
