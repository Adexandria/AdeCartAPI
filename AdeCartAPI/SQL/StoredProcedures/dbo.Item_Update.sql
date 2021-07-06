CREATE PROCEDURE [dbo].[Item_Update](
    @ItemId      int,
    @ItemName    NCHAR(100),
	@ItemPrice   smallmoney,
	@AvailableItem tinyint,
    @ItemDescription      NCHAR (300))
AS
BEGIN
   UPDATE Item
   SET ItemName = ISNULL(@ItemName ,ItemName ),
   ItemPrice = ISNULL(@ItemPrice, ItemPrice),
   AvailableItem = ISNULL(@AvailableItem, AvailableItem),
   ItemDescription = ISNULL(@ItemDescription,ItemDescription)
   WHERE ItemId = @ItemId;
END
