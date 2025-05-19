# Dental Service Web Application

Ứng dụng web quản lý dịch vụ nha khoa được xây dựng bằng ASP.NET Core MVC.

## Yêu cầu hệ thống

- .NET 6.0 SDK hoặc cao hơn
- MySQL Server (được cài đặt thông qua Laragon)
- Laragon (để quản lý MySQL và phpMyAdmin)
- Visual Studio 2022 hoặc Visual Studio Code

## Cài đặt

1. Cài đặt Laragon:
   - Tải Laragon từ https://laragon.org/download/
   - Cài đặt Laragon với các tùy chọn mặc định
   - Khởi động Laragon và click "Start All" để khởi động MySQL

2. Clone repository:
```bash
git clone [URL_REPOSITORY]
cd DentalMvcApp
```

3. Tạo cơ sở dữ liệu:
   - Mở trình duyệt và truy cập http://localhost/phpmyadmin
   - Đăng nhập với tài khoản mặc định:
     - Username: root
     - Password: (để trống)
   - Click "New" để tạo database mới
   - Đặt tên database là "dental_sys"
   - Chọn collation là "utf8mb4_unicode_ci"
   - Click "Create"
   - Chọn database "dental_sys" vừa tạo
   - Click tab "Import"
   - Chọn file `Database/CreateDatabase.sql`
   - Click "Go" để thực thi script

4. Cấu hình ứng dụng:
   - Mở file `appsettings.json`
   - Cập nhật connection string:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=dental_sys;User=root;Password=;"
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
   - Kiểm tra MySQL đã được khởi động trong Laragon
   - Kiểm tra connection string trong `appsettings.json`
   - Đảm bảo database "dental_sys" đã được tạo

2. Lỗi khi import SQL:
   - Kiểm tra file `CreateDatabase.sql` có đúng cú pháp MySQL
   - Thử import từng phần của script nếu cần

3. Lỗi khi chạy ứng dụng:
   - Kiểm tra .NET SDK đã được cài đặt đúng phiên bản
   - Chạy `dotnet restore` để cập nhật các package
   - Kiểm tra các lỗi trong console

## Đóng góp

Mọi đóng góp đều được hoan nghênh. Vui lòng tạo issue hoặc pull request để đóng góp. 