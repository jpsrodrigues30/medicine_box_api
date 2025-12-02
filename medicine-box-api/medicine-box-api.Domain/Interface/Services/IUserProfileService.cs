using medicine_box_api.Domain.Dtos;
using medicine_box_api.Domain.Dtos.Requests;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace medicine_box_api.Domain.Interface.Services;
public interface IUserProfileService
{
    Task<List<ProfileDetails>> GetUserProfiles(List<Guid>? id = null, string? role = null);
    Task<List<ProfileDetails>> GetUserPatientCaregiverProfiles(Guid userId, bool isPatient = false);
    Task<bool> CreateUserProfile(ProfileCreateRequest request);
}
