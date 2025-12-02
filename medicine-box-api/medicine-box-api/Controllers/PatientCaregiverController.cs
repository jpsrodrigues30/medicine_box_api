using medicine_box_api.Domain.Interface.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace medicine_box_api.Api.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
public class PatientCaregiverController(IPatientCaregiverService patientCaregiverService) : Controller
{
    [HttpDelete("PatientCaregiverRelationship")]
    public async Task<bool> RemovePatientCaregiverRelationship(
        [FromQuery] Guid patientId,
        [FromQuery] Guid caregiverId)
    {
        var result = await patientCaregiverService.RemovePatientCaregiverRelation(patientId: patientId, caregiverId: caregiverId);

        return result;
    }
}
