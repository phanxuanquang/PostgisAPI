using g3;
using NetTopologySuite.Geometries;
using System.Collections.Generic;

namespace PostgisUltilities
{
    public class OrientedBoundingBox
    {
        private Vector3d centroid;
        private Vector3d xAxis;
        private Vector3d yAxis;
        private Vector3d zAxis;
        private Vector3d halfSizeVector;
        private NetTopologySuite.Geometries.Point AsPointFrom(Vector3d point)
        {
            return new NetTopologySuite.Geometries.Point(point.x, point.y, point.z);
        }
        public OrientedBoundingBox(Geometry geometry)
        {
            List<Vector3d> vertices = new List<Vector3d>();
            foreach (CoordinateZ vertex in geometry.Coordinates)
            {
                vertices.Add(new Vector3d(vertex.X, vertex.Y, vertex.Z));
            }

            ContOrientedBox3 obb = new ContOrientedBox3(vertices);

            centroid = obb.Box.Center;

            xAxis = obb.Box.AxisX;
            yAxis = obb.Box.AxisY;
            zAxis = obb.Box.AxisZ;

            halfSizeVector = obb.Box.Extent;
        }
        public Geometry AsGeometry()
        {
            CoordinateZ AsCoordinateZ(Vector3d point)
            {
                return new CoordinateZ(point.x, point.y, point.z);
            }

            Vector3d A = centroid - halfSizeVector.z * zAxis - halfSizeVector.x * xAxis - yAxis * halfSizeVector.y;
            Vector3d B = centroid - halfSizeVector.z * zAxis + halfSizeVector.x * xAxis - yAxis * halfSizeVector.y;
            Vector3d C = centroid - halfSizeVector.z * zAxis + halfSizeVector.x * xAxis + yAxis * halfSizeVector.y;
            Vector3d D = centroid - halfSizeVector.z * zAxis - halfSizeVector.x * xAxis + yAxis * halfSizeVector.y;

            Vector3d E = centroid + halfSizeVector.z * zAxis - halfSizeVector.x * xAxis - yAxis * halfSizeVector.y;
            Vector3d F = centroid + halfSizeVector.z * zAxis + halfSizeVector.x * xAxis - yAxis * halfSizeVector.y;
            Vector3d G = centroid + halfSizeVector.z * zAxis + halfSizeVector.x * xAxis + yAxis * halfSizeVector.y;
            Vector3d H = centroid + halfSizeVector.z * zAxis - halfSizeVector.x * xAxis + yAxis * halfSizeVector.y;

            LineString lineString = new LineString(new[]
            {
                AsCoordinateZ(A), AsCoordinateZ(B), AsCoordinateZ(C),
                AsCoordinateZ(A), AsCoordinateZ(C), AsCoordinateZ(D),
                AsCoordinateZ(E), AsCoordinateZ(F), AsCoordinateZ(G),
                AsCoordinateZ(E), AsCoordinateZ(G), AsCoordinateZ(H),
                AsCoordinateZ(B), AsCoordinateZ(C), AsCoordinateZ(G),
                AsCoordinateZ(B), AsCoordinateZ(G), AsCoordinateZ(F),
                AsCoordinateZ(D), AsCoordinateZ(C), AsCoordinateZ(G),
                AsCoordinateZ(D), AsCoordinateZ(G), AsCoordinateZ(H),
                AsCoordinateZ(A), AsCoordinateZ(D), AsCoordinateZ(H),
                AsCoordinateZ(A), AsCoordinateZ(H), AsCoordinateZ(E),
                AsCoordinateZ(A), AsCoordinateZ(B), AsCoordinateZ(F),
                AsCoordinateZ(A), AsCoordinateZ(F), AsCoordinateZ(E),
                AsCoordinateZ(A)
            });

            GeometryFactory geometryFactory = new GeometryFactory();
            LinearRing linearRing = geometryFactory.CreateLinearRing(lineString.Coordinates);

            return geometryFactory.CreatePolygon(linearRing);
        }
        public NetTopologySuite.Geometries.Point AxisX()
        {
            return AsPointFrom(xAxis);
        }
        public NetTopologySuite.Geometries.Point AxisY()
        {
            return AsPointFrom(yAxis);
        }
        public NetTopologySuite.Geometries.Point AxisZ()
        {
            return AsPointFrom(zAxis);
        }
        public NetTopologySuite.Geometries.Point HalfSizeVector()
        {
            return AsPointFrom(halfSizeVector);
        }
        public bool Contains(NetTopologySuite.Geometries.Point point)
        {
            return AsGeometry().Contains(point);
        }
    }
}
