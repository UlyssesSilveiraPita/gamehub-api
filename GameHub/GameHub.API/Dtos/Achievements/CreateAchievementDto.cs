namespace GameHub.API.Dtos.Achievements;

public class CreateAchievementDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Points { get; set; } 
}
