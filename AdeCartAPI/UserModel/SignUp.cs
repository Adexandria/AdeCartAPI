using System.ComponentModel.DataAnnotations;

namespace AdeCartAPI.UserModel
{ 
    public class SignUp
    {
        [Required(ErrorMessage = "Enter Firstname"),StringLength(20)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Enter LastName"),StringLength(20)]
        public string LastName { get; set; }
        [Required(ErrorMessage ="Enter Username"),StringLength(10)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Enter Password")]
        public string Password { get; set; }  
        [Required(ErrorMessage = "Password not equal")]
        public string RetypePassword { get; set; }
        [Required(ErrorMessage = "Enter Email")]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
      
       
    }
}
