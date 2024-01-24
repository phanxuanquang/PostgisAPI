using Newtonsoft.Json;
using PostgisAPI.DTO;
using PostgisUltilities;
using PostgisUltilities.Bounding_Boxes;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostgisAPI.Models
{
    [Table("modelitem")]
    public class ModelItem
    {
        [Column("id")]
        public int ID { get; set; }

        [Column("hierachyindex")]
        public int HierarchyIndex { get; set; }

        [Column("modelid")]
        public Guid ModelID { get; set; }

        [Column("modelitemid")]
        public Guid ModelItemID { get; set; } = Guid.NewGuid();

        [Column("parenthierachyindex")]
        public int ParentHierachyIndex { get; set; }

        [Column("displayname")]
        public string DisplayName { get; set; }
        [Column("path")]
        public string Path { get; set; }
        [Column("color", TypeName = "jsonb")]
        public string Color { get; set; }
        [Column("mesh", TypeName = "jsonb")]
        public string Mesh { get; set; }
        [Column("matrix", TypeName = "real[]")]
        public double[] Matrix { get; set; }
        [Column("aabb", TypeName = "jsonb")]
        public string? AABB { get; set; }
        [Column("batchedmodelitemid")]
        public Guid? BatchedModelItemID { get; set; }
        [Column("properties")]
        public string Properties { get; set; }
        [Column("lastmodifiedtime")]
        public DateTime LastModifiedTime { get; set; } = DateTime.Now;

        public ModelItemGetDTO AsDTO()
        {
            return new ModelItemGetDTO
            {
                ModelID = ModelID,
                ModelItemID = ModelItemID,
                HierarchyIndex = HierarchyIndex,
                ParentHierachyIndex = ParentHierachyIndex,
                DisplayName = DisplayName,
                Path = Path,
                Color = JsonConvert.DeserializeObject<Color>(Color),
                Mesh = JsonConvert.DeserializeObject<Mesh>(Mesh),
                Matrix = Matrix,
                AABB = JsonConvert.DeserializeObject<AxisAlignedBoundingBox>(AABB),
                BatchedModelItemID = BatchedModelItemID,
                Properties = Properties,
                LastModifiedTime = LastModifiedTime
            };
        }
    }
}
