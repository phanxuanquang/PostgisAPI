using g3;
using NetTopologySuite.Geometries;

namespace PostgisUltilities
{
    /// <summary>
    /// Represents an axis-aligned bounding box (AABB) in 3D space.
    /// </summary>
    public class AxisAlignedBoundingBox
    {
        /// <summary>
        /// Gets or sets the minimum point of the axis-aligned bounding box.
        /// </summary>
        public Point MinPoint { get; set; }

        /// <summary>
        /// Gets or sets the maximum point of the axis-aligned bounding box.
        /// </summary>
        public Point MaxPoint { get; set; }

        /// <summary>
        /// Initializes a new instance of the AxisAlignedBoundingBox class based on the provided geometry.
        /// </summary>
        /// <param name="geometry">The input geometry used to create the axis-aligned bounding box.</param>
        public AxisAlignedBoundingBox(Geometry geometry)
        {
            MinPoint = GetMinPoint(geometry);
            MaxPoint = GetMaxPoint(geometry);
        }

        /// <summary>
        /// Convert the AABB into well-known text format
        /// </summary>
        /// <returns>The well-known text format of the AABB</returns>
        public string AsText()
        {
            return $"BOX3D({MinPoint.X} {MinPoint.Y} {MinPoint.Z}, {MaxPoint.X} {MaxPoint.Y} {MaxPoint.Z})";
        }
        public Geometry AsGeometry()
        {
            CoordinateZ AsCoordinateZ(Vector3d point)
            {
                return new CoordinateZ(point.x, point.y, point.z);
            }

            Box3d aabb = new Box3d(AsVector3dFrom(MinPoint), AsVector3dFrom(MaxPoint));

            Vector3d A = aabb.ComputeVertices()[0];
            Vector3d B = aabb.ComputeVertices()[1];
            Vector3d C = aabb.ComputeVertices()[2];
            Vector3d D = aabb.ComputeVertices()[3];

            Vector3d E = aabb.ComputeVertices()[4];
            Vector3d F = aabb.ComputeVertices()[5];
            Vector3d G = aabb.ComputeVertices()[6];
            Vector3d H = aabb.ComputeVertices()[7];

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
        private Point GetMaxPoint(Geometry geometry)
        {
            double xMax = geometry.Coordinate.X;
            double yMax = geometry.Coordinate.Y;
            double zMax = geometry.Coordinate.Z;

            foreach (Coordinate vertex in geometry.Coordinates)
            {
                if (vertex.X > xMax)
                {
                    xMax = vertex.X;
                }
                if (vertex.Y > yMax)
                {
                    yMax = vertex.Y;
                }
                if (vertex.Z > zMax)
                {
                    zMax = vertex.Z;
                }
            }

            return new Point(xMax, yMax, zMax);
        }
        private Point GetMinPoint(Geometry geometry)
        {
            double xMin = geometry.Coordinate.X;
            double yMin = geometry.Coordinate.Y;
            double zMin = geometry.Coordinate.Z;

            foreach (Coordinate vertex in geometry.Coordinates)
            {
                if (vertex.X < xMin)
                {
                    xMin = vertex.X;
                }
                if (vertex.Y < yMin)
                {
                    yMin = vertex.Y;
                }
                if (vertex.Z < zMin)
                {
                    zMin = vertex.Z;
                }
            }

            return new Point(xMin, yMin, zMin);
        }
        private Vector3d AsVector3dFrom(Point point)
        {
            return new Vector3d(point.X, point.Y, point.Z);
        }
        public bool Contains(Point point)
        {
            return AsGeometry().Contains(point);
        }
        public bool Equals(AxisAlignedBoundingBox aabb)
        {
            return MinPoint == aabb.MinPoint && MaxPoint == aabb.MaxPoint && MinPoint.Z == aabb.MinPoint.Z && MaxPoint.Z == aabb.MaxPoint.Z;
        }
    }
}
