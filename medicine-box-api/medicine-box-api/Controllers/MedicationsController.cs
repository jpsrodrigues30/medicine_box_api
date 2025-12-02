using MediatR;
using medicine_box_api.Domain.Dtos;
using medicine_box_api.Domain.Dtos.Requests;
using medicine_box_api.Domain.Interface.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace medicine_box_api.Api.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
public class MedicationsController(
    IMedicationsService medicationService) : Controller
{
    [HttpGet]
    public async Task<List<MedicationsDetails>> GetMedicationDetails(
        [FromQuery] Guid? userId = null,
        [FromQuery] List<Guid>? medicationId = null)
    {
        var result = await medicationService.GetMedications(id: medicationId, userId: userId);

        return result;
    }

    [HttpPost]
    public async Task<Guid?> CreateNewMedication(
        [FromBody] MedicationCreateRequest body)
    {
        var result = await medicationService.CreateNewMedication(body);

        return result;
    }

    [HttpPut]
    public async Task<bool> UpdateMedication(
        [FromQuery] Guid userId,
        [FromQuery] Guid medicationId,
        [FromBody] MedicationUpdateRequest body)
    {
        var result = await medicationService.UpdateMedication(id: medicationId, userId: userId, request: body);

        return result;
    }

    [HttpDelete]
    public async Task<bool> DeleteMedication(
        [FromQuery] Guid medicationId,
        [FromQuery] Guid userId)
    {
        var result = await medicationService.DeleteMedication(medicationId, userId);

        return result;
    }

    [HttpGet("ActiveOnly")]
    public async Task<List<MedicationsDetails>> GetActiveMedicationDetails(
        [FromQuery] Guid userId)
    {
        var result = await medicationService.GetActiveMedications(userId);

        return result;
    }
}
