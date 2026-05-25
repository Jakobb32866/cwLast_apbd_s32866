namespace cwLast_apbd_s32866.Models;

public class Bed
{
    public int Id { get; set; }
    public string RoomId { get; set; } = null!;
    public int BedTypeId { get; set; }

    public Room Room { get; set; } = null!;
    public BedType BedType { get; set; } = null!;
    public ICollection<BedAssignment> BedAssignments { get; set; } = new List<BedAssignment>();
}