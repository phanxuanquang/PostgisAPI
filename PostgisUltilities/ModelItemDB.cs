using System;

namespace PostgisUltilities
{
    public class ModelItemDB
    {
        public int HierarchyIndex { get; set; }
        public Guid ModelID { get; set; }
        public Guid ModelItemID { get; set; }
        public int ParentHierachyIndex { get; set; }
        public string DisplayName { get; set; }
        public string Color { get; set; }
        public string Mesh { get; set; }
        public double[] Matrix { get; set; }
        public Guid? BatchedModelItemID { get; set; }
        public string Properties { get; set; }
        public DateTime LastModifiedTime { get; set; }
        public int? FeatureID;
        public int? GlbIndex;

        public ModelItemDB(int hierarchyIndex, Guid modelID, Guid modelItemID, int parentHierachyIndex, string displayName, string color, string mesh, double[] matrix, Guid? batchedModelItemID, string properties, DateTime lastModifiedTime, int? featureID, int? glbIndex)
        {
            HierarchyIndex = hierarchyIndex;
            ModelID = modelID;
            ModelItemID = modelItemID;
            ParentHierachyIndex = parentHierachyIndex;
            DisplayName = displayName;
            Color = color;
            Mesh = mesh;
            Matrix = matrix;
            BatchedModelItemID = batchedModelItemID;
            Properties = properties;
            LastModifiedTime = lastModifiedTime;
            FeatureID = featureID;
            GlbIndex = glbIndex;
        }
    }
}
