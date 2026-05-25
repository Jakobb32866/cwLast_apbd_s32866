using cwLast_apbd_s32866.DTOs;

namespace cwLast_apbd_s32866.Services;

public interface IPatientService
{
    Task<IEnumerable<PatientDto>> GetAllAsync(string? search);
    Task<BedAssignmentDto> AssignBedAsync(string pesel, BedAssignmentCreateDto dto);
}