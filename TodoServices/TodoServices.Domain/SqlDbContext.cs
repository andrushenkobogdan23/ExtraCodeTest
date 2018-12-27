using TodoServices.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace TodoServices.Domain
{
    public class SqlDbContext : DbContext
    {
        public SqlDbContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Todo> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Todo");

            DescribeTodo(modelBuilder);

            DoIndexes(modelBuilder);

           
        }

        private void DoIndexes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Todo>()
                .HasIndex(i => new { i.Priority, i.CreateDate, i.CompleteDate });

            modelBuilder.Entity<Todo>()
                .HasIndex(i => i.AuthorId);

            modelBuilder.Entity<Todo>()
                .HasIndex(i => i.PerformerId);

            modelBuilder.Entity<Todo>()
               .HasIndex(i => i.ParentId);

        }

        private void DescribeTodo(ModelBuilder modelBuilder)
        {
            // fields
            modelBuilder.Entity<Todo>()
                .Property(p => p.Title)
                .HasMaxLength(16)
                .IsRequired();

            modelBuilder.Entity<Todo>()
                .Property(p => p.Description)
                .HasMaxLength(32)
                .IsRequired();

            modelBuilder.Entity<Todo>()
               .HasOne<Todo>(p => p.Parent)
               .WithMany(p => p.Childs)
               .HasForeignKey(p => p.ParentId);

            //modelBuilder.Entity<Todo>()
            //   .HasMany(p => p.Childs)
            //   .WithOne(x => x.Parent)
            //   .HasForeignKey(x => x.ParentId)
            //   .IsRequired(false);
            DescribeFieldTypes(modelBuilder);
        }

        protected virtual void DescribeFieldTypes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Todo>()
                .Property(p => p.CreateDate)
                .HasColumnType("date")
                .IsRequired();
            modelBuilder.Entity<Todo>()
                .Property(p => p.CompleteDate)
                .HasColumnType("date");
            modelBuilder.Entity<Todo>()
                .Property(p => p.EstimatedCost)
                .HasColumnType("smallmoney")
                .IsRequired();
            modelBuilder.Entity<Todo>()
                .Property(p => p.Cost)
                .HasColumnType("smallmoney");
            //modelBuilder.Entity<Todo>()
            //   .Property(p => p.ChildCost)
            //   .HasColumnType("smallmoney");
        }
    }
}