using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truestory.Domain.Models
{
    public class UpdateDevice
    {
        [JsonProperty("name")]
        public string name { get; set; }
        public UpdateDeviceProperties data { get; set; }
    }
}
