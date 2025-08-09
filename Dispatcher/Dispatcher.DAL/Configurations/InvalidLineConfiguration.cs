using Dispatcher.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dispatcher.DAL.Configurations
{
    public class InvalidLineConfiguration : IEntityTypeConfiguration<InvalidLine>
    {
        public void Configure(EntityTypeBuilder<InvalidLine> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.FileProcessingTask).WithMany(x => x.InvalidLines).HasForeignKey(x => x.FileProcessingTaskId);

            builder.HasIndex(x => new { x.LineNumber, x.FileProcessingTaskId }).IsUnique();
        }
    }
}
