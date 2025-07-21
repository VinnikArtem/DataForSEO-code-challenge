using Dispatcher.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Dispatcher.DAL.EF
{
    public class ApplicationContext : DbContext
    {
        public DbSet<SuperTask> SuperTasks { get; set; }

        public DbSet<FileProcessingTask> FileProcessingTasks { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
