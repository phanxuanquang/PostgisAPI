using Newtonsoft.Json;
using PostgisAPI.Models.Supporters;
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
        public Guid ModelID { get; set; } = Guid.NewGuid();

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
        [Column("properties", TypeName = "jsonb")]
        public string Properties { get; set; }
        [Column("lastmodifiedtime")]
        public DateTime LastModifiedTime { get; set; } = DateTime.Now;

        public bool Contains(PointZ point)
        {
            AxisAlignedBoundingBox aabb = JsonConvert.DeserializeObject<AxisAlignedBoundingBox>(AABB);
            return aabb.Contains(point);
        }
    }
}
