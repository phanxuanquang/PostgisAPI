using Microsoft.AspNetCore.Mvc;
using PostgisAPI.DTO.Model;
using PostgisAPI.Models;

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
        [HttpGet("all")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ModelGetDTO>))]
        public ActionResult<IEnumerable<ModelGetDTO>> GetAll()
        {
            IEnumerable<ModelGetDTO> models = context.Models.Select(item => item.AsDTO());

            List<ModelGetDTO> res = models.ToList();
            return Ok(new { total = res.Count, models = res });
        }

        /// <summary>
        /// Find a model by its GUID
        /// </summary>
        /// <param name="modelid">The GUID of the model.</param>
        /// <returns>Returns a <see cref="ModelGetDTO"/> representing the model.</returns>
        /// <response code="200">The requested model.</response>
        /// <response code="404">The model with provided GUID does not exist in the database.</response>
        [HttpGet("guid")]
        [ProducesResponseType(200, Type = typeof(ModelGetDTO))]
        [ProducesResponseType(404)]
        public ActionResult<ModelGetDTO> GetByGuid(Guid modelid)
        {
            Model? model = context.Models.FirstOrDefault(item => item.ModelID == modelid);

            if (model == null)
            {
                return NotFound("Not found");
            }

            return model.AsDTO();
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
            Model model = modelItemDTO.AsModelDB();

            context.Models.Add(model);
            context.SaveChanges();

            return Created("Create successfully", model.ModelID.ToString());
        }
    }
}
