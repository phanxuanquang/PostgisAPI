using Newtonsoft.Json;
using System;

namespace PostgisUltilities
{
    public class ModelCreateDTO
    {
        public string DisplayName { get; set; }

        public string AsJson()
        {
            return JsonConvert.SerializeObject(this);
        }
        public Model AsModelDB()
        {
            return new Model()
            {
                ModelID = Guid.NewGuid(),
                DisplayName = DisplayName,
                LastModifiedTime = DateTime.Now
            };
        }
    }
}
