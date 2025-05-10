using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truestory.Domain.Models
{
    public class UpdateDeviceProperties
    {
        [JsonProperty("year")]
        public int year { get; set; }
        [JsonProperty("price")]
        public double price { get; set; }

        [JsonProperty("CPU model")]
        public string CPUmodel { get; set; }

        [JsonProperty("Hard disk size")]
        public string Harddisksize { get; set; }
        [JsonProperty("color")]
        public string color { get; set; }

    }
}
