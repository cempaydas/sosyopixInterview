using System.Collections.Generic;
using System.Threading.Tasks;
using PhotoBoom.Models;

namespace PhotoBoom.Contracts
{
    public interface IGalleryRepository
    {
        Task<IEnumerable<Gallery>> GetAllAsync();
        Task<Gallery> GetItemAsync(int? id);
        Task CreateAsync(Gallery gallery);
        Task DeleteAsync(Gallery gallery);
    }
}