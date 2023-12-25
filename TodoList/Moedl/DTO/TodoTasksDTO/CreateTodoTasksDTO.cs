using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todolist.Model.DTO.TodoTasksDTO
{
    public class CreateTodoTasksDTO
    {

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

    }
}
