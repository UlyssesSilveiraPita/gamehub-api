using GameHub.API.Data;
using GameHub.API.Dtos.Players;
using GameHub.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace GameHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly GameHubDbContext _context; // acessa o banco de dados

    public PlayersController(GameHubDbContext context)
    {
        _context = context;
    }
    
    [HttpGet] // pega todos os player do banco de dados
    public async Task<ActionResult<IEnumerable<PlayerResponseDto>>> Getall()
    {
        var players = await _context.Players
            .Where(p => !p.IsDeleted)
            .Select(p => new PlayerResponseDto
            {
                Id = p.Id,
                NickName = p.NickName,
                Level = p.Level,
                Experience = p.Experience,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync();

        return Ok(players);
        
    }

    [HttpPost] // Criacao do Player 
    public async Task<ActionResult<PlayerResponseDto>> Create(CreatePlayerDto dto)
    {
        var player = new Player
        {
            Id = Guid.NewGuid(),
            NickName = dto.NickName,
            Level = 1,
            Experience = 0,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false,
            UserId = string.Empty
        };

        _context.Players.Add(player);
        await _context.SaveChangesAsync();

        var response = new PlayerResponseDto
        {
            Id = player.Id,
            NickName = player.NickName,
            Level = player.Level,
            Experience = player.Experience, 
            CreatedAt = DateTime.UtcNow,    
        };


        return CreatedAtAction(nameof(GetById), new {id = player.Id }, response);
    }

    [HttpGet("{id}")] // pega player pelo Id
    public async Task<ActionResult<PlayerResponseDto>> GetById(Guid id)
    {
        var player = await _context.Players.FindAsync(id);

        if (player is null || player.IsDeleted)
        {
            return NotFound();
        }

        var response = new PlayerResponseDto
        {
            Id = player.Id,
            NickName = player.NickName,
            Level = player.Level,
            Experience = player.Experience,
            CreatedAt = player.CreatedAt

        };

        return Ok(response);
    }

    [HttpPut("{id}")] // atualizad o jogador
    public async Task<ActionResult> Update(Guid id, UpdatePlayerDto dto)
    {
        var player = await _context.Players.FindAsync(id);

        if(player is null || player.IsDeleted)
        {
            return NotFound();
        }

        player.NickName = dto.NickName;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")] //marca que foi deletado mas permanece no banco de dados
    public async Task<ActionResult> Delete(Guid id)
    {
        var player = await _context.Players.FindAsync(id);

        if( player is null || player.IsDeleted)
        {
            return NoContent();
        }

        player.IsDeleted = true; 

        await _context.SaveChangesAsync();
        
        return NoContent();
    }

}
