using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Model.Models;
using TodoList.Model.DTO.ListsDTO;
using TodoList.Data;
using TodoList.DataAccess.Repositories;

namespace TodoList.Model.Services
{
    public class ListServices  : IListsServices
    {

  

        private readonly IMapper _mapper;

        private readonly MyMemoryCache _memoryCache;

        private readonly IListsRepo _repository;
        public ListServices(  IMapper mapper , MyMemoryCache memoryCache , IListsRepo repository) 
        {
        
            _mapper = mapper;
            _memoryCache = memoryCache;
            _repository = repository;
        } 




        //Store data in the cache memory
        private void cacheData<T>(string cacheKey, T cachedData)
        {
            //Set the size and the Expired time
            var cacheOptions = new MemoryCacheEntryOptions()
                                    .SetSize(1)
                                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            _memoryCache.Cache.Set(cacheKey, cachedData, cacheOptions);

        }



        //Get  all user lists
        public async  Task<List<GetListDTO>> GetUserLists(string UserId, string? listName, bool? sortingByLetters, int page  )
        {


            //How does the caching works?

            //The GetUserLists() checks the requested  data ( pageResponse ) inside inmemorey cache

            //If cache hit(data exist) then it returns the data from the cache memorey and making copy to filter it if filtring is not empty.

            //And sorting it if sorting  is true 

            //If cache miss(data doesn't exist ) then it request the data from the DataBase and filter it if filtring is not empty.

            //And sorting it if sorting  is true 

            //the Save() method delete the data in the memory cache if it was changed, removed or added to in the dataBase


            //Cache key for storing the cache
            var cacheKey = $"Key_{UserId}";

            // Check if data is in the in-memory cache
            if (_memoryCache.Cache.TryGetValue(cacheKey, out List<GetListDTO> cachedResponse))
            {
                // Make a deep copy of the cached response to filter and sort it with out effecting the sotred cahced data
                var filteredResponse = cachedResponse;

                //Filter and sort the cahced data
                filteredResponse = ApplyFiltering(  filteredResponse, listName);

                filteredResponse =  ApplySorting(  filteredResponse, sortingByLetters);

                return filteredResponse;
            }

            var query =  _repository.GetUserLists(UserId);

            // Execute the query and retrieve all data
            var allData =   query.Select(userList => _mapper.Map<GetListDTO>(userList)).ToList();

            // Cache the data
            cacheData(cacheKey, allData);

            //Filter and sort the requested data

            var filteerdData = ApplyFiltering(allData, listName);
                filteerdData = ApplySorting(allData, sortingByLetters);

            return filteerdData;
        }




        // Applying filtering  

        private  List<GetListDTO> ApplyFiltering(   List<GetListDTO>  ListDTO , string? listName)
        {
            if (string.IsNullOrWhiteSpace(listName))
            {
                return ListDTO;
            }
            return  ListDTO.Where(u => u.name.Contains(listName)).ToList();
          
        }

        // Applying sorting   
        private List<GetListDTO>  ApplySorting(List<GetListDTO>  ListDTO, bool? sortingByLetters)
        {
            if ( sortingByLetters != true )
            {
                return ListDTO;
            }

            return ListDTO =  ListDTO.OrderBy(p => p.name).ToList();
            
        }


        //Get the user id for to create a list or get his lists

        public async Task<string> GetUserId(string Email)
        {
            string data = await _repository.GetUserId(Email);

            if (data == null)
            {
                return null;
            }

            return data;
        }


        //Get list data with its id
        public async Task<Lists> GetAlist(int ListId)
        {
            return await _repository.GetAlist(ListId);

        }

        //Save changes after every action in the database
        public async Task removeCachedData( bool dataSavedSuccessfully , string userID )
        {

            if (dataSavedSuccessfully  == true)
            {

                //Remove the cached data when it's updated
                _memoryCache.Cache.Remove($"Key_{userID}");
               
            }
           
        }

       

        //Post a List into the data base 

        public async Task InsertlIST(string userId, ListsDTO Data)
        {
            //Mapping  
            var List = _mapper.Map<Lists>(Data);

            var saveData = await _repository.InsertlIST(userId, List);

            await removeCachedData(saveData, userId);
        }


        //Delete a list 
        public async Task DeleteList(Lists entity)
        {
            var saveData =  await  _repository.DeleteList(entity);

            await removeCachedData(saveData, entity.UserId);
        }


        //Update a list
        public async Task UpdateList(Lists entity)
        {
            var saveData =  await _repository.UpdateList(entity);

            await removeCachedData(saveData, entity.UserId);
        }

       
    }
}
