using NetTopologySuite.Geometries;
using Newtonsoft.Json;

namespace PostgisUltilities
{
    public class PointZ
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public PointZ(double x = 0, double y = 0, double z = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        private string AsJson()
        {
            return JsonConvert.SerializeObject(this);
        }
        public Point AsGeometry()
        {
            return new Point(x, y, z);
        }
    }
}
