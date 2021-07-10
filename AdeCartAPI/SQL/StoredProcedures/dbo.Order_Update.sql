CREATE PROCEDURE [dbo].[Order_Update](
   @OrderId    int,
   @ItemId     INT,
	@OrderCartId  INT,
	@Quantity     INT)
AS
BEGIN
   UPDATE [dbo].[Order]
   SET ItemId = @ItemId,
   Quantity = @Quantity
   WHERE OrderId = @OrderId and
    OrderCartId = @OrderCartId;
END