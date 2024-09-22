DELETE FROM [dbo].[OrderHeaders]
DBCC CHECKIDENT ('[ArtAuctionManage].[dbo].[OrderHeaders]', RESEED, 0)

TRUNCATE TABLE [dbo].[ShoppingCarts]