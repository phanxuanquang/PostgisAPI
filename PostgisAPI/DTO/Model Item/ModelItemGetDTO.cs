using Newtonsoft.Json;
using PostgisAPI.Models;
using PostgisAPI.Models.Supporters;

namespace PostgisAPI.DTO
{
    public class ModelItemGetDTO : ModelItemCreateDTO
    {
        public Guid ModelID { get; set; }
        public Guid ModelItemID { get; set; }
        public DateTime LastModifiedTime { get; set; }
    }
}