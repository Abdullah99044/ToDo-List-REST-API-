using TodoList.Model.DTO.ListsDTO;
 

namespace TodoList.Model.DTO
{
    public class pageResponse
    {



        public List<GetListDTO> lists { get; set; }

        public int currentPage { get; set; }

        public double pages { get; set; }



    }
}
