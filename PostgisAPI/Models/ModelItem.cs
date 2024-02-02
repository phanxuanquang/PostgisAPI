using Newtonsoft.Json;
using PostgisUltilities;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostgisAPI.Models
{
    [Table("modelitem")]
    public class ModelItem
    {
        [Column("id")]
        public int ID { get; set; }

        [Column("hierachyindex")]
        public int HierarchyIndex { get; set; }

        [Column("modelid")]
        public Guid ModelID { get; set; }

        [Column("modelitemid")]
        public Guid ModelItemID { get; set; } = Guid.NewGuid();

        [Column("parenthierachyindex")]
        public int ParentHierachyIndex { get; set; }

        [Column("displayname")]
        public string DisplayName { get; set; }
        [Column("color", TypeName = "jsonb")]
        public string Color { get; set; }
        [Column("mesh", TypeName = "jsonb")]
        public string Mesh { get; set; }
        [Column("matrix", TypeName = "real[]")]
        public double[] Matrix { get; set; }
        [Column("batchedmodelitemid")]
        public Guid? BatchedModelItemID { get; set; }
        [Column("properties")]
        public string Properties { get; set; }
        [Column("lastmodifiedtime")]
        public DateTime LastModifiedTime { get; set; } = DateTime.Now;
        [Column("featureid")]
        public int? FeatureID { get; set; }
        [Column("glbindex")]
        public int? GlbIndex {  get; set; }

        public DTO.ModelItemGetDTO AsDTO()
        {
            return new DTO.ModelItemGetDTO
            {
                ModelID = ModelID,
                ModelItemID = ModelItemID,
                HierarchyIndex = HierarchyIndex,
                ParentHierachyIndex = ParentHierachyIndex,
                DisplayName = DisplayName,
                Color = JsonConvert.DeserializeObject<Color>(Color),
                Mesh = JsonConvert.DeserializeObject<Mesh>(Mesh),
                Matrix = Matrix,
                BatchedModelItemID = BatchedModelItemID,
                Properties = Properties,
                LastModifiedTime = LastModifiedTime,
                FeatureID = FeatureID,
                GlbIndex = GlbIndex,
            };
        }
    }
}
