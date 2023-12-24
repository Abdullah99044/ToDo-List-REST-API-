using Model.Models.DTO.ToDoListsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Fixtures
{
    public static class ListsFixtures
    {
        public static List<CreateToDoListDTO> GetTestLists() => new() {


            new CreateToDoListDTO
            {
                Name = "List 1",

                Priority    = "Important"
            },

            new CreateToDoListDTO
            {
                Name = "List 2",

                Priority    = "Important"
            },


            new CreateToDoListDTO
            {
                Name = "List 3",

                Priority    = "Important"
            }


        };
    }
}
