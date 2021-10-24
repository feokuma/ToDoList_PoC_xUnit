using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Data;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Route("v1")]
    public class TodoController : ControllerBase
    {
        public AppDbContext Context { get; }

        public TodoController(AppDbContext context)
        {
            Context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("todos")]
        public async Task<ActionResult<List<Todo>>> GetAsync()
        {
            var todos = await Context
                .Todos
                .ToListAsync();

            return Ok(todos);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("todos")]
        public async Task<ActionResult<Todo>> PostAsync([FromBody] TodoRequest todoRequest)
        {
            var todo = new Todo { Title = todoRequest.Title };
            await Context.Todos.AddAsync(todo);
            await Context.SaveChangesAsync();

            return await Task.FromResult(Created($"/v1/todos/{todo.Id}", todo));
        }
    }
}