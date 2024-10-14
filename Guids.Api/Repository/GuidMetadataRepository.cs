using Guids.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Guids.Api.Repository
{

    public class GuidMetadataRepository : IGuidMetadataRepository
    {
        private readonly ApplicationDbContext _context;

        public GuidMetadataRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GuidMetadata>> GetAllAsync()
        {
            return await _context.Guids.ToListAsync();
        }

        public async Task<GuidMetadata> GetByGuidIdAsync(string guid)
        {
            return await _context.Guids.AsNoTracking().FirstOrDefaultAsync(g => g.Guid == guid);
        }

        public async Task AddAsync(GuidMetadata guidMetadata)
        {
            await _context.Guids.AddAsync(guidMetadata);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(GuidMetadata guidMetadata)
        {
            _context.Guids.Update(guidMetadata);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string guid)
        {
            var guidMetadata = await _context.Guids.AsNoTracking().FirstOrDefaultAsync(g => g.Guid == guid);
            if (guidMetadata != null)
            {
                _context.Guids.Remove(guidMetadata);
                await _context.SaveChangesAsync();
            }
        }
    }
}