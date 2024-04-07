using AttendenceSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendenceSystem.Confug
{
    public class InstructorTrackConfig: IEntityTypeConfiguration<InstructorTrack>
    {
        public void Configure(EntityTypeBuilder<InstructorTrack> builder)
        {
            builder.HasKey(s => new { s.TrackId, s.InstructorId });
        }
    }
}