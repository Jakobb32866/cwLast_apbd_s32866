namespace cwLast_apbd_s32866.Models;

public class Room
{
    public string Id { get; set; } = null!;
    public int WardId { get; set; }
    public bool HasTv { get; set; }

    public Ward Ward { get; set; } = null!;
    public ICollection<Bed> Beds { get; set; } = new List<Bed>();
}