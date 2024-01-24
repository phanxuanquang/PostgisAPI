using g3;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace PostgisUltilities
{
    public class Mesh
    {
        public List<Point> vertices { get; set; }
        public List<int> faceIndexes { get; set; }
        public Mesh()
        {
            vertices = new List<Point>();
            faceIndexes = new List<int>();
        }
        public string AsJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public Geometry AsGeometry()
        {
            CoordinateZ AsCoordinateZ(Point point)
            {
                return new CoordinateZ(point.x, point.y, point.z);
            }

            List<CoordinateZ> fullVertices = new List<CoordinateZ>();
            for(int i = 0; i < faceIndexes.Count; i++)
            {
                fullVertices.Add(AsCoordinateZ(vertices[faceIndexes[i]]));
            }
            fullVertices.Add(AsCoordinateZ(vertices[faceIndexes[0]]));

            LineString lineString = new LineString(null);
            GeometryFactory geometryFactory = new GeometryFactory();
            LinearRing linearRing = geometryFactory.CreateLinearRing(lineString.Coordinates);

            return geometryFactory.CreatePolygon(linearRing);
        }

        public bool TouchedBy(Point hitPoint)
        {
            NetTopologySuite.Geometries.Point point = new NetTopologySuite.Geometries.Point(hitPoint.x, hitPoint.y, hitPoint.z);
            return this.AsGeometry().Touches(point);
        }
    }
}
