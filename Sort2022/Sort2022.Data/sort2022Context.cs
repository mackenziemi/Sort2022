using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Sort2022.Data.Models;

namespace Sort2022.Data
{
    public partial class sort2022Context : DbContext
    {
        public sort2022Context()
        {
        }

        public sort2022Context(DbContextOptions<sort2022Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Sort2022.Data.Models.Task> Tasks { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:sort2022.database.windows.net,1433;Initial Catalog=sort2022;Persist Security Info=False;User ID=sort2022admin;Password=S0rt2022!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
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
