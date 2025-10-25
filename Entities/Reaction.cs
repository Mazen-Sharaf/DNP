namespace Entities;

public class Reaction
{
    public int ByUserId { get; set; }
    public int PostId { get; set; }
    public string Type { get; set; }
}