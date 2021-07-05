using AdeCartAPI.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdeCartAPI.UserService
{
    public interface  IAddress
    {
        Task CreateAddress(UserAddress userAddress);
        Task UpdateAddress(UserAddress updatedAddress);
        Task DeleteAddress(Guid addressId);
        
    }
}
