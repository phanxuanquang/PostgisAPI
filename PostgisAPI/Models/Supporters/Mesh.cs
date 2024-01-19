namespace PostgisAPI.Models.Supporters
{
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
}
