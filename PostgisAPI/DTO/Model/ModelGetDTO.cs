﻿namespace PostgisAPI.DTO.Model
{
    public class ModelGetDTO : ModelCreateDTO
    {
        public Guid ModelID { get; set; }
        public DateTime LastModifiedTime { get; set; }
    }
}
