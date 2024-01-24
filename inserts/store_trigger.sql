CREATE TRIGGER ArchivingStores ON [phoneShopDB].[dbo].[Store]
INSTEAD OF DELETE AS
BEGIN

SET NOCOUNT ON;

UPDATE [phoneShopDB].[dbo].[Store] SET [Archived] = 1 WHERE [Id] IN (SELECT Id FROM deleted);

DELETE FROM [phoneShopDB].[dbo].[Product]
WHERE [StoreId] IN (SELECT Id FROM deleted);

END;
