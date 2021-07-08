CREATE TABLE [dbo].[Order](
[OrderId] int IDENTITY(1,1) NOT NULL,
[ItemId] int NOT NULL,
[OrderCartId] int  NOT NULL,
CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED ([OrderId] ASC),
CONSTRAINT [FK_Order_Item_ItemId] FOREIGN KEY ([ItemId]) REFERENCES [dbo].[Item] ([ItemId]) ON DELETE CASCADE,
CONSTRAINT [FK_Order_OrderCart_OrderCartId] FOREIGN KEY ([OrderCartId]) REFERENCES [dbo].[OrderCart] ([OrderCartId]) ON DELETE CASCADE
);