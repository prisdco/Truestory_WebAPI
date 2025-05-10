using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Truestory.Domain.Models
{
    public class AddObjectResponse
    {
        public string id { get; set; }
        public string name { get; set; }
        public dynamic data { get; set; }
        public DateTime createdAt { get; set; }
    }
}
