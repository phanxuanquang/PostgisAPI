using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Index.HPRtree;
using Newtonsoft.Json;
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
            IEnumerable<ModelItemGetDTO> modelItemsDTO = context.ModelItems.Where(item => item.ModelID == modelid).Select(item => new ModelItemGetDTO
            {
                ModelID = item.ModelID,
                ModelItemID = item.ModelItemID,
                HierarchyIndex = item.HierarchyIndex,
                ParentHierachyIndex = item.ParentHierachyIndex,
                DisplayName = item.DisplayName,
                Path = item.Path,
                Color = JsonConvert.DeserializeObject<Color>(item.Color),
                Mesh = JsonConvert.DeserializeObject<Mesh>(item.Mesh),
                Matrix = item.Matrix,
                AABB = JsonConvert.DeserializeObject<AxisAlignedBoundingBox>(item.AABB),
                BatchedModelItemID = item.BatchedModelItemID,
                Properties = item.Properties,
                LastModifiedTime = item.LastModifiedTime
            });

            return modelItemsDTO.ToList();
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
            ModelItem? modelItem = context.ModelItems.FirstOrDefault(item => item.ModelID == modelid && item.ModelItemID == modelitemid);

            if (modelItem == null)
            {
                return NotFound();
            }

            ModelItemGetDTO modelItemDTO = new ModelItemGetDTO
            {
                ModelID = modelItem.ModelID,
                ModelItemID = modelItem.ModelItemID,
                HierarchyIndex = modelItem.HierarchyIndex,
                ParentHierachyIndex = modelItem.ParentHierachyIndex,
                DisplayName = modelItem.DisplayName,
                Path = modelItem.Path,
                Color = JsonConvert.DeserializeObject<Color>(modelItem.Color),
                Mesh = JsonConvert.DeserializeObject<Mesh>(modelItem.Mesh),
                Matrix = modelItem.Matrix,
                AABB = JsonConvert.DeserializeObject<AxisAlignedBoundingBox>(modelItem.AABB),
                BatchedModelItemID = modelItem.BatchedModelItemID,
                Properties = modelItem.Properties,
                LastModifiedTime = modelItem.LastModifiedTime
            };

            return modelItemDTO;
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
            ModelItem modelItem = new ModelItem
            {
                ModelItemID = Guid.NewGuid(),
                HierarchyIndex = modelItemDTO.HierarchyIndex,
                ParentHierachyIndex = modelItemDTO.ParentHierachyIndex,
                ModelID = modelid,
                BatchedModelItemID = modelItemDTO.BatchedModelItemID,
                DisplayName = modelItemDTO.DisplayName,
                Path = modelItemDTO.Path,
                Color = JsonConvert.SerializeObject(modelItemDTO.Color),
                Matrix = modelItemDTO.Matrix,
                AABB = JsonConvert.SerializeObject(modelItemDTO.AABB),
                Mesh = JsonConvert.SerializeObject(modelItemDTO.Mesh),
                Properties = modelItemDTO.Properties
            };

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
        public ActionResult<IEnumerable<ModelItemGetDTO>> GetByHitPoint(Guid modelid, PointZ hitPoint)
        {
            IEnumerable<ModelItemGetDTO> modelItems = context.ModelItems.Where(item => item.ModelID == modelid && item.Contains(hitPoint)).Select(item => new ModelItemGetDTO
            {
                ModelID = item.ModelID,
                ModelItemID = item.ModelItemID,
                HierarchyIndex = item.HierarchyIndex,
                ParentHierachyIndex = item.ParentHierachyIndex,
                DisplayName = item.DisplayName,
                Path = item.Path,
                Color = JsonConvert.DeserializeObject<Color>(item.Color),
                Mesh = JsonConvert.DeserializeObject<Mesh>(item.Mesh),
                Matrix = item.Matrix,
                AABB = JsonConvert.DeserializeObject<AxisAlignedBoundingBox>(item.AABB),
                BatchedModelItemID = item.BatchedModelItemID,
                Properties = item.Properties,
                LastModifiedTime = item.LastModifiedTime
            });
            return modelItems.ToList();
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
            IEnumerable<ModelItemGetDTO> modelItems = context.ModelItems.Where(item => item.ModelID == modelid && item.BatchedModelItemID == batchedmodelitemid).Select(item => new ModelItemGetDTO
            {
                ModelID = item.ModelID,
                ModelItemID = item.ModelItemID,
                HierarchyIndex = item.HierarchyIndex,
                ParentHierachyIndex = item.ParentHierachyIndex,
                DisplayName = item.DisplayName,
                Path = item.Path,
                Color = JsonConvert.DeserializeObject<Color>(item.Color),
                Mesh = JsonConvert.DeserializeObject<Mesh>(item.Mesh),
                Matrix = item.Matrix,
                AABB = JsonConvert.DeserializeObject<AxisAlignedBoundingBox>(item.AABB),
                BatchedModelItemID = item.BatchedModelItemID,
                Properties = item.Properties,
                LastModifiedTime = item.LastModifiedTime
            });

            if (modelItems.Any() )
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
            var modelItem = context.ModelItems.FirstOrDefault(item => item.ModelID == modelid && item.HierarchyIndex == hierachyindex);

            if (modelItem == null)
            {
                return NotFound();
            }

            ModelItemGetDTO modelItemDTO = new ModelItemGetDTO
            {
                ModelID = modelItem.ModelID,
                ModelItemID = modelItem.ModelItemID,
                HierarchyIndex = modelItem.HierarchyIndex,
                ParentHierachyIndex = modelItem.ParentHierachyIndex,
                DisplayName = modelItem.DisplayName,
                Path = modelItem.Path,
                Color = JsonConvert.DeserializeObject<Color>(modelItem.Color),
                Mesh = JsonConvert.DeserializeObject<Mesh>(modelItem.Mesh),
                Matrix = modelItem.Matrix,
                AABB = JsonConvert.DeserializeObject<AxisAlignedBoundingBox>(modelItem.AABB),
                BatchedModelItemID = modelItem.BatchedModelItemID,
                Properties = modelItem.Properties,
                LastModifiedTime = modelItem.LastModifiedTime
            };

            return modelItemDTO;
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
            var modelItem = await context.ModelItems.FirstOrDefaultAsync(item => item.ModelID == modelid && item.HierarchyIndex == hierachyindex);

            if (modelItem == null)
            {
                return NotFound();
            }

            modelItem.HierarchyIndex = modelItemDTO.HierarchyIndex;
            modelItem.ParentHierachyIndex = modelItemDTO.ParentHierachyIndex;
            modelItem.DisplayName = modelItemDTO.DisplayName;
            modelItem.Path = modelItemDTO.Path;
            modelItem.Color = JsonConvert.SerializeObject(modelItemDTO.Color);
            modelItem.Mesh = JsonConvert.SerializeObject(modelItemDTO.Mesh);
            modelItem.Matrix = modelItemDTO.Matrix;
            modelItem.AABB = JsonConvert.SerializeObject(modelItemDTO.AABB);
            modelItem.BatchedModelItemID = modelItemDTO.BatchedModelItemID;
            modelItem.Properties = modelItemDTO.Properties;
            modelItem.LastModifiedTime = DateTime.Now;

            await context.SaveChangesAsync();
            return Ok("Update successfully");
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
            var modelItem = await context.ModelItems.FirstOrDefaultAsync(item => item.ModelID == modelid && item.HierarchyIndex == hierachyindex);

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
