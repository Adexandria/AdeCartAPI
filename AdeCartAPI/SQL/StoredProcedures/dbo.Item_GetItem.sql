CREATE PROCEDURE [dbo].[Item_GetItem](
    @ItemId      int)
AS
BEGIN
   SELECT * FROM Item
   WHERE ItemId = @ItemId;
END
