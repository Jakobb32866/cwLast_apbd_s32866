namespace cwLast_apbd_s32866.Models;

public class BedType
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public ICollection<Bed> Beds { get; set; } = new List<Bed>();
}