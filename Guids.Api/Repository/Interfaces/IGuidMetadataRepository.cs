using Guids.Data.Models;

namespace Guids.Api.Repository
{
    public interface IGuidMetadataRepository
    {
        Task<IEnumerable<GuidMetadata>> GetAllAsync();
        Task<GuidMetadata> GetByGuidIdAsync(string guid);
        Task AddAsync(GuidMetadata guidMetadata);
        Task UpdateAsync(GuidMetadata guidMetadata);
        Task DeleteAsync(string guid);
    }
}