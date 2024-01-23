using Newtonsoft.Json;

namespace PostgisAPI.Models.Supporters
{
    public class Point
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public Point(double x = 0, double y = 0, double z = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        private string AsJson()
        {
            return JsonConvert.SerializeObject(this);
        }
        public NetTopologySuite.Geometries.Point AsGeometry()
        {
            return new NetTopologySuite.Geometries.Point(x, y, z);
        }
    }
}
