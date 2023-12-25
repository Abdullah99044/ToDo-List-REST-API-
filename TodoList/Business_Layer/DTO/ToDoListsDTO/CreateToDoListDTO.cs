using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Models.DTO.ToDoListsDTO
{
    public class CreateToDoListDTO
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public string color { get; set; }


    }
}
