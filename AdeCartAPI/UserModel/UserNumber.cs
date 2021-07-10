using System.ComponentModel.DataAnnotations;

namespace AdeCartAPI.UserModel
{
    public class UserNumber
    {
        [Required]
        public string PhoneNumber { get; set; }
    }
}
