CREATE PROCEDURE [dbo].[Order_GetOrder](
	@OrderId     INT)
AS
BEGIN
   SELECT * FROM [dbo].[Order]
   WHERE OrderId = @OrderId;
END