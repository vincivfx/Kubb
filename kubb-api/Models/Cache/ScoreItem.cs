namespace KubbAdminAPI.Models.Cache;

public class ScoreItem
{
    public required string Text { get; set; }
    public required List<string> Partitions { get; set; } = [];
    public DateTime Expires { get; set; } = DateTime.UtcNow.AddMinutes(15);
}