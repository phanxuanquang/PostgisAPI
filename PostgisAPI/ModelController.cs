using Microsoft.AspNetCore.Mvc;

namespace PostgisAPI
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
    }
}
