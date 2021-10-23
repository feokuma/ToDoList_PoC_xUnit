using System.Data.Entity.ModelConfiguration;
using ToDoApp.Models;

namespace ToDoApp.Data.Configurations
{
    public class ToDoMap : EntityTypeConfiguration<Todo>
    {
        public ToDoMap()
        {
            ToTable("Todos");

            HasKey(key => key.Id);

            Property(p => p.Title)
                .IsRequired()
                .HasColumnName("NM_Title");

            Property(p => p.Done)
                .HasColumnName("BL_Done");

            Property(p => p.CreationDate)
                .HasColumnName("DT_Date")
                .IsRequired();
        }
    }
}