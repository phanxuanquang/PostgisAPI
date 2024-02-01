using System;

namespace PostgisUltilities
{
    public class Model
    {
        public int ID { get; set; }
        public Guid ModelID { get; set; }
        public string DisplayName { get; set; }
        public DateTime LastModifiedTime { get; set; }
    }
}