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
public class ProfileController(IUserProfileService userProfileService) : Controller
{
    [HttpGet]
    public async Task<List<ProfileDetails>> GetProfileDetails(
        [FromQuery] List<Guid>? userId = null,
        [FromQuery] string? role = null)
    {
        var result = await userProfileService.GetUserProfiles(userId);

        return result;
    }

    [HttpGet("GetUserCaregivers")]
    public async Task<List<ProfileDetails>> GetUserCaregivers(
        [FromQuery] Guid userId)
    {
        var result = await userProfileService.GetUserPatientCaregiverProfiles(userId, true);

        return result;
    }

    [HttpGet("GetUserPatients")]
    public async Task<List<ProfileDetails>> GetUserPatients(
        [FromQuery] Guid userId)
    {
        var result = await userProfileService.GetUserPatientCaregiverProfiles(userId);

        return result;
    }

    [HttpPost]
    public async Task<bool> UpsertNewProfile(
        [FromBody] ProfileCreateRequest request)
    {
        var result = await userProfileService.CreateUserProfile(request);

        return result;
    }
}
