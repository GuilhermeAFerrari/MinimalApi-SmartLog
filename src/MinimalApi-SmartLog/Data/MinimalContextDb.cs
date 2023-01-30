using Microsoft.EntityFrameworkCore;
using MinimalApi_SmartLog.Models;

namespace MinimalApi_SmartLog.Data;

public class MinimalContextDb : DbContext
{
    public MinimalContextDb(DbContextOptions<MinimalContextDb> options) : base(options) { }

    public DbSet<Log> Logs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Log>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<Log>()
            .Property(p => p.IdSecondary)
            .HasColumnType("uniqueidentifier");

        modelBuilder.Entity<Log>()
            .Property(p => p.Message)
            .IsRequired()
            .HasColumnType("varchar(200)");

        modelBuilder.Entity<Log>()
            .Property(p => p.StackTrace)
            .HasColumnType("varchar(200)");

        modelBuilder.Entity<Log>()
            .Property(p => p.Request)
            .HasColumnType("varchar(200)");

        modelBuilder.Entity<Log>()
            .Property(p => p.Response)
            .HasColumnType("varchar(200)");

        modelBuilder.Entity<Log>()
            .Property(p => p.Level)
            .HasColumnType("int");

        modelBuilder.Entity<Log>()
            .Property(p => p.Date)
            .IsRequired()
            .HasColumnType("datetime")
            .HasDefaultValueSql("(sysutcdatetime())");

        modelBuilder.Entity<Log>()
            .Property(p => p.Active)
            .IsRequired()
            .HasColumnType("bit")
            .HasDefaultValueSql("1");
    }
}