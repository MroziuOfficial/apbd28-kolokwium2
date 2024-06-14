using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace apbdkol2.Models;

[Table("character_titles")]
[PrimaryKey(nameof(CharacterId), nameof(TitlesId))]
public class CharacterTitle
{
    public int CharacterId { get; set; }
    [ForeignKey(nameof(CharacterId))] public Character Character { get; set; } = null!;
    
    public int TitlesId { get; set; }
    [ForeignKey(nameof(TitlesId))] public Title Title { get; set; } = null!;
    
    public DateTime AcquiredAt { get; set; }
}