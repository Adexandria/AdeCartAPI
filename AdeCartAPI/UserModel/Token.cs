using System.ComponentModel.DataAnnotations;

namespace AdeCartAPI.Model
{
    public class Token
    {  
        [Required]
        public string GeneratedToken { get; set; }
    }
}