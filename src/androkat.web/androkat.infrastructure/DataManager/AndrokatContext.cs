using androkat.infrastructure.Model.SQLite;
using Microsoft.EntityFrameworkCore;

namespace androkat.infrastructure.DataManager;

public class AndrokatContext : DbContext
{
	//public AndrokatContext() : base()
	//{
	//}

	public AndrokatContext(DbContextOptions<AndrokatContext> options) : base(options)
	{
		Database.EnsureCreated();
	}

	public virtual DbSet<Napiolvaso> Content { get; set; }
	public DbSet<FixContent> FixContent { get; set; }
	public DbSet<Maiszent> MaiSzent { get; set; }
	public DbSet<Ima> ImaContent { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// Map table names
		modelBuilder.Entity<Napiolvaso>().ToTable("napiolvaso");
		modelBuilder.Entity<Napiolvaso>(entity =>
		{
			entity.HasKey(e => e.Nid);
			entity.Property(u => u.Cim).HasColumnType("text");
			entity.Property(u => u.Idezet).HasColumnType("longtext");
			entity.Property(u => u.Inserted).HasColumnType("timestamp");
		});

		modelBuilder.Entity<FixContent>().ToTable("napifixolvaso");
		modelBuilder.Entity<FixContent>(entity =>
		{
			entity.HasKey(e => e.Nid);
			entity.Property(u => u.Cim).HasColumnType("text");
			entity.Property(u => u.Idezet).HasColumnType("longtext");
		});

		modelBuilder.Entity<Maiszent>().ToTable("maiszent");
		modelBuilder.Entity<Maiszent>(entity =>
		{
			entity.HasKey(e => e.Nid);
			entity.Property(u => u.Cim).HasColumnType("text");
			entity.Property(u => u.Idezet).HasColumnType("longtext");
			entity.Property(u => u.Inserted).HasColumnType("timestamp");
		});

		modelBuilder.Entity<Ima>().ToTable("ima");
		modelBuilder.Entity<Ima>().HasKey(u => u.Nid);

		base.OnModelCreating(modelBuilder);
	}
}