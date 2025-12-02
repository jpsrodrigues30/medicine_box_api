using medicine_box_api.Domain.Interface.Repositories;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using medicine_box_api.Domain.Interface.Services;
using medicine_box_api.Domain.Dtos.Entities;
using System.Collections.Generic;
using System.Linq;

namespace medicine_box_api.Application.Services;
public class PatientCaregiverService(
    ILogger<PatientCaregiverService> logger,
    IPatientCaregiverRepository patientCaregiverRepository) : IPatientCaregiverService
{
    public async Task<List<Guid>> GetUserPatientCaregiver(Guid userId, bool isPatient = false)
    {
        try
        {
            logger.LogInformation($"Inicializando a busca pelos pacientes/cuidadores do usuário {userId}");

            var profiles = new List<PatientCaregiversEntity>();

            if (isPatient)
                profiles = await patientCaregiverRepository.Get(patientId: userId);
            else
                profiles = await patientCaregiverRepository.Get(caregiverId: userId);

            if (profiles == null
                || profiles.Count == 0)
            {
                logger.LogWarning("Não foram encontrados pacientes/cuidadores para esse paciente");
                return new List<Guid>();
            }

            var profileIds = profiles!.Select(x => isPatient ? x.CaregiverId : x.PatientId).ToList();

            return profileIds ?? new List<Guid>();
        }
        catch (Exception ex)
        {
            logger.LogError("Erro ao procurar os perfis de pacientes/cuidadores para o usuário");
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> RemovePatientCaregiverRelation(Guid patientId, Guid caregiverId)
    {
        try
        {
            logger.LogInformation($"Removendo a relação de cuidador entre o paciente {patientId} e o cuidador {caregiverId}");
            var patientsCaregivers = await patientCaregiverRepository.Get(patientId);

            if (patientsCaregivers.Count == 0) return false;

            var relation = patientsCaregivers.FirstOrDefault();

            var hasCaregiverId = relation!.CaregiverId.Equals(caregiverId);

            if (!hasCaregiverId)
            {
                logger.LogWarning("Esses usuários não tem uma relação de cuidador registrada");
                return false;
            }

            relation.DeletedAt = DateTime.UtcNow;  

            await patientCaregiverRepository.Update(relation);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError("Erro ao tentar excluir a relação entre os usuários");
            throw new Exception(ex.Message);
        }
    }
}
