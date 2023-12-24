using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models.DTO.ToDoListsDTO
{
    public class TodoListDTO
    {
 

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }


        [Required]

        public string Priority { get; set; }

         

        [Required]

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
