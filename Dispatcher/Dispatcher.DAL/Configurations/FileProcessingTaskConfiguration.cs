using Dispatcher.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dispatcher.DAL.Configurations
{
    public class FileProcessingTaskConfiguration : IEntityTypeConfiguration<FileProcessingTask>
    {
        public void Configure(EntityTypeBuilder<FileProcessingTask> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.SuperTask).WithMany(x => x.FileProcessingTasks).HasForeignKey(x => x.SuperTaskId);
        }
    }
}
