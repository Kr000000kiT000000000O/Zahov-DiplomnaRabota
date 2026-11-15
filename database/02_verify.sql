-- Изпълнете в SSMS след създаване на BikeStoreDb и първото стартиране на приложението.
USE BikeStoreDb;
GO
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME;
SELECT Id, Name FROM dbo.Categories ORDER BY Id;
SELECT Id, Brand, Name, SalePrice, DailyRentalPrice, StockQuantity FROM dbo.Bicycles ORDER BY Id;
SELECT Name FROM dbo.AspNetRoles ORDER BY Name;
GO
