using Newtonsoft.Json;

namespace PostgisUltilities
{
    public class AABB
    {
        public Point MinPoint { get; set; }
        public Point MaxPoint { get; set; }
        public AABB()
        {
            MinPoint = new Point();
            MaxPoint = new Point();
        }
        public string AsJson()
        {
            return JsonConvert.SerializeObject(this);
        }
        public bool Contains(Point point)
        {
            NetTopologySuite.Geometries.Point minPoint = MinPoint.AsGeometry();
            NetTopologySuite.Geometries.Point maxPoint = MaxPoint.AsGeometry();

            AxisAlignedBoundingBox aabb = new AxisAlignedBoundingBox(minPoint, maxPoint);

            return aabb.Contains(point.AsGeometry());
        }
    }
}
