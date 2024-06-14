using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apbdkol2.Models;

[Table("characters")]
public class Character
{
    [Key]
    public int Id { get; set; }
    
    [MaxLength(50)]
    [Required]
    public string FirstName { get; set; }
    
    [MaxLength(120)]
    [Required]
    public string LastName { get; set; }
    
    public int CurrentWeight { get; set; }
    
    public int MaxWeight { get; set; }
    
    public ICollection<Backpack> Backpacks { get; set; }
    public ICollection<CharacterTitle> CharacterTitles { get; set; }
}