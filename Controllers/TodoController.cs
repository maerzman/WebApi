using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TodoClassLib;
using System.Linq;
using Microsoft.AspNetCore.Cors;

namespace TodoListWebApi.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    [EnableCors ("schirch")]
    public class TodoController : ControllerBase
    {
        private ITodoRepository repo;
        private readonly ILogger<TodoController> _logger;

        public TodoController (ILogger<TodoController> logger, ITodoRepository repo) {
            _logger = logger;
            this.repo = repo;
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteItem(int id) {
            if (id <= 0)
                return NotFound();

            repo.Delete(id);
            return Ok();
        }

        [HttpGet]
        public ActionResult<Todo[]> GetTodos() {
            return repo.ReadAll().ToArray();
        }

        [HttpGet("{id}")]
        public ActionResult<Todo> GetItem(int id) {
            if (id <= 0)
                return NotFound();
            
            return repo.Read(id);
        }

        [HttpPut("{id}")]
        public ActionResult<Todo> UpdateItem(Todo todo) {
            if (todo.Id <= 0 || repo.Read(todo.Id) == null)
                return NotFound();
            if (!repo.Update(todo))
                return BadRequest();
            return todo;
        }

        [HttpPost]
        public ActionResult<Todo> Post([FromBody] Todo todo) {
            var res = repo.Create(todo);
            if (res) {
                return todo;
            }
            return BadRequest();
        }
    }
}