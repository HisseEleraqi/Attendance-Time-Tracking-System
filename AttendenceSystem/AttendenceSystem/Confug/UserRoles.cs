using AttendenceSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AttendenceSystem.Confug
{
    public class UserRoles: IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(s => new { s.UserId, s.RoleId });
        }
    }
}
