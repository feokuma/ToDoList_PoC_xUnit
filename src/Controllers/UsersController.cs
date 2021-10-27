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
    public class UsersController : ControllerBase
    {
        public AppDbContext Context { get; }

        public UsersController(AppDbContext context)
        {
            Context = context;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<User>>> GetAsync()
        {
            var users = await Context.Users.ToListAsync();
            return Ok(users);
        }
    }
}