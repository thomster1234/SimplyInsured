
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VerzekeringApi.Models;

public class Opstalverzekering
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public int PolisNummer { get; set; }  // Uniek

    [Required]
    public Guid KlantId { get; set; }

    public Klant? Klant { get; set; }

    [Required, MaxLength(75)]
    public string TypeDekking { get; set; } = default!;

    [MaxLength(255)]
    public string? GedekteGebeurtenissen { get; set; }

    [MaxLength(255)]
    public string? Uitsluitingen { get; set; }

    [Column(TypeName = "decimal(12,2)")]
    public decimal Herbouwwaarde { get; set; }

    [Column(TypeName = "decimal(12,2)")]
    public decimal Inboedelwaarde { get; set; }

    [Column(TypeName = "decimal(12,2)")]
    public decimal Premie { get; set; }

    [Required, MaxLength(50)]
    public string Betaaltermijn { get; set; } = default!; // bijv. "Maandelijks"

    [MaxLength(255)]
    public string? AanvullendeOpties { get; set; }

    [Required]
    public DateTime BeginDatum { get; set; }

    public DateTime? EindDatum { get; set; }
}
