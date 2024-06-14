using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apbdkol2.Models;

[Table("titles")]
public class Title
{
    [Key]
    public int Id { get; set; }
    
    [MaxLength(100)]
    [Required]
    public string Name { get; set; }
    
    public ICollection<CharacterTitle> CharacterTitles { get; set; }
}