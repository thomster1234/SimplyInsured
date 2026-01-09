
using System.ComponentModel.DataAnnotations;

namespace VerzekeringApi.Dtos;

public class SetEinddatumDto
{
    // Optioneel: als geen datum meegegeven wordt, zet de API 'Now'
    public DateTime? EindDatum { get; set; }
}
