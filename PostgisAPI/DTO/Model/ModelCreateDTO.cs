using PostgisAPI.Models.Supporters;

namespace PostgisAPI.DTO.Model
{
    public class ModelCreateDTO
    {
        public string DisplayName { get; set; }
        public AxisAlignedBoundingBox? AABB { get; set; }
    }
}
