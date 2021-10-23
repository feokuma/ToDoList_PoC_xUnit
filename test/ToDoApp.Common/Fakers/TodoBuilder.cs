using AutoBogus;
using ToDoApp.Models;

namespace ToDoApp.Common.Fakers
{
    public class TodoBuilder : AutoFaker<Todo>
    {
        public TodoBuilder()
        {
            var id = 1;
            RuleFor(x => x.Id, faker => id++);
            RuleFor(x => x.Title, faker => faker.Name.JobType());
            RuleFor(x => x.Done, faker => faker.Random.Bool());
            RuleFor(x => x.CreationDate, faker => faker.Date.Future());
        }
    }
}