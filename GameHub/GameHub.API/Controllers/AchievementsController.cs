using GameHub.API.Data;
using GameHub.API.Dtos.Achievements;
using GameHub.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameHub.API.Controllers;

[ApiController] // e uma classe API
[Route("api/[controller]")] // rota
public class AchievementsController : ControllerBase
{
    private readonly GameHubDbContext _context; // acessa o banco

    public AchievementsController(GameHubDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AchievementResponseDto>>> GetAll() // cria o EndPoint
    {
        var achievements = await _context.Achievements
            .Select(a => new AchievementResponseDto // transformando entidade em DTO
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                Points = a.Points
            })
            .ToListAsync();
        return Ok(achievements);
    }

    [HttpPost]
    public async Task<ActionResult<AchievementResponseDto>> Create(CreateAchievementDto dto)
    {
        var achievement = new Achievement
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            Points = dto.Points

        };

        _context.Achievements.Add(achievement);
        await _context.SaveChangesAsync();

        var response = new AchievementResponseDto
        {
            Id = achievement.Id,
            Name = achievement.Name,
            Description = achievement.Description,
            Points = achievement.Points
        };


        return CreatedAtAction(nameof(GetById), new { id = achievement.Id }, response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AchievementResponseDto>> GetById(Guid id)
    {
        var achievement = await _context.Achievements.FindAsync(id);

        if(achievement is null)
        {
            return NotFound();
        }

        var response = new AchievementResponseDto
        {
            Id = achievement.Id,
            Name = achievement.Name,
            Description = achievement.Description,
            Points = achievement.Points
        };

        return Ok(response);
    }
}
