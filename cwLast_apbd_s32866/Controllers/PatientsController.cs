using cwLast_apbd_s32866.DTOs;
using cwLast_apbd_s32866.Exceptions;
using cwLast_apbd_s32866.Services;
using Microsoft.AspNetCore.Mvc;

namespace cwLast_apbd_s32866.Controllers;

[ApiController]
[Route("api/patients")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search)
    {
        var result = await _patientService.GetAllAsync(search);
        return Ok(result);
    }

    [HttpPost("{pesel}/bedassignments")]
    public async Task<IActionResult> AssignBed(string pesel, [FromBody] BedAssignmentCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await _patientService.AssignBedAsync(pesel, dto);
            return StatusCode(StatusCodes.Status201Created, result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}