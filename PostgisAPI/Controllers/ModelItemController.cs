
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostgisAPI.DTO;
using PostgisAPI.DTO.Model;
using PostgisAPI.Models;
using PostgisAPI.Models.Supporters;

namespace PostgisAPI.Controllers
{
    /// <summary>
    /// Controller for model item management
    /// </summary>
    [ApiController]
    [Route("modelitem")]
    public class ModelItemController : ControllerBase
    {
        private readonly ApiDbContext context;

        public ModelItemController(ApiDbContext dbContext)
        {
            context = dbContext;
        }

        /// <summary>
        /// Find a model item by its GUID
        /// </summary>
        /// <remarks>
        /// Get a specific model item based on its GUID.
        /// </remarks>
        /// <param name="modelitemid">The GUID of the model item to get.</param>
        /// <returns>The requested model item.</returns>
        /// <response code="200">The requested model item.</response>
        /// <response code="404">No model item is found for the requested GUID.</response>
        [HttpGet("{modelitemid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<ModelItemGetDTO> GetByGuid(Guid modelitemid)
        {
            ModelItem? modelItem = context.ModelItems
                .FirstOrDefault(item => item.ModelItemID == modelitemid);

            if (modelItem == null)
            {
                return NotFound();
            }

            return modelItem.AsDTO();
        }

        /// <summary>
        /// Gets model items by the row index range
        /// </summary>
        /// <remarks>
        /// Get model items associated with the specified model within the specified row index range (get all model items by default).
        /// </remarks>
        /// <param name="modelid">The GUID of the model for which to get model items.</param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex">-1 means the total model items assosicated with the provided model.</param>
        /// <returns>A list of model items within the specified row index range.</returns>
        /// <response code="200">The list of model items within the specified row index range.</response>
        /// <response code="404">No model items are found for the specified model or the row index range is invalid.</response>
        [HttpGet("{modelid}/inRange")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByRowIndexRange(Guid modelid, int startIndex = 0, int endIndex = -1)
        {
            var modelItems = await context.ModelItems
                .Where(item => item.ModelID == modelid)
                .ToListAsync();

            if (endIndex == -1)
            {
                endIndex = modelItems.Count() - 1;
            }

            var res = modelItems.Where(item => startIndex <= item.ID && item.ID <= endIndex).Select(item => item.AsDTO());

            return Ok(res);
        }

        /// <summary>
        /// Creates a new model item
        /// </summary>
        /// <remarks>
        /// Creates a new model item using the provided data and associates it with the specified model.
        /// </remarks>
        /// <param name="modelid">The GUID of the model to which the new model item will be associated.</param>
        /// <param name="modelItemDTO">The data for creating the new model item with nullable 'batchedModelItemID' field.</param>
        /// <returns>The created model item.</returns>
        /// <response code="201">The created model item with its information.</response>
        /// <response code="400">The request data is invalid or incomplete.</response>
        [HttpPost("{modelid}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public ActionResult<ModelItem> Create(Guid modelid, ModelItemCreateDTO modelItemDTO)
        {
            ModelItem modelItem = modelItemDTO.AsModelDB(modelid);

            context.ModelItems.Add(modelItem);
            context.SaveChanges();

            return modelItem;
        }

        /// <summary>
        /// Gets a list of model items of a specific model that contain a given hit point.
        /// </summary>
        /// <param name="modelid">The ID of the model.</param>
        /// <param name="hitPoint">The hit point to check for containment.</param>
        /// <returns>Returns a list of <see cref="ModelItemGetDTO"/> representing the model items containing the hit point.</returns>
        [HttpGet("{modelid}/hitPoint")]
        public ActionResult<IEnumerable<ModelItemGetDTO>> GetByHitPoint(Guid modelid, Point hitPoint)
        {
            IEnumerable<ModelItemGetDTO> modelItems = context.ModelItems
                .Where(item => item.ModelID == modelid && item.TouchedBy(hitPoint))
                .Select(item => item.AsDTO());

            if (modelItems.Any())
            {
                return modelItems.ToList();
            }
            return NotFound();
        }

        /// <summary>
        /// Get model items batched by the specific model item
        /// </summary>
        /// <remarks>
        /// Get the model items associated with the specific model item.
        /// </remarks>
        /// <param name="batchedmodelitemid">The GUID of the batched model item for which to get associated model items.</param>
        /// <returns>A list of model items associated with the specified batching model item.</returns>
        /// <response code="200">The model items for the specified batching model item.</response>
        /// <response code="404">No model items are found for the specified batching model item.</response>
        [HttpGet("batchedModelItem")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<ModelItemGetDTO>> GetByBatchedModelItem( Guid batchedmodelitemid)
        {
            IEnumerable<ModelItemGetDTO> modelItems = context.ModelItems
                .Where(item => item.BatchedModelItemID == batchedmodelitemid)
                .Select(item => item.AsDTO());

            if (modelItems.Any())
            {
                return modelItems.ToList();
            }
            return NotFound();
        }

        /// <summary>
        /// Get a specific model item by its model GUID and hierarchy index
        /// </summary>
        /// <remarks>
        /// Get a specific model item based on its hierarchy index within the specified model.
        /// </remarks>
        /// <param name="modelid">The GUID of the model for which to get the model item.</param>
        /// <param name="hierachyindex">The hierarchy index of the model item within the specified model.</param>
        /// <returns>The requested model item.</returns>
        /// <response code="200">The requested model item.</response>
        /// <response code="404">No model item is found for the specified model and hierarchy index.</response>
        [HttpGet("{modelid}/hierachyIndex")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<ModelItemGetDTO> GetByHierachyIndex(Guid modelid, int hierachyindex)
        {
            ModelItem? modelItem = context.ModelItems
                .FirstOrDefault(item => item.ModelID == modelid && item.HierarchyIndex == hierachyindex);

            if (modelItem == null)
            {
                return NotFound();
            }
            return modelItem.AsDTO();
        }

        /// <summary>
        /// Update a model item by its hierachy index
        /// </summary>
        /// <param name="hierachyindex">Hierachy index of the model item</param>
        /// <param name="modelItemDTO"></param>
        /// <returns></returns>
        [HttpPut("{modelid}/{hierachyindex}")]
        public async Task<IActionResult> Update(Guid modelid, int hierachyindex, [FromBody] ModelItemCreateDTO modelItemDTO)
        {
            ModelItem? modelItem = await context.ModelItems
                .FirstOrDefaultAsync(item => item.ModelID == modelid && item.HierarchyIndex == hierachyindex);

            if (modelItem == null)
            {
                return NotFound();
            }

            modelItem = modelItemDTO.AsModelDB(modelid);

            await context.SaveChangesAsync();
            return Ok(new { result = "Update successfully", updatedModelItem = modelItemDTO });
        }

        /// <summary>
        /// Update one or many attributes of a model item
        /// </summary>
        /// <param name="modelid">ID of the model</param>
        /// <param name="hierachyindex">Hierachy index of the model item</param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        [HttpPatch("{modelid}/{hierachyindex}")]
        public async Task<IActionResult> Patch(Guid modelid, int hierachyindex, [FromBody] JsonPatchDocument<ModelItemCreateDTO> patchDocument)
        {
            ModelItem? existingModelItem = await context.ModelItems.FirstOrDefaultAsync(item => item.ModelID == modelid && item.HierarchyIndex == hierachyindex);

            if (existingModelItem == null)
            {
                return NotFound();
            }
            ModelItemCreateDTO modelItemDTO = existingModelItem.AsDTO();

            patchDocument.ApplyTo(modelItemDTO, ModelState);


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            existingModelItem = modelItemDTO.AsModelDB(modelid);

            await context.SaveChangesAsync();

            return Ok(new { result = "Update successfully", updatedModelItem = modelItemDTO });
        }

        /// <summary>
        /// Delete a model item by its hierachy index
        /// </summary>
        /// <param name="modelid">The ID of the model</param>
        /// <param name="hierachyindex">The hierachy index of the model item</param>
        /// <returns></returns>
        [HttpDelete("{modelid}/{hierachyindex}")]
        public async Task<IActionResult> Delete(Guid modelid, int hierachyindex)
        {
            ModelItem? modelItem = await context.ModelItems
                .FirstOrDefaultAsync(item => item.ModelID == modelid && item.HierarchyIndex == hierachyindex);

            if (modelItem == null)
            {
                return NotFound();
            }

            context.ModelItems.Remove(modelItem);
            await context.SaveChangesAsync();

            return Ok("Delete successfully");
        }
    }
}
