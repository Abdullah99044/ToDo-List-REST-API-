using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models.DTO.ListsDTO
{
    public class UpdateListDTO
    {

        [Key]
        public int id { get; set; }


        [Required]

        [MaxLength(20)]
        public string name { get; set; }

    }
}
