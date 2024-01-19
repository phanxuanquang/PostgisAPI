using PostgisAPI.Models.Supporters;

namespace PostgisAPI
{
    public class ModelCreateDTO
    {
        public Guid ModelID { get; set; }
        public string? DisplayName { get; set; }
        public AxisAlignedBoundingBox? AABB { get; set; }
        public DateTime LastModifiedTime { get; set; }
    }
}
