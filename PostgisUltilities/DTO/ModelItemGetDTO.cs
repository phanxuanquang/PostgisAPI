using Newtonsoft.Json;
using System;

namespace PostgisUltilities
{
    public class ModelItemGetDTO : ModelItemCreateDTO
    {
        public Guid ModelID { get; set; }
        public Guid ModelItemID { get; set; }
        public DateTime LastModifiedTime { get; set; }
        public string AsJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
