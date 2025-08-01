# TaskManager API Güvenlik Rehberi

## 🔒 Mevcut Güvenlik Önlemleri

### ✅ Uygulanan Güvenlik Önlemleri:
1. **JWT Authentication** - Token tabanlı kimlik doğrulama
2. **BCrypt Password Hashing** - Güvenli şifre hash'leme
3. **Input Validation** - DataAnnotations ile giriş doğrulama
4. **Authorization Policies** - Rol tabanlı yetkilendirme
5. **Rate Limiting** - API rate limiting koruması
6. **CORS Configuration** - Cross-origin resource sharing kontrolü
7. **Security Headers** - Güvenlik header'ları
8. **HTTPS Redirection** - Güvenli bağlantı zorunluluğu

## 🚨 Production Ortamı İçin Kritik Öneriler

### 1. **Environment Variables Kullanın**
```bash
# appsettings.Production.json yerine environment variables kullanın
JWT__SECRET=your-super-secure-secret-key-here
JWT__ISSUER=your-domain.com
JWT__AUDIENCE=your-app-users
```

### 2. **Güçlü JWT Secret Oluşturun**
```bash
# 256-bit (32 byte) random key oluşturun
openssl rand -base64 32
```

### 3. **Database Güvenliği**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your-server;Database=TaskManagerDb;User Id=app_user;Password=strong_password;TrustServerCertificate=true;"
  }
}
```

### 4. **HTTPS Zorunluluğu**
```csharp
// Program.cs'de
if (!app.Environment.IsDevelopment())
{
    app.UseHsts(); // HTTP Strict Transport Security
}
```

### 5. **Logging ve Monitoring**
```csharp
// Güvenlik olaylarını loglayın
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
    logging.AddEventLog();
});
```

## 🛡️ Ek Güvenlik Önlemleri

### 1. **API Key Authentication**
```csharp
// Kritik endpoint'ler için API key ekleyin
[ApiKey]
public class ApiKeyAttribute : Attribute, IAsyncActionFilter
{
    // Implementation
}
```

### 2. **Request Size Limiting**
```csharp
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 10 * 1024 * 1024; // 10MB
});
```

### 3. **SQL Injection Koruması**
- Entity Framework kullanıyoruz (✅)
- Raw SQL kullanmaktan kaçının
- Parameterized queries kullanın

### 4. **XSS Koruması**
```csharp
// Input sanitization ekleyin
builder.Services.AddAntiforgery();
```

### 5. **CSRF Koruması**
```csharp
[ValidateAntiForgeryToken]
public async Task<IActionResult> SensitiveAction()
{
    // Implementation
}
```

## 🔍 Güvenlik Testleri

### 1. **Penetration Testing**
- OWASP ZAP kullanın
- Burp Suite ile test edin
- Automated security scanning

### 2. **Dependency Scanning**
```bash
dotnet list package --vulnerable
```

### 3. **Code Analysis**
```bash
dotnet format --verify-no-changes
dotnet analyze
```

## 📋 Güvenlik Checklist

- [ ] Environment variables kullanılıyor
- [ ] Güçlü JWT secret (256-bit+)
- [ ] HTTPS zorunlu
- [ ] Rate limiting aktif
- [ ] CORS yapılandırılmış
- [ ] Security headers ekli
- [ ] Input validation mevcut
- [ ] Authorization policies tanımlı
- [ ] Error handling güvenli
- [ ] Logging yapılandırılmış
- [ ] Database bağlantısı güvenli
- [ ] Dependencies güncel
- [ ] Security testing yapıldı

## 🚨 Güvenlik Uyarıları

1. **JWT Secret'ı asla source control'e commit etmeyin**
2. **Production'da debug mode'u kapatın**
3. **Sensitive data'yı loglamayın**
4. **Default credentials kullanmayın**
5. **Unnecessary ports'ları açmayın**

## 📞 Güvenlik İletişimi

Güvenlik açığı bulursanız: security@yourcompany.com 