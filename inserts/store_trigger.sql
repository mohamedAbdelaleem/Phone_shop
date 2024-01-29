CREATE TRIGGER ArchivingStores ON [BlogDb].[dbo].[Store]
INSTEAD OF DELETE AS
BEGIN

SET NOCOUNT ON;

UPDATE [BlogDb].[dbo].[Store] SET [Archived] = 1 WHERE [Id] IN (SELECT Id FROM deleted);

DELETE FROM [BlogDb].[dbo].[Product]
WHERE [StoreId] IN (SELECT Id FROM deleted);

END;
