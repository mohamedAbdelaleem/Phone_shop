CREATE TRIGGER ArchivingProducts ON [phoneShopDB].[dbo].[Product]
INSTEAD OF DELETE AS
BEGIN

SET NOCOUNT ON;

UPDATE [phoneShopDB].[dbo].[Product] SET [Archived] = 1 WHERE [Id] IN (SELECT Id FROM deleted);

DELETE FROM [phoneShopDB].[dbo].[Review]
WHERE [ProductID] IN (SELECT Id FROM deleted);

DELETE FROM [phoneShopDB].[dbo].[ShoppingCartItems]
WHERE [ProductID] IN (SELECT Id FROM deleted);

END;
