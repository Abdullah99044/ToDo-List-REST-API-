using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class TodoListsDTO
    {

        [Key]
        
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }


        [Required]

        public string Priority { get; set; }

        [Required]

        public int ListId { get; set; }

        public Lists Lists { get; set; }


        [Required]

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
 
