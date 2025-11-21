ProjectManager

ProjectManager, projelerinizi, görevlerinizi ve ekiplerinizi tek bir yerden verimli bir şekilde yönetmenizi sağlayan bir proje yönetim aracıdır.


📝 Hakkında

Bu proje, Kullanıcıların projeler oluşturmasına, görevler atamasına ve ilerlemeyi anlık olarak takip etmesine olanak tanır.

✨ Özellikler

    Proje Yönetimi: Sınırsız sayıda proje oluşturun, düzenleyin ve arşivleyin.

    Görev Takibi: Projelere görevler ekleyin, son tarihler belirleyin ve görev durumlarını (Örn: Beklemede, Devam Ediyor, Tamamlandı) güncelleyin.

    Ekip Yönetimi: Projelere ekip üyeleri davet edin ve görev atamaları yapın.

    Kullanıcı Dostu Arayüz: Kolay ve anlaşılır bir arayüz ile projelerinizi rahatça yönetin.


🚀 Kullanılan Teknolojiler

Projenin geliştirilmesinde kullanılan ana teknolojiler ve kütüphaneler aşağıda listelenmiştir.

    Backend: [Örn: .NET 7, C#]

    Frontend: [Örn: React, Angular, Vue.js veya MVC]

    Veritabanı: [Örn: Microsoft SQL Server, PostgreSQL, MySQL]

    API Teknolojisi: [Örn: RESTful API, GraphQL]


🏁 Başlarken

Bu bölüm, projeyi yerel makinenizde kurup çalıştırmanız için size yol gösterecektir.

Gereksinimler

Projeyi çalıştırmak için sisteminizde yüklü olması gereken yazılımlar:

    [Örn: .NET 7 SDK]

    [Örn: Node.js v18.x]

    [Örn: Microsoft SQL Server]

Kurulum

    git clone https://github.com/mrkad61/ProjectManager.git

Proje dizinine gidin:
```csharp
cd ProjectManager
```
Gerekli bağımlılıkları yükleyin:

```csharp
dotnet restore

cd client && npm install
```


Veritabanı bağlantı ayarlarını yapın:

    appsettings.json dosyasındaki ConnectionString bölümünü kendi veritabanı bilgilerinizle güncelleyin.

Veritabanını oluşturun ve güncelleyin (migrations):
Bash

    dotnet ef database update

kullanım

Projeyi başlatmak için aşağıdaki komutu çalıştırın:

```csharp
dotnet run
```


Uygulama varsayılan olarak https://localhost:[PORT_NUMARASI] adresinde çalışmaya başlayacaktır.

🤝 Katkıda Bulunma

Katkılarınız projeyi daha iyi bir hale getirecektir! Katkıda bulunmak isterseniz, lütfen aşağıdaki adımları izleyin:

    Projeyi "Fork" edin.

    Kendi özellik dalınızı (feature/YeniOzellik) oluşturun.

    Değişikliklerinizi "Commit" edin (git commit -m 'Yeni bir özellik eklendi').

    Dalınızı "Push" edin (git push origin feature/YeniOzellik).

    Bir "Pull Request" açın.


Proje Linki: https://github.com/mrkad61/ProjectManager/
