using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class Lists
    {
        [Key]
        public int id { get; set; }


        [Required]

        [MaxLength(20)]
        public string name { get; set; }


        [MaxLength(100)]
        public string description { get; set; }


        [Required]

        public string UserId { get; set; }

        public Users Users { get; set; }

        public ICollection<TodoList1> TodoLists { get; set; }


        [Required]

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
