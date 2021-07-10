CREATE PROCEDURE [dbo].[OrderCart_Update](
	@OrderCartId     INT,
	@UserId nvarchar(450),
	 @OrderStatus int) 
AS
BEGIN
   update OrderCart
   set OrderStatus = @OrderStatus
   WHERE OrderCartId = @OrderCartId and
   UserId = @UserId;
END