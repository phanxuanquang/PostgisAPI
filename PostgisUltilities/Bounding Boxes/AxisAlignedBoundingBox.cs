using NetTopologySuite.Geometries;
using Newtonsoft.Json;

namespace PostgisUltilities.Bounding_Boxes
{
    public class AxisAlignedBoundingBox
    {
        public PointZ MinPoint { get; set; }
        public PointZ MaxPoint { get; set; }
        public AxisAlignedBoundingBox()
        {
            MinPoint = new PointZ();
            MaxPoint = new PointZ();
        }
        public Geometry AsGeometry()
        {
            Point minPoint = new Point(MinPoint.x, MinPoint.y, MinPoint.z);
            Point maxPoint = new Point(MaxPoint.x, MaxPoint.y, MaxPoint.z);
            AabbAsGeo axisAlignedBoundingBox = new AabbAsGeo(minPoint, maxPoint);
            return axisAlignedBoundingBox.AsGeometry();
        }
        public string AsJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
