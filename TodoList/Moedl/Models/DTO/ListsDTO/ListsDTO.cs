using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models.DTO.ListsDTO
{
    public class ListsDTO
    {


        [Required]

        public int Id { get; set; }

        [Required]

        [MaxLength(20)]
        public string name { get; set; }


        [MaxLength(100)]
        public string description { get; set; }

        public string email { get; set; }


    }
}
