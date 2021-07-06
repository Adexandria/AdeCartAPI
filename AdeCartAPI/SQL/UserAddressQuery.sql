﻿CREATE TABLE [dbo].[UserAddress](
 [AddressId] INT IDENTITY(1,1) NOT NULL,
 [AddressBox1] NVARCHAR(20) NOT NULL,
 [AddressBox2] NVARCHAR(20) NULL,
 [UserId] NVARCHAR (450) NOT NULL
 CONSTRAINT [PK_UserAddress] PRIMARY KEY CLUSTERED ([AddressId] ASC),
 CONSTRAINT [FK_UserAddress_AspNetUsers_Id] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);