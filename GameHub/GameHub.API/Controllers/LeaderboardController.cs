using GameHub.API.Data;
using GameHub.API.Dtos.Leaderboard;
using GameHub.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaderboardController : ControllerBase
{
    private readonly GameHubDbContext _context;

    public LeaderboardController(GameHubDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<LeaderboardEntryResponseDto>> Create(CreateLeaderboardEntryDto dto)
    {
        var player = await _context.Players.FindAsync(dto.PlayerId);

        if (player is null || player.IsDeleted)
        {
            return BadRequest("Player nao encontrado");
        }

        var entry = new LeaderboardEntry
        {
            Id = Guid.NewGuid(),
            PlayerId = dto.PlayerId,
            Score = dto.Score,
            CreatedAt = DateTime.UtcNow
        };

        _context.LeaderboardEntries.Add(entry); // add 

        await _context.SaveChangesAsync();

        var response = new LeaderboardEntryResponseDto
        {
            Id = entry.Id,
            PlayerId = player.Id,
            PlayerName = player.NickName,
            Score = entry.Score,
            CreatedAt = entry.CreatedAt
        };

        return Ok(response);
    }
}
