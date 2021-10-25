using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using ToDoApp.Common.Fakers;
using ToDoApp.Integration.Setup;
using ToDoApp.Models;
using Xunit;

namespace ToDoApp.Integration.Controllers
{
    public class TodosControllerTests : TestBase
    {
        // O código colocado no construtor será executado antes de cada teste e antes do InitializeAsync do TestBase
        public TodosControllerTests(ApiWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task GetShouldReturnStatusCode200Ok()
        {
            var response = await Client.GetAsync("v1/todos");

            response.Should().Be200Ok();
        }

        [Fact]
        public async Task GetAsyncShouldReturnAllTodosInDatabase()
        {
            var todosExpected = new TodoBuilder().Generate(3);
            await Context.Todos.AddRangeAsync(todosExpected);
            await Context.SaveChangesAsync();

            var response = await Client.GetFromJsonAsync<List<Todo>>("v1/todos");

            response.Should().BeEquivalentTo(todosExpected, options => options.Excluding(x => x.CreationDate));
        }

        [Fact]
        public async Task GetAsyncByIdShouldReturn404NotFoundWhenToDoItemDoesntExists()
        {
            var todoItem = new TodoBuilder().Generate();
            await Context.Todos.AddAsync(todoItem);
            await Context.SaveChangesAsync();
            var invalidToDoItemId = todoItem.Id + 1;

            var response = await Client.GetAsync($"v1/todos/{invalidToDoItemId}");

            response.Should().Be404NotFound();
        }

        [Fact]
        public async Task GetAsyncByIdShouldReturn200OkWithToDoItemWhenExistsOnDatabase()
        {
            var todoItems = new TodoBuilder().Generate(3);
            await Context.Todos.AddRangeAsync(todoItems);
            await Context.SaveChangesAsync();
            var todoItemExpected = todoItems.First();

            var response = await Client.GetAsync($"v1/todos/{todoItemExpected.Id}");

            response.Should().Be200Ok();
            var todoItemReturned = await response.Content.ReadFromJsonAsync<Todo>();
            todoItemReturned.Should().BeEquivalentTo(todoItemExpected, options
                => options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromSeconds(1)))
                    .WhenTypeIs<DateTime>());
        }

        [Fact]
        public async Task PostAsyncShouldReturn201CreatedIfPersistsCorrectly()
        {
            var todoPost = new TodoRequestBuilder().Generate();

            var response = await Client.PostAsJsonAsync("v1/todos", todoPost);

            response.Should().Be201Created();
        }

        [Fact]
        public async Task PostAsyncShouldReturnLocationHeaderWithUrlToGetToDoItemCreaated()
        {
            var todoPost = new TodoRequestBuilder().Generate();


            var response = await Client.PostAsJsonAsync("v1/todos", todoPost);


            var todoOnDatabase = Context.Todos.FirstOrDefault(x => x.Title == todoPost.Title);

            response.Headers.GetValues("Location").Should().NotBeEmpty()
                .And.Contain($"/v1/todos/{todoOnDatabase.Id}");
        }

        [Fact]
        public async Task PostAsyncShouldReturnTodoItemCreated()
        {
            var todoPost = new TodoRequestBuilder().Generate();


            var response = await Client.PostAsJsonAsync("v1/todos", todoPost);


            var todoReturned = await response.Content.ReadFromJsonAsync<Todo>();
            var todoOnDatabase = Context.Todos.FirstOrDefault(x => x.Title == todoPost.Title);

            todoReturned.Should().BeEquivalentTo(todoOnDatabase, options
                => options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromSeconds(1)))
                    .WhenTypeIs<DateTime>());
        }
    }
}