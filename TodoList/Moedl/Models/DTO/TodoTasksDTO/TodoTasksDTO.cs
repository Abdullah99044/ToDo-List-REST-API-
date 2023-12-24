﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models.DTO.TodoTasksDTO
{
    public class TodoTasksDTO
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
