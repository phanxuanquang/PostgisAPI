﻿using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using PostgisUltilities.Bounding_Boxes;
using System.Collections.Generic;

namespace PostgisUltilities
{
    public class Mesh
    {
        public List<PointZ> vertices { get; set; }
        public List<int> faceIndexes { get; set; }
        public Mesh()
        {
            vertices = new List<PointZ>();
            faceIndexes = new List<int>();
        }
        public string AsJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public Geometry AsGeometry()
        {
            CoordinateZ AsCoordinateZ(PointZ point)
            {
                return new CoordinateZ(point.x, point.y, point.z);
            }

            List<CoordinateZ> fullVertices = new List<CoordinateZ>();
            for (int i = 0; i < faceIndexes.Count; i++)
            {
                fullVertices.Add(AsCoordinateZ(vertices[faceIndexes[i]]));
            }
            fullVertices.Add(AsCoordinateZ(vertices[faceIndexes[0]]));

            LineString lineString = new LineString(null);
            GeometryFactory geometryFactory = new GeometryFactory();
            LinearRing linearRing = geometryFactory.CreateLinearRing(lineString.Coordinates);

            return geometryFactory.CreatePolygon(linearRing);
        }

        public bool TouchedBy(PointZ hitPoint)
        {

            Point point = new Point(hitPoint.x, hitPoint.y, hitPoint.z);
            return AsGeometry().Touches(point);
        }

        public bool Intersects(PointZ hitPoint)
        {
            Point point = new Point(hitPoint.x, hitPoint.y, hitPoint.z);
            return AsGeometry().Intersects(point);
        }

        public OrientedBoundingBox GetObb()
        {
            return new OrientedBoundingBox(this.AsGeometry());
        }
        public AxisAlignedBoundingBox GetAabb()
        {
            return new AxisAlignedBoundingBox(this.AsGeometry());
        }
    }
}
