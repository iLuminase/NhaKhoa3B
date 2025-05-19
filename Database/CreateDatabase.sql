-- Tạo cơ sở dữ liệu
CREATE DATABASE IF NOT EXISTS DentalDB;
USE DentalDB;

-- Tạo bảng Users
CREATE TABLE Users (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Username VARCHAR(50) NOT NULL UNIQUE,
    Password VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    FullName VARCHAR(100) NOT NULL,
    Role VARCHAR(20) NOT NULL,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Tạo bảng Patients
CREATE TABLE Patients (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    FullName VARCHAR(100) NOT NULL,
    DateOfBirth DATE NOT NULL,
    Gender VARCHAR(10) NOT NULL,
    PhoneNumber VARCHAR(20) NOT NULL,
    Email VARCHAR(100),
    Address VARCHAR(200),
    MedicalHistory TEXT,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Tạo bảng Services
CREATE TABLE Services (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Description TEXT,
    Price DECIMAL(18,2) NOT NULL,
    Duration INT NOT NULL, -- Thời gian thực hiện (phút)
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Tạo bảng Appointments
CREATE TABLE Appointments (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    PatientId INT NOT NULL,
    ServiceId INT NOT NULL,
    DoctorId INT NOT NULL,
    AppointmentDate DATETIME NOT NULL,
    Status VARCHAR(20) NOT NULL, -- Pending, Confirmed, Completed, Cancelled
    Notes TEXT,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (PatientId) REFERENCES Patients(Id),
    FOREIGN KEY (ServiceId) REFERENCES Services(Id),
    FOREIGN KEY (DoctorId) REFERENCES Users(Id)
);

-- Tạo bảng Treatments
CREATE TABLE Treatments (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    AppointmentId INT NOT NULL,
    Diagnosis TEXT NOT NULL,
    TreatmentPlan TEXT NOT NULL,
    Notes TEXT,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (AppointmentId) REFERENCES Appointments(Id)
);

-- Tạo bảng Payments
CREATE TABLE Payments (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    AppointmentId INT NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    PaymentDate DATETIME NOT NULL,
    PaymentMethod VARCHAR(50) NOT NULL,
    Status VARCHAR(20) NOT NULL, -- Pending, Completed, Refunded
    Notes TEXT,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (AppointmentId) REFERENCES Appointments(Id)
);

-- Thêm dữ liệu mẫu cho bảng Users
INSERT INTO Users (Username, Password, Email, FullName, Role)
VALUES 
('admin', 'admin123', 'admin@dental.com', 'Administrator', 'Admin'),
('doctor1', 'doctor123', 'doctor1@dental.com', 'Dr. John Doe', 'Doctor'),
('doctor2', 'doctor123', 'doctor2@dental.com', 'Dr. Jane Smith', 'Doctor');

-- Thêm dữ liệu mẫu cho bảng Services
INSERT INTO Services (Name, Description, Price, Duration)
VALUES 
('Khám tổng quát', 'Khám và tư vấn sức khỏe răng miệng', 200000, 30),
('Cạo vôi răng', 'Làm sạch cao răng và mảng bám', 300000, 45),
('Trám răng', 'Trám răng sâu', 500000, 60),
('Nhổ răng', 'Nhổ răng khôn', 1000000, 90); 