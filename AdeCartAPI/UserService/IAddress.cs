using AdeCartAPI.UserModel;
using System.Threading.Tasks;

namespace AdeCartAPI.UserService
{
    public interface  IAddress
    {
        Task CreateAddress(UserAddress userAddress);
        Task UpdateAddress(UserAddress updatedAddress);
        Task DeleteAddress(int addressId);
        int GetAddress(int addressId);
        int GetAddressByUserId(string userId);

    }
}
