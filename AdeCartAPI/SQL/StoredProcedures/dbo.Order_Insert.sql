CREATE PROCEDURE [dbo].[Order_Insert](
	@ItemId     INT,
	@OrderCartId  INT,
	@Quantity     INT)
AS
BEGIN
  INSERT INTO [dbo].[Order](ItemId,OrderCartId,Quantity) VALUES(@ItemId,@OrderCartId,@Quantity);
END