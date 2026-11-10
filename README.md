# BÁO CÁO BÀI TẬP / ĐỒ ÁN

## THÔNG TIN SINH VIÊN
- **Họ và tên:** Nguyễn Khánh Đức
- **Mã số sinh viên:** 24810320072
- **Lớp:** D19QTANM1
- **Tên môn học:** Lập trình C# / Windows Forms
- **Tên bài tập:** BÀI 6.2: HỆ THỐNG QUẢN LÝ NHÂN SỰ & TÍNH LƯƠNG (HRM &
PAYROLL SYSTEM)

---

## KẾT QUẢ THỰC HÀNH

- Giao diện chính
<img width="1440" height="912" alt="Screenshot 2026-11-10 184320" src="https://github.com/user-attachments/assets/3a5db6f6-15ab-4a58-a67d-cc5e567935f9" />

- Giao diện tính lương nhân viên
<img width="1438" height="921" alt="Screenshot 2026-11-10 184353" src="https://github.com/user-attachments/assets/6a6d2dd8-e75d-4373-852c-8ee585dc9720" />

- Giao diện chốt lương thành công
<img width="1419" height="910" alt="Screenshot 2026-11-10 184406" src="https://github.com/user-attachments/assets/ebbac592-3f86-45dc-b4da-54974388652b" />

## HƯỚNG DẪN CÀI ĐẶT VÀ CHẠY DỰ ÁN:
1. **Khởi tạo CSDL**:
   - Mở file `Database.sql` có sẵn trong thư mục dự án bằng SQL Server Management Studio (SSMS) hoặc Visual Studio.
   - Chạy toàn bộ script (Execute) để tự động tạo CSDL `HRMPRDB` cùng các bảng và dữ liệu mẫu.

2. **Cấu hình chuỗi kết nối**:
   - Mở file `Dbhelper.cs`.
   - Kiểm tra và đổi lại `Data Source` cho khớp với tên SQL Server trên máy chấm (Mặc định đang để kết nối tới `(localdb)\MSSQLLocalDB`).

3. **Chạy chương trình**:
   - Nhấn **F5** (hoặc nút Start) trên Visual Studio để chạy hệ thống.
