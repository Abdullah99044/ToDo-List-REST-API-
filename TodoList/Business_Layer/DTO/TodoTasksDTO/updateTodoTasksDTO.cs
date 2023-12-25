using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todolist.Model.DTO.TodoTasksDTO
{
    public class updateTodoTasksDTO
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]

        public string Name { get; set; }

        [Required]
        public string status { get; set; }

    }
}
