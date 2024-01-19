namespace PostgisAPI.Models.Supporters
{
    public class Mesh
    {
        public List<PointZ> vertices { get; set; }
        public List<int> faceIndexes { get; set; }
        public Mesh()
        {
            vertices = new List<PointZ>();
            faceIndexes = new List<int>();
        }
    }
}
