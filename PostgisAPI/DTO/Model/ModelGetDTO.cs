using NetTopologySuite.Geometries;
using PostgisUltilities;

namespace PostgisAPI.DTO.Model
{
    public class ModelGetDTO : ModelCreateDTO
    {
        public DateTime LastModifiedTime { get; set; }
    }
}
