
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VerzekeringApi.Data;
using VerzekeringApi.Dtos;
using VerzekeringApi.Models;

namespace VerzekeringApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KlantenController : ControllerBase
{
    private readonly AppDbContext _db;

    public KlantenController(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Maak een nieuwe klant aan; BeginDatum = DateTime.Now
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateKlant([FromBody] CreateKlantDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var klant = new Klant
        {
            Voornaam = dto.Voornaam,
            Tussenvoegsel = dto.Tussenvoegsel,
            Achternaam = dto.Achternaam,
            Geboortedatum = dto.Geboortedatum,
            Woonplaats = dto.Woonplaats,
            BeginDatum = DateTime.Now
        };

        _db.Klanten.Add(klant);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetKlantById), new { id = klant.Id }, klant);
    }

    /// <summary>
    /// (Extra) Ophalen klant op id
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetKlantById(Guid id)
    {
        var klant = await _db.Klanten
            .Include(k => k.Opstalverzekeringen)
            .First(k => k.Id == id);

        return klant is not null ? Ok(klant) : NotFound();
    }

    /// <summary>
    /// Zet einddatum op klant (als niet gezet)
    /// </summary>
    [HttpPost("{id:guid}/einddatum")]
    public async Task<IActionResult> SetEinddatum(Guid id, [FromBody] SetEinddatumDto dto)
    {
        var klant = await _db.Klanten.FindAsync(id);
        if (klant is null) return NotFound();

        if (klant.EindDatum is not null)
            return BadRequest(new { message = "Klant heeft al een einddatum." });

        klant.EindDatum = dto.EindDatum ?? DateTime.Now;
        await _db.SaveChangesAsync();

        return Ok(klant);
    }
}
