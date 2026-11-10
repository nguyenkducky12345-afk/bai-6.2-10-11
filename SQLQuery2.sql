-- 1. Bảng Phòng Ban
CREATE TABLE PhongBan (
    MaPB INT IDENTITY(1,1) PRIMARY KEY,
    TenPB NVARCHAR(100) NOT NULL,
    Mota NVARCHAR(255)
);

-- 2. Bảng Nhân Viên
CREATE TABLE NhanVien (
    MaNV INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    NgaySinh DATETIME,
    CCCD VARCHAR(20) UNIQUE,
    MaPB INT FOREIGN KEY REFERENCES PhongBan(MaPB),
    LuongCoBan DECIMAL(18,2) CHECK (LuongCoBan > 0),
    HeSoLuong FLOAT DEFAULT 1.0
);

-- 3. Bảng Chấm Công
CREATE TABLE BangChamCong (
    MaCong INT IDENTITY(1,1) PRIMARY KEY,
    MaNV INT FOREIGN KEY REFERENCES NhanVien(MaNV),
    ThangNam VARCHAR(7), -- Ví dụ: '11/2026'
    SoNgayCong FLOAT CHECK (SoNgayCong <= 31),
    SoGioOT FLOAT DEFAULT 0,
    CONSTRAINT UQ_ChamCong UNIQUE (MaNV, ThangNam)
);

-- 4. Bảng Lương
CREATE TABLE BangLuong (
    MaLuong INT IDENTITY(1,1),
    MaNV INT FOREIGN KEY REFERENCES NhanVien(MaNV),
    ThangNam VARCHAR(7),
    ThucLinh DECIMAL(18,2),
    PRIMARY KEY (MaNV, ThangNam)
);

-- DỮ LIỆU MẪU ĐỂ TEST
INSERT INTO PhongBan (TenPB, Mota) VALUES 
(N'Phòng Kỹ Thuật', N'Phát triển phần mềm'),
(N'Phòng Nhân Sự', N'Tuyển dụng và tính lương');

INSERT INTO NhanVien (HoTen, NgaySinh, CCCD, MaPB, LuongCoBan, HeSoLuong) VALUES
(N'Nguyễn Văn A', '1995-05-15', '001234567890', 1, 10000000, 1.2),
(N'Trần Thị B', '1998-08-20', '001234567891', 2, 8000000, 1.0);