<img width="1280" height="568" alt="1" src="https://github.com/user-attachments/assets/635e7944-c1c1-4244-9f31-3e11493df3ad" />
<img width="1165" height="938" alt="4" src="https://github.com/user-attachments/assets/253da675-0de3-4f33-9a69-03031d3dae05" />
<img width="800" height="782" alt="3" src="https://github.com/user-attachments/assets/ec364a5d-fb7c-4875-ad37-6e2b72e2366b" />
<img width="782" height="800" alt="2" src="https://github.com/user-attachments/assets/d48cafae-9ab2-48a8-a84a-243d36cfe666" />


# Course Note Sharing System

Course Note Sharing System, öğrencilerin ders notlarını yükleyip paylaşabildiği; yöneticilerin ise kullanıcı, rol, kurs ve içerik yönetimi yapabildiği bir ASP.NET Core MVC (Razor Views) uygulamasıdır.

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
- Hatalı girişlerde lockout desteği (3 deneme)
- “Remember Me” ile kalıcı oturum
- Şifre sıfırlama akışı

### Not ve Dosya Yönetimi
- Not yükleme, görüntüleme, düzenleme ve silme
- Not arama, kurs filtresi ve sıralama (tarih/indirilen)
- Desteklenen dosya türleri: `.pdf`, `.doc`, `.docx`, `.txt`, `.pptx`
- Maksimum dosya boyutu: 50 MB
- Not durumları: `Pending`, `Approved`, `Rejected`
- Notlara yorum ekleme
- İndirme sayısı ve indirme kayıtlarının tutulması

### Dashboard ve Profil Yönetimi
- Kullanıcı dashboard’u ile not, yorum ve indirme özetleri
- Profil bilgilerini görüntüleme ve güncelleme
- Admin dashboard üzerinden kullanıcı, rol, kurs ve not yönetimi
- Onay bekleyen notların admin tarafından yönetimi

### Keşfet ve Popüler Notlar
- Onaylı notları listeleme (Explore)
- En çok indirilen notlar

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
- Keşfet sayfası: `Home/Explore`
- Admin paneli: `AdminDashboard/Index`
- Kullanıcı paneli: `UserDashboard/Index`

## Notlar

- Yüklenen dosyalar `CourseNoteSharingSystem/wwwroot/uploads` altında tutulur.
- Kimlik doğrulama çerezi adı: `CNSSAuthCookie`
- Cookie oturum süresi: 5 dakika
