DELETE FROM [dbo].[OrderHeaders]
DBCC CHECKIDENT ('[BooksStore].[dbo].[OrderHeaders]', RESEED, 0)

TRUNCATE TABLE [dbo].[ShoppingCarts]