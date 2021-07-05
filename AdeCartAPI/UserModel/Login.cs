using System.ComponentModel.DataAnnotations;


namespace AdeCartAPI.UserModel
{
    public class Login
    {
        [Required(ErrorMessage = "Enter Username")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Enter Password")]
        public string Password { get; set; }
    }
}
