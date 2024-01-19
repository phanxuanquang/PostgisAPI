namespace PostgisAPI.Models.Supporters
{
    public class AxisAlignedBoundingBox
    {
        public Point? MinPoint { get; set; }
        public Point? MaxPoint { get; set; }
        public AxisAlignedBoundingBox()
        {
            MinPoint = new Point();
            MaxPoint = new Point();
        }

    }
}
