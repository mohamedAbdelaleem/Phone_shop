CREATE TRIGGER ArchivingProducts ON [BlogDb].[dbo].[Product]
INSTEAD OF DELETE AS
BEGIN

SET NOCOUNT ON;

UPDATE [BlogDb].[dbo].[Product] SET [Archived] = 1 WHERE [Id] IN (SELECT Id FROM deleted);

DELETE FROM [BlogDb].[dbo].[Review]
WHERE [ProductID] IN (SELECT Id FROM deleted);

DELETE FROM [BlogDb].[dbo].[ShoppingCartItems]
WHERE [ProductID] IN (SELECT Id FROM deleted);

END;
