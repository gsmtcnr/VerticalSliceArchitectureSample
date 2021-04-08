using Microsoft.AspNetCore.Mvc;

namespace VerticalSliceArchitectureSample.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class BaseApiController : ControllerBase
    {
    }
}
