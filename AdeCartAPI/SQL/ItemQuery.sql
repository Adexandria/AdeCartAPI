CREATE TABLE [dbo].[Item](
 [ItemId] int IDENTITY(1,1) NOT NULL,
 [ItemName] nvarchar(450) NOT NULL,
 [ItemPrice] smallmoney NOT NULL,
 [ItemDescription] nvarchar(450) NOT NULL,
 [AvailableItem] tinyint NOT NULL,
 CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED ([ItemId] ASC)
);
INSERT INTO Item(ItemName,ItemPrice,ItemDescription,AvailableItem) 
   VALUES('Gucci Watch',2300,'This is a gucci watch original',3);