INSERT INTO [phoneShopDB].[dbo].[Product]([Id], [SellerId], [Name], [CategoryId], [StoreId], [Description], ImgUrl,
											[Price], [CreatedAt], [IsActive])
VALUES 
(1, '0e9057bd-92d0-49f3-9606-c8489c5a8968', 'Samsung v1',1 ,1 , 'Samsung v1 Description', 'img_url', 12, GETDATE(), 0),
(2, '0e9057bd-92d0-49f3-9606-c8489c5a8968', 'Samsung v2',1, 1, 'Samsung v2 Description', 'img_url', 13, GETDATE(), 0) ;
