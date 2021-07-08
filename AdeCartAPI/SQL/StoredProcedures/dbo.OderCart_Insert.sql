CREATE PROCEDURE [dbo].[OrderCart_Insert](
	@UserId nvarchar(450),
	 @OrderStatus int)
AS
BEGIN
    insert into OrderCart(UserId, OrderStatus) VALUES(@UserId,@OrderStatus);

END