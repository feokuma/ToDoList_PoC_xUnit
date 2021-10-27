using System;

namespace ToDoApp.Models
{
    public class User : BaseEntity
    {
        public string Name { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}