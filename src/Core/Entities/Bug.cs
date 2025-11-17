namespace Core.Entities;

public class Bug
{
    public Guid Id { get; set; }
    
    public string Species { get; set; } = "";
    public int DangerLevel { get; set; }
    public string Description { get; set; } = "";
}
