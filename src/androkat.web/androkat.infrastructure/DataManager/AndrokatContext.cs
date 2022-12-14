using androkat.infrastructure.Model.SQLite;
using Microsoft.EntityFrameworkCore;

namespace androkat.infrastructure.DataManager;

public class AndrokatContext : DbContext
{
	public AndrokatContext(DbContextOptions<AndrokatContext> options) : base(options)
	{
		Database.EnsureCreated();
	}

	public virtual DbSet<Napiolvaso> Content { get; set; }
	public DbSet<FixContent> FixContent { get; set; }
	public DbSet<Maiszent> MaiSzent { get; set; }
	public DbSet<Ima> ImaContent { get; set; }
	public DbSet<Video> VideoContent { get; set; }
	public DbSet<Radio> RadioMusor { get; set; }
	public DbSet<Systeminfo> systeminfo { get; set; }

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

		modelBuilder.Entity<Video>().ToTable("video");
		modelBuilder.Entity<Video>().HasKey(u => u.Nid);

		modelBuilder.Entity<Radio>().ToTable("radio");
		modelBuilder.Entity<Radio>().HasKey(u => u.Nid);

		modelBuilder.Entity<Systeminfo>().ToTable("systeminfo");
		modelBuilder.Entity<Systeminfo>().HasKey(u => u.Id);

		base.OnModelCreating(modelBuilder);
	}
}