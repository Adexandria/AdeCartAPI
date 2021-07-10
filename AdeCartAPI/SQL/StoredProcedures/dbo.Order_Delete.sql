CREATE PROCEDURE [dbo].[Order_Delete](
	@OrderId     INT) 
AS
BEGIN
  DELETE [dbo].[Order]
  WHERE OrderId = @OrderId
END