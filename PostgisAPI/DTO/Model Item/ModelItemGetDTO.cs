namespace PostgisAPI.DTO
{
    public class ModelItemGetDTO : ModelItemCreateDTO
    {
        public Guid ModelID { get; set; }
        public DateTime LastModifiedTime { get; set; }
    }
}