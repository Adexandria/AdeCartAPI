CREATE PROCEDURE [dbo].[Item_Delete](
    @ItemId      int)
AS
BEGIN
   Delete Item
   WHERE ItemId = @ItemId;
END
