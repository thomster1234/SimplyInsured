
using System.ComponentModel.DataAnnotations;

namespace VerzekeringApi.Dtos;

public class CreateOpstalverzekeringDto
{
    [Required]
    public int PolisNummer { get; set; }

    [Required]
    public Guid KlantId { get; set; }

    [Required, MaxLength(75)]
    public string TypeDekking { get; set; } = default!;

    [MaxLength(255)]
    public string? GedekteGebeurtenissen { get; set; }

    [MaxLength(255)]
    public string? Uitsluitingen { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Herbouwwaarde { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Inboedelwaarde { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Premie { get; set; }

    [Required, MaxLength(50)]
    public string Betaaltermijn { get; set; } = default!;

    [MaxLength(255)]
    public string? AanvullendeOpties { get; set; }
}
