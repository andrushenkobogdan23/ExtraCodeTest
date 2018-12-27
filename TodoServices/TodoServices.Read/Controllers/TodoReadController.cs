using TodoServices.Read.Application;
using Microsoft.AspNetCore.Mvc;
using SerilogTimings;
using System.Collections.Generic;
using TodoServices.Domain;
using TodoServices.Shared.Dto;
using Newtonsoft.Json;
using Shared.Middleware.Filters;
using Microsoft.AspNetCore.Authorization;

namespace TodoServices.Read
{
    [Route("")]
    [Authorize]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    [Benchmark]
    public class TodoReadController : Controller
    {
        TodoReadLogic _logic;

        public TodoReadController(SqlDbContext context)
        {
            _logic = new TodoReadLogic(context);
        }

        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<TodoInfo> items = null;
            using (var op = Operation.At(Serilog.Events.LogEventLevel.Debug).Begin("load todos"))
            {
                items = _logic.Get();
                op.Complete();
            }
            return Ok(items);
        }

        [HttpGet("my/{userId}")]
        [MapToApiVersion("1.1")]
        public IActionResult My(int userId)
        {
            IEnumerable<TodoInfo> items = null;
            using (var op = Operation.At(Serilog.Events.LogEventLevel.Debug).Begin("load my todo: ", userId))
            {
                items = _logic.My(userId);
                op.Complete();
            }
            return Ok(items);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            TodoInfo item = null;
            using (var op = Operation.At(Serilog.Events.LogEventLevel.Debug).Begin("load todo: ", id))
            {
                item = _logic.Get(id);
                op.Complete();
            }
            return Ok(item);
        }


        [HttpPost]
        public IActionResult Post([FromBody]BaseFilter filter)
        {
            IEnumerable<TodoInfo> items = null;
            using (var op = Operation.At(Serilog.Events.LogEventLevel.Debug).Begin("find todos: {0}", JsonConvert.SerializeObject(filter)))
            {
                items = _logic.Get(filter);
                op.Complete();
            }
            return Ok(items);
        }

        [HttpGet("states")]
        public IActionResult States()
        {
            IEnumerable<TodoState> items = null;
            using (var op = Operation.At(Serilog.Events.LogEventLevel.Debug).Begin("get todos states"))
            {
                items = MessageLogic.Avatar.TodoStates;
                op.Complete();
            }
            return Ok(items);
        }
    }
}
