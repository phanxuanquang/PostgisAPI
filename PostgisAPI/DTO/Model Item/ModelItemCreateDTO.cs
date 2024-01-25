using Newtonsoft.Json;
using PostgisAPI.Models;
using PostgisUltilities;
using PostgisUltilities.Bounding_Boxes;

namespace PostgisAPI.DTO
{
    public class ModelItemCreateDTO
    {
        public int HierarchyIndex { get; set; }
        public int ParentHierachyIndex { get; set; }
        public string DisplayName { get; set; }
        public string Path { get; set; }
        public Color Color { get; set; }
        public Mesh Mesh { get; set; }
        public double[]? Matrix { get; set; }
        public AxisAlignedBoundingBox? AABB { get; set; }
        public Guid? BatchedModelItemID { get; set; }
        public string Properties { get; set; }

        public ModelItem AsModelDB(Guid ModelID)
        {
            return new ModelItem
            {
                ModelItemID = Guid.NewGuid(),
                HierarchyIndex = HierarchyIndex,
                ParentHierachyIndex = ParentHierachyIndex,
                ModelID = ModelID,
                BatchedModelItemID = BatchedModelItemID,
                DisplayName = DisplayName,
                Path = Path,
                Color = Color.AsJson(),
                Matrix = Matrix,
                AABB = AABB.AsJson(),
                Mesh = Mesh.AsJson(),
                Properties = Properties,
                LastModifiedTime = DateTime.Now

            };
        }
    }
}
