CREATE TABLE [dbo].[OrderCart](
[OrderCartId] int IDENTITY(1,1) NOT NULL,
[UserId] NVARCHAR (450) NOT NULL,
[OrderStatus] int  NOT NULL,
CONSTRAINT [PK_OrderCart] PRIMARY KEY CLUSTERED ([OrderCartId] ASC),
CONSTRAINT [FK_Order_AspNetUsers_Id] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);