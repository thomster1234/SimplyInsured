
using System.ComponentModel.DataAnnotations;

namespace VerzekeringApi.Models;

public class Klant
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(100)]
    public string Voornaam { get; set; } = default!;

    [MaxLength(50)]
    public string? Tussenvoegsel { get; set; }

    [Required, MaxLength(100)]
    public string Achternaam { get; set; } = default!;

    [Required]
    public DateTime Geboortedatum { get; set; }

    [Required, MaxLength(150)]
    public string Woonplaats { get; set; } = default!;

    [Required]
    public DateTime BeginDatum { get; set; }

    public DateTime? EindDatum { get; set; }

    public ICollection<Opstalverzekering> Opstalverzekeringen { get; set; } = new List<Opstalverzekering>();
}
