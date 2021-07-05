using AdeCartAPI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AdeCartAPI.UserModel
{
    public class UserAddress
    {
        [Key]
        public int AddressId { get; set; }
        public string AddressBox1 { get; set; }
        [ForeignKey("Id")]
        public string UserId { get; set; }
        public virtual User User {get;set;}

    }
}
