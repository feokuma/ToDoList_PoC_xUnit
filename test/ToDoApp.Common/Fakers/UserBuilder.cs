using System;
using AutoBogus;
using ToDoApp.Models;

namespace ToDoApp.Common
{
    public class UserBuilder : AutoFaker<User>
    {
        public UserBuilder()
        {
            var id = 1;
            RuleFor(x => x.Id, () => id++);
            RuleFor(x => x.Name, faker => faker.Person.FullName);
            RuleFor(x => x.CreationDate, () => DateTime.Now);
        }
    }
}