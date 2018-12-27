using System;
using Microsoft.EntityFrameworkCore;
using TodoServices.Domain.Model;

namespace TodoServices.Domain.Postgres
{
    public class PostgresDbContext : SqlDbContext
    {
        public PostgresDbContext(DbContextOptions options) : base(options)
        { }

        protected override void DescribeFieldTypes(ModelBuilder modelBuilder)
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
                .HasColumnType("money")
                .IsRequired();
            modelBuilder.Entity<Todo>()
                .Property(p => p.Cost)
                .HasColumnType("money");

        }
    }
}
