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
        public string Path { get; set; }
        public string Color { get; set; }
        public string Mesh { get; set; }
        public double[] Matrix { get; set; }
        public string AABB { get; set; }
        public Guid? BatchedModelItemID { get; set; }
        public string Properties { get; set; }
        public DateTime LastModifiedTime { get; set; }


        /// <summary>
        /// Create a new model item.
        /// </summary>
        /// <param name="hierarchyIndex">The hierarchical index of the model item.</param>
        /// <param name="modelID">The ID of the model to which this item belongs.</param>
        /// <param name="parentHierachyIndex">The hierarchical index of the parent model item.</param>
        /// <param name="displayName">The display name of the model item.</param>
        /// <param name="path">The path associated with the model item.</param>
        /// <param name="color">The color of the model item in JSON format.</param>
        /// <param name="mesh">The mesh representation of the model item in JSON format.</param>
        /// <param name="matrix">The transformation matrix applied to the model item.</param>
        /// <param name="aABB">The axis-aligned bounding box of the model item in JSON format.</param>
        /// <param name="batchedModelItemID">The ID of the batched model item, it is nullable.</param>
        /// <param name="properties">Additional properties associated with the model item.</param>
        public ModelItemDB(int hierarchyIndex, Guid modelID, int parentHierachyIndex, string displayName, string path, Color color, Mesh mesh, double[] matrix, AABB aABB, Guid? batchedModelItemID, string properties)
        {
            HierarchyIndex = hierarchyIndex;
            ModelID = modelID;
            ModelItemID = Guid.NewGuid();
            ParentHierachyIndex = parentHierachyIndex;
            DisplayName = displayName;
            Path = path;
            Color = color.AsJson();
            Mesh = mesh.AsJson();
            Matrix = matrix;
            AABB = aABB.AsJson();
            BatchedModelItemID = batchedModelItemID;
            Properties = properties;
            LastModifiedTime = DateTime.Now;
        }
    }
}
