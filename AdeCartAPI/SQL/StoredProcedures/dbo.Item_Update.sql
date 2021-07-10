CREATE PROCEDURE [dbo].[Item_Update](
    @ItemId      int,
    @ItemName    NVARCHAR(450),
	@ItemPrice   smallmoney,
	@AvailableItem tinyint,
    @ItemDescription      NVARCHAR (450))
AS
BEGIN
   UPDATE Item
   SET ItemName = ISNULL(@ItemName ,ItemName ),
   ItemPrice = ISNULL(@ItemPrice, ItemPrice),
   AvailableItem = ISNULL(@AvailableItem, AvailableItem),
   ItemDescription = ISNULL(@ItemDescription,ItemDescription)
   WHERE ItemId = @ItemId;
END
