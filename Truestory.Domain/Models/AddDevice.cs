using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Truestory.Domain.Models
{    
    public class AddDevice
    {
        public string name { get; set; }
        public AddDeviceProperties data { get; set; }
    }
}
