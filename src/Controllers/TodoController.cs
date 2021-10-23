using System.Collections.Generic;
using System.Threading.Tasks;
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
        [Route("todos")]
        public async Task<ActionResult<List<Todo>>> GetAsync()
        {
            var todos = await Context
                .Todos
                .ToListAsync();

            return Ok(todos);
        }
    }
}