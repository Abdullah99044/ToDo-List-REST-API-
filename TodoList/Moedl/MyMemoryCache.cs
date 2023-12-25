using Microsoft.Extensions.Caching.Memory;

namespace TodoList.Model
{
    public class MyMemoryCache
    {
        public MemoryCache Cache { get; } = new MemoryCache(
            new MemoryCacheOptions
            {
                SizeLimit = 30 * 1024 * 1024

            });


    }
}
