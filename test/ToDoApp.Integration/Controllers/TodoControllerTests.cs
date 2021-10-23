using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using ToDoApp.Common.Fakers;
using ToDoApp.Integration.Setup;
using ToDoApp.Models;
using Xunit;

namespace ToDoApp.Integration.Controllers
{
    public class TodoControllerTests : TestBase
    {
        // O código colocado no construtor será executado antes de cada teste e antes do InitializeAsync do TestBase
        public TodoControllerTests(ApiWebApplicationFactory factory) : base(factory)
        {
            Console.WriteLine("teste");
        }

        [Fact]
        public async Task GetShouldReturnStatusCode200Ok()
        {
            var response = await Client.GetAsync("/v1/todos");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetAsyncShouldReturnAllTodosInDatabase()
        {
            var todosExpected = new TodoBuilder().Generate(3);
            await Context.Todos.AddRangeAsync(todosExpected);
            await Context.SaveChangesAsync();

            var result = await Client.GetFromJsonAsync<List<Todo>>("/v1/todos");

            _ = result.Should().BeEquivalentTo(todosExpected, options => options.Excluding(x => x.CreationDate));
        }
    }
}