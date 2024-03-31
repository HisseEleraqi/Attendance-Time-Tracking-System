using AttendenceSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendenceSystem.Confug
{
    public class UserConfug: IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
           builder.HasKey(u => u.Id);
           builder.Property(u => u.Name).IsRequired().HasMaxLength(50);

            builder.Property(u => u.Email).IsRequired().HasMaxLength(50);
            builder.HasIndex(u => u.Email).IsUnique();



        }
    }
}
