CREATE PROCEDURE [dbo].[Address_Get]
	(@AddressId int)
AS
BEGIN
    SELECT * FROM UserAddress 
	WHERE AddressId = @AddressId
END