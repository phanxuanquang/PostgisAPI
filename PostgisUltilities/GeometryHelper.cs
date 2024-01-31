using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace PostgisUltilities
{
    public class GeometryHelper
    {
        /// <summary>
        /// Retrieves a list of geometries that intersect with a provided bounding box geometry.
        /// </summary>
        /// <param name="geo">The bounding box geometry for intersection testing, it can be AABB or OBB.</param>
        /// <param name="geometries">List of geometries to check for intersection with the bounding box.</param>
        /// <returns>List of geometries that intersect with the provided bounding box.</returns>
        public List<Geometry> GetIntersection(Geometry geo, List<Geometry> geometries)
        {
            List<Geometry> intersectingGeometries = new List<Geometry>();

            foreach (Geometry geometry in geometries)
            {
                if (geometry.Intersects(geo))
                {
                    intersectingGeometries.Add(geometry);
                }
            }
            return intersectingGeometries;
        }

        /// <summary>
        /// Get the oriented bounding box of a specific geometry set
        /// </summary>
        /// <param name="geometries">Input geometry set</param>
        /// <returns>The oriented bounding box of the input geometry set</returns>
        public OrientedBoundingBox GetObbOf(List<Geometry> geometries)
        {
            return new OrientedBoundingBox(MergeGeometryFrom(geometries));
        }
        public AxisAlignedBoundingBox GetAabbOf(List<Geometry> geometries)
        {
            return new AxisAlignedBoundingBox(MergeGeometryFrom(geometries));
        }

        /// <summary>
        /// Export a specific geometry to an .obj file
        /// </summary>
        /// <param name="geometry">Input geometry</param>
        /// <param name="outputPath">Output .obj file path</param>
        public void ExportAsObjFileOf(Geometry geometry, string outputPath)
        {
            if (geometry != null && !string.IsNullOrEmpty(outputPath))
            {
                string outputDirectory = Path.GetDirectoryName(outputPath);
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                using (StreamWriter writer = new StreamWriter(outputPath))
                {
                    List<int> faceIndexes = new List<int>();
                    List<CoordinateZ> vertices = new List<CoordinateZ>();

                    foreach (CoordinateZ vertex in geometry.Coordinates)
                    {
                        int idx = vertices.FindIndex(x => x.Equals3D(vertex));
                        if (idx == -1)
                        {
                            vertices.Add(vertex);
                            idx = vertices.Count - 1;
                        }
                        faceIndexes.Add(idx);
                    }

                    foreach (CoordinateZ vertex in vertices)
                    {
                        writer.WriteLine($"v {vertex.X} {vertex.Y} {vertex.Z}");
                    }
                    for (int i = 0; i < faceIndexes.Count - 3; i += 3)
                    {
                        writer.WriteLine($"f {faceIndexes[i] + 1} {faceIndexes[i + 1] + 1} {faceIndexes[i + 2] + 1}");
                    }
                    writer.Close();
                }

                MessageBox.Show("Extracting to .obj file complete", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Extracting to .obj file failed", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Export a specific geometry set to an .obj file
        /// </summary>
        /// <param name="geos">Input geometry set</param>
        /// <param name="outputPath">Output .obj file path</param>
        public void ExportAsObjFileOf(List<Geometry> geos, string outputPath)
        {
            if (geos != null && geos.Count > 0 && !string.IsNullOrEmpty(outputPath))
            {
                string outputDirectory = Path.GetDirectoryName(outputPath);
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                using (StreamWriter writer = new StreamWriter(outputPath))
                {
                    List<int> faceIndexesTotal = new List<int>();
                    List<CoordinateZ> uniqueVerticesTotal = new List<CoordinateZ>();
                    foreach (Geometry geo in geos)
                    {
                        List<int> faceIndexes = new List<int>();
                        List<CoordinateZ> uniqueVertices = new List<CoordinateZ>();
                        foreach (CoordinateZ vertex in geo.Coordinates)
                        {
                            int idx = uniqueVertices.FindIndex(x => x.Equals3D(vertex));
                            if (idx == -1)
                            {
                                uniqueVertices.Add(vertex);
                                idx = uniqueVertices.Count - 1;
                            }
                            faceIndexes.Add(idx + uniqueVerticesTotal.Count);

                        }
                        faceIndexes.RemoveAt(faceIndexes.Count - 1);
                        uniqueVerticesTotal.AddRange(uniqueVertices);
                        faceIndexesTotal.AddRange(faceIndexes);
                    }

                    foreach (CoordinateZ vertex in uniqueVerticesTotal)
                    {
                        writer.WriteLine($"v {vertex.X} {vertex.Y} {vertex.Z}");
                    }
                    for (int i = 0; i < faceIndexesTotal.Count - 3; i += 3)
                    {
                        writer.WriteLine($"f {faceIndexesTotal[i] + 1} {faceIndexesTotal[i + 1] + 1} {faceIndexesTotal[i + 2] + 1}");
                    }
                    writer.Close();
                }
                MessageBox.Show("Extracting to .obj file complely", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Extracting to .obj file failed", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Unify the geometry set into one geometry
        /// </summary>
        /// <param name="geometries">The geometry set</param>
        /// <returns>The unified geometry</returns>
        public Geometry MergeGeometryFrom(List<Geometry> geometries)
        {
            GeometryFactory geometryFactory = new GeometryFactory();
            Geometry[] geometryArray = geometries.ToArray();
            GeometryCollection geometryCollection = new GeometryCollection(geometryArray, geometryFactory);
            return geometryCollection;
        }

        #region Value Retrievers

        /// <summary>
        /// Gets the distance between two geometries.
        /// </summary>
        /// <param name="geometry1">The first geometry.</param>
        /// <param name="geometry2">The second geometry.</param>
        /// <returns>The distance between the two geometries.</returns>
        public double ST_Distance(Geometry geometry1, Geometry geometry2)
        {
            return geometry1.Distance(geometry2);
        }

        /// <summary>
        /// Gets the area of a geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The area of the geometry.</returns>
        public double ST_Area(Geometry geometry)
        {
            return geometry.Area;
        }

        /// <summary>
        /// Gets the length of a geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The length of the geometry.</returns>
        public double ST_Length(Geometry geometry)
        {
            return geometry.Length;
        }

        /// <summary>
        /// Gets the geometry type of the provided geometry.
        /// </summary>
        /// <param name="geometry">The input geometry.</param>
        /// <returns>The Geometry Type of the input geometry.</returns>
        public string ST_GeometryType(Geometry geometry)
        {
            return geometry.GeometryType;
        }

        /// <summary>
        /// Generates the Well-Known Text (WKT) representation of a geometry.
        /// </summary>
        /// <param name="geometry">The input geometry.</param>
        /// <returns>The Well-Known Text representation of the geometry.</returns>
        public string ST_AsText(List<Geometry> geos)
        {
            try
            {
                return MergeGeometryFrom(geos).AsText();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cannot generate text from geometry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        /// <summary>
        /// Gets the Open Geospatial Consortium (OGC) geometry type of the provided geometry.
        /// </summary>
        /// <param name="geometry">The input geometry.</param>
        /// <returns>The OGC geometry type of the input geometry.</returns>
        public OgcGeometryType GeometryType(Geometry geometry)
        {
            return geometry.OgcGeometryType;
        }
        #endregion

        #region Geometric Methods

        /// <summary>
        /// Computes the intersection of two geometries.
        /// </summary>
        /// <param name="geometry1">The first geometry.</param>
        /// <param name="geometry2">The second geometry.</param>
        /// <returns>The intersection geometry.</returns>
        public Geometry ST_Intersection(Geometry geometry1, Geometry geometry2)
        {
            return geometry1.Intersection(geometry2);
        }

        /// <summary>
        /// Computes the difference of two geometries.
        /// </summary>
        /// <param name="geometry1">The first geometry.</param>
        /// <param name="geometry2">The second geometry.</param>
        /// <returns>The difference geometry.</returns>
        public Geometry ST_Difference(Geometry geometry1, Geometry geometry2)
        {
            return geometry1.Difference(geometry2);
        }

        /// <summary>
        /// Computes the union of two geometries.
        /// </summary>
        /// <param name="geometry1">The first geometry.</param>
        /// <param name="geometry2">The second geometry.</param>
        /// <returns>The union geometry.</returns>
        public Geometry ST_Union(Geometry geometry1, Geometry geometry2)
        {
            return geometry1.Union(geometry2);
        }

        /// <summary>
        /// Computes the symmetric difference of two geometries.
        /// </summary>
        /// <param name="geometry1">The first geometry.</param>
        /// <param name="geometry2">The second geometry.</param>
        /// <returns>The symmetric difference geometry.</returns>
        public Geometry ST_SymDifference(Geometry geometry1, Geometry geometry2)
        {
            return geometry1.SymmetricDifference(geometry2);
        }

        /// <summary>
        /// Computes the buffer of a geometry with specified parameters.
        /// </summary>
        /// <param name="geometry">The input geometry.</param>
        /// <param name="radius">The buffer radius.</param>
        /// <param name="quadrantSegments">The number of quadrant segments.</param>
        /// <param name="endcap">The style of endcap (round, flat, square).</param>
        /// <returns>The buffered geometry.</returns>
        public Geometry ST_Buffer(Geometry geometry, double radius, int quadrantSegments = 8, string endcap = "round")
        {
            NetTopologySuite.Operation.Buffer.EndCapStyle endCapStyle;
            switch (endcap.ToLower())
            {
                case "round":
                    endCapStyle = NetTopologySuite.Operation.Buffer.EndCapStyle.Round;
                    break;
                case "flat":
                    endCapStyle = NetTopologySuite.Operation.Buffer.EndCapStyle.Flat;
                    break;
                case "square":
                    endCapStyle = NetTopologySuite.Operation.Buffer.EndCapStyle.Square;
                    break;
                default:
                    MessageBox.Show("Endcap style does not exist", "Cannot create endcap style", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
            }

            return geometry.Buffer(radius, quadrantSegments, endCapStyle);
        }

        /// <summary>
        /// Creates a valid representation of a given invalid geometry without losing any of the input vertices. 
        /// </summary>
        /// <param name="geometry">The input geometry.</param>
        /// <returns></returns>
        public string ST_MakeValid(Geometry geometry)
        {
            //if (ST_IsValid(geometry))
            //{
            //    return false;
            //}
            //return NetTopologySuite.Geometries.Utilities.GeometryFixer.Fix(geometry).IsValid;
            return geometry.AsText();
        }

        /// <summary>
        /// Generates a geometry from Well-Known Text (WKT) representation.
        /// </summary>
        /// <param name="text">The Well-Known Text representation of the geometry.</param>
        /// <returns>The generated geometry.</returns>
        public Geometry ST_GeometryFromText(string text)
        {
            WKTReader readerGeometry = new WKTReader();
            Geometry geometry = null;
            try
            {
                geometry = readerGeometry.Read(text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cannot generate geometry from text", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return geometry;
        }

        /// <summary>
        /// Converts a GeoJSON string into a Geometry object.
        /// </summary>
        /// <param name="geoJSON">The GeoJSON string to convert.</param>
        /// <returns>The Geometry object parsed from the GeoJSON.</returns>
        public Geometry ST_GeomFromGeoJSON(string geoJSON)
        {
            Geometry geometry = null;

            JsonSerializer serializer = GeoJsonSerializer.Create();
            using (StringReader stringReader = new StringReader(geoJSON))
            using (JsonTextReader jsonReader = new JsonTextReader(stringReader))
            {
                geometry = serializer.Deserialize<Geometry>(jsonReader);
            }
            return geometry;
        }

        /// <summary>
        /// Converts a Geometry object into a GeoJSON string.
        /// </summary>
        /// <param name="geometry">The Geometry object to convert.</param>
        /// <returns>The GeoJSON string representing the Geometry.</returns>
        public string ST_AsGeoJSON(Geometry geometry)
        {
            string geoJSON = string.Empty;

            JsonSerializer serializer = GeoJsonSerializer.Create();
            using (StringWriter stringWriter = new StringWriter())
            using (JsonTextWriter jsonWriter = new JsonTextWriter(stringWriter))
            {
                jsonWriter.Formatting = Formatting.Indented;
                serializer.Serialize(jsonWriter, geometry);
                geoJSON = stringWriter.ToString();
            }
            return geoJSON;
        }
        #endregion
    }
}
