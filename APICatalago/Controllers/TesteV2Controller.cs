using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APICatalago.Controllers
{
    [Route("api/v{version:apiVersion}/teste")]
    [ApiController]
    [ApiVersion("2.0")]
    public class TesteV2Controller : ControllerBase
    {
        [HttpGet]
        public string GetVersion()
        {
            return "Teste v1 GET Api versão 2.0";
        }
    }
}
