
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostgisAPI.DTO;
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

        /// <summary>
        /// Initialize a controller for model item
        /// </summary>
        /// <param name="dbContext"></param>
        public ModelItemController(ApiDbContext dbContext)
        {
            context = dbContext;
        }

        /// <summary>
        /// Get a list of model items for a specific model.
        /// </summary>
        /// <param name="modelid">The ID of the model.</param>
        /// <returns>Returns a list of <see cref="ModelItemGetDTO"/> representing the model items.</returns>
        [HttpGet("{modelid}")]
        public ActionResult<IEnumerable<ModelItemGetDTO>> GetAll(Guid modelid)
        {
            IEnumerable<ModelItemGetDTO> modelItemsDTO = context.ModelItems
                .Where(item => item.ModelID == modelid)
                .Select(item => item.AsDTO());

            if (modelItemsDTO.Any())
            {
                return modelItemsDTO.ToList();
            }

            return NotFound();
        }

        /// <summary>
        /// Gets a specific model item by its model item ID.
        /// </summary>
        /// <param name="modelid">The ID of the model.</param>
        /// <param name="modelitemid">The ID of the model item.</param>
        /// <returns>Returns a <see cref="ModelItemGetDTO"/> representing the model item.</returns>
        [HttpGet("{modelid}/{modelitemid}")]
        public ActionResult<ModelItemGetDTO> GetById(Guid modelid, Guid modelitemid)
        {
            ModelItem? modelItem = context.ModelItems
                .FirstOrDefault(item => item.ModelID == modelid && item.ModelItemID == modelitemid);

            if (modelItem == null)
            {
                return NotFound();
            }

            return modelItem.AsDTO();
        }

        /// <summary>
        /// Create a new model item for a specific model.
        /// </summary>
        /// <param name="modelid">The ID of the model.</param>
        /// <param name="modelItemDTO">The data to create the new model item.</param>
        /// <returns>Returns the created <see cref="ModelItem"/>.</returns>
        [HttpPost("{modelid}")]
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
        [HttpPost("{modelid}/hitpoint")]
        public ActionResult<IEnumerable<ModelItemGetDTO>> GetByHitPoint(Guid modelid, Point hitPoint)
        {
            IEnumerable<ModelItemGetDTO> modelItems = context.ModelItems
                .Where(item => item.ModelID == modelid && item.Contains(hitPoint))
                .Select(item => item.AsDTO());

            if (modelItems.Any())
            {
                return modelItems.ToList();
            }
            return NotFound();
        }

        /// <summary>
        /// Gets a list of model items for a specific model and batched model item ID.
        /// </summary>
        /// <param name="modelid">The ID of the model.</param>
        /// <param name="batchedmodelitemid">The ID of the batched model item.</param>
        /// <returns>Returns a list of <see cref="ModelItemGetDTO"/> representing the model items for the specified batched model item.</returns>
        [HttpPost("{modelid}/batchedmodelitem")]
        public ActionResult<IEnumerable<ModelItemGetDTO>> GetByBatchedModelItem(Guid modelid, Guid batchedmodelitemid)
        {
            IEnumerable<ModelItemGetDTO> modelItems = context.ModelItems
                .Where(item => item.ModelID == modelid && item.BatchedModelItemID == batchedmodelitemid)
                .Select(item => item.AsDTO());

            if (modelItems.Any())
            {
                return modelItems.ToList();
            }
            return NotFound();
        }

        /// <summary>
        /// Gets a specific model item by its model ID and hierarchy index.
        /// </summary>
        /// <param name="modelid">The ID of the model.</param>
        /// <param name="hierachyindex">The hierarchy index of the model item.</param>
        /// <returns>Returns a <see cref="ModelItemGetDTO"/> representing the model item.</returns>
        [HttpPost("{modelid}/hierachyindex")]
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
