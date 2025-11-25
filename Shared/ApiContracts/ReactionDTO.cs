namespace ApiContracts;

public class ReactionDTO
{
    public int ByUserId { get; set; }
    public int PostId { get; set; }
    public string Type { get; set; }
    public int ReactionId { get; set; }
}