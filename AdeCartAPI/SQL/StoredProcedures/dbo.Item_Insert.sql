CREATE PROCEDURE [dbo].[Item_Insert](
    @ItemName    NVARCHAR(450),
	@ItemPrice   smallmoney,
	@AvailableItem tinyint,
    @ItemDescription      NVarCHAR (450))
AS
BEGIN
    INSERT INTO Item(ItemName,ItemPrice,AvailableItem,ItemDescription) VALUES (@ItemName,@ItemPrice,@AvailableItem,@ItemPrice)
END
