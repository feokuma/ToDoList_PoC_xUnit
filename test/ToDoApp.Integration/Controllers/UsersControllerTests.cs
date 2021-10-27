using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using ToDoApp.Common;
using ToDoApp.Integration.Setup;
using ToDoApp.Models;
using Xunit;

namespace ToDoApp.Integration.Controllers
{
	public class UserControllerTests : TestBase
	{
		public UserControllerTests(ApiWebApplicationFactory factory) : base(factory) { }

		[Fact]
		public async Task GetAsyncShouldReturnListOfUsersWithStatusCode200Ok()
		{
			var users = new UserBuilder().Generate(3);
			await Context.Users.AddRangeAsync(users);
			await Context.SaveChangesAsync();

			var response = await Client.GetAsync("v1/users");

			response.Should().Be200Ok();
			var usersFromApi = await response.Content.ReadFromJsonAsync<List<User>>();
			usersFromApi.Should().BeEquivalentTo(users, options
				=> options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromSeconds(1)))
					.WhenTypeIs<DateTime>());
		}

		[Fact]
		public async Task GetAsyncByIdShouldReturn404NotFoundWhenUserDoesntExists()
		{
			var user = new UserBuilder().Generate();
			await Context.Users.AddAsync(user);
			await Context.SaveChangesAsync();
			var invalidUserId = user.Id + 1;

			var response = await Client.GetAsync($"v1/users/{invalidUserId}");

			response.Should().Be404NotFound();
		}

		[Fact]
		public async Task GetAsyncByIdShouldReturn200OkWithUserWhenExistsOnDatabase()
		{
			var users = new UserBuilder().Generate(3);
			await Context.Users.AddRangeAsync(users);
			await Context.SaveChangesAsync();
			var userExpected = users.First();

			var response = await Client.GetAsync($"v1/users/{userExpected.Id}");

			response.Should().Be200Ok();
			var userReturned = await response.Content.ReadFromJsonAsync<User>();
		    userReturned.Should().BeEquivalentTo(userExpected, options
				=> options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromSeconds(1)))
					.WhenTypeIs<DateTime>());
		}

		[Fact]
		public async Task PostAsyncShouldCreateUserOnDatabaseAndReturnEntityFromAPI()
		{
			var user = new UserBuilder().Generate();

			var userReturned = await Client.PostAsJsonAsync("v1/users", user);

			var userOnDatabase = Context.Users.First(x => x.Name == user.Name);
			userOnDatabase.Should().BeEquivalentTo(userOnDatabase, options
				=> options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromSeconds(1)))
					.WhenTypeIs<DateTime>());
		}
	}
}