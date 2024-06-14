using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apbdkol2.Models;

[Table("items")]
public class Item
{
    [Key]
    public int Id { get; set; }
    
    [MaxLength(100)]
    [Required]
    public string Name { get; set; }
    
    public int Weight { get; set; }
    
    public ICollection<Backpack> Backpacks { get; set; }
}