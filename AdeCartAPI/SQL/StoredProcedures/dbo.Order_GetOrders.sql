CREATE PROCEDURE [dbo].[Order_GetOrders](
    @OrderCartId   int) 
AS
BEGIN
   SELECT * FROM [dbo].[Order]
   WHERE OrderCartId = @OrderCartId;
END