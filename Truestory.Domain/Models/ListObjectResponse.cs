using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truestory.Domain.Models
{
    public class ListObjectResponse
    {
        public string id { get; set; }
        public string name { get; set; }
        public dynamic data { get; set; }
    }
}
