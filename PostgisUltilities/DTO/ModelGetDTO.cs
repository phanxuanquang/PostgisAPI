using Newtonsoft.Json;
using System;

namespace PostgisUltilities
{
    public class ModelGetDTO : ModelCreateDTO
    {
        public Guid ModelID { get; set; }
        public DateTime LastModifiedTime { get; set; }
        public string AsJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
