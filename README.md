# BugTrackerSystem


# Hướng dẫn nhanh EF Core Migrations (.NET 8)

Sử dụng EF Core CLI để tạo và áp dụng migration cho `ApplicationDbContext` và `ApplicationIdentityDbContext` trong dự án **BugTrackerSystem**.

---

### 1. Cài đặt EF Core CLI
```bash
dotnet tool install --global dotnet-ef --version 8.*
```

### 2. Tạo Migration

- **Cho ApplicationDbContext**
```bash
dotnet ef migrations add InitialApp --project BugTracker.Data --startup-project BugTracker.Web --context ApplicationDbContext --output-dir Migrations
```

### 3. Áp dụng Migration lên Database

- **ApplicationDbContext**
```bash
dotnet ef database update --project BugTracker.Data --startup-project BugTracker.Web --context ApplicationDbContext
```


# BugTrackerSystem

## Hướng dẫn nhanh EF Core Migrations (.NET 8)

Sử dụng EF Core CLI để tạo và áp dụng migration cho **ApplicationDbContext** duy nhất trong dự án **BugTrackerSystem**.

---

### Thông tin đăng nhập mẫu
Dưới đây là danh sách các tài khoản mẫu đã được seed sẵn vào database:

| Tài khoản  | Email                      | Mật khẩu       | Vai trò                       |
|------------|----------------------------|----------------|-------------------------------|
| tester1    | tester1@example.com        | Password123!   | Tester                        |
| dev1       | dev1@example.com           | Password123!   | Developer                     |
| pm1        | pm1@example.com            | Password123!   | ProjectManager                |
| admin1     | admin1@example.com         | Password123!   | Admin                         |
| autotest1  | autotest1@example.com      | Password123!   | Tester (Automation Test)      |

