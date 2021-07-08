CREATE PROCEDURE [dbo].[Order_Update](
   @OrderId    int,
   @ItemId     INT,
	@OrderCartId  INT)
AS
BEGIN
   UPDATE [dbo].[Order]
   SET ItemId = @ItemId
   WHERE OrderId = @OrderId and
    OrderCartId = @OrderCartId;
END