CREATE PROCEDURE [dbo].[OrderCart_GetCarts](
	@UserId nvarchar(450))
AS
BEGIN
   SELECT * FROM [dbo].[OrderCart]
   WHERE UserId = @UserId;
END