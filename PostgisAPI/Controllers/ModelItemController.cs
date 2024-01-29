
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostgisAPI.DTO;
using PostgisAPI.Models;
using PostgisUltilities;

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
        /// Get detail of a model item
        /// </summary>
        /// <remarks>
        /// Get a specific model item based on its GUID.
        /// </remarks>
        /// <param name="modelitemid">The GUID of the model item to get.</param>
        /// <returns>The requested model item.</returns>
        /// <response code="200">The requested model item.</response>
        /// <response code="404">No model item is found for the requested GUID.</response>
        [HttpGet("guid")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<ModelItemGetDTO> GetByGuid(Guid modelitemid)
        {
            ModelItem? modelItem = context.ModelItems
                .FirstOrDefault(item => item.ModelItemID == modelitemid);

            if (modelItem == null)
            {
                return NotFound("Not found");
            }

            return modelItem.AsDTO();
        }

        /// <summary>
        /// Get all model item
        /// </summary>
        /// <remarks>
        /// Get model items associated with the specified model within the specified row index range (get all model items by default).
        /// </remarks>
        /// <param name="modelid">The GUID of the model for which to get model items.</param>
        /// <returns>A list of model items within the specified row index range.</returns>
        /// <response code="200">The list of model items within the specified row index range.</response>
        /// <response code="404">No model items are found for the specified model or the row index range is invalid.</response>
        [HttpGet("all")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByRowIndexRange(Guid modelid)
        {
            IEnumerable<ModelItemGetDTO> res = context.ModelItems.Where(item => item.ModelID == modelid).Select(item => item.AsDTO());

            int total = res.Count();
            if (total == 0)
            {
                return NotFound("Not found");
            }
            return Ok(new { total = total, modelItems = res });
        }

        /// <summary>
        /// Create a new model item
        /// </summary>
        /// <remarks>
        /// Creates a new model item using the provided data and associates it with the specified model.
        /// </remarks>
        /// <param name="modelid">The GUID of the model to which the new model item will be associated.</param>
        /// <param name="modelItemDTO">The data for creating the new model item with nullable 'batchedModelItemID' field.</param>
        /// <returns>The created model item.</returns>
        /// <response code="201">The created model item.</response>
        /// <response code="400">The request data is invalid or incomplete.</response>
        [HttpPost("create")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public ActionResult<ModelItem> Create(Guid modelid, ModelItemCreateDTO modelItemDTO)
        {
            ModelItem modelItem = modelItemDTO.AsModelDB(modelid);

            context.ModelItems.Add(modelItem);
            context.SaveChanges();

            return Created("Create successfully", modelItem.ModelItemID.ToString());
        }

        /// <summary>
        /// Find the model item hit by the given point
        /// </summary>
        /// <remarks>
        /// Get a specific model item based on the specified hit point within the specified model.
        /// </remarks>
        /// <param name="modelid">The GUID of the model for which to get the model item.</param>
        /// <param name="hitPoint">The hit point used to find the intersecting model item.</param>
        /// <returns>The model item hit by the given point.</returns>
        /// <response code="200">The model item hit by the given point.</response>
        /// <response code="404">No model item is found for the specified model and hit point.</response>
        [HttpPost("findByHitPoint")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<ModelItemGetDTO> GetByHitPoint(Guid modelid, PointZ hitPoint)
        {
            foreach (ModelItem modelItem in context.ModelItems)
            {
                if (modelItem.ModelID == modelid && modelItem.AsDTO().Mesh.TouchedBy(hitPoint))
                {
                    return modelItem.AsDTO();
                }
            }
            return NotFound("Not found");
        }

        /// <summary>
        /// Find model items batched by another model item
        /// </summary>
        /// <remarks>
        /// Get model items batched by the specific model item
        /// </remarks>
        /// <param name="modelid">The GUID of the model contains the model items.</param>
        /// <param name="batchedmodelitemid">The GUID of the batched model item for which to get associated model items.</param>
        /// <returns>A list of model items associated with the specified batching model item.</returns>
        /// <response code="200">The model items for the specified batching model item.</response>
        /// <response code="404">No model items are found for the specified batching model item.</response>
        [HttpGet("batchedModelItem")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<ModelItemGetDTO>> GetByBatchedModelItem(Guid modelid, Guid? batchedmodelitemid)
        {
            IEnumerable<ModelItemGetDTO> modelItems = null;
            if (batchedmodelitemid == null)
            {
                modelItems = context.ModelItems
                .Where(item => item.BatchedModelItemID == null && item.ModelID == modelid)
                .Select(item => item.AsDTO());
            }
            else
            {
                modelItems = context.ModelItems
                .Where(item => item.BatchedModelItemID == batchedmodelitemid && item.ModelID == modelid)
                .Select(item => item.AsDTO());
            }

            int total = modelItems.Count();
            if (total == 0)
            {
                return NotFound("Not found");
            }
            return Ok(new { total = total, modelItems = modelItems.ToList() });
        }

        /// <summary>
        /// Find a model item by its hierarchy index
        /// </summary>
        /// <remarks>
        /// Get a specific model item based on its hierarchy index within the specified model.
        /// </remarks>
        /// <param name="modelid">The GUID of the model for which to get the model item.</param>
        /// <param name="hierachyindex">The hierarchy index of the model item within the specified model.</param>
        /// <returns>The requested model item.</returns>
        /// <response code="200">The requested model item.</response>
        /// <response code="404">No model item is found for the specified model and hierarchy index.</response>
        [HttpGet("hierarchyIndex")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<ModelItemGetDTO> GetByHierachyIndex(Guid modelid, int hierachyindex = 0)
        {
            ModelItem? modelItem = context.ModelItems
                .FirstOrDefault(item => item.ModelID == modelid && item.HierarchyIndex == hierachyindex);

            if (modelItem == null)
            {
                return NotFound("Not found");
            }
            return modelItem.AsDTO();
        }

        /// <summary>
        /// Update a model item
        /// </summary>
        /// <param name="hierachyindex">Hierachy index of the model item</param>
        /// <param name="modelItemDTO"></param>
        /// <returns></returns>
        [HttpPut("hierarchyIndex")]
        public async Task<IActionResult> Update(Guid modelid, int hierachyindex, [FromBody] ModelItemCreateDTO modelItemDTO)
        {
            ModelItem? modelItem = await context.ModelItems
                .FirstOrDefaultAsync(item => item.ModelID == modelid && item.HierarchyIndex == hierachyindex);

            if (modelItem == null)
            {
                return NotFound("Not found");
            }

            modelItem = modelItemDTO.AsModelDB(modelid);

            await context.SaveChangesAsync();
            return Ok(new { result = "Update successfully", updatedModelItem = modelItemDTO });
        }

        /// <summary>
        /// Update a model item partially
        /// </summary>
        /// <remarks>
        /// Update one or many attributes of a model item. In case you want to update all attributes of a model item, use PUT method instead.
        /// </remarks>
        /// <param name="modelid">The GUID of the model for which to get the model item.</param>
        /// <param name="hierachyindex">The hierarchy index of the model item within the specified model.</param>
        /// <param name="patchData">The attributes to be update with new values. The 'operationType' and 'from' fields can be null.</param>
        /// <returns>Updating status</returns>
        /// <response code="200">Success</response>
        /// <response code="404">No model item is found for the specified model and hierarchy index.</response>
        [HttpPatch("hierarchyIndex")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Patch(Guid modelid, int hierachyindex, [FromBody] JsonPatchDocument<ModelItem> patchData)
        {
            ModelItem? modelItem = await context.ModelItems.Where(item => item.ModelID == modelid && item.HierarchyIndex == hierachyindex).FirstAsync();

            if (modelItem == null)
            {
                return NotFound("Not found");
            }

            patchData.ApplyTo(modelItem, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await context.SaveChangesAsync();

            return Ok(new { result = "Update successfully", updatedModelItem = modelItem });
        }

        /// <summary>
        /// Delete a model item
        /// </summary>
        /// <param name="modelid">The ID of the model</param>
        /// <param name="hierachyindex">The hierachy index of the model item</param>
        /// <returns></returns>
        [HttpDelete("hierarchyIndex")]
        public async Task<IActionResult> Delete(Guid modelid, int hierachyindex)
        {
            ModelItem? modelItem = await context.ModelItems
                .FirstOrDefaultAsync(item => item.ModelID == modelid && item.HierarchyIndex == hierachyindex);

            if (modelItem == null)
            {
                return NotFound("Not found");
            }
            string deletedModelItemID = modelItem.ModelItemID.ToString();

            context.ModelItems.Remove(modelItem);
            await context.SaveChangesAsync();

            return Ok(deletedModelItemID);
        }

        /// <summary>
        /// Batch model items
        /// </summary>
        /// <param name="modelId">The GUID of the model contains the model items to be batched.</param>
        /// <param name="modelItemBatchedModelItemPairs">The key-value pairs containing GUID of model items and GUID of batched model items.</param>
        /// <returns>A response indicating the success or failure of the operation.</returns>
        /// <response code="200">Success.</response>
        /// <response code="400">Invalid input data or bad request format.</response>
        /// <response code="404">No model items found.</response>
        /// <response code="500">An error occurred while processing the request.</response>
        [HttpPut("batchModelItems")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        public async Task<IActionResult> BatchModelItems(Guid modelId, [FromBody] Dictionary<Guid, Guid?> modelItemBatchedModelItemPairs)
        {
            List<ModelItem> modelItemsToUpdate = await context.ModelItems
                .Where(item => item.ModelID == modelId)
                .ToListAsync();

            if (modelItemsToUpdate == null || modelItemsToUpdate.Count == 0)
            {
                return NotFound("Not found");
            }

            foreach (ModelItem? modelItem in modelItemsToUpdate)
            {
                if (modelItemBatchedModelItemPairs.TryGetValue(modelItem.ModelItemID, out Guid? batchedModelItemId))
                {
                    modelItem.BatchedModelItemID = batchedModelItemId;
                }
            }

            await context.SaveChangesAsync();

            return Ok("Success");
        }

    }
}
