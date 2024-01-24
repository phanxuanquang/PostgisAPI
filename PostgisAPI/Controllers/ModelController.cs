using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PostgisAPI.DTO.Model;
using PostgisAPI.Models;
using PostgisUltilities.Bounding_Boxes;

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

        /// <summary>
        /// Get all models 
        /// </summary>
        /// <returns>Returns a list of <see cref="ModelGetDTO"/> representing the model.</returns>
        /// <response code="200">Information of all models in the database</response>
        [HttpGet("getAll")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ModelGetDTO>))]
        public ActionResult<IEnumerable<ModelGetDTO>> GetAll()
        {
            IEnumerable<ModelGetDTO> models = context.Models.Select(item => new ModelGetDTO
            {
                ModelID = item.ModelID,
                DisplayName = item.DisplayName,
                AABB = JsonConvert.DeserializeObject<AxisAlignedBoundingBox>(item.AABB),
                LastModifiedTime = item.LastModifiedTime
            });

            return models.ToList();
        }

        /// <summary>
        /// Find a model by its GUID
        /// </summary>
        /// <param name="modelid">The GUID of the model.</param>
        /// <returns>Returns a <see cref="ModelGetDTO"/> representing the model.</returns>
        /// <response code="200">The requested model.</response>
        /// <response code="404">The model with provided GUID does not exist in the database.</response>
        [HttpGet("{modelid}")]
        [ProducesResponseType(200, Type = typeof(ModelGetDTO))]
        [ProducesResponseType(404)]
        public ActionResult<ModelGetDTO> GetByGuid(Guid modelid)
        {
            Model? model = context.Models.FirstOrDefault(item => item.ModelID == modelid);

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

        /// <summary>
        /// Create a new model
        /// </summary>
        /// <remarks>
        /// Create a new model with the display name and its axis-aligned bounding boxes (AABB).
        /// </remarks>
        /// <param name="modelItemDTO">All the fields are required. If not provided, leave them with default values.</param>
        /// <returns>Returns the created <see cref="Model"/>.</returns>
        /// <response code="201">Created model, including its GUID and created time.</response>
        [HttpPost("create")]
        [ProducesResponseType(201, Type = typeof(Model))]
        public ActionResult<string> Create(ModelCreateDTO modelItemDTO)
        {
            Model model = new Model
            {
                ModelID = Guid.NewGuid(),
                DisplayName = modelItemDTO.DisplayName,
                AABB = JsonConvert.SerializeObject(modelItemDTO.AABB),
            };

            context.Models.Add(model);
            context.SaveChanges();
            return model.ModelID.ToString();
        }
    }
}
