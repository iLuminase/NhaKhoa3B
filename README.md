# Dental Service Web Application

Ứng dụng web quản lý dịch vụ nha khoa được xây dựng bằng ASP.NET Core MVC.

## Yêu cầu hệ thống

- .NET 6.0 SDK hoặc cao hơn
- SQL Server (Express hoặc Developer Edition)
- SQL Server Management Studio (SSMS)
- Visual Studio 2022 hoặc Visual Studio Code

## Cài đặt

1. Cài đặt SQL Server:
   - Tải SQL Server Express từ https://www.microsoft.com/en-us/sql-server/sql-server-downloads
   - Cài đặt SQL Server với các tùy chọn mặc định
   - Cài đặt SQL Server Management Studio (SSMS) từ https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms

2. Clone repository:
```bash
git clone [URL_REPOSITORY]
cd DentalMvcApp
```

3. Tạo cơ sở dữ liệu:
   - Mở SQL Server Management Studio
   - Kết nối đến SQL Server instance
   - Mở file `Database/CreateSqlServerDatabase_Manual.sql` trong SSMS
   - Thực thi script để tạo database và các bảng
   - Mở file `Database/SeedSqlServerData_Manual.sql` trong SSMS
   - Thực thi script để tạo dữ liệu mẫu

4. Cấu hình ứng dụng:
   - Mở file `appsettings.json`
   - Cập nhật connection string (mặc định đã được cấu hình):
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=.;Database=DentalDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
     }
   }
   ```

5. Chạy ứng dụng:
```bash
dotnet run
```

## Cấu trúc dự án

- `Controllers/`: Chứa các controller xử lý request
- `Models/`: Chứa các model đại diện cho dữ liệu
- `Views/`: Chứa các view template
- `Services/`: Chứa các service xử lý business logic
- `Data/`: Chứa các class liên quan đến database context
- `Migrations/`: Chứa các migration để tạo và cập nhật database
- `wwwroot/`: Chứa các file tĩnh (CSS, JavaScript, images)

## Tính năng chính

- Quản lý thông tin bệnh nhân
- Quản lý lịch hẹn
- Quản lý dịch vụ nha khoa
- Quản lý nhân viên
- Báo cáo thống kê

## Xử lý sự cố

1. Lỗi kết nối database:
   - Kiểm tra SQL Server đã được khởi động
   - Kiểm tra connection string trong `appsettings.json`
   - Đảm bảo database "DentalDB" đã được tạo
   - Kiểm tra quyền truy cập của tài khoản Windows đến SQL Server
   - Đảm bảo đã bật chế độ xác thực Windows trong SQL Server

2. Lỗi khi thực thi SQL script:
   - Kiểm tra file `CreateSqlServerDatabase_Manual.sql` có đúng cú pháp SQL Server
   - Thử thực thi từng phần của script nếu cần
   - Kiểm tra log lỗi trong SQL Server Management Studio
   - Đảm bảo bạn đang sử dụng SQL Server, không phải MySQL

3. Lỗi khi chạy ứng dụng:
   - Kiểm tra .NET SDK đã được cài đặt đúng phiên bản
   - Chạy `dotnet restore` để cập nhật các package
   - Kiểm tra các lỗi trong console
   - Đảm bảo đã cài đặt package Microsoft.EntityFrameworkCore.SqlServer

## Đóng góp

Mọi đóng góp đều được hoan nghênh. Vui lòng tạo issue hoặc pull request để đóng góp.