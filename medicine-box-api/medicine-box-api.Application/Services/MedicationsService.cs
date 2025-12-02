using medicine_box_api.Domain.Dtos;
using medicine_box_api.Domain.Dtos.Entities;
using medicine_box_api.Domain.Dtos.Requests;
using medicine_box_api.Domain.Interface.Repositories;
using medicine_box_api.Domain.Interface.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace medicine_box_api.Application.Services;
public class MedicationsService(
    ILogger<MedicationsService> logger,
    IMedicationsRepository medicationsRepository) : IMedicationsService
{
    public async Task<List<MedicationsDetails>> GetMedications(List<Guid>? id = null, Guid? userId = null)
    {
        try
        {
            logger.LogInformation($"Buscando as medicações do usuário {userId}");
            var result = await medicationsRepository.Get(id: id, userId: userId);

            if (result.Count == 0)
            {
                logger.LogWarning($"Não foram encontradas nenhuma medicação");
                return new List<MedicationsDetails>();
            }

            logger.LogDebug($"Medicações: {result}");
            return result.Select(x => new MedicationsDetails
            {
                Id = x.Id,
                Name = x.Name,
                Dosage = x.Dosage,
                Schedules = x.Schedules,
                Days = x.Days,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
            }).ToList();
        }
        catch (Exception ex)
        {
            logger.LogError($"Erro ao buscar as medicações do usuário {userId}");
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<List<MedicationsDetails>> GetMedicationsFromList(List<Guid> ids)
    {
        try
        {
            logger.LogInformation($"Buscando as medicações do usuário");
            var result = await medicationsRepository.GetByList(ids);

            if (result.Count == 0)
            {
                logger.LogWarning($"Não foram encontradas nenhuma medicação");
                return new List<MedicationsDetails>();
            }

            logger.LogDebug($"Medicações: {result}");
            return result.Select(x => new MedicationsDetails
            {
                Id = x.Id,
                Name = x.Name,
                Dosage = x.Dosage,
                Schedules = x.Schedules,
                Days = x.Days,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
            }).ToList();
        }
        catch (Exception ex)
        {
            logger.LogError($"Erro ao buscar as medicações do usuário para essa lista de IDs");
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<Guid?> CreateNewMedication(MedicationCreateRequest request)
    {
        try
        {
            logger.LogInformation($"Criando nova medicação para o usuário {request.UserId}");

            var guid = Guid.NewGuid();

            var medicationEntity = new MedicationEntity
            {
                Id = guid,
                CreatedAt = DateTime.UtcNow,
                DeletedAt = null,
                UserId = request.UserId,
                Name = request.Name,
                Dosage = request.Dosage,
                Days = request.Days,
                Schedules = request.Schedules,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
            };

            await medicationsRepository.Create(medicationEntity);

            var validationResult = await GetMedications(id: new List<Guid> { guid });

            if (validationResult == null
                || validationResult.Count == 0)
            {
                logger.LogWarning($"Falha durante a criação da medicação para o usuario - Guid gerado: {guid}");
                return null;
            }

            return guid;
        }
        catch (Exception ex)
        {
            logger.LogError("Erro durante a criação de uma nova medicação");
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<bool> UpdateMedication(Guid id, Guid userId, MedicationUpdateRequest request)
    {
        try
        {
            logger.LogInformation($"Atualizando os dados da medicação {id} do usuário {userId}");

            var med = await medicationsRepository.Get(id: new List<Guid> { id }, userId);

            if (med == null
                || med.Count == 0)
            {
                logger.LogWarning($"Não existe nenhuma medicação para esse ID");
                return false;
            }

            var entity = med.FirstOrDefault() ?? new MedicationEntity();

            var newEntity = MapMedicationToEntity(request: request, entity: entity);

            logger.LogDebug($"Nova entidade atualizada: {newEntity}");

            await medicationsRepository.Update(newEntity);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError($"Erro ao tentar atualziar a medicação {id}");
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<bool> DeleteMedication(Guid id, Guid userId)
    {
        try
        {
            logger.LogInformation($"Deletando a medicação {id}");
            var med = await medicationsRepository.Get(id: new List<Guid> { id }, userId);

            if (med == null
                || med.Count == 0)
            {
                logger.LogWarning("Não foi encontrada nenhuma medicação para esse ID");
                return false;
            }

            var medication = med.FirstOrDefault() ?? new MedicationEntity();

            await medicationsRepository.Delete(medication);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError($"Erro ao deletar o medicamento {id}");
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<List<MedicationsDetails>> GetActiveMedications(Guid userId)
    {
        try
        {
            logger.LogInformation($"Iniciando a busca por medicações ativas para o usuário {userId}");

            var result = await medicationsRepository.GetActiveMeds(userId: userId);

            if (result.Count == 0)
            {
                logger.LogWarning("Não foi encontrada nenhuma medicação ativa");
                return new List<MedicationsDetails>();
            }

            return result.Select(x => new MedicationsDetails
            {
                Id = x.Id,
                Name = x.Name,
                Dosage = x.Dosage,
                Schedules = x.Schedules,
                Days = x.Days,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
            }).ToList();
        }
        catch (Exception ex)
        {
            logger.LogError($"Erro ao buscar as medicações do usuário {userId}");
            throw new Exception(ex.Message, ex);
        }
    }

    #region Métodos Privados
    private MedicationEntity MapMedicationToEntity(MedicationUpdateRequest request, MedicationEntity entity)
    {
        var newEntity = new MedicationEntity
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            DeletedAt = entity.DeletedAt,
            UserId = entity.UserId,
            Name = request.Name != null
                && request.Name != string.Empty ? request.Name : entity.Name,
            Dosage = request.Dosage != null ? request.Dosage.Value : entity.Dosage,
            Days = request.Days.Count != 0 ? request.Days : entity.Days,
            Schedules = request.Schedules.Count != 0 ? request.Schedules : entity.Schedules,
            StartDate = request.StartDate != null ? request.StartDate.Value : entity.StartDate,
            EndDate = request.EndDate != null ? request.EndDate.Value : entity.EndDate,
        };

        return newEntity;
    }
    #endregion
}
