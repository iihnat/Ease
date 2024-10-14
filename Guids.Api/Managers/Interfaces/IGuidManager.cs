using Guids.Api.Models;
using Guids.Api.Models.Dtos;

namespace Guids.Api.Managers
{
    public interface IGuidManager
    {
        Task<ApiResponse<GuidInfo>> GetById(string id);
        Task<ApiResponse<GuidInfo>> CreateGuid(CreateGuidRq createGuidRq);
        Task<ApiResponse<GuidInfo>> UpdateGuidMetadata(string id, UpdateGuidMetadataRq updateGuidMetadataRq);
        Task<ApiResponse<bool>> Delete(string id);
    }
}