namespace PostgisAPI.DTO.Model
{
    public class ModelCreateDTO
    {
        public string DisplayName { get; set; }
        public Models.Model AsModelDB()
        {
            return new Models.Model()
            {
                ModelID = Guid.NewGuid(),
                DisplayName = DisplayName,
                LastModifiedTime = DateTime.Now
            };
        }
    }
}
