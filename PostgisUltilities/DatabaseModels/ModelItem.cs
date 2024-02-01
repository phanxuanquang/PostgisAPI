using System;

namespace PostgisUltilities
{
    public class ModelItem
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
    }
}
