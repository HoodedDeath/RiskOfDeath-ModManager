using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RiskOfDeath_ModManager
{
    public class Profile
    {
        [JsonProperty("name")]
        public string Name { get; set; } = "NULL PROFILE";

        [JsonProperty("mods")]
        public List<string> Mods { get; set; } = new List<string>();
    }
}
