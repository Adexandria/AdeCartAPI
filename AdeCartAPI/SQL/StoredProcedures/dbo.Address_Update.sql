CREATE PROCEDURE [dbo].[Address_Update](
    @AddressBox1    VARCHAR(120),
	@UserId       NVARCHAR (450),
	@AddressId    INT)
AS
BEGIN
   UPDATE UserAddress
   SET AddressBox1 = @AddressBox1
   WHERE UserId = @UserId AND
   AddressId = @AddressId;
END