﻿using Newtonsoft.Json;

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

        private string AsJson()
        {
            return JsonConvert.SerializeObject(this);
        }
        public bool Contains(Point point)
        {
            NetTopologySuite.Geometries.Point minPoint = MinPoint.AsGeometry();
            NetTopologySuite.Geometries.Point maxPoint = MaxPoint.AsGeometry();

            PostgisUltilities.AxisAlignedBoundingBox aabb = new PostgisUltilities.AxisAlignedBoundingBox(minPoint, maxPoint);

            return aabb.Contains(point.AsGeometry());
        }
    }
}
