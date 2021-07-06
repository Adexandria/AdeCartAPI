CREATE PROCEDURE [dbo].[Address_Delete]
	(@AddressId    INT)
AS
BEGIN
   DELETE UserAddress
   WHERE AddressId = @AddressId;
END