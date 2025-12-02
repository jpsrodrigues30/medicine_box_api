using medicine_box_api.Domain.Dtos;
using medicine_box_api.Domain.Dtos.Entities;
using medicine_box_api.Domain.Dtos.Enum;
using medicine_box_api.Domain.Dtos.Requests;
using medicine_box_api.Domain.Helpers;
using medicine_box_api.Domain.Interface.Repositories;
using medicine_box_api.Domain.Interface.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace medicine_box_api.Application.Services;
public class UserProfilesService(
    ILogger<UserProfilesService> logger, 
    IPatientCaregiverService patientCaregiverService,
    IUserProfilesRepository userProfilesRepository) : IUserProfileService
{
    public async Task<List<ProfileDetails>> GetUserProfiles(List<Guid>? id = null, string? role = null)
    {
        try
        {
            logger.LogInformation("Inicializando busca por perfis de usuário");

            if (role != null)
            {
                logger.LogDebug($"Verificanedo se a role {role} é válida...");
                var mappedRole = EnumHelper.TryParseEnum<UserProfileRoleEnum>(role, true);

                if (mappedRole == null)
                {
                    logger.LogWarning("Role inválida!");
                    return new List<ProfileDetails>();
                }

                role = mappedRole.Value.ToString();
            }

            var profiles = await userProfilesRepository.Get(id);

            if (profiles.Count == 0)
            {
                logger.LogWarning("Não foram encontrados perfis de usuários cadastrados");
                return new List<ProfileDetails>();
            }

            return profiles.Select(p => new ProfileDetails
            {
                Id = p.Id,
                CaregiverId = p.CaregiverId,
                FullName = p.FullName ?? string.Empty,
                Email = p.Email ?? string.Empty,
                Role = p.Role ?? string.Empty,
                PhoneNumber = p.PhoneNumber ?? string.Empty,
                Timezone = p.Timezone ?? string.Empty,
            }).ToList();
        }
        catch (Exception ex)
        {
            logger.LogError("Erro ao procurar os perfis de usuário");
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<ProfileDetails>> GetUserPatientCaregiverProfiles(Guid userId, bool isPatient = false)
    {
        try
        {
            logger.LogInformation($"Inicializando a busca pelo perfil dos pacientes/cuidadores para o usuário {userId}");

            var profileIds = await patientCaregiverService.GetUserPatientCaregiver(userId, isPatient);

            if (profileIds.Count == 0) return new List<ProfileDetails>();

            var profileDetails = await GetUserProfiles(id: profileIds);

            return profileDetails ?? new List<ProfileDetails>();
        }
        catch (Exception ex)
        {
            logger.LogError("Erro ao procurar os perfis de pacientes/cuidadores para o usuário");
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> CreateUserProfile(ProfileCreateRequest request)
    {
        try
        {
            logger.LogInformation("Criando novo perfil de usuário");
            var role = EnumHelper.TryParseEnum<UserProfileRoleEnum>(request.Role, true);

            if (role == null)
            {
                logger.LogWarning("Role inválida!");
                return false;
            }

            var newEntity = new UserProfileEntity()
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                Email = request.Email,
                CaregiverId = null,
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                Role = role.ToString(),
                Timezone = request.Timezone,
            };

            logger.LogDebug($"ID do novo usuário: {newEntity.Id}");

            await userProfilesRepository.Create(newEntity);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError("Erro ao tentar criar um novo perfil de usuário");
            throw new Exception(ex.Message);
        }
    }
}
