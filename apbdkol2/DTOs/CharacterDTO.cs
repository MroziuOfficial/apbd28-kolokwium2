namespace apbdkol2.DTOs;

public class CharacterDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int CurrentWeight { get; set; }
    public int MaxWeight { get; set; }
    public List<BackpackItemDTO> BackpackItems { get; set; }
    public List<CharacterTitleDTO> Titles { get; set; }
}