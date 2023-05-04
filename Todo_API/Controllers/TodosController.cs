
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoLibrary.DataAccess;
using TodoLibrary.Models;

namespace Todo_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodosController : ControllerBase
    {
        private readonly ITodoData _data;
        private readonly ILogger<TodosController> _logger;
        public TodosController(ITodoData data, ILogger<TodosController> logger)
        {
            _data = data;
            _logger = logger;
        }

        private int GetUserId()
        {
            var userIdText = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(userIdText!);
        }

        //[AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<TodoModel>>> Get()
        {
            _logger.LogInformation("GET: api/Todos");
            try
            {
                var output = await _data.GetAllAssigned(GetUserId());
                return Ok(output);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The GET call to api/Todos faild");
                return BadRequest();
            }

        }

        [HttpGet("{todoId}")]
        public async Task<ActionResult<TodoModel>> Get(int todoId)
        {
            _logger.LogInformation("GET: api/Todos/{TodoId}", todoId);
            try
            {
                var output = await _data.GetOneAssigned(GetUserId(), todoId);
                return Ok(output);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The GET call to {ApiPAth} failed. The Id was {todoId}", $"api/Todos/Id", todoId);
                return BadRequest();
            }
        }

        // POST api/Todos
        [HttpPost]
        public async Task<ActionResult<TodoModel>> Post([FromBody] string task)
        {
            _logger.LogInformation("GET: api/Todos (Task: {Task})", task);
            try
            {
                var output = await _data.Create(GetUserId(), task);
                return Ok(output);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The POST call to api/todos failed. Task value was {Task}", task);
                return BadRequest();
            }
        }

        // PUT api/Todos/5
        [HttpPut("{todoId}")]
        public async Task<ActionResult> Put(int todoId, [FromBody] string task)
        {

            _logger.LogInformation("PUT: api/Todos/{TodoId} (Task: {Task})", todoId, task);
            try
            {
                await _data.UpdateTask(GetUserId(), todoId, task);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The PUT call to api/todos/{TodoId} failed. Task value was {Task}", todoId, task);
                return BadRequest();
            }
        }

        // PUT api/Todos/5/Complete
        [HttpPut("{todoId}/Complete")]
        public async Task<IActionResult> Complete(int todoId)
        {

            _logger.LogInformation("PUT: api/Todos/{TodoId}/Complete", todoId);
            try
            {
                await _data.CompleteTodo(GetUserId(), todoId);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The PUT call to api/todos/{TodoId}/Complete failed", todoId);
                return BadRequest();
            }
        }

        [HttpPut("{todoId}/UndoComplete")]
        public async Task<IActionResult> UndoComplete(int todoId)
        {

            _logger.LogInformation("PUT: api/Todos/{TodoId}/UndoComplete", todoId);
            try
            {
                await _data.UndoComplete(GetUserId(), todoId);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The PUT call to api/todos/{TodoId}/UndoComplete failed", todoId);
                return BadRequest();
            }
        }

        // DELETE api/Todos/5
        [HttpDelete("{todoId}")]
        public async Task<IActionResult> Delete(int todoId)
        {
            _logger.LogInformation("DELETE: api/Todos/{TodoId}", todoId);
            try
            {
                await _data.Delete(GetUserId(), todoId);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The DELETE call to api/todos/{TodoId} failed", todoId);
                return BadRequest();
            }
        }
    }

}