using Microsoft.AspNetCore.Mvc;
using Shared.Middleware.Filters;
using TodoServices.Shared.Dto;
using TodoServices.Message.Application;
using Microsoft.AspNetCore.Authorization;

namespace TodoServices.Message
{
    [Route("")]
    [Authorize]
    [ApiVersion("1.0")]
    [Benchmark]
    public class MessageController : Controller
    {

        public MessageController()
        { }


        [HttpPost]
        public IActionResult Post([FromBody]TodoMessage msg)
        {
            MessageLogic.Avatar.Send(msg);
            return Ok(msg);
        }
    }
}
