using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PostgisUltilities
{
    public class ModelItemDB
    {
        public int ID { get; set; }
        public int HierarchyIndex { get; set; }
        public Guid ModelID { get; set; }
        public Guid ModelItemID { get; }
        public string DisplayName { get; set; }
        public string Path { get; set; }
        public string Color { get; set; }
        public string Mesh { get; set; }
        public double[] Matrix { get; set; }
        public string AABB { get; set; }
        public Guid BatchedModelItemID { get; set; }
        public string Properties { get; set; }
        public DateTime LastModifiedTime { get; }
        public ModelItemDB(Guid ModelID, int HierarchyIndex, string DisplayName, string Path, Color Color, Mesh Mesh, double[] Matrix, AxisAlignedBoundingBox AABB, string Properties, Guid BatchedModelItemID)
        {
            try
            {
                if (Matrix.Length != 16)
                {
                    MessageBox.Show("The Matrix must contain 16 items exactly", "Invalid Matrix", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                JsonConvert.DeserializeObject(Properties);
            }
            catch (JsonException)
            {
                MessageBox.Show("The Properties must be a JSON", "Invalid Properties", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            ModelItemID = Guid.NewGuid();
            LastModifiedTime = DateTime.Now;

            this.ModelID = ModelID;
            this.HierarchyIndex = HierarchyIndex;
            this.DisplayName = DisplayName;
            this.Path = Path;
            this.Matrix = Matrix;
            this.Properties = Properties;
            this.BatchedModelItemID = BatchedModelItemID;

            this.Color = JsonConvert.SerializeObject(Color);
            this.Mesh = JsonConvert.SerializeObject(Mesh);
            this.AABB = JsonConvert.SerializeObject(AABB);
        }
    }
    public class Mesh
    {
        public List<Vertex> vertices { get; set; }
        public List<int> faceIndexes { get; set; }
        public Mesh()
        {
            vertices = new List<Vertex>();
            faceIndexes = new List<int>();
        }
    }
    public class Vertex
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public Vertex()
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
    public class AABB
    {
        public Vertex MinPoint { get; set; }
        public Vertex MaxPoint { get; set; }
        public AABB()
        {
            MinPoint = new Vertex();
            MaxPoint = new Vertex();
        }

    }
}
