using System;
using System.Threading.Tasks;
using TodoServices.Shared.Dto;
using TodoServices.Command.Handlers;
using TodoServices.Command.Commands;
using Shared.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SerilogTimings;
using Serilog;
using Newtonsoft.Json;
using Shared.Common.Exceptions;
using Shared.Common.Sql;
using System.Net.Http;
using System.IO;

namespace TodoServices.Command.Controllers
{
    [Route("")]
    [Authorize]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    public class TodoCmdController : Controller
    {
        TodoCommandHadnlers _hadnler;

        public TodoCmdController(IUnitOfWork work)
        {
            _hadnler = new TodoCommandHadnlers(work);
            
            
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddTodoCommand cmd)
        {

            var command = new TodoAddCommand(cmd.AuthorId, cmd.Title, cmd.Description, cmd.EstimatedCost, cmd.Priority,cmd.ParentId);

            try
            {
                using (var op = Operation.At(Serilog.Events.LogEventLevel.Debug).Begin("add cmd: {0}", JsonConvert.SerializeObject(cmd)))
                {
                    await _hadnler.Handle(command, ModelState);
                    if (ModelState.IsValid)
                    {
                        op.Complete();
                        var link = new Uri($"{APIUrls.Todos}/{command.Id}");
                        return Created(link, cmd);
                    }
                    else
                        return BadRequest(ModelState);
                }
            }
            catch (AggregateNotFoundException fe)
            {
                return NotFoundResponse(fe);
            }
            catch (AggregateReadonlyException re)
            {
                return ReadOnlyResponse(re);
            }
        }


        [HttpPut("finish/{id}")]
        public async Task<IActionResult> Finish(int id, [FromBody]CompleteTodoCommand cmd)
        {
            var command = new TodoCompleteCommand(cmd.Id, cmd.PerformerId, cmd.Cost,cmd.ParentId);

            try
            {
                using (var op = Operation.At(Serilog.Events.LogEventLevel.Debug).Begin("finish cmd: {0}", JsonConvert.SerializeObject(cmd)))
                {
                    await _hadnler.Handle(command, ModelState);
                    if (ModelState.IsValid)
                    {
                        op.Complete();
                        return Ok();
                    }
                    else
                        return BadRequest(ModelState);
                }
            }
            catch (AggregateNotFoundException fe)
            {
                return NotFoundResponse(fe);
            }
            catch (AggregateReadonlyException re)
            {
                return ReadOnlyResponse(re);
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]UpdateTodoCommand cmd)
        {
            var command = new TodoUpdateCommand(id, cmd.AuthorId, cmd.Title, cmd.Description, cmd.EstimatedCost, cmd.Priority,cmd.ParentId);

            try
            {
                using (var op = Operation.At(Serilog.Events.LogEventLevel.Debug).Begin("update cmd: {0}", JsonConvert.SerializeObject(cmd)))
                {
                    await _hadnler.Handle(command, ModelState);
                    if (ModelState.IsValid)
                    {
                        op.Complete();
                        return Ok();
                    }
                    else
                        return BadRequest(ModelState);
                }
            }
            catch (AggregateNotFoundException fe)
            {
                return NotFoundResponse(fe);
            }
            catch (AggregateReadonlyException re)
            {
                return ReadOnlyResponse(re);
            }
        }

        [HttpDelete]
        [MapToApiVersion("1.1")]
        public async Task<IActionResult> Delete(int id, int userId)
        {
            try
            {
                using (var op = Operation.At(Serilog.Events.LogEventLevel.Debug).Begin("delete id={0}, userid={1}", id, userId))
                {
                    await _hadnler.Handle(id, userId);
                    op.Complete();
                }
                return Ok();
            }
            catch (AggregateNotFoundException fe)
            {
                return NotFoundResponse(fe);
            }
            catch (AggregateReadonlyException re)
            {
                return ReadOnlyResponse(re);
            }
        }


        private IActionResult ReadOnlyResponse(AggregateReadonlyException e)
        {
            Log.Error(e.Message);
            return BadRequest(e.ToJson());
        }

        private IActionResult NotFoundResponse(AggregateNotFoundException e)
        {
            Log.Error(e.Message);
            return NotFound(e.ToJson());
        }
    }
}
