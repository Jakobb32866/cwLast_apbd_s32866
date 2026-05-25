namespace cwLast_apbd_s32866.Models;

public class Patient
{
    public string Pesel { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public int Age { get; set; }
    public bool Sex { get; set; }

    public ICollection<Admission> Admissions { get; set; } = new List<Admission>();
    public ICollection<BedAssignment> BedAssignments { get; set; } = new List<BedAssignment>();
}