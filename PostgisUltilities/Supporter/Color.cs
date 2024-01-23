using Newtonsoft.Json;

namespace PostgisUltilities
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

        public string AsJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
