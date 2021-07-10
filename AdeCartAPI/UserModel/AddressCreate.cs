using System.ComponentModel.DataAnnotations;


namespace AdeCartAPI.UserModel
{
    public class AddressCreate
    {
        [Required]
        public string AddressBox1 { get; set; }
    }
}
