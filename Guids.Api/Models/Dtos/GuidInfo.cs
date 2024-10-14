using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guids.Api.Models.Dtos
{
    public class GuidInfo
    {
        public string Guid { get; set; }
        public string User { get; set; }
        public DateTime? Expires { get; set; }
    }
}