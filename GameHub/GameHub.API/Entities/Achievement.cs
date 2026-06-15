namespace GameHub.API.Entities;

public class Achievement
{
    public Guid Id { get; set; } 
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty; // recebe vasio
    public int Points { get; set; }

    //=============================
    // Relacionamentos \\
    //=============================

    public ICollection<PlayerAchievement> PlayerAchievements { get; set; } = new List<PlayerAchievement>(); //descobre quem desbloqueou a conquista
}
