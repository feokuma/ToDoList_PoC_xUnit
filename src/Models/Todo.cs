using System;

namespace ToDoApp.Models
{
    public class Todo : BaseEntity
    {
        public string Title { get; set; }
        public bool Done { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}