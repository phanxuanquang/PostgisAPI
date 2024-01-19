namespace PostgisAPI.Models.Supporters
{
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
}
