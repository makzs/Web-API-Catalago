using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APICatalago.Controllers
{
    [Route("api/teste")]
    [ApiController]
    [ApiVersion(3)]
    [ApiVersion(4)]
    public class TesteV3Controller : ControllerBase
    {
        [MapToApiVersion(3)]
        [HttpGet]
        public string GetVersion3()
        {
            return "Teste v1 GET Api versão 3.0";
        }

        [MapToApiVersion(4)]
        [HttpGet]
        public string GetVersion4()
        {
            return "Teste v1 GET Api versão 4.0";
        }
    }
}
