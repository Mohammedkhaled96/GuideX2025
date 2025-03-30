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

    CREATE TABLE IF NOT EXISTS `Items` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `user_id` int NOT NULL,
        `name_EN` longtext NOT NULL,
        `description_EN` longtext NOT NULL,
        `color_EN` longtext NOT NULL,
        `name_AR` longtext NOT NULL,
        `description_AR` longtext NOT NULL,
        `color_AR` longtext NOT NULL,
        PRIMARY KEY (`Id`)
    );

    SET @columnExists = 0;
    SELECT COUNT(*) INTO @columnExists 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'Items' AND COLUMN_NAME = 'name_EN';
    
    IF @columnExists = 0 THEN
        SET @alterSQL = CONCAT('ALTER TABLE `Items` ADD COLUMN IF NOT EXISTS `name_EN` longtext NOT NULL;');
        PREPARE alterStmt FROM @alterSQL;
        EXECUTE alterStmt;
        DEALLOCATE PREPARE alterStmt;
        
        SET @alterSQL = CONCAT('ALTER TABLE `Items` ADD COLUMN IF NOT EXISTS `description_EN` longtext NOT NULL;');
        PREPARE alterStmt FROM @alterSQL;
        EXECUTE alterStmt;
        DEALLOCATE PREPARE alterStmt;
    END IF;
    
    CREATE TABLE IF NOT EXISTS `QRCodes` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `ItemId` int NOT NULL,
        `Code` varchar(100) NOT NULL,
        `CreatedAt` datetime NOT NULL,
        PRIMARY KEY (`Id`),
        CONSTRAINT `FK_QRCodes_Items_ItemId` FOREIGN KEY (`ItemId`) REFERENCES `Items` (`Id`) ON DELETE CASCADE
    );
    
    SET @indexExists = 0;
    SELECT COUNT(*) INTO @indexExists 
    FROM INFORMATION_SCHEMA.STATISTICS 
    WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'QRCodes' AND INDEX_NAME = 'IX_QRCodes_ItemId';
    
    IF @indexExists = 0 THEN
        CREATE INDEX `IX_QRCodes_ItemId` ON `QRCodes` (`ItemId`);
    END IF;
    
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES (@MigrationId, '8.0.0');
    
END IF;

COMMIT;
