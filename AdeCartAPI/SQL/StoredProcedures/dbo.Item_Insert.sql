CREATE PROCEDURE [dbo].[Item_Insert](
    @ItemName    NCHAR(100),
	@ItemPrice   smallmoney,
	@AvailableItem tinyint,
    @ItemDescription      NCHAR (300))
AS
BEGIN
    INSERT INTO Item(ItemName,ItemPrice,AvailableItem,ItemDescription) VALUES (@ItemName,@ItemPrice,@AvailableItem,@ItemPrice)
END
