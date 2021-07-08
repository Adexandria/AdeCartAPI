CREATE PROCEDURE [dbo].[Order_Delete](
    @UserId      NVARCHAR (450),
	@OrderId     INT,
	@ItemId       INT,
	@OrderStatus   INT)
AS
BEGIN
  DELETE [dbo].[Order]
  WHERE OrderId = @OrderId
END
