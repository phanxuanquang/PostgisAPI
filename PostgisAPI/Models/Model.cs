using Newtonsoft.Json;
using PostgisAPI.DTO.Model;
using PostgisUltilities.Bounding_Boxes;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostgisAPI.Models
{
    [Table("model")]
    public class Model
    {
        [Column("id")]
        public int ID { get; set; }
        [Column("modelid")]
        public Guid ModelID { get; set; } = Guid.NewGuid();
        [Column("displayname")]
        public string DisplayName { get; set; }
        [Column("aabb", TypeName = "jsonb")]
        public string AABB { get; set; }
        [Column("lastmodifiedtime")]
        public DateTime LastModifiedTime { get; set; } = DateTime.Now;

        public ModelGetDTO AsDTO()
        {
            return new ModelGetDTO
            {
                ModelID = this.ModelID,
                DisplayName = this.DisplayName,
                AABB = JsonConvert.DeserializeObject<AxisAlignedBoundingBox>(AABB),
                LastModifiedTime = this.LastModifiedTime
            };
        }
    }
}
