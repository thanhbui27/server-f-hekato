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
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230417152808_UserDB')
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230417152808_UserDB')
BEGIN
    CREATE TABLE [AspNetUsers] (
        [Id] uniqueidentifier NOT NULL,
        [firstName] nvarchar(max) NOT NULL,
        [lastName] nvarchar(max) NOT NULL,
        [type] nvarchar(max) NOT NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230417152808_UserDB')
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] uniqueidentifier NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230417152808_UserDB')
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] uniqueidentifier NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230417152808_UserDB')
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230417152808_UserDB')
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] uniqueidentifier NOT NULL,
        [RoleId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230417152808_UserDB')
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] uniqueidentifier NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230417152808_UserDB')
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230417152808_UserDB')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230417152808_UserDB')
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230417152808_UserDB')
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230417152808_UserDB')
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230417152808_UserDB')
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230417152808_UserDB')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230417152808_UserDB')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230417152808_UserDB', N'6.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230417154559_updatedb')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'type');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [AspNetUsers] DROP COLUMN [type];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230417154559_updatedb')
BEGIN
    ALTER TABLE [AspNetUsers] ADD [dob] datetime2 NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230417154559_updatedb')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230417154559_updatedb', N'6.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    ALTER TABLE [AspNetRoleClaims] DROP CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    ALTER TABLE [AspNetUserClaims] DROP CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    ALTER TABLE [AspNetUserLogins] DROP CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    ALTER TABLE [AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    ALTER TABLE [AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    ALTER TABLE [AspNetUserTokens] DROP CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    ALTER TABLE [AspNetUserTokens] DROP CONSTRAINT [PK_AspNetUserTokens];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    DROP INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    ALTER TABLE [AspNetUserLogins] DROP CONSTRAINT [PK_AspNetUserLogins];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    DROP INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    ALTER TABLE [AspNetUsers] DROP CONSTRAINT [PK_AspNetUsers];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    DROP INDEX [EmailIndex] ON [AspNetUsers];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    DROP INDEX [UserNameIndex] ON [AspNetUsers];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    ALTER TABLE [AspNetUserClaims] DROP CONSTRAINT [PK_AspNetUserClaims];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    DROP INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    ALTER TABLE [AspNetRoles] DROP CONSTRAINT [PK_AspNetRoles];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    DROP INDEX [RoleNameIndex] ON [AspNetRoles];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    ALTER TABLE [AspNetRoleClaims] DROP CONSTRAINT [PK_AspNetRoleClaims];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    DROP INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    EXEC sp_rename N'[AspNetUsers]', N'Users';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    EXEC sp_rename N'[AspNetUserClaims]', N'UserClaims';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    EXEC sp_rename N'[AspNetRoles]', N'Roles';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    EXEC sp_rename N'[AspNetRoleClaims]', N'RoleClaims';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUserTokens]') AND [c].[name] = N'Name');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUserTokens] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [AspNetUserTokens] ALTER COLUMN [Name] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUserTokens]') AND [c].[name] = N'LoginProvider');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUserTokens] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [AspNetUserTokens] ALTER COLUMN [LoginProvider] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUserLogins]') AND [c].[name] = N'ProviderKey');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUserLogins] DROP CONSTRAINT [' + @var3 + '];');
    ALTER TABLE [AspNetUserLogins] ALTER COLUMN [ProviderKey] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    DECLARE @var4 sysname;
    SELECT @var4 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUserLogins]') AND [c].[name] = N'LoginProvider');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUserLogins] DROP CONSTRAINT [' + @var4 + '];');
    ALTER TABLE [AspNetUserLogins] ALTER COLUMN [LoginProvider] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    DECLARE @var5 sysname;
    SELECT @var5 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'UserName');
    IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var5 + '];');
    ALTER TABLE [Users] ALTER COLUMN [UserName] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    DECLARE @var6 sysname;
    SELECT @var6 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'NormalizedUserName');
    IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var6 + '];');
    ALTER TABLE [Users] ALTER COLUMN [NormalizedUserName] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    DECLARE @var7 sysname;
    SELECT @var7 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'NormalizedEmail');
    IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var7 + '];');
    ALTER TABLE [Users] ALTER COLUMN [NormalizedEmail] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    DECLARE @var8 sysname;
    SELECT @var8 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'Email');
    IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var8 + '];');
    ALTER TABLE [Users] ALTER COLUMN [Email] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    DECLARE @var9 sysname;
    SELECT @var9 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Roles]') AND [c].[name] = N'NormalizedName');
    IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [Roles] DROP CONSTRAINT [' + @var9 + '];');
    ALTER TABLE [Roles] ALTER COLUMN [NormalizedName] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    DECLARE @var10 sysname;
    SELECT @var10 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Roles]') AND [c].[name] = N'Name');
    IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [Roles] DROP CONSTRAINT [' + @var10 + '];');
    ALTER TABLE [Roles] ALTER COLUMN [Name] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    ALTER TABLE [AspNetUserTokens] ADD CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    ALTER TABLE [AspNetUserLogins] ADD CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    ALTER TABLE [Users] ADD CONSTRAINT [PK_Users] PRIMARY KEY ([Id]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    ALTER TABLE [UserClaims] ADD CONSTRAINT [PK_UserClaims] PRIMARY KEY ([Id]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    ALTER TABLE [Roles] ADD CONSTRAINT [PK_Roles] PRIMARY KEY ([Id]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    ALTER TABLE [RoleClaims] ADD CONSTRAINT [PK_RoleClaims] PRIMARY KEY ([Id]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    CREATE TABLE [Category] (
        [CategoryId] int NOT NULL IDENTITY,
        [CategoryName] nvarchar(50) NOT NULL,
        CONSTRAINT [PK_Category] PRIMARY KEY ([CategoryId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    CREATE TABLE [Product] (
        [ProductId] int NOT NULL IDENTITY,
        [ProductName] nvarchar(128) NOT NULL,
        [Image_Url] nvarchar(max) NOT NULL,
        [PriceNew] decimal(18,2) NOT NULL,
        [PriceOld] decimal(18,2) NOT NULL,
        [ShortDetails] nvarchar(max) NOT NULL,
        [ProductDescription] nvarchar(max) NOT NULL,
        [dateAdd] datetime2 NULL,
        [CategoryId] int NOT NULL,
        CONSTRAINT [PK_Product] PRIMARY KEY ([ProductId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    CREATE TABLE [ProductImages] (
        [Id] int NOT NULL IDENTITY,
        [url_image] nvarchar(200) NOT NULL,
        [timeAdd] datetime2 NOT NULL,
        [ProductId] int NOT NULL,
        CONSTRAINT [PK_ProductImages] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ProductImages_Product_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Product] ([ProductId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    CREATE TABLE [ProductInCategory] (
        [ProductId] int NOT NULL,
        [CategoryId] int NOT NULL,
        CONSTRAINT [PK_ProductInCategory] PRIMARY KEY ([ProductId], [CategoryId]),
        CONSTRAINT [FK_ProductInCategory_Category_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Category] ([CategoryId]) ON DELETE CASCADE,
        CONSTRAINT [FK_ProductInCategory_Product_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Product] ([ProductId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    CREATE INDEX [IX_ProductImages_ProductId] ON [ProductImages] ([ProductId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    CREATE INDEX [IX_ProductInCategory_CategoryId] ON [ProductInCategory] ([CategoryId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230419190037_db-product')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230419190037_db-product', N'6.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230426141338_update-user-db')
BEGIN
    DECLARE @var11 sysname;
    SELECT @var11 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'firstName');
    IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var11 + '];');
    ALTER TABLE [Users] DROP COLUMN [firstName];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230426141338_update-user-db')
BEGIN
    EXEC sp_rename N'[Users].[lastName]', N'fullName', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230426141338_update-user-db')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230426141338_update-user-db', N'6.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230426170647_update-product')
BEGIN
    ALTER TABLE [Product] ADD [quantity] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230426170647_update-product')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230426170647_update-product', N'6.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230427040542_update-type-user')
BEGIN
    ALTER TABLE [Users] ADD [type] nvarchar(max) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230427040542_update-type-user')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230427040542_update-type-user', N'6.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230428023346_product-action-db')
BEGIN
    CREATE TABLE [ProductAction] (
        [Id] int NOT NULL IDENTITY,
        [ProductId] int NOT NULL,
        [NewArrival] bit NOT NULL,
        [BestSeller] bit NOT NULL,
        [Featured] bit NOT NULL,
        [SpecialOffer] bit NOT NULL,
        CONSTRAINT [PK_ProductAction] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230428023346_product-action-db')
BEGIN
    ALTER TABLE [Product] ADD CONSTRAINT [FK_Product_ProductAction_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [ProductAction] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230428023346_product-action-db')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230428023346_product-action-db', N'6.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230428024649_test')
BEGIN
    ALTER TABLE [Product] DROP CONSTRAINT [FK_Product_ProductAction_ProductId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230428024649_test')
BEGIN
    ALTER TABLE [Product] ADD CONSTRAINT [FK_Product_ProductAction_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [ProductAction] ([Id]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230428024649_test')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230428024649_test', N'6.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230428031128_bd-v2')
BEGIN
    ALTER TABLE [Product] DROP CONSTRAINT [FK_Product_ProductAction_ProductId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230428031128_bd-v2')
BEGIN
    CREATE UNIQUE INDEX [IX_ProductAction_ProductId] ON [ProductAction] ([ProductId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230428031128_bd-v2')
BEGIN
    ALTER TABLE [ProductAction] ADD CONSTRAINT [FK_ProductAction_Product_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Product] ([ProductId]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230428031128_bd-v2')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230428031128_bd-v2', N'6.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230428170355_update-product-action')
BEGIN
    ALTER TABLE [ProductAction] ADD [trendSmall] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230428170355_update-product-action')
BEGIN
    ALTER TABLE [ProductAction] ADD [trending] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230428170355_update-product-action')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230428170355_update-product-action', N'6.0.0');
END;
GO

COMMIT;
GO

