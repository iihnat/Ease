using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guids.Api.Models
{
    public class ApiResponse<T> 
    {
        public T Value { get; set; }
        public ErrorResult Error { get; set; }
    }
}