CREATE PROCEDURE [dbo].[Address_GetByUserId]
	(@UserId    NVARCHAR(450))
AS
BEGIN
   SELECT * FROM UserAddress
   WHERE UserId = @UserId;
END