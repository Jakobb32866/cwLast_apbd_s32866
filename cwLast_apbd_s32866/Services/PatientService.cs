using cwLast_apbd_s32866.Data;
using cwLast_apbd_s32866.DTOs;
using cwLast_apbd_s32866.Exceptions;
using cwLast_apbd_s32866.Models;
using Microsoft.EntityFrameworkCore;

namespace cwLast_apbd_s32866.Services;

public class PatientService : IPatientService
{
    private readonly HospitalDbContext _context;

    public PatientService(HospitalDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PatientDto>> GetAllAsync(string? search)
    {
        var query = _context.Patients
            .AsNoTracking()
            .Include(p => p.Admissions)
                .ThenInclude(a => a.Ward)
            .Include(p => p.BedAssignments)
                .ThenInclude(ba => ba.Bed)
                    .ThenInclude(b => b.BedType)
            .Include(p => p.BedAssignments)
                .ThenInclude(ba => ba.Bed)
                    .ThenInclude(b => b.Room)
                        .ThenInclude(r => r.Ward)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p =>
                EF.Functions.Like(p.FirstName, $"%{search}%") ||
                EF.Functions.Like(p.LastName, $"%{search}%")
            );
        }

        var patients = await query.ToListAsync();

        return patients.Select(p => new PatientDto(
            p.Pesel.Trim(),
            p.FirstName,
            p.LastName,
            p.Age,
            p.Sex ? "Male" : "Female",
            p.Admissions.Select(a => new AdmissionDto(
                a.Id,
                a.AdmissionDate,
                a.DischargeDate,
                new WardDto(a.Ward.Id, a.Ward.Name, a.Ward.Description)
            )),
            p.BedAssignments.Select(ba => new BedAssignmentDto(
                ba.Id,
                ba.From,
                ba.To,
                new BedDto(
                    ba.Bed.Id,
                    new BedTypeDto(ba.Bed.BedType.Id, ba.Bed.BedType.Name, ba.Bed.BedType.Description),
                    new RoomDto(
                        ba.Bed.Room.Id.Trim(),
                        ba.Bed.Room.HasTv,
                        new WardDto(ba.Bed.Room.Ward.Id, ba.Bed.Room.Ward.Name, ba.Bed.Room.Ward.Description)
                    )
                )
            ))
        ));
    }

    public async Task<BedAssignmentDto> AssignBedAsync(string pesel, BedAssignmentCreateDto dto)
    {
        var patient = await _context.Patients.FindAsync(pesel)
            ?? throw new NotFoundException($"Pacjent o PESEL '{pesel}' nie istnieje.");

        var ward = await _context.Wards.FirstOrDefaultAsync(w => w.Name == dto.Ward)
            ?? throw new NotFoundException($"Oddział '{dto.Ward}' nie istnieje.");

        var bedType = await _context.BedTypes.FirstOrDefaultAsync(bt => bt.Name == dto.BedType)
            ?? throw new NotFoundException($"Typ łóżka '{dto.BedType}' nie istnieje.");

        var requestedTo = dto.To ?? DateTime.MaxValue;

        var availableBed = await _context.Beds
            .Include(b => b.Room)
                .ThenInclude(r => r.Ward)
            .Include(b => b.BedType)
            .Include(b => b.BedAssignments)
            .Where(b =>
                b.BedTypeId == bedType.Id &&
                b.Room.WardId == ward.Id &&
                !b.BedAssignments.Any(ba =>
                    ba.From < requestedTo &&
                    (ba.To == null || ba.To > dto.From)
                )
            )
            .FirstOrDefaultAsync()
            ?? throw new NotFoundException(
                $"Brak wolnego łóżka typu '{dto.BedType}' " +
                $"w oddziale '{dto.Ward}' " +
                $"w okresie {dto.From:yyyy-MM-dd HH:mm} – {dto.To?.ToString("yyyy-MM-dd HH:mm") ?? "bez daty końcowej"}."
            );

        var assignment = new BedAssignment
        {
            PatientPesel = pesel,
            BedId = availableBed.Id,
            From = dto.From,
            To = dto.To
        };

        _context.BedAssignments.Add(assignment);
        await _context.SaveChangesAsync();

        return new BedAssignmentDto(
            assignment.Id,
            assignment.From,
            assignment.To,
            new BedDto(
                availableBed.Id,
                new BedTypeDto(availableBed.BedType.Id, availableBed.BedType.Name, availableBed.BedType.Description),
                new RoomDto(
                    availableBed.Room.Id.Trim(),
                    availableBed.Room.HasTv,
                    new WardDto(availableBed.Room.Ward.Id, availableBed.Room.Ward.Name, availableBed.Room.Ward.Description)
                )
            )
        );
    }
}