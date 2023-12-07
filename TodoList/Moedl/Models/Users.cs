using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class Users : IdentityUser
    {


        [MaxLength(100)]
        public string FistName { get; set; }  

        [MaxLength(100)]

        public string LastName { get; set; }  


        public ICollection<Lists> Lists { get; set; }
    }
}
