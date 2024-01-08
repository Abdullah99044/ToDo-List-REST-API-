using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Models.DTO.ToDoListsDTO
{
    public class UpdateToDoListDTO
    {

        [Key]

        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

 
    }
}
