using System.Net;
using Guids.Api.Models;
using Guids.Api.Models.Dtos;
using Guids.Data.Models;
using Microsoft.Extensions.Caching.Distributed;
using Guids.Api.Repository;

namespace Guids.Api.Managers
{
    public class GuidManager : IGuidManager
    {
        private readonly IGuidMetadataRepository _guidMetadataRepository;
        private readonly IDistributedCache _cache;
        private readonly int _defaultExpirationDays;
        public GuidManager(IConfiguration configuration, IGuidMetadataRepository guidMetadataRepository, IDistributedCache cache)
        {
            _guidMetadataRepository = guidMetadataRepository;
            _cache = cache;
            _defaultExpirationDays = configuration.GetSection("GuidManagerSettings")?.GetValue<int?>("DefaultExpirationDays") ?? 30;
        }

        public async Task<ApiResponse<GuidInfo>> GetById(string id)
        {
            var guid = await _cache.GetAsync($"guids-{id}",
                async token =>
                {
                    var guid = await _guidMetadataRepository.GetByGuidIdAsync(id);

                    return guid;
                },
                CacheOptions.DefaultExpiration);
                
            if (guid == null)
            {
                return new ApiResponse<GuidInfo>()
                {
                    Error = new ErrorResult(HttpStatusCode.NotFound , "Guid not found.")
                };
            }
            else if (guid.Expires < DateTime.Now)
            {
                return new ApiResponse<GuidInfo>()
                {
                    Error = new ErrorResult(HttpStatusCode.NotFound , "Guid has expired.")
                };
            }

            var guidInfo = guid.MapToGuidInfo();

            return new ApiResponse<GuidInfo>()
            {
                Value = guid.MapToGuidInfo()
            };
        }
        public async Task<ApiResponse<bool>> Delete(string id)
        {
            var guid = await _guidMetadataRepository.GetByGuidIdAsync(id);

            if (guid is null)
            {
                return new ApiResponse<bool>()
                {
                    Error = new ErrorResult(HttpStatusCode.NotFound , "Guid not found.")
                };
            }

            await Task.WhenAll(
                _guidMetadataRepository.DeleteAsync(id), 
                _cache.RemoveAsync($"guids-{id}"));

            return new ApiResponse<bool>()
            {
                Value = true
            };
        }

        public async Task<ApiResponse<GuidInfo>> CreateGuid(CreateGuidRq createGuidRq)
        {
            var guid = new GuidMetadata()
            {
                Guid = Guid.NewGuid().ToString().ToUpper().Replace("-",""),
                User = createGuidRq.User,
                Expires = createGuidRq.Expires != null ? createGuidRq.Expires.Value : DateTime.UtcNow.AddDays(_defaultExpirationDays),
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            await _guidMetadataRepository.AddAsync(guid);
           
            return new ApiResponse<GuidInfo>()
            {
                Value = guid.MapToGuidInfo()
            };
        }

        public async Task<ApiResponse<GuidInfo>> UpdateGuidMetadata(string id, UpdateGuidMetadataRq updateGuidMetadataRq)
        {
            var guid = await _guidMetadataRepository.GetByGuidIdAsync(id);

            if (guid is null)
            {
                return new ApiResponse<GuidInfo>()
                {
                    Error = new ErrorResult(HttpStatusCode.NotFound , "Guid not found.")
                };
            }

            if (updateGuidMetadataRq.Expires != null)
            {
                guid.Expires = updateGuidMetadataRq.Expires.Value;
            }

            if (!string.IsNullOrEmpty(updateGuidMetadataRq.User))
            {
                guid.User = updateGuidMetadataRq.User;
            }

            guid.UpdatedDate = DateTime.UtcNow;

            await Task.WhenAll(
                _guidMetadataRepository.UpdateAsync(guid), 
                _cache.RemoveAsync($"guids-{id}"));

            return new ApiResponse<GuidInfo>()
            {
                Value = guid.MapToGuidInfo()
            };
        }
    }
}