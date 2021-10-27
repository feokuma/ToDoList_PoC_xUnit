using System.Data.Entity.ModelConfiguration;
using ToDoApp.Models;

namespace ToDoApp.Data.Configurations
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            ToTable("Users");

            HasKey(key => key.Id);

            Property(p => p.Name)
                .HasColumnName("NM_Name")
                .IsRequired();

            Property(p => p.CreationDate)
                .HasColumnName("DT_CreationDate");
        }
    }
}