using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class BaseController : ControllerBase
{

}