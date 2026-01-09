
using Microsoft.EntityFrameworkCore;
using VerzekeringApi.Models;

namespace VerzekeringApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Klant> Klanten => Set<Klant>();
    public DbSet<Opstalverzekering> Opstalverzekeringen => Set<Opstalverzekering>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Unieke index op PolisNummer
        modelBuilder.Entity<Opstalverzekering>()
            .HasIndex(o => o.PolisNummer)
            .IsUnique();

        // Relatie Klant -> Opstalverzekeringen
        modelBuilder.Entity<Opstalverzekering>()
            .HasOne(o => o.Klant)
            .WithMany(k => k.Opstalverzekeringen)
            .HasForeignKey(o => o.KlantId)
            .OnDelete(DeleteBehavior.Cascade);

        // Lengtebeperkingen (extra defensief)
        modelBuilder.Entity<Opstalverzekering>()
            .Property(o => o.TypeDekking).HasMaxLength(75);

        modelBuilder.Entity<Opstalverzekering>()
            .Property(o => o.GedekteGebeurtenissen).HasMaxLength(255);

        modelBuilder.Entity<Opstalverzekering>()
            .Property(o => o.Uitsluitingen).HasMaxLength(255);

        modelBuilder.Entity<Opstalverzekering>()
            .Property(o => o.AanvullendeOpties).HasMaxLength(255);

        // In SQLite wordt precision genegeerd; decimaal blijft bruikbaar in .NET
        modelBuilder.Entity<Opstalverzekering>()
            .Property(o => o.Herbouwwaarde).HasPrecision(12, 2);

        modelBuilder.Entity<Opstalverzekering>()
            .Property(o => o.Inboedelwaarde).HasPrecision(12, 2);

        modelBuilder.Entity<Opstalverzekering>()
            .Property(o => o.Premie).HasPrecision(12, 2);
    }
}
