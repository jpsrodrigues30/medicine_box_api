using medicine_box_api.Domain.Dtos;
using medicine_box_api.Domain.Interface.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace medicine_box_api.Api.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
public class MedicationsHistoryController(IMedicationsHistoryService medicationsHistoryService) : Controller
{
    [HttpGet]
    public async Task<List<MedicationHistoryDetails>> GetMedicationHistoryDetalis(
        [FromQuery] Guid userId)
    {
        var result = await medicationsHistoryService.GetMedicationsHistory(userId);

        return result;
    }

    [HttpGet("NextMedicationScheduled")]
    public async Task<NextMedicationSchedule> GetNextUserMedication(
        [FromQuery] Guid userId,
        [FromQuery] DateTime? nextScheduledDate = null)
    {
        var result = await medicationsHistoryService.GetNextMedicationHistory(userId, nextScheduledDate);

        return result;
    }

    [HttpGet("WithMedicationDetails")]
    public async Task<List<MedicationHistoryWithMedDetails>> GetMedHistoryWithDetails(
        [FromQuery] Guid userId)
    {
        var result = await medicationsHistoryService.GetMedicationHistoryWithMedDetails(userId);

        return result;
    }

    [HttpPost]
    public async Task<bool> UpsertMedicationSchedule(
        [FromQuery] Guid userId,
        [FromQuery] Guid medicationId)
    {
        var result = await medicationsHistoryService.UpsertMedicationSchedule(userId, medicationId);

        return result;
    }

    [HttpPut("StatusChange")]
    public async Task<bool> ChangeMedicationStatus(
        [FromQuery] Guid userId,
        [FromQuery] Guid medSchedId,
        [FromQuery] string status)
    {
        var result = await medicationsHistoryService.UpdateMedicationScheduleStatus(userId, medSchedId, status);

        return result;
    }

    [HttpPut("DosageUpdate")]
    public async Task<bool> UpdateMedicationDosage(
        [FromQuery] Guid userId,
        [FromQuery] Guid medicationId,
        [FromQuery] int dosage)
    {
        var result = await medicationsHistoryService.UpdateMedicationScheduleDosage(userId, medicationId, dosage);

        return result;
    }

    [HttpPut("MedicationSchedule/CancelAllEvents")]
    public async Task<bool> CancelAllMedicationEvents(
        [FromQuery] Guid userId,
        [FromQuery] Guid medicationId)
    {
        var result = await medicationsHistoryService.CancellAllMedicationSchedules(userId, medicationId);

        return result;
    }
}
