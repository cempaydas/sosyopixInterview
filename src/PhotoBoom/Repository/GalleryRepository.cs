using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhotoBoom.Contracts;
using PhotoBoom.Models;

namespace PhotoBoom.Repository
{
    public class GalleryRepository : IGalleryRepository
    {
        private readonly PhotoBoomContext _context;

        public GalleryRepository(PhotoBoomContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Gallery gallery)
        {
            _context.Add(gallery);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Gallery gallery)
        {
            _context.Gallery.Remove(gallery);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Gallery>> GetAllAsync()
        {
            return await _context.Gallery.ToListAsync();
        }

        public async Task<Gallery> GetItemAsync(int? id)
        {
            return await _context.Gallery
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}