using System;

namespace PostgisUltilities
{
    public class Model
    {
        public int ID { get; set; }
        public Guid ModelID { get; set; }
        public string DisplayName { get; set; }
        public DateTime LastModifiedTime { get; set; }
        /// <summary>
        /// Create a new model
        /// </summary>
        public Model(int iD, Guid modelID, string displayName, DateTime lastModifiedTime)
        {
            ID = iD;
            ModelID = modelID;
            DisplayName = displayName;
            LastModifiedTime = lastModifiedTime;
        }
    }
}
