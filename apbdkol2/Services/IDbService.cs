using apbdkol2.Models;
using apbdkol2.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace apbdkol2.Services;

public interface IDbService
{
    Task<Character?> GetCharacterByIdAsync(int id);
    Task AddItemsToCharacterBackpackAsync(int characterId, List<int> itemIds);
}