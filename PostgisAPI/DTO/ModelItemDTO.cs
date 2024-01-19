namespace PostgisAPI.DTO
{
    public class ModelItemDTO : ModelItemCreateDTO
    {
        public int RowIndex {  get; set; }
        public Guid ModelID { get; set; }
    }
}
