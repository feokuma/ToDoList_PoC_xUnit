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
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class TodosController : ControllerBase
    {
        public AppDbContext Context { get; }

        public TodosController(AppDbContext context)
        {
            Context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Todo>>> GetAsync()
        {
            var todos = await Context
                .Todos
                .ToListAsync();

            return Ok(todos);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{id:long}")]
        public async Task<ActionResult<Todo>> GetAsync(long id)
        {
            var todoItem = await Context.Todos.FirstOrDefaultAsync(x => x.Id == id);

            if (todoItem == null)
                return NotFound();

            return Ok(todoItem);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Todo>> PostAsync([FromBody] TodoRequest todoRequest)
        {
            var todo = new Todo { Title = todoRequest.Title };
            await Context.Todos.AddAsync(todo);
            await Context.SaveChangesAsync();

            return Created($"/v{ApiVersion.Default.MajorVersion}/todos/{todo.Id}", todo);
        }
    }
}