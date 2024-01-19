using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PostgisAPI.DTO;
using PostgisAPI.Models;
using PostgisAPI.Models.Supporters;
using System.Reflection;

namespace PostgisAPI.Controllers
{
    [ApiController]
    [Route("modelitem")]
    public class ModelItemController : ControllerBase
    {
        private readonly ApiDbContext context;

        public ModelItemController(ApiDbContext dbContext)
        {
            context = dbContext;
        }
        [HttpGet("{modelid}")]
        public ActionResult<IEnumerable<ModelItemGetDTO>> Get(Guid modelid)
        {
            IEnumerable<ModelItemGetDTO> modelItemsDTO = context.ModelItems.Where(item => item.ModelID == modelid).Select(item => new ModelItemGetDTO
            {
                ModelID = item.ModelID,
                ModelItemID = item.ModelItemID,
                HierarchyIndex = item.HierarchyIndex,
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
        [HttpPost("{modelid}")]
        public ActionResult<ModelItem> Post(Guid modelid, ModelItemCreateDTO modelItemDTO)
        {
            ModelItem modelItem = new ModelItem
            {
                ModelItemID = modelItemDTO.ModelItemID,
                HierarchyIndex = modelItemDTO.HierarchyIndex,
                ModelID = modelid, // 08f54e0f-c3a1-42ea-8943-f58cd74a2c75
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
    }
}
