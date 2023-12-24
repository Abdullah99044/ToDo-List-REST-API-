using Model.Models.DTO.ListsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class pageResponse
    {



        public List<GetListDTO> lists { get; set; }

        public int currentPage { get; set; }

        public double pages { get; set; }

      
        
    }
}
