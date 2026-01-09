
using System.ComponentModel.DataAnnotations;

namespace VerzekeringApi.Dtos;

public class CreateKlantDto
{
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
}
