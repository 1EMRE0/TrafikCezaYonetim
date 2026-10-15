#  Trafik Cezası Yönetim ve Onay Modülü (TCTS)

Bu proje; kurumsal filolara ait araçların trafik cezalarını kayıt altına almak, rol bazlı çok aşamalı bir onay iş akışından geçirmek ve gerçekleşen tüm eylemleri denetim geçmişi (audit log) olarak kayıt altında tutmak amacıyla geliştirilmiş bir **.NET Core MVC** uygulamasıdır[cite: 4].

Proje; **Clean / N-Tier Architecture** prensiplerine uygun olarak geliştirilmiş olup, case çalışmasındaki tüm fonksiyonel gereksinimleri ve Miro tahtasında modellenen tasarım ve iş kurallarını eksiksiz karşılamaktadır[cite: 4, 8, 9].

---

##  Temel Özellikler ve Modüller

### 1. Araç Yönetimi (Fleet Management)
* **Desteklenen Araç Tipleri:** Binek, Çekici, Dorse, Kiralık Araç[cite: 4].
* **Detaylı Takip:** Plaka, Marka/Model, Araç/Makina Adı, Seri No, Model Yılı, Yakıt Türü ve Aitlik (Özmal / Kiralık) yönetimi[cite: 4, 5].
* **Sigorta & Muayene & Kasko:** Poliçe numaraları, sigorta başlangıç/bitiş tarihleri, muayene takvimi ve prim tutarları takibi[cite: 5, 6].
* Araçların tanımlanması, listelenmesi ve güncellenmesi (Full CRUD döngüsü)[cite: 4, 5, 6].

### 2. Çok Aşamalı Onay Mekanizması (State Machine Workflow)
Trafik cezaları tanımlanan iş akışına göre aşamalı olarak ilerler[cite: 4, 8]:

```text
       [YENİ CEZA]
            │
            ▼
   [YÖNETİCİ ONAYINDA] ──(Reddet)──► [REDDEDİLDİ]
            │                              ▲
         (Onayla)                          │
            ▼                              │
    [FİNANS ONAYINDA]  ──(Reddet)──────────┘
            │
         (Onayla)
            ▼
      [TAMAMLANDI]
```

#### Rol Tabanlı Yetki Dağılımı:
* **Personel:** Yeni ceza kaydı oluşturabilir[cite: 4, 9, 13]. Onaylama veya reddetme yetkisi bulunmaz[cite: 4, 9].
* **Yönetici:** Yalnızca `Yönetici Onayında` olan cezaları inceleyip bir sonraki aşamaya geçirebilir veya reddedebilir[cite: 4, 8, 9].
* **Finans:** Yalnızca `Finans Onayında` olan cezaları inceleyip süreci tamamlayabilir veya reddedebilir[cite: 4, 8, 9].

---

##  Miro İş Kuralları (Business & Validation Rules)

Miro analiz tahtasında belirlenen çekirdek iş kuralları sisteme entegre edilmiştir[cite: 8, 9]:

1. **Onay Kuralları:**
   * **Sıra Atlama Engeli:** `Yönetici Onayında` statüsündeki bir kaydı Finans yetkilisi onaylayamaz[cite: 8, 9].
   * **Yetki Kontrolü:** Sadece yetkili rol sahibi ilgili aşamada işlem yapabilir[cite: 4, 9].
   * **Kendi İşlemini Onaylama Yasağı:** Cezayı sisteme kaydeden kullanıcı, rolü yönetici veya finans olsa dahi kendi açtığı kayda onay veremez[cite: 9].

2. **Değiştirilemezlik (Immutability):**
   * **Tamamlanma Durumu:** `Tamamlandı` durumuna ulaşan ceza kaydı silinemez; ceza tutarı ve plakası değiştirilemez[cite: 9].
   * **Reddedilen Kayıt Kuralı:** Durumu `Reddedildi` olan kayıt üzerinde onay adımları devre dışı kalır[cite: 9]. Sadece cezayı oluşturan kişi kaydı düzenleyip tekrar `Yönetici Onayında` statüsüne çekebilir[cite: 9].

3. **Veri Doğrulama (Validation):**
   * **Zorunlu Ret Gerekçesi:** İşlem tipi `Reddedildi` ise ret açıklaması boş bırakılamaz ve en az 10 karakter olmalıdır[cite: 4, 9].
   * **Tutar Mantığı:** Ceza tutarı 0'dan büyük olmalıdır (`TotalAmount > 0`)[cite: 9].
   * **Otomatik Hesaplama:** `Ödenecek Tutar = Ceza Tutarı - Erken Ödeme İndirimi` (`PayableAmount = TotalAmount - DiscountAmount`)[cite: 9, 13].
   * **Tarih Kontrolü:** Ceza tarihi (`PenaltyDate`) gelecekteki bir tarih olamaz[cite: 9].

4. **Veritabanı Bütünlüğü:**
   * **İşlem Bütünlüğü (Atomic Transaction):** Ceza durumu güncellenirken onay geçmişi tablosuna (`PenaltyApprovalHistories`) log yazılamazsa, durum güncellemesi geri alınır (Rollback)[cite: 9].
   * **Benzersiz Plaka:** Araç tablosunda aynı plaka ile mükerrer araç kaydedilemez[cite: 9].

---

##  Denetim Geçmişi (Audit Log Timeline)

Her ceza kaydı için gerçekleşen tüm adımlar zaman çizelgesi (timeline) formatında saklanır ve izlenir[cite: 4, 14]:
* **İşlemi Yapan:** Ad, Soyad ve Sistem Rolü[cite: 4, 14]
* **İşlem Tarihi ve Saati:** `DateTime` damgası[cite: 4, 14]
* **Durum Değişimi:** Önceki Durum ➔ Yeni Durum[cite: 4, 14]
* **Açıklama / Ret Gerekçesi:** İşlem sırasında girilen notlar[cite: 4, 14]

---

##  Miro Tasarım ve Mockup Uyumu

Arayüz bileşenleri, renk paleti ve modal pencereleri Miro tasarımlarına sadık kalınarak kodlanmıştır[cite: 2, 5, 11, 12, 13, 14]:

| Giriş Ekranı (Login) | Ceza Listesi (Dashboard) |
| :---: | :---: |
| ![Giriş](docs/login-mockup.png) | ![Ceza Listesi](docs/dashboard-mockup.png) |

| Araç Ekle & Düzenle | Ceza Detay & Zaman Çizelgesi |
| :---: | :---: |
| ![Araç Formu](docs/vehicle-edit-mockup.png) | ![Zaman Çizelgesi](docs/audit-timeline-mockup.png) |

---

##  Kullanılan Teknolojiler

* **Çatı:** .NET Core MVC (C#)[cite: 4]
* **Veritabanı & ORM:** SQL Server & Entity Framework Core (Code-First)[cite: 4]
* **Kimlik Doğrulama:** Cookie-Based Authentication & Role-Based Authorization
* **Frontend:** Bootstrap 5.3 CDN, Bootstrap Icons, Custom CSS

---

##  Katmanlı Mimari (N-Tier)

```text
TrafikCezaYonetim/
├── TrafikCezaYonetim.Core/           # Varlıklar (Entities), Sabitler ve Enumlar
├── TrafikCezaYonetim.DataAccess/     # DbContext, Yapılandırmalar, Seed Data ve Migrations
├── TrafikCezaYonetim.Business/       # DTOlar, Validasyonlar, Servis Arayüzleri ve İş Mantığı
└── TrafikCezaYonetim.Web/            # MVC Controllerları, Viewlar ve UI Bileşenleri
```

---

##  Kurulum ve Çalıştırma

1. **Projeyi Klonlayın:**
   ```bash
   git clone [https://github.com/kullanici-adi/TrafikCezaYonetim.git](https://github.com/kullanici-adi/TrafikCezaYonetim.git)
   cd TrafikCezaYonetim
   ```

2. **Veritabanı Bağlantı Dizesini Belirleyin:**
   `TrafikCezaYonetim.Web/appsettings.json` dosyasını açıp yerel SQL Server bağlantınızı girin:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=TrafikCezaDb;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

3. **Veritabanını Oluşturun ve Migration'ları Uygulayın:**
   Package Manager Console üzerinden:
   ```shell
   Update-Database -Project TrafikCezaYonetim.DataAccess -StartupProject TrafikCezaYonetim.Web
   ```

4. **Projeyi Başlatın:**
   ```bash
   dotnet run --project TrafikCezaYonetim.Web
   ```

---

##  Ön Tanımlı Test Kullanıcıları

| Rol | Kullanıcı Adı | Şifre | Yetki Kapsamı |
| :--- | :--- | :--- | :--- |
| **Personel** | `personel` | `123456` | Yeni ceza kaydı açma, araç listesini inceleme[cite: 4, 9, 13] |
| **Yönetici** | `yonetici` | `123456` | 1. Seviye Onay / Ret işlemleri[cite: 4, 8, 9] |
| **Finans** | `finans` | `123456` | 2. Seviye Onay / Ret işlemleri ve Tamamlama[cite: 4, 8, 9] |
