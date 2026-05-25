namespace cwLast_apbd_s32866.DTOs;

public record WardDto(int Id, string Name, string Description);

public record AdmissionDto(
    int Id,
    DateTime AdmissionDate,
    DateTime? DischargeDate,
    WardDto Ward
);

public record BedTypeDto(int Id, string Name, string Description);

public record RoomDto(string Id, bool HasTv, WardDto Ward);

public record BedDto(int Id, BedTypeDto BedType, RoomDto Room);

public record BedAssignmentDto(int Id, DateTime From, DateTime? To, BedDto Bed);

public record PatientDto(
    string Pesel,
    string FirstName,
    string LastName,
    int Age,
    string Sex,
    IEnumerable<AdmissionDto> Admissions,
    IEnumerable<BedAssignmentDto> BedAssignments
);

public record BedAssignmentCreateDto(
    DateTime From,
    DateTime? To,
    string BedType,
    string Ward
);