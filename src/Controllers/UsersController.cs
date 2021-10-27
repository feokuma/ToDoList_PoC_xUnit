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

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[Route("{id:long}")]
		public async Task<ActionResult<User>> GetAsync(long id)
		{
			var user = await Context.Users.FirstOrDefaultAsync(x => x.Id == id);

			if (user == null)
				return NotFound();

			return Ok(user);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public async Task<ActionResult<User>> PostAsync([FromBody] UserRequest userRequest)
		{
			var user = new User { Name = userRequest.Name };
			await Context.Users.AddAsync(user);
			await Context.SaveChangesAsync();

			return Created($"/v{ApiVersion.Default.MajorVersion}/users/{user.Id}", user);
		}
	}
}