using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PostgisAPI.DTO;
using PostgisAPI.DTO.Model;
using PostgisAPI.Models;
using PostgisAPI.Models.Supporters;

namespace PostgisAPI.Controllers
{
    [ApiController]
    [Route("model")]
    public class ModelController : ControllerBase
    {
        private readonly ApiDbContext context;

        public ModelController(ApiDbContext dbContext)
        {
            context = dbContext;
        }
        [HttpGet]
        public ActionResult<IEnumerable<ModelGetDTO>> Get()
        {
            IEnumerable<ModelGetDTO> models = context.ModelItems.Select(item => new ModelGetDTO
            {
                ModelID = item.ModelID,
                DisplayName = item.DisplayName,
                AABB = JsonConvert.DeserializeObject<AxisAlignedBoundingBox>(item.AABB),
                LastModifiedTime = item.LastModifiedTime
            });

            return models.ToList();
        }
        [HttpGet("{modelid}")]
        public ActionResult<ModelGetDTO> GetById(Guid modelid)
        {
            ModelItem? model = context.ModelItems.FirstOrDefault(item => item.ModelID == modelid);

            if (model == null)
            {
                return NotFound();
            }

            ModelGetDTO item = new ModelGetDTO
            {
                ModelID = model.ModelID,
                DisplayName = model.DisplayName,
                AABB = JsonConvert.DeserializeObject<AxisAlignedBoundingBox>(model.AABB),
                LastModifiedTime = model.LastModifiedTime
            };

            return item;
        }
        [HttpPost("{modelid}")]
        public ActionResult<Model> Post(Guid modelid, ModelCreateDTO modelItemDTO)
        {
            Model model = new Model
            {
                ModelID = modelItemDTO.ModelID,
                DisplayName = modelItemDTO.DisplayName,
                AABB = JsonConvert.SerializeObject(modelItemDTO.AABB),
            };

            context.Models.Add(model);
            context.SaveChanges();

            return model;
        }
    }
}
