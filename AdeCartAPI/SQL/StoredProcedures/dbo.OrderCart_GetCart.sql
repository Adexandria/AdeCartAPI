CREATE PROCEDURE [dbo].[OrderCart_GetCart](
	@OrderCartId     INT,
	@UserId nvarchar(450)) 
AS
BEGIN
   SELECT * FROM [dbo].[OrderCart]
   WHERE OrderCartId = @OrderCartId and
   UserId = @UserId;
END