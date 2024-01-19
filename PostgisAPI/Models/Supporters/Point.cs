namespace PostgisAPI.Models.Supporters
{
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
}
