
-- ========================================
-- TẠO DATABASE
-- ========================================
CREATE DATABASE BookBorrowingSystem;
GO

USE BookBorrowingSystem;
GO

-- ========================================
-- TẠO BẢNG ACCOUNTS
-- ========================================
CREATE TABLE Accounts (
    AccountId INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(MAX) NOT NULL,
    Email NVARCHAR(MAX) NOT NULL,
    Password NVARCHAR(MAX) NOT NULL,
    Role NVARCHAR(MAX) NOT NULL
);
GO

-- ========================================
-- TẠO BẢNG BOOKS
-- ========================================
CREATE TABLE Books (
    BookId INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(MAX) NOT NULL,
    Author NVARCHAR(MAX) NOT NULL,
    Quantity INT NOT NULL,
    Available INT NOT NULL,
    ImagePath NVARCHAR(MAX)
);
GO

-- ========================================
-- TẠO BẢNG BORROWREQUESTS
-- ========================================
CREATE TABLE BorrowRequests (
    RequestId INT IDENTITY(1,1) PRIMARY KEY,
    BookId INT NOT NULL,
    AccountId INT NOT NULL,
    RequestDate DATETIME2 NOT NULL,
    BorrowDate DATETIME2 NOT NULL,
    ReturnDate DATETIME2 NOT NULL,
    Status NVARCHAR(MAX) NOT NULL,
    ProcessedById INT NULL,

    CONSTRAINT FK_BorrowRequests_Account FOREIGN KEY (AccountId) REFERENCES Accounts(AccountId) ON DELETE CASCADE,
    CONSTRAINT FK_BorrowRequests_Book FOREIGN KEY (BookId) REFERENCES Books(BookId) ON DELETE CASCADE,
    CONSTRAINT FK_BorrowRequests_ProcessedBy FOREIGN KEY (ProcessedById) REFERENCES Accounts(AccountId)
);
GO

-- ========================================
-- TẠO INDEX
-- ========================================
CREATE INDEX IX_BorrowRequests_AccountId ON BorrowRequests(AccountId);
CREATE INDEX IX_BorrowRequests_BookId ON BorrowRequests(BookId);
CREATE INDEX IX_BorrowRequests_ProcessedById ON BorrowRequests(ProcessedById);
GO

-- ========================================
-- DỮ LIỆU MẪU: ACCOUNTS
-- ========================================
INSERT INTO Accounts (Username, Email, Password, Role) VALUES
(N'admin', N'admin@library.com', N'123456', N'Admin'),
(N'librarian', N'lib@gmail.com', N'123', N'Librarian'),
(N'jane_smith', N'jane@example.com', N'qwerty', N'User');
GO

-- ========================================
-- DỮ LIỆU MẪU: BOOKS
-- ========================================
INSERT INTO Books (Title, Author, Quantity, Available, ImagePath) VALUES
(N'Harry Potter and the Sorcerer''s Stone', N'J.K. Rowling', 5, 3, '/images/ThienDuongTungTang.jpg'),
(N'To Kill a Mockingbird', N'Harper Lee', 4, 4, '/images/ThienDuongTungTang.jpg');

GO

-- ========================================
-- DỮ LIỆU MẪU: BORROWREQUESTS
-- ========================================
INSERT INTO BorrowRequests (BookId, AccountId, RequestDate, BorrowDate, ReturnDate, Status, ProcessedById) VALUES
(1, 2, GETDATE(), DATEADD(DAY, 1, GETDATE()), DATEADD(DAY, 7, GETDATE()), N'Borrowed', 2),
(2, 3, GETDATE(), DATEADD(DAY, 2, GETDATE()), DATEADD(DAY, 9, GETDATE()), N'Pending', NULL);
GO
