<h1># 🦷 Dental Service Web Application
</h1>
<h2>1. Trang Chủ</h2>

![image](https://github.com/user-attachments/assets/6e9ae933-47e7-463b-8c5b-7a914d4d6dd5)

<h2>2. Trang đăng nhập và đăng ký</h2>

![image](https://github.com/user-attachments/assets/ed7fb046-7e5a-4f96-a688-67d19dc61840)

![image](https://github.com/user-attachments/assets/534e5272-961a-48bc-9216-5ad3cd80f44e)

<h2>3. Trang quản lý admin</h2>

![image](https://github.com/user-attachments/assets/d1eb2172-6d57-4887-82ff-943497257a12)

<h2>4. Trang đặt lịch hẹn và quản lý lịch</h2>

![image](https://github.com/user-attachments/assets/5cd34fa6-9709-4a00-99ad-b551a69dd8df)

![image](https://github.com/user-attachments/assets/648dde3d-107f-4042-b3c0-f84b163f9e9a)

<h2>5. Quản lý account người dùng</h2>
![image](https://github.com/user-attachments/assets/05b5df2d-dfc5-4390-b00c-71168055157d)
...Các CRUD khác cho bệnh nhân và bác sĩ...
<h2>6. Quản lý thanh toán-Tích hợp QR MOMO(chỉ triển khai trên local nên chưa lấy success message từ app Sandbox của MOMO)</h2>

![image](https://github.com/user-attachments/assets/82be1d27-3402-40da-8ffe-41163744731c)

![image](https://github.com/user-attachments/assets/56f1a14e-1e10-448c-b272-09c85974a2db)

![image](https://github.com/user-attachments/assets/4bd4762b-f9d7-4a84-a2e0-1a2e48d2c539)

<h2>6.1 Quản lý lịch sử thanh toán và in hoá đơn</h2>

![image](https://github.com/user-attachments/assets/108b9e39-b3ac-4719-ae20-cdb1bf4b700f)

![image](https://github.com/user-attachments/assets/637710ed-a633-4ee3-be2e-072d139d43e4)

<h2>7. Báo cáo thống kê</h2>

![image](https://github.com/user-attachments/assets/fe4081fd-53ab-4d2f-81a8-3f7dd7b31d92)

## Đang phát triển thêm...

Ứng dụng web quản lý dịch vụ nha khoa được xây dựng bằng ASP.NET Core MVC với Entity Framework Core và SQL Server.

## 📋 Yêu cầu hệ thống

- **.NET 9.0 SDK** hoặc cao hơn
- **SQL Server** (Express, Developer, hoặc Standard Edition)
- **SQL Server Management Studio (SSMS)** (khuyến nghị)
- **Visual Studio 2022** hoặc **Visual Studio Code**
- **Git** để clone repository

## 🚀 Hướng dẫn cài đặt

### Bước 1: Cài đặt môi trường

#### 1.1. Cài đặt .NET 9.0 SDK
```bash
# Kiểm tra phiên bản .NET hiện tại
dotnet --version

# Tải .NET 9.0 SDK từ: https://dotnet.microsoft.com/download
```

#### 1.2. Cài đặt SQL Server
```bash
# Tải SQL Server Express (miễn phí):
# https://www.microsoft.com/en-us/sql-server/sql-server-downloads


```

#### 1.3. Cài đặt SQL Server Management Studio (SSMS)
```bash
# Tải SSMS từ:
# https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms
```

### Bước 2: Clone và cấu hình dự án

#### 2.1. Clone repository
```bash
git clone [URL_REPOSITORY]
cd Dental_ServiceWeb/DentalMvcApp/MyMvcApp
```

#### 2.2. Restore packages
```bash
dotnet restore
```

### Bước 3: Cấu hình Database

#### 3.1. Cấu hình Connection String

Mở file `appsettings.json` và cập nhật connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=DentalDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```



#### 3.2. Tạo Database bằng Entity Framework Migrations

**Phương pháp 1: Sử dụng Entity Framework Migrations (Khuyến nghị)**

```bash
# Kiểm tra các migration hiện có
dotnet ef migrations list

# Tạo database và áp dụng migrations
dotnet ef database update

# Nếu cần tạo migration mới
dotnet ef migrations add "TenMigration"
```

**Phương pháp 2: Xóa và tạo lại database**

```bash
# Xóa database hiện tại (cẩn thận!)
dotnet ef database drop

# Tạo lại database từ migrations
dotnet ef database update
```

### Bước 4: Chạy ứng dụng

```bash
# Chạy ứng dụng
dotnet run

# Hoặc chạy với watch mode (tự động reload khi có thay đổi)
dotnet watch run
```

Ứng dụng sẽ chạy tại:
- **HTTPS**: https://localhost:7115
- **HTTP**: http://localhost:5170

### Bước 5: Đăng nhập hệ thống

**Tài khoản Admin mặc định:**
- **Email**: `admin@dental.com`
- **Password**: `Admin@123`

## 🗄️ Quản lý Database

### Cấu trúc Database

Dự án sử dụng **Entity Framework Core** với **Code First Migrations**:

```
DentalDB/
├── Users (AspNetUsers)          # Quản lý người dùng
├── Roles (AspNetRoles)          # Phân quyền
├── Patients                     # Thông tin bệnh nhân
├── Appointments                 # Lịch hẹn
├── Services                     # Dịch vụ nha khoa
├── Payments                     # Thanh toán
├── DentalRecords               # Hồ sơ nha khoa
├── Activities                  # Nhật ký hoạt động
└── PaymentTransactions         # Giao dịch thanh toán
```

### Đồng bộ Database

#### Cập nhật Database Schema

```bash
# Tạo migration mới khi có thay đổi model
dotnet ef migrations add "TenMigrationMoi"

# Áp dụng migration vào database
dotnet ef database update

# Xem lịch sử migrations
dotnet ef migrations list

# Rollback đến migration cụ thể
dotnet ef database update TenMigrationCu
```

#### Backup và Restore Database

**Backup Database:**
```sql
-- Trong SSMS hoặc sqlcmd
BACKUP DATABASE DentalDB
TO DISK = 'C:\Backup\DentalDB.bak'
WITH FORMAT, INIT;
```

**Restore Database:**
```sql
-- Restore từ backup
RESTORE DATABASE DentalDB
FROM DISK = 'C:\Backup\DentalDB.bak'
WITH REPLACE;
```

#### Reset Database

```bash
# Xóa hoàn toàn database
dotnet ef database drop --force

# Tạo lại database từ đầu
dotnet ef database update

# Hoặc reset migrations (cẩn thận!)
rm -rf Migrations/
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## 📁 Cấu trúc dự án

```
MyMvcApp/
├── Controllers/                 # Controllers xử lý request
│   ├── AdminController.cs      # Quản lý admin
│   ├── PatientController.cs    # Quản lý bệnh nhân
│   ├── AppointmentController.cs # Quản lý lịch hẹn
│   └── PaymentController.cs    # Xử lý thanh toán
├── Models/                     # Data models
│   ├── ApplicationUser.cs      # User model
│   ├── Patient.cs             # Bệnh nhân
│   ├── Appointment.cs         # Lịch hẹn
│   └── Payment.cs             # Thanh toán
├── Views/                      # Razor views
│   ├── Admin/                 # Views cho admin
│   ├── Patient/               # Views cho bệnh nhân
│   └── Shared/                # Shared layouts
├── Data/                       # Database context
│   └── ApplicationDbContext.cs # EF Core context
├── Migrations/                 # EF Core migrations
├── Services/                   # Business logic services
├── wwwroot/                    # Static files
│   ├── css/                   # Stylesheets
│   ├── js/                    # JavaScript
│   └── images/                # Images
└── appsettings.json           # Configuration
```

## 🎯 Tính năng chính

- ✅ **Quản lý bệnh nhân**: Thêm, sửa, xóa thông tin bệnh nhân
- ✅ **Quản lý lịch hẹn**: Đặt lịch, theo dõi lịch hẹn
- ✅ **Quản lý dịch vụ**: Danh sách dịch vụ nha khoa
- ✅ **Quản lý nhân viên**: Phân quyền và quản lý user
- ✅ **Báo cáo thống kê**: Dashboard và báo cáo doanh thu
- ✅ **Thanh toán**: Tích hợp MoMo Payment Gateway
- ✅ **Bảo mật**: ASP.NET Core Identity với phân quyền

## 🔧 Xử lý sự cố

### 1. Lỗi kết nối Database

```bash
# Kiểm tra SQL Server đang chạy
services.msc # Tìm SQL Server services

# Test connection string
dotnet ef dbcontext info

# Kiểm tra database tồn tại
sqlcmd -S . -E -Q "SELECT name FROM sys.databases WHERE name = 'DentalDB'"
```

**Các lỗi thường gặp:**
- ❌ `Cannot open database "DentalDB"` → Chạy `dotnet ef database update`
- ❌ `Login failed for user` → Kiểm tra connection string
- ❌ `Server does not exist` → Kiểm tra SQL Server đã khởi động

### 2. Lỗi Entity Framework

```bash
# Cài đặt EF Core tools
dotnet tool install --global dotnet-ef

# Kiểm tra EF tools
dotnet ef --version

# Rebuild project
dotnet clean
dotnet build
```

### 3. Lỗi Migration

```bash
# Xem chi tiết migration
dotnet ef migrations script

# Xóa migration cuối cùng
dotnet ef migrations remove

# Force update database
dotnet ef database update --force
```

### 4. Lỗi Dependencies

```bash
# Restore packages
dotnet restore

# Clear cache
dotnet nuget locals all --clear

# Rebuild solution
dotnet clean
dotnet build
```

## 🚀 Deployment

### Development Environment

```bash
# Chạy với Development settings
dotnet run --environment Development

# Với hot reload
dotnet watch run
```

### Production Environment

```bash
# Build for production
dotnet publish -c Release -o ./publish

# Chạy production build
cd publish
dotnet MyMvcApp.dll
```

### Docker Deployment

```dockerfile
# Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["MyMvcApp.csproj", "."]
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MyMvcApp.dll"]
```

```bash
# Build và chạy Docker
docker build -t dental-app .
docker run -p 8080:80 dental-app
```

## 🤝 Đóng góp

1. Fork repository
2. Tạo feature branch: `git checkout -b feature/TenTinhNang`
3. Commit changes: `git commit -m 'Thêm tính năng mới'`
4. Push to branch: `git push origin feature/TenTinhNang`
5. Tạo Pull Request

## 📝 License

Dự án này được phát hành dưới [MIT License](LICENSE).

## 📞 Hỗ trợ

Nếu gặp vấn đề, vui lòng:
1. Kiểm tra [Issues](../../issues) hiện có
2. Tạo [Issue mới](../../issues/new) với mô tả chi tiết
3. Liên hệ team phát triển
