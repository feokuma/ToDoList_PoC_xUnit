using AutoBogus;
using ToDoApp.Models;

namespace ToDoApp.Common.Fakers
{
    public class TodoRequestBuilder : AutoFaker<TodoRequest>
    {
        public TodoRequestBuilder()
        {
            RuleFor(x => x.Title, faker => faker.Lorem.Word());
        }
    }
}