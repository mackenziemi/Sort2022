using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Sort2022.Data.Models;

namespace Sort2022.Data
{
    public partial class sort2022Context : DbContext
    {
        private readonly string _connectionString;

        
        public sort2022Context()
        {
        }

        public sort2022Context(DbContextOptions<sort2022Context> options)
            : base(options)
        {
        }

        public sort2022Context(string connectionString)
        {
            _connectionString = connectionString;
        }

        public virtual DbSet<Sort2022.Data.Models.Task> Tasks { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString); 
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sort2022.Data.Models.Task>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(1024);

                entity.Property(e => e.IsCompleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
