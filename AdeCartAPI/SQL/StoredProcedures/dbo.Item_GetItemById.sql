CREATE PROCEDURE [dbo].[Item_GetItemById](
    @ItemId     int)
AS
BEGIN
   SELECT * FROM Item
   WHERE ItemId = @ItemId;
END