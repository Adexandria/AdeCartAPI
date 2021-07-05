using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdeCartAPI.UserModel
{
    public class UserNumber
    {
        [Required]
        public string PhoneNumber { get; set; }
    }
}
