using System.ComponentModel.DataAnnotations;

namespace AdeCartAPI.UserModel
{
    public class UserName
    {
        [Required]
        public string Username { get; set; }
    }
}
