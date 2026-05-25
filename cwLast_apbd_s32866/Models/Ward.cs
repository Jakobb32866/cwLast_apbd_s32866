namespace cwLast_apbd_s32866.Models;

public class Ward
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public ICollection<Admission> Admissions { get; set; } = new List<Admission>();
    public ICollection<Room> Rooms { get; set; } = new List<Room>();
}