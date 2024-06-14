using apbdkol2.Data;
using apbdkol2.Models;
using apbdkol2.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apbdkol2.Services;

public class DbService : IDbService
{
    private readonly Apbdkol2Context _context;

    public DbService(Apbdkol2Context context)
    {
        _context = context;
    }

    public async Task<Character?> GetCharacterByIdAsync(int id)
    {
        return await _context.Characters
            .Include(c => c.Backpacks)
                .ThenInclude(b => b.Item)
            .Include(c => c.CharacterTitles)
                .ThenInclude(ct => ct.Title)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddItemsToCharacterBackpackAsync(int characterId, List<int> itemIds)
    {
        var character = await _context.Characters.FindAsync(characterId);
        if (character == null)
        {
            throw new Exception("Character not found");
        }

        var items = await _context.Items.Where(i => itemIds.Contains(i.Id)).ToListAsync();
        if (items.Count != itemIds.Count)
        {
            throw new Exception("Some items do not exist");
        }

        int totalNewWeight = items.Sum(i => i.Weight);
        if (character.CurrentWeight + totalNewWeight > character.MaxWeight)
        {
            throw new Exception("Character cannot carry that much weight");
        }

        foreach (var item in items)
        {
            var backpack = await _context.Backpacks
                .FirstOrDefaultAsync(b => b.CharacterId == characterId && b.ItemId == item.Id);

            if (backpack == null)
            {
                _context.Backpacks.Add(new Backpack
                {
                    CharacterId = characterId,
                    ItemId = item.Id,
                    Amount = 1
                });
            }
            else
            {
                backpack.Amount += 1;
            }

            character.CurrentWeight += item.Weight;
        }

        await _context.SaveChangesAsync();
    }
}