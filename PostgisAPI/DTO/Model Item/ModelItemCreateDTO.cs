using PostgisUltilities;

namespace PostgisAPI.DTO
{
    public class ModelItemCreateDTO
    {
        public int HierarchyIndex { get; set; }
        public int ParentHierachyIndex { get; set; }
        public string DisplayName { get; set; }
        public Color Color { get; set; }
        public Mesh Mesh { get; set; }
        public double[]? Matrix { get; set; }
        public AxisAlignedBoundingBox? AABB { get; set; }
        public Guid? BatchedModelItemID { get; set; }
        public string Properties { get; set; }
        public int? FeatureID { get; set; }
        public int? GlbIndex { get; set; }
        public Models.ModelItem AsModelDB(Guid ModelID)
        {
            return new Models.ModelItem
            {
                ModelItemID = Guid.NewGuid(),
                HierarchyIndex = HierarchyIndex,
                ParentHierachyIndex = ParentHierachyIndex,
                ModelID = ModelID,
                BatchedModelItemID = BatchedModelItemID,
                DisplayName = DisplayName,
                Color = Color.AsJson(),
                Matrix = Matrix,
                Mesh = Mesh.AsJson(),
                Properties = Properties,
                LastModifiedTime = DateTime.Now,
                FeatureID = FeatureID,
                GlbIndex = GlbIndex,
            };
        }
    }
}
