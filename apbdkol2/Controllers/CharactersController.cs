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
    }
}