CREATE DATABASE CompanyAPIDB;
GO
IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Companies] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Exchange] nvarchar(max) NOT NULL,
    [Ticker] nvarchar(max) NOT NULL,
    [Isin] nvarchar(12) NOT NULL,
    [Website] nvarchar(max) NULL,
    CONSTRAINT [PK_Companies] PRIMARY KEY ([Id])
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Exchange', N'Isin', N'Name', N'Ticker', N'Website') AND [object_id] = OBJECT_ID(N'[Companies]'))
    SET IDENTITY_INSERT [Companies] ON;
INSERT INTO [Companies] ([Id], [Exchange], [Isin], [Name], [Ticker], [Website])
VALUES (1, N'NASDAQ', N'US0378331005', N'Apple Inc.', N'AAPL', N'http://www.apple.com'),
(2, N'Pink Sheets', N'US1104193065', N'British Airways Plc', N'US1104193065', NULL),
(3, N'Euronext Amsterdam', N'NL0000009165', N'Heineken NV', N'HEIA', NULL),
(4, N'Tokyo Stock Exchange', N'JP3866800000', N'Panasonic Corp', N'6752', N'http://www.panasonic.co.jp'),
(5, N'Deutsche Börse', N'DE000PAH0038', N'Porsche Automobil', N'PAH3', N'https://www.porsche.com/');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Exchange', N'Isin', N'Name', N'Ticker', N'Website') AND [object_id] = OBJECT_ID(N'[Companies]'))
    SET IDENTITY_INSERT [Companies] OFF;

CREATE UNIQUE INDEX [IX_Companies_Isin] ON [Companies] ([Isin]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250215122130_SetUpDatabse', N'9.0.2');

COMMIT;
GO

