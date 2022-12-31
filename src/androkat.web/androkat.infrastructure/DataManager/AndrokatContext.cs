using androkat.infrastructure.Model.SQLite;
using Microsoft.EntityFrameworkCore;

namespace androkat.infrastructure.DataManager;

public class AndrokatContext : DbContext
{
    public AndrokatContext() : base()
    {
    }

    public AndrokatContext(DbContextOptions<AndrokatContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public virtual DbSet<Napiolvaso> Content { get; set; }
   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Map table names
        modelBuilder.Entity<Napiolvaso>().ToTable("napiolvaso");
        modelBuilder.Entity<Napiolvaso>(entity =>
        {
            entity.HasKey(e => e.Nid);
            entity.Property(u => u.Cim).HasColumnType("text");
        });       

        base.OnModelCreating(modelBuilder);
    }
}