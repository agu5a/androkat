using androkat.infrastructure.Model.SQLite;
using Microsoft.EntityFrameworkCore;

namespace androkat.infrastructure.DataManager;

public class AndrokatContext : DbContext
{
	public AndrokatContext(DbContextOptions<AndrokatContext> options) : base(options)
	{
		Database.EnsureCreated();
	}

    public virtual DbSet<Content> Content { get; set; }
	public DbSet<FixContent> FixContent { get; set; }
	public DbSet<Maiszent> MaiSzent { get; set; }
    public DbSet<ImaContent> ImaContent { get; set; }
    public DbSet<VideoContent> VideoContent { get; set; }
    public DbSet<RadioMusor> RadioMusor { get; set; }
    public DbSet<SystemInfo> SystemInfo { get; set; }
    public DbSet<TempContent> TempContent { get; set; }
    public DbSet<Admin> Admin { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AndrokatContext).Assembly);
	}
}