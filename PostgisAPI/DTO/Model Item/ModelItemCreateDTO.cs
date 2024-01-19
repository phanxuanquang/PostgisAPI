using PostgisAPI.Models.Supporters;

namespace PostgisAPI.DTO
{
    public class ModelItemCreateDTO
    {
        public Guid ModelItemID { get; set; }
        public int? HierarchyIndex { get; set; }
        public string? DisplayName { get; set; }
        public string? Path { get; set; }
        public Color? Color { get; set; }
        public Mesh? Mesh { get; set; }
        public double[]? Matrix { get; set; }
        public AxisAlignedBoundingBox? AABB { get; set; }
        public Guid BatchedModelItemID { get; set; }
        public string? Properties { get; set; }
    }
}
