namespace Core.Entities;

public class Sighting
{
    public Guid Id { get; set; }

    public Guid BugId { get; set; }
    public Guid UserId { get; set; }
    public Bug Bug { get; set; } = null!;
    public User User { get; set; } = null!;

    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime SeenAt { get; set; }
    public string Notes { get; set; } = "";

    public DateTime CreatedAt { get; set; }
}
