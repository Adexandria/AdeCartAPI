CREATE PROCEDURE [dbo].[Addresses_Insert](
    @AddressBox1    VARCHAR(120),
    @UserId      NVARCHAR (450))
AS
BEGIN
    INSERT INTO UserAddress(AddressBox1,UserId) VALUES (@AddressBox1,@UserId)
END