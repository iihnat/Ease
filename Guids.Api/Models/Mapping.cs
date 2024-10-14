using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guids.Api.Models.Dtos;
using Guids.Data.Models;

namespace Guids.Api.Models
{
    public static class MappingExtensions
    {
        public static GuidInfo MapToGuidInfo(this GuidMetadata entity)
        {
            return new GuidInfo
            {
                Guid = entity.Guid,
                User = entity.User,
                Expires = entity.Expires
            };
        }
    }
}