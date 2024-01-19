using System.ComponentModel.DataAnnotations.Schema;

namespace PostgisAPI.Models
{
    [Table("modelitem")]
    public class ModelItem
    {
        [Column("id")]
        public int ID { get; set; }
        [Column("hierarchyindex")]
        public int? HierarchyIndex { get; set; }

        [Column("modelid")]
        [ForeignKey("model")]
        public Guid ModelID { get; set; } = Guid.NewGuid();

        [Column("modelitemid")]
        public Guid ModelItemID { get; set; } = Guid.NewGuid();

        [Column("displayname")]
        public string? DisplayName { get; set; }
        [Column("path")]
        public string? Path { get; set; }
        [Column("color", TypeName = "jsonb")]
        public string? Color { get; set; }
        [Column("mesh", TypeName = "jsonb")]
        public string? Mesh { get; set; }
        [Column("matrix", TypeName = "real[]")]
        public double[]? Matrix { get; set; }
        [Column("aabb", TypeName = "jsonb")]
        public string? AABB { get; set; }
        [Column("batchedmodelitemid")]
        public Guid BatchedModelItemID { get; set; }
        [Column("properties", TypeName = "jsonb")]
        public string? Properties { get; set; }
        [Column("lastmodifiedtime")]
        public DateTime LastModifiedTime { get; set; } = DateTime.Now;
    }

    public class Mesh
    {
        public List<Point> vertices { get; set; }
        public List<int> faceIndexes { get; set; }
        public Mesh()
        {
            vertices = new List<Point>();
            faceIndexes = new List<int>();
        }
    }
    public class Point
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public Point()
        {
            x = y = z = 0;
        }
    }
    public class Color
    {
        public double a { get; set; }
        public double r { get; set; }
        public double g { get; set; }
        public double b { get; set; }
        public Color()
        {
            r = g = b = a = 0;
        }
    }
    public class AxisAlignedBoundingBox
    {
        public Point? MinPoint { get; set; }
        public Point? MaxPoint { get; set; }
        public AxisAlignedBoundingBox()
        {
            MinPoint = new Point();
            MaxPoint = new Point();
        }

    }
}
