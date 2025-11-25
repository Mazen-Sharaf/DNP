namespace Entities;

public class Reaction
{
    public User ByUser { get; set; }
    public Post OnPost { get; set; }
    public string Type { get; set; }
    public int ReactionId { get; set; }
}