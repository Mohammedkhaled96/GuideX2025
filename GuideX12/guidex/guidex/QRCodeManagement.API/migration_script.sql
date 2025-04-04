CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    PRIMARY KEY (`MigrationId`)
);

START TRANSACTION;

SET @MigrationId = '20250306152406_InitialCreate';
SET @MigrationExists = 0;
SELECT COUNT(*) INTO @MigrationExists FROM `__EFMigrationsHistory` WHERE `MigrationId` = @MigrationId;

IF @MigrationExists = 0 THEN
    -- Create Users table
    CREATE TABLE IF NOT EXISTS `Users` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `username` longtext NOT NULL,
        `Email` longtext NOT NULL,
        `PasswordHash` longtext NOT NULL,
        PRIMARY KEY (`Id`)
    );

    -- Create Items table
    CREATE TABLE IF NOT EXISTS `Items` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `user_id` int NOT NULL,
        `name_EN` longtext NOT NULL,
        `description_EN` longtext NOT NULL,
        `color_EN` longtext NOT NULL,
        `name_AR` longtext NOT NULL,
        `description_AR` longtext NOT NULL,
        `color_AR` longtext NOT NULL,
        PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Items_Users_user_id` FOREIGN KEY (`user_id`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
    );

    -- Create QRCodes table
    CREATE TABLE IF NOT EXISTS `QRCodes` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `UserId` int NOT NULL,
        `ItemId` int NOT NULL,
        `Code` varchar(100) NOT NULL,
        `createdat` datetime NOT NULL,
        PRIMARY KEY (`Id`),
        CONSTRAINT `FK_QRCodes_Items_ItemId` FOREIGN KEY (`ItemId`) REFERENCES `Items` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_QRCodes_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
    );

    -- Create Scans table
    CREATE TABLE IF NOT EXISTS `Scans` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `user_id` int NOT NULL,
        `qr_code_id` int NOT NULL,
        `scan_time` datetime NOT NULL,
        PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Scans_Users_user_id` FOREIGN KEY (`user_id`) REFERENCES `Users` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_Scans_QRCodes_qr_code_id` FOREIGN KEY (`qr_code_id`) REFERENCES `QRCodes` (`Id`) ON DELETE CASCADE
    );

    -- Create indexes
    CREATE INDEX `IX_Items_user_id` ON `Items` (`user_id`);
    CREATE INDEX `IX_QRCodes_ItemId` ON `QRCodes` (`ItemId`);
    CREATE INDEX `IX_QRCodes_UserId` ON `QRCodes` (`UserId`);
    CREATE INDEX `IX_Scans_user_id` ON `Scans` (`user_id`);
    CREATE INDEX `IX_Scans_qr_code_id` ON `Scans` (`qr_code_id`);

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES (@MigrationId, '8.0.0');
END IF;

COMMIT;