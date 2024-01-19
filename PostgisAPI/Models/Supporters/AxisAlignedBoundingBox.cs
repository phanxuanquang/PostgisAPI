using NetTopologySuite.Geometries;

namespace PostgisAPI.Models.Supporters
{
    public class AxisAlignedBoundingBox
    {
        public PointZ? MinPoint { get; set; }
        public PointZ? MaxPoint { get; set; }
        public AxisAlignedBoundingBox()
        {
            MinPoint = new PointZ();
            MaxPoint = new PointZ();
        }

        public bool Contains(PointZ point)
        {
            Point minPoint = MinPoint.AsGeometry();
            Point maxPoint = MaxPoint.AsGeometry();

            PostgisUltilities.AxisAlignedBoundingBox aabb = new PostgisUltilities.AxisAlignedBoundingBox(minPoint, maxPoint);

            return aabb.Contains(point.AsGeometry());
        }
    }
}
