# BugTrackerSystem

## 1. Giới thiệu
**BugTrackerSystem** là hệ thống quản lý lỗi (Issue Tracking System) Full-stack, bao gồm Backend, Frontend, và các bộ test tự động. Dự án được phát triển bằng **ASP.NET Core 8.0**, **C#**, và **Entity Framework Core**.

## 2. Công nghệ chính
- **ASP.NET Core 8.0**
- **Entity Framework Core** (Code-First, Migrations)
- **MySQL** (có thể dùng container qua Docker)
- **Docker & Docker Compose**
- **MSTest**, **Playwright** cho Unit/E2E Tests
- **Razor Pages / MVC**, **ASP.NET Core Identity**

## 3. Cấu trúc thư mục
```plaintext
BugTrackerSystem/                      
├── BugTracker.Business/               # Business logic layer
├── BugTracker.Business.Tests/         # Unit tests cho Business
├── BugTracker.Data/                   # EF Core DbContext, Models, Repositories
│   ├── ApplicationDbContext.cs        
│   ├── Migrations/                    
│   ├── Models/                        
│   └── Repositories/                  
├── BugTracker.E2ETests/               # Playwright end-to-end tests
├── BugTracker.TestSupport/            # Helpers cho testing  
├── BugTracker.Web/                    # ASP.NET Core Web App (UI + API)
│   ├── Areas/Identity/Pages/          
│   ├── Controllers/                   
│   ├── DesignTimeFactories/           
│   ├── Extensions/                    
│   ├── ViewModels/                    
│   ├── Views/                         
│   ├── wwwroot/                       
│   ├── Dockerfile                     
│   ├── Program.cs                     
│   ├── appsettings.json               
│   └── appsettings.Development.json   
├── BugTracker.Web.Tests/              # Unit tests cho Web
├── .dockerignore                      # Loại trừ file khi build Docker
├── docker-compose.yml                 # Docker Compose cấu hình đầy đủ (bao gồm MySQL)
├── BugTracker.sln                     # Solution file tổng
└── README.md                          # (Bạn đang đọc)
```

## 4. Yêu cầu môi trường
- **.NET 8 SDK**: cài từ https://dotnet.microsoft.com/download/dotnet/8.0  
- **EF Core CLI** (cho migrations): `dotnet tool install --global dotnet-ef --version 8.*`  
- **Docker & Docker Compose** (tuỳ chọn chạy qua container)  
- **MySQL** (Local hoặc container)

## 5. Cài đặt & khởi chạy

### 5.1 Clone repository
```bash
git clone https://github.com/hoangsnowy/BugTrackerSystem.git
cd BugTrackerSystem
```

### 5.2 Cấu hình kết nối database
Mở `appsettings.json` trong `BugTracker.Web` và sửa:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=3306;Database=BugTrackerDb;User=root;Password=YourPassword;"
}
```
> Nếu dùng Docker Compose, file `docker-compose.yml` đã cấu hình service MySQL; mặc định connection string kết nối tới `mysql` host.

### 5.3 Áp dụng EF Core Migrations
```bash
# Tạo migration ban đầu (nếu chưa có)
dotnet ef migrations add Initial --project BugTracker.Data --startup-project BugTracker.Web --context ApplicationDbContext --output-dir Migrations

# Cập nhật database
dotnet ef database update --project BugTracker.Data --startup-project BugTracker.Web --context ApplicationDbContext
```

### 5.4 Chạy ứng dụng

#### A. Qua Docker Compose  
```bash
docker-compose up -d
```  
- Truy cập web UI: http://localhost:5000 hoặc https://localhost:5001

#### B. Local (không Docker)  
```bash
cd BugTracker.Web
dotnet run
```  
- Truy cập: http://localhost:5000 / https://localhost:5001

## 6. Tài khoản mẫu (Seed Data)
| Tên đăng nhập | Email                 | Mật khẩu     | Role                      |
|---------------|-----------------------|--------------|---------------------------|
| tester1       | tester1@example.com   | Password123! | Tester                    |
| dev1          | dev1@example.com      | Password123! | Developer                 |
| pm1           | pm1@example.com       | Password123! | ProjectManager            |
| admin1        | admin1@example.com    | Password123! | Admin                     |
| autotest1     | autotest1@example.com | Password123! | Tester (E2E Automation)   |



## 7. Chạy bộ test
```bash
# Unit tests với MSTest
dotnet test BugTracker.Business.Tests
dotnet test BugTracker.Web.Tests

# E2E tests với Playwright
dotnet test BugTracker.E2ETests
```

## 8. Đóng góp
- Fork repository  
- Tạo branch `feature/...` hoặc `fix/...`  
- Mở Pull Request và mô tả chi tiết thay đổi

## 9. License
Distributed under the **MIT License**. Xem chi tiết trong file [LICENSE](LICENSE).

---

_Cảm ơn bạn đã sử dụng và đóng góp cho BugTrackerSystem!_