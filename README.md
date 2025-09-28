# MyAcademyIdentity – Email App (ASP.NET Core .NET 9, Identity, EF Core)

Kullanıcı kimlik doğrulaması (ASP.NET Core Identity), mesajlaşma (gelen kutusu/gönderilenler/taslaklar), yıldızlı/önemli, okunmuş/okunmamış ve çöp kutusu yönetimi sunan .NET 9 MVC tabanlı örnek uygulama.

> **Stack:** .NET 9 • ASP.NET Core MVC • Identity • Entity Framework Core • SQL Server (2019/2022) • AdminLTE 3 • Bootstrap • Font Awesome

## 🚀 Özellikler

* **Kimlik Doğrulama:** ASP.NET Core Identity ile kayıt, giriş/çıkış.
* **Mesaj Kutusu:** Gelen, Gönderilen, Taslaklar, Çöp Kutusu sekmeleri.
* **Okunmuş/Okunmamış:** Mesaj detayı açıldığında `IsRead = true`; liste aksiyonlarından okunma durumunu hızlıca değiştir.
* **Yıldızlı (Starred):** Listede yıldız ikonu ile `IsStarred` hızlı toggle (doluyken **fas fa-star**, boşken **far fa-star**).
* **Sil/Çöp Kutusu:** Seçili mesajları çöp kutusuna taşı, geri al veya kalıcı sil (opsiyonel).
* **Anti‑Forgery:** Form işlemlerinde `[ValidateAntiForgeryToken]` kullanımı.
* **Sayfalama & Sıralama:** (İsteğe bağlı) liste performansı için hazır altyapı.

## 📦 Proje Yapısı (özet)

```
MyAcademyIdentity/
 ├─ Controllers/
 │   ├─ AccountController.cs
 │   └─ MessageController.cs   // Index, Compose, Send, Detail, ToggleStar, MarkRead, MoveToTrash...
 ├─ Data/
 │   └─ ApplicationDbContext.cs
 ├─ Models/
 │   └─ Message.cs             // MessageId, SenderId, ReceiverId, Subject, Body, IsRead, IsStarred, IsDeleted, CreatedAt
 ├─ Views/
 │   ├─ Message/
 │   │   ├─ Index.cshtml       // Gelen kutusu varsayılan
 │   │   ├─ Sent.cshtml
 │   │   ├─ Drafts.cshtml
 │   │   ├─ Trash.cshtml
 │   │   └─ Detail.cshtml
 │   └─ Shared/
 ├─ wwwroot/ (AdminLTE, css/js)
 ├─ appsettings.json
 └─ Program.cs
```

## 🛠️ Kurulum

### Gereksinimler

* [.NET SDK 9](https://dotnet.microsoft.com/)
* SQL Server 2019/2022 (LocalDB veya tam sürüm)
* (Opsiyonel) Node.js – sadece asset yönetimi gerekiyorsa

### Çalıştırma Adımları

1. **Depoyu klonla**

   ```bash
   git clone https://github.com/merveearp/MyAcademyIdentity.git
   cd MyAcademyIdentity
   ```
2. **Bağlantı dizesini ayarla** (`appsettings.json` → `ConnectionStrings:DefaultConnection`)

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=.;Database=MyAcademyIdentityDb;Trusted_Connection=True;TrustServerCertificate=True;"
     }
   }
   ```
3. **Veritabanını oluştur (EF Core Migrations)**

   ```bash
   dotnet tool install --global dotnet-ef
   dotnet ef database update
   ```

   > *Hata alırsan:* `dotnet ef migrations add Init` ile ilk migrasyonu oluşturup tekrar `update` çalıştır.
4. **Uygulamayı başlat**

   ```bash
   dotnet run
   ```

   Tarayıcı: `https://localhost:5001` (veya konsolda yazan port)

### Varsayılan Roller / Seed (opsiyonel)

İlk açılışta bir **admin** kullanıcısı oluşturmak için `ApplicationDbContext` veya `Program.cs` içinde seed bloğu ekleyebilirsin:

```csharp
// Seed örneği (Program.cs)
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var email = "admin@local";
    var user = await userManager.FindByEmailAsync(email);
    if (user is null)
    {
        user = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };
        await userManager.CreateAsync(user, "Admin*12345");
    }
}
```

## ✉️ Mesaj Modeli

```csharp
public class Message
{
    public int MessageId { get; set; }
    public string SenderId { get; set; } = default!;   // IdentityUser FK
    public string ReceiverId { get; set; } = default!; // IdentityUser FK
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public bool IsStarred { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
```

## 🔄 Tipik Aksiyonlar (Controller)

* **Liste:** `Index()` → `ReceiverId == user.Id && !IsDeleted`
* **Detay:** `MessageDetail(int id)` → `Include(Sender)` + `IsRead = true`
* **Yıldız Değiştir:** `ToggleStar(int id)` → `message.IsStarred = !message.IsStarred`
* **Okundu/Okunmadı:** `ToggleRead(int id)` → `message.IsRead = !message.IsRead`
* **Çöpe Taşı / Geri Al:** `MoveToTrash(int id)` → `message.IsDeleted = !message.IsDeleted`
* **Gönder:** `Send(MessageDto dto)` → `SenderId = user.Id; ReceiverId = dto.ReceiverId;` kayıt
* Tüm `POST` aksiyonlarında `[ValidateAntiForgeryToken]`

## 🧩 UI Notları

* **AdminLTE 3** temeli (sidebar, mailbox şablonu).
* Yıldız ikonu:


  ```html
  <i class="@(item.IsStarred ? "fas fa-star text-warning" : "far fa-star text-muted")"></i>
  ```
* Okunmuş/okunmamış için **badge** ve **ikon** farkı kullanılabilir.

## 📸 Ekran Görüntüleri
![8](https://github.com/user-attachments/assets/15c40bad-32dc-44aa-a6d3-7f4d817d6e1e)
![7](https://github.com/user-attachments/assets/18402606-48e1-4d5e-8a3a-f0d5f3d5ccb7)
![6](https://github.com/user-attachments/assets/dadfddbe-483e-43d4-9c9c-da8869ab93f4)
![5](https://github.com/user-attachments/assets/af7795f4-10f9-42cf-b083-a94fb1a4b5ac)
![4](https://github.com/user-attachments/assets/a534ca02-a301-4cbc-b022-a3e562442734)
![3](https://github.com/user-attachments/assets/b2d320a0-eb0e-4572-822e-2a5d1b240e50)
![2](https://github.com/user-attachments/assets/b7f7c89d-b137-41c6-a41b-3fd07d9c69f5)
![1](https://github.com/user-attachments/assets/5e74394d-f1ff-4b77-822b-ded00e05975d)
![11](https://github.com/user-attachments/assets/33f6f07a-fb90-429f-a070-18ae5edca770)
![10](https://github.com/user-attachments/assets/22c25590-117d-467b-89c5-40b93ba450d8)
![9](https://github.com/user-attachments/assets/7870b531-8c01-4949-83dc-b6e1f4591089)

