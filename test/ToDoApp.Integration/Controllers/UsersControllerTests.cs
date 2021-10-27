using System;
using System.Collections.Generic;
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
    }
}