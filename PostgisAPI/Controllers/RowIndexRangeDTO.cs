namespace PostgisAPI.Controllers
{
    public class RowIndexRangeDTO
    {
        public Guid ModelID { get; set; }
        public int? StartIndex { get; set; }
        public int? EndIndex { get; set; }
    }
}
