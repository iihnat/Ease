using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guids.Api.Models.Dtos
{
    public class UpdateGuidMetadataRq
    {
        public string User { get; set; }
        public DateTime? Expires { get; set; }
    }
}