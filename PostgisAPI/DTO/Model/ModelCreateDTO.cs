using PostgisUltilities.Bounding_Boxes;

namespace PostgisAPI.DTO.Model
{
    public class ModelCreateDTO
    {
        public string DisplayName { get; set; }
        public AxisAlignedBoundingBox? AABB { get; set; }
        public Models.Model AsModelDB()
        {
            return new Models.Model()
            {
                ModelID = Guid.NewGuid(),
                DisplayName = DisplayName,
                AABB = AABB.AsJson(),
                LastModifiedTime = DateTime.Now
            };
        }
    }
}
