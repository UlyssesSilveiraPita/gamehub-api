namespace GameHub.API.Entities;

public class Achievement
{
    public Guid Id { get; set; } 
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty; // recebe vasio
    public int Points { get; set; }

}
