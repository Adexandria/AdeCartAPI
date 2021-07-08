CREATE PROCEDURE [dbo].[Order_Insert](
	@ItemId     INT,
	@OrderCartId  INT)
AS
BEGIN
  INSERT INTO [dbo].[Order](ItemId,OrderCartId) VALUES(@ItemId,@OrderCartId)
END