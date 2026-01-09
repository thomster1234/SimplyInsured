
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VerzekeringApi.Data;
using VerzekeringApi.Dtos;
using VerzekeringApi.Models;

namespace VerzekeringApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OpstalverzekeringenController : ControllerBase
{
    private readonly AppDbContext _db;

    public OpstalverzekeringenController(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Maak een nieuwe opstalverzekering aan; BeginDatum = DateTime.Now
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateOpstalverzekering([FromBody] CreateOpstalverzekeringDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // klant moet bestaan en actief zijn
        var klantActief = await _db.Klanten.AnyAsync(k => k.Id == dto.KlantId && k.EindDatum == null);
        if (!klantActief)
            return BadRequest(new { message = "Klant bestaat niet of is beëindigd." });

        // polisnummer uniek
        var bestaat = await _db.Opstalverzekeringen.AnyAsync(o => o.PolisNummer == dto.PolisNummer);
        if (bestaat)
            return Conflict(new { message = "Polisnummer bestaat al." });

        var ov = new Opstalverzekering
        {
            PolisNummer = dto.PolisNummer,
            KlantId = dto.KlantId,
            TypeDekking = dto.TypeDekking,
            GedekteGebeurtenissen = dto.GedekteGebeurtenissen,
            Uitsluitingen = dto.Uitsluitingen,
            Herbouwwaarde = dto.Herbouwwaarde,
            Inboedelwaarde = dto.Inboedelwaarde,
            Premie = dto.Premie,
            Betaaltermijn = dto.Betaaltermijn,
            AanvullendeOpties = dto.AanvullendeOpties,
            BeginDatum = DateTime.Now
        };

        _db.Opstalverzekeringen.Add(ov);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = ov.Id }, ov);
    }

    /// <summary>
    /// (Extra) Ophalen opstalverzekering op id
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var ov = await _db.Opstalverzekeringen
            .Include(o => o.Klant)
            .FirstOrDefaultAsync(o => o.Id == id);

        return ov is not null ? Ok(ov) : NotFound();
    }

    /// <summary>
    /// GET: actieve opstalverzekering op polisnummer
    /// </summary>
    [HttpGet("by-polisnummer/{polisnummer:int}")]
    public async Task<IActionResult> GetByPolisNummer(int polisnummer)
    {
        var ov = await _db.Opstalverzekeringen
            .Where(o => o.PolisNummer == polisnummer && o.EindDatum == null)
            .FirstOrDefaultAsync();

        return ov is not null ? Ok(ov) : NotFound();
    }

    /// <summary>
    /// GET: actieve opstalverzekeringen op klantId
    /// </summary>
    [HttpGet("by-klant/{klantId:guid}")]
    public async Task<IActionResult> GetByKlant(Guid klantId)
    {
        var list = await _db.Opstalverzekeringen
            .Where(o => o.KlantId == klantId && o.EindDatum == null)
            .ToListAsync();

        return Ok(list);
    }

    /// <summary>
    /// GET: actieve opstalverzekeringen op typeDekking (exacte match)
    /// </summary>
    [HttpGet("by-typedekking/{typeDekking}")]
    public async Task<IActionResult> GetByTypeDekking(string typeDekking)
    {
        var list = await _db.Opstalverzekeringen
            .Where(o => o.TypeDekking == typeDekking && o.EindDatum == null)
            .ToListAsync();

        return Ok(list);
    }

    /// <summary>
    /// Zet einddatum op opstalverzekering (als niet gezet)
    /// </summary>
    [HttpPost("{id:guid}/einddatum")]
    public async Task<IActionResult> SetEinddatum(Guid id, [FromBody] SetEinddatumDto dto)
    {
        var ov = await _db.Opstalverzekeringen.FindAsync(id);
        if (ov is null) return NotFound();

        if (ov.EindDatum is not null)
            return BadRequest(new { message = "Opstalverzekering heeft al een einddatum." });

        ov.EindDatum = dto.EindDatum ?? DateTime.Now;
        await _db.SaveChangesAsync();

        return Ok(ov);
    }
}
