using System.Transactions;
using apbdkol2.Data;
using apbdkol2.DTOs;
using apbdkol2.Models;
using apbdkol2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apbdkol2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharactersController : ControllerBase
    {
        private readonly IDbService _service;
        private readonly Apbdkol2Context _context;

        public CharactersController(IDbService s, Apbdkol2Context c)
        {
            _service = s;
            _context = c;
        }

        [HttpGet("{characterId}")]
        public async Task<IActionResult> GetCharacter(int characterId)
        {
            var character = await _context.Characters
                .Include(c => c.Backpacks)
                .ThenInclude(b => b.Item)
                .Include(c => c.CharacterTitles)
                .ThenInclude(ct => ct.Title)
                .FirstOrDefaultAsync(c => c.Id == characterId);

            if (character == null)
            {
                return NotFound("Character doesn't exist");
            }

            var characterDto = new 
            {
                firstName = character.FirstName,
                lastName = character.LastName,
                currentWeight = character.CurrentWeight,
                maxWeight = character.MaxWeight,
                backpackItems = character.Backpacks.Select(b => new 
                {
                    itemName = b.Item.Name,
                    itemWeight = b.Item.Weight,
                    amount = b.Amount
                }).ToList(),
                titles = character.CharacterTitles.Select(ct => new 
                {
                    title = ct.Title.Name,
                    acquiredAt = ct.AcquiredAt
                }).ToList()
            };

            return Ok(characterDto);
        }
        
        [HttpPost("{characterId}/backpacks")]
        public async Task<IActionResult> AddItemsToCharacterBackpack(int characterId, [FromBody] List<int> itemIds)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            var character = await _context.Characters.FindAsync(characterId);
            if (character == null)
            {
                return NotFound("Character doesn't exist");
            }

            var items = await _context.Items.Where(i => itemIds.Contains(i.Id)).ToListAsync();
            if (items.Count != itemIds.Count)
            {
                return NotFound("One or more items do not exist");
            }

            var totalWeight = items.Sum(i => i.Weight);
            if (character.CurrentWeight + totalWeight > character.MaxWeight)
            {
                return BadRequest("Character cannot carry that much weight");
            }

            foreach (var i in items)
            {
                var backpackItem = await _context.Backpacks
                    .FirstOrDefaultAsync(b => b.CharacterId == characterId && b.ItemId == i.Id);
                
                if (backpackItem == null)
                {
                    _context.Backpacks.Add(new Backpack
                    {
                        CharacterId = characterId,
                        ItemId = i.Id,
                        Amount = 1
                    });
                }
                else
                {
                    backpackItem.Amount++;
                }
            }

            character.CurrentWeight += totalWeight;
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            var result = items.Select(item => new 
            {
                amount = 1,
                itemId = item.Id,
                characterId = characterId
            }).ToList();

            return Ok(result);
        }
    }
}