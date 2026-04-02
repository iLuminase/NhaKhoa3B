# TÀI LIỆU MÔ TẢ CHỨC NĂNG HỆ THỐNG - NHA KHOA 3B

**Mục đích tài liệu:** 
Tài liệu này được biên soạn dành cho các thành viên mới tham gia dự án (Newbies, QCs, Testers, Developers). Mục tiêu là giúp mọi người nắm bắt nhanh luồng nghiệp vụ (Business Logic) của hệ thống "Nha Khoa 3B", từ đó làm cơ sở để viết tài liệu báo cáo và xây dựng kịch bản kiểm thử (Test Cases), đặc biệt là phục vụ cho công cụ kiểm thử tự động **Katalon Studio** (chứ không phải Kafalon như bạn thường gọi nhầm).

---

## 1. TỔNG QUAN HỆ THỐNG & CÔNG NGHỆ CHÍNH
- **Nền tảng chính:** .NET 9 (ASP.NET Core MVC & Razor Pages).
- **Cơ sở dữ liệu:** SQL Server thông qua Entity Framework Core.
- **Xác thực & Phân quyền:** ASP.NET Core Identity.
- **Giao diện:** Bootstrap 5, jQuery, DataTables (quản lý bảng dữ liệu), Chart.js (vẽ biểu đồ báo cáo).
- **Phân quyền người dùng (Roles):**
  - `Admin`: Toàn quyền quyết định hệ thống (Quản lý User, Dịch vụ, Báo cáo...).
  - `Dentist` (Bác sĩ): Xem lịch hẹn, hồ sơ bệnh nhân của mình.
  - `Staff` (Nhân viên): Quản lý lịch hẹn, bệnh nhân, và thanh toán.

---

## 2. CHI TIẾT CÁC MODULE CHỨC NĂNG (Gợi ý Viết Test Case cho Katalon)

### 2.1. Module Xác thực & Quản lý Tài khoản cá nhân (Authentication)
*Đây là phần quan trọng nhất cần bao phủ Test Case bảo mật.*

- **Đăng nhập (Login):**
  - Người dùng nhập Email và Mật khẩu.
  - **Logic Khóa tài khoản (Lockout - Mới cập nhật):**
    - Nếu nhập sai mật khẩu **5 lần liên tiếp**, tài khoản sẽ bị khóa trong **24 giờ**.
    - Hệ thống có Auto-check: Khi người dùng gõ Email, hệ thống sẽ gọi API ngầm (`CheckLockoutStatus`) kiểm tra xem account đó có đang bị khóa hay không. Nếu đang khóa, vô hiệu hóa luôn nút Đăng nhập và báo thời gian mở khóa.
    - Hiển thị cảnh báo số lần nhập sai còn lại (Ví dụ: "Bạn còn 3 lần thử...").
  - **Logic Trạng thái Kích hoạt (IsActive):** 
    - Nếu `IsActive = false` (Admin chủ động khóa), user không thể đăng nhập.
  - **Test Cases trên Katalon:** 
    - TC_Login_Success, TC_Login_Fail_WrongPassword, TC_Login_Fail_Count_Exceeded_Lockout, TC_Login_Inactive_Account.

- **Đổi mật khẩu / Thông tin cá nhân:**
  - User đang đăng nhập có thể đổi Auth Info thông qua `Account/Profile` và `Account/ChangePassword`.

### 2.2. Module Quản lý Người Dùng (User Management) - *Chỉ Admin*
- **Xem danh sách User:** Sử dụng DataTables.
- **Tạo mới User:** Admin tạo tài khoản cho Staff/Dentist (Tính năng tự đăng ký (RegisterPublic) đã bị loại bỏ/ẩn khỏi người dùng ngoài để bảo mật).
- **Bật/Tắt trạng thái (Toggle IsActive):** Admin có quyền khóa tài khoản của nhân viên/bác sĩ (Nút hình ổ khóa). Khi khóa, user không thể truy cập hoặc đăng nhập.
- **Reset Mật Khẩu (Admin Reset Password):** Admin có thể click vào nút "Chìa khóa" để đặt lại mật khẩu mới cho bất kỳ User nào mà không cần xác nhận qua email (Bỏ qua luồng Forgot Password qua Email rườm rà).
- **Test Cases trên Katalon:**
  - TC_Admin_Create_User, TC_Admin_Toggle_LockUser, TC_Admin_Reset_Employee_Password.

### 2.3. Module Quản lý Bệnh Nhân (Patient Management)
- **Logic:** Nơi lưu trữ thông tin khách hàng đến khám (Tên, SĐT, Ngày sinh, Địa chỉ, Email...).
- Mọi thao tác Thêm (Create), Sửa (Update), Chi tiết (Details), Xóa (Delete) đều có.
- **Test Cases trên Katalon:**
  - Định danh các field bắt buộc nhập (Tên, Ngày sinh). 
  - Validate SDT và Email đúng định dạng.
  - Thao tác Xóa (cần bấm xác nhận Modal Cảnh báo).

### 2.4. Module Quản lý Dịch vụ (Service Management)
- **Logic:** Quản lý danh mục các dịch vụ nha khoa (Tẩy trắng răng, Nhổ răng, Niềng răng...).
- Thông tin: Tên dịch vụ, Giá tiền, Thời lượng dự kiến.
- Dịch vụ có trạng thái `IsActive` (Nếu ngưng cung cấp thì Disable đi thay vì xóa cứng để không ảnh hưởng dữ liệu lịch sử).

### 2.5. Module Quản lý Lịch hẹn (Appointment Management) *[Giao diện User/Staff]*
- **Logic:** Gắn kết `Patient` + `Service` + `Dentist` + `Thời gian đặt`.
- Lịch trình theo ngày.

### 2.6. Module Thanh Toán (Payment & Thu Ngân)
- **Logic:** Sau khi khám xong (Appointment hoàn thành), sẽ chuyển sang thanh toán.
- **Phương thức thanh toán:**
  1. **Tiền mặt (Cash):** Thu ngân xác nhận thu đủ tiền, hệ thống cập nhật DB.
  2. **MoMo QR Code:** Hệ thống tự động tạo QR Code MoMo động (Có số tiền và thông tin). QR Code có thời gian hết hạn (5 phút). Tích hợp logic mô phỏng (Simulate) để test do đang ở môi trường dev.
- **Test Cases trên Katalon:**
  - TC_Payment_Cash_Success, TC_Payment_MoMo_QR_Generate, TC_Payment_MoMo_Timeout (Test case đợi QR hết hạn).

### 2.7. Module Báo Cáo & Thống Kê (Dashboard / Reports)
- Hệ thống đã được tinh gọn, **chỉ xuất báo cáo Doanh Thu theo Tháng**.
- **Logic:**
  - User chọn "Năm" và "Tháng".
  - Thống kê **Tổng doanh thu** và **Tổng số lịch hẹn** trong tháng đó.
  - Biểu đồ (Chart.js) Bar Chart hiển thị doanh thu rải rác theo từng ngày trong tháng giúp cái nhìn trực quan.
- **Test Cases trên Katalon:**
  - Chuyển tháng và verify xem số liệu Tổng doanh thu có cập nhật đúng với dữ liệu test đã seed trong DB hay không.

---

## 3. HƯỚNG DẪN KIỂM THỬ TỰ ĐỘNG BẰNG KATALON STUDIO

Để team QC/QA áp dụng kịch bản bằng Katalon, hãy cấu trúc Repository kiểm thử như sau:

1. **Object Repository:** 
   - Mapping toàn bộ các Input, Button theo ID hoặc XPath. 
   - *Tip cho app này:* Hệ thống xài Bootstrap, các Element thường có cấu trúc rõ ràng. Ví dụ login luôn là `#emailInput`, `#loginSubmit`.

2. **Test Data:** 
   - Cần 1 tập dữ liệu (Excel/CSV) chứa sẵn 1 list User hợp lệ, User sai password, User bị khóa. Bơm vào cho Data-Driven Testing.

3. **Luồng Test quan trọng nhất cần làm ngay:**
   - **Luồng 1 (Security Auth):** Viết script lặp lại vòng lặp 5 lần điền sai mật khẩu -> Assert (Kiểm tra) xem thẻ `div#lockout-message` có hiện ra với Text báo "24 giờ" không -> Assert thẻ button có bị `disabled` không.
   - **Luồng 2 (Admin Role):** Login = Admin -> Vào URL `/Admin/Admin/UserManagement` -> Chọn 1 User test -> Đổi mật khẩu -> Đăng xuất -> Đăng nhập lại bằng user kia với mật khẩu mới.
   - **Luồng 3 (Payment):** Đi tới màn hình Thanh Toán -> Tạo 1 đơn Tiền mặt -> Modal hiện lên -> Click "Xác nhận" -> Đợi Page Reload -> Assert Toast message thành công.

---
*Tài liệu nội bộ - Cập nhật theo kiến trúc mới nhất áp dụng cơ chế tự động khóa và tinh giản hệ thống báo cáo.*
