using Dispatcher.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dispatcher.DAL.Configurations
{
    public class SuperTaskConfiguration : IEntityTypeConfiguration<SuperTask>
    {
        public void Configure(EntityTypeBuilder<SuperTask> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.FileProcessingTasks).WithOne(x => x.SuperTask);
        }
    }
}
