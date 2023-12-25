using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class TodoTasks
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]

        public string Name { get; set; }

        [Required]
        public string status { get; set; }

        [Required]

        public DateTime Created { get; set; } = DateTime.Now;

        [Required]

        public DateTime Updated { get; set; } = DateTime.Now;


        [Required]

        public int todoListId { get; set; }

        public TodoList1 TodoList1 { get; set; }
    }
}
