-- BIKESTORE - АЛТЕРНАТИВНА НАЧАЛНА БАЗА ДАННИ ЗА SQL SERVER / SSMS
-- Използвайте само ако НЕ изпълнявате Add-Migration + Update-Database върху същата БД.
-- Изпълнете в нова, празна инстанция/база за демонстрация. Скриптът НЕ изтрива съществуващи данни.
IF DB_ID(N'BikeStoreDb') IS NULL CREATE DATABASE BikeStoreDb;
GO
USE BikeStoreDb;
GO
-- ASP.NET Core Identity 8 - минимална съвместима структура
CREATE TABLE dbo.AspNetRoles (
 Id nvarchar(450) NOT NULL PRIMARY KEY,
 Name nvarchar(256) NULL,
 NormalizedName nvarchar(256) NULL,
 ConcurrencyStamp nvarchar(max) NULL
);
CREATE UNIQUE INDEX RoleNameIndex ON dbo.AspNetRoles(NormalizedName) WHERE NormalizedName IS NOT NULL;
GO
CREATE TABLE dbo.AspNetUsers (
 Id nvarchar(450) NOT NULL PRIMARY KEY,
 FullName nvarchar(100) NULL,
 UserName nvarchar(256) NULL,
 NormalizedUserName nvarchar(256) NULL,
 Email nvarchar(256) NULL,
 NormalizedEmail nvarchar(256) NULL,
 EmailConfirmed bit NOT NULL,
 PasswordHash nvarchar(max) NULL,
 SecurityStamp nvarchar(max) NULL,
 ConcurrencyStamp nvarchar(max) NULL,
 PhoneNumber nvarchar(max) NULL,
 PhoneNumberConfirmed bit NOT NULL,
 TwoFactorEnabled bit NOT NULL,
 LockoutEnd datetimeoffset NULL,
 LockoutEnabled bit NOT NULL,
 AccessFailedCount int NOT NULL
);
CREATE INDEX EmailIndex ON dbo.AspNetUsers(NormalizedEmail);
CREATE UNIQUE INDEX UserNameIndex ON dbo.AspNetUsers(NormalizedUserName) WHERE NormalizedUserName IS NOT NULL;
GO
CREATE TABLE dbo.AspNetRoleClaims (
 Id int IDENTITY(1,1) PRIMARY KEY,
 RoleId nvarchar(450) NOT NULL,
 ClaimType nvarchar(max) NULL,
 ClaimValue nvarchar(max) NULL,
 FOREIGN KEY (RoleId) REFERENCES dbo.AspNetRoles(Id) ON DELETE CASCADE
);
CREATE INDEX IX_AspNetRoleClaims_RoleId ON dbo.AspNetRoleClaims(RoleId);
GO
CREATE TABLE dbo.AspNetUserClaims (
 Id int IDENTITY(1,1) PRIMARY KEY,
 UserId nvarchar(450) NOT NULL,
 ClaimType nvarchar(max) NULL,
 ClaimValue nvarchar(max) NULL,
 FOREIGN KEY (UserId) REFERENCES dbo.AspNetUsers(Id) ON DELETE CASCADE
);
CREATE INDEX IX_AspNetUserClaims_UserId ON dbo.AspNetUserClaims(UserId);
GO
CREATE TABLE dbo.AspNetUserLogins (
 LoginProvider nvarchar(450) NOT NULL,
 ProviderKey nvarchar(450) NOT NULL,
 ProviderDisplayName nvarchar(max) NULL,
 UserId nvarchar(450) NOT NULL,
 CONSTRAINT PK_AspNetUserLogins PRIMARY KEY(LoginProvider,ProviderKey),
 FOREIGN KEY (UserId) REFERENCES dbo.AspNetUsers(Id) ON DELETE CASCADE
);
CREATE INDEX IX_AspNetUserLogins_UserId ON dbo.AspNetUserLogins(UserId);
GO
CREATE TABLE dbo.AspNetUserRoles (
 UserId nvarchar(450) NOT NULL,
 RoleId nvarchar(450) NOT NULL,
 CONSTRAINT PK_AspNetUserRoles PRIMARY KEY(UserId,RoleId),
 FOREIGN KEY (UserId) REFERENCES dbo.AspNetUsers(Id) ON DELETE CASCADE,
 FOREIGN KEY (RoleId) REFERENCES dbo.AspNetRoles(Id) ON DELETE CASCADE
);
CREATE INDEX IX_AspNetUserRoles_RoleId ON dbo.AspNetUserRoles(RoleId);
GO
CREATE TABLE dbo.AspNetUserTokens (
 UserId nvarchar(450) NOT NULL,
 LoginProvider nvarchar(450) NOT NULL,
 Name nvarchar(450) NOT NULL,
 Value nvarchar(max) NULL,
 CONSTRAINT PK_AspNetUserTokens PRIMARY KEY(UserId,LoginProvider,Name),
 FOREIGN KEY (UserId) REFERENCES dbo.AspNetUsers(Id) ON DELETE CASCADE
);
GO
-- DOMAIN TABLES (съответстват на Models и ApplicationDbContext)
CREATE TABLE dbo.Categories (
 Id int IDENTITY(1,1) PRIMARY KEY,
 Name nvarchar(80) NOT NULL,
 Description nvarchar(500) NULL
);
CREATE UNIQUE INDEX IX_Categories_Name ON dbo.Categories(Name);
GO
CREATE TABLE dbo.Bicycles (
 Id int IDENTITY(1,1) PRIMARY KEY,
 Name nvarchar(120) NOT NULL,
 Brand nvarchar(80) NOT NULL,
 Description nvarchar(1500) NULL,
 SalePrice decimal(18,2) NOT NULL,
 DailyRentalPrice decimal(18,2) NOT NULL,
 StockQuantity int NOT NULL,
 CanBeRented bit NOT NULL,
 ImageUrl nvarchar(500) NULL,
 CategoryId int NOT NULL,
 CONSTRAINT FK_Bicycles_Categories_CategoryId FOREIGN KEY(CategoryId) REFERENCES dbo.Categories(Id),
 CONSTRAINT CK_Bicycles_StockQuantity CHECK (StockQuantity >= 0),
 CONSTRAINT CK_Bicycles_SalePrice CHECK (SalePrice > 0)
);
CREATE INDEX IX_Bicycles_CategoryId_Name ON dbo.Bicycles(CategoryId,Name);
GO
CREATE TABLE dbo.Orders (
 Id int IDENTITY(1,1) PRIMARY KEY,
 ApplicationUserId nvarchar(450) NOT NULL,
 CreatedAtUtc datetime2 NOT NULL,
 Status int NOT NULL,
 TotalAmount decimal(18,2) NOT NULL,
 CONSTRAINT FK_Orders_AspNetUsers_ApplicationUserId FOREIGN KEY(ApplicationUserId) REFERENCES dbo.AspNetUsers(Id)
);
CREATE INDEX IX_Orders_ApplicationUserId ON dbo.Orders(ApplicationUserId);
GO
CREATE TABLE dbo.OrderItems (
 Id int IDENTITY(1,1) PRIMARY KEY,
 OrderId int NOT NULL,
 BicycleId int NOT NULL,
 Quantity int NOT NULL,
 UnitPrice decimal(18,2) NOT NULL,
 CONSTRAINT FK_OrderItems_Orders_OrderId FOREIGN KEY(OrderId) REFERENCES dbo.Orders(Id) ON DELETE CASCADE,
 CONSTRAINT FK_OrderItems_Bicycles_BicycleId FOREIGN KEY(BicycleId) REFERENCES dbo.Bicycles(Id)
);
CREATE INDEX IX_OrderItems_OrderId ON dbo.OrderItems(OrderId);
CREATE INDEX IX_OrderItems_BicycleId ON dbo.OrderItems(BicycleId);
GO
CREATE TABLE dbo.RentalReservations (
 Id int IDENTITY(1,1) PRIMARY KEY,
 BicycleId int NOT NULL,
 ApplicationUserId nvarchar(450) NOT NULL,
 StartDate datetime2 NOT NULL,
 EndDate datetime2 NOT NULL,
 TotalPrice decimal(18,2) NOT NULL,
 Status int NOT NULL,
 CONSTRAINT FK_RentalReservations_Bicycles_BicycleId FOREIGN KEY(BicycleId) REFERENCES dbo.Bicycles(Id),
 CONSTRAINT FK_RentalReservations_AspNetUsers_ApplicationUserId FOREIGN KEY(ApplicationUserId) REFERENCES dbo.AspNetUsers(Id),
 CONSTRAINT CK_Rentals_Dates CHECK (EndDate > StartDate)
);
CREATE INDEX IX_RentalReservations_ApplicationUserId ON dbo.RentalReservations(ApplicationUserId);
CREATE INDEX IX_RentalReservations_BicycleId_StartDate_EndDate ON dbo.RentalReservations(BicycleId,StartDate,EndDate);
GO
PRINT N'BikeStoreDb създадена. Стартирайте BikeStore за seed на роли и примерни продукти.';
GO
