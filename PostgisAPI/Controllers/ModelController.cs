using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PostgisAPI.DTO.Model;
using PostgisAPI.Models;
using PostgisAPI.Models.Supporters;

namespace PostgisAPI.Controllers
{
    /// <summary>
    /// Controller for model management
    /// </summary>
    [ApiController]
    [Route("model")]
    public class ModelController : ControllerBase
    {
        private readonly ApiDbContext context;

        /// <summary>
        /// Initialize a controller for model
        /// </summary>
        /// <param name="dbContext"></param>
        public ModelController(ApiDbContext dbContext)
        {
            context = dbContext;
        }

        /// <summary>
        /// Get a list of all model.
        /// </summary>
        /// <returns>Returns a list of <see cref="ModelGetDTO"/> representing the model.</returns>
        /// <response code="200">Returns the list of model.</response>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ModelGetDTO>))]
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

        /// <summary>
        /// Get a specific model by its ID.
        /// </summary>
        /// <param name="modelid">The unique identifier of the model.</param>
        /// <returns>Returns a <see cref="ModelGetDTO"/> representing the model.</returns>
        /// <response code="200">Returns the requested model.</response>
        /// <response code="404">If the model with the specified ID is not found.</response>
        [HttpGet("{modelid}")]
        [ProducesResponseType(200, Type = typeof(ModelGetDTO))]
        [ProducesResponseType(404)]
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

        /// <summary>
        /// Create a new model.
        /// </summary>
        /// <param name="modelItemDTO">The data to create the new model.</param>
        /// <returns>Returns the created <see cref="Model"/>.</returns>
        /// <response code="201">Returns the newly created model.</response>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Model))]
        public ActionResult<Model> Post(ModelCreateDTO modelItemDTO)
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
