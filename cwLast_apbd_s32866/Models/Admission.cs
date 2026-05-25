namespace cwLast_apbd_s32866.Models;

public class Admission
{
    public int Id { get; set; }
    public DateTime AdmissionDate { get; set; }
    public DateTime? DischargeDate { get; set; }
    public string PatientPesel { get; set; } = null!;
    public int WardId { get; set; }

    public Patient Patient { get; set; } = null!;
    public Ward Ward { get; set; } = null!;
}