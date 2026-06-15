namespace GameHub.API.Entities;

public class Player
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public int Level { get; set; } = 1;
    public int Experience { get; set; } = 0;
    public bool isDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // pega a data e hora do computador do ususario no momento que ele for criado.

}
