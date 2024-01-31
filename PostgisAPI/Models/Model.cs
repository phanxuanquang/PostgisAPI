using PostgisAPI.DTO.Model;
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
        [Column("lastmodifiedtime")]
        public DateTime LastModifiedTime { get; set; } = DateTime.Now;

        public ModelGetDTO AsDTO()
        {
            return new ModelGetDTO
            {
                ModelID = ModelID,
                DisplayName = DisplayName,
                LastModifiedTime = LastModifiedTime
            };
        }
    }
}
