using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace apbdkol2.Models;

[Table("backpacks")]
[PrimaryKey(nameof(CharacterId), nameof(ItemId))]
public class Backpack
{
    public int CharacterId { get; set; }
    [ForeignKey(nameof(CharacterId))] public Character Character { get; set; } = null!;
    
    public int ItemId { get; set; }
    [ForeignKey(nameof(ItemId))] public Item Item { get; set; } = null!;
    
    public int Amount { get; set; }
}