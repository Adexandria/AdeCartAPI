CREATE PROCEDURE [dbo].[Address_Insert](
    @AddressBox1    VARCHAR(120),
    @AddressBox2    VARCHAR(120),
    @UserId      NVARCHAR (450))
AS
BEGIN
    INSERT INTO UserAddress(AddressBox1,AddressBox2,UserId) VALUES (@AddressBox1,@AddressBox2,@UserId)
END
