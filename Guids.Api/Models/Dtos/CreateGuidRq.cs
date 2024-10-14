using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Guids.Api.Models.Dtos
{
    public class CreateGuidRq
    {
        [Required]
        public string User { get; set; }
        public DateTime? Expires { get; set; }
    
    }
}