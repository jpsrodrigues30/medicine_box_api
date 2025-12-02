using medicine_box_api.Domain.Dtos;
using medicine_box_api.Domain.Dtos.Dictionary;
using medicine_box_api.Domain.Dtos.Entities;
using medicine_box_api.Domain.Dtos.Enum;
using medicine_box_api.Domain.Helpers;
using medicine_box_api.Domain.Interface.Repositories;
using medicine_box_api.Domain.Interface.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace medicine_box_api.Application.Services;
public class MedicationsHistoryService(
    ILogger<MedicationsHistoryService> logger,
    IMedicationsService medicationService,
    IUserProfileService userProfileService,
    IMedicationsHistoryRepository medicationsHistoryRepository) : IMedicationsHistoryService
{
    public async Task<List<MedicationHistoryDetails>> GetMedicationsHistory(Guid? userId = null, Guid? medicationId = null)
    {
        var result = await medicationsHistoryRepository.Get(userId);

        if (result == null
            || result.Count == 0)
        {
            logger.LogWarning("Não foi encontrado histórico de medicações");
            return new List<MedicationHistoryDetails>();
        }

        return result.Select(x => new MedicationHistoryDetails
        {
            UserId = x.UserId,
            MedicationId = x.MedicationId,
            Status = x.Status,
            ScheduledAt = x.ScheduledAt,
            Dosage = x.Dosage,
            Timezone = x.Timezone ?? string.Empty
        }).ToList();
    }

    public async Task<bool> UpsertMedicationSchedule(Guid userId, Guid medicationId)
    {
        try
        {
            logger.LogInformation($"Criando agendamentos para a medicação {medicationId} do usuário {userId}");

            var meds = await medicationService.GetMedications(id: new List<Guid> { medicationId }, userId: userId);

            if (meds.Count == 0) return false;

            var medDetails = meds.FirstOrDefault();

            if (medDetails == null)
            {
                logger.LogWarning("Não existem detalhes para esta medicação");
                return false;
            }

            logger.LogDebug($"MedDetails: {medDetails}");

            var selectedDays = new HashSet<DayOfWeek>(
                medDetails!.Days
                .Where(d => WeekdayMapper.TryGetDay(d, out _))
                .Select(d => WeekdayMapper.All[d])
             );

            logger.LogDebug($"Dias selecionados: {selectedDays}");

            var userTimezone = await GetUserTimezone(userId);

            logger.LogDebug($"timezone: {userTimezone}");

            var timezone = TimezoneResolver.TryResolve(userTimezone, out var tzInfo)
                ? tzInfo
                : TimeZoneInfo.Utc;

            logger.LogDebug($"timezone resolved: {timezone}");

            var nowUtc = DateTime.UtcNow;
            var clientNow = TimeZoneInfo.ConvertTime(nowUtc, timezone);
            var dosage = medDetails.Dosage ?? 1;

            var candidates = new List<MedicationHistoryEntity>();

            for (var day = medDetails.StartDate; day <= medDetails.EndDate; day = day.Value.AddDays(1))
            {
                var weekDay = day.Value.ToDateTime(TimeOnly.MinValue).DayOfWeek;

                if (!selectedDays.Contains(weekDay)) continue;

                foreach (var sched in medDetails.Schedules)
                {
                    var localUnspecified = day.Value.ToDateTime(sched);
                    var localForCompare = DateTime.SpecifyKind(localUnspecified, DateTimeKind.Unspecified);

                    if (localForCompare < clientNow) continue;

                    var scheduledAtUtc = TimeZoneInfo.ConvertTimeToUtc(localForCompare, timezone);

                    logger.LogDebug($"Agendamento criado para horário (UTC) {scheduledAtUtc} - timezone original: {timezone}");
                    candidates.Add(new MedicationHistoryEntity
                    {
                        Id = Guid.NewGuid(),
                        MedicationId = medicationId,
                        UserId = userId,
                        Status = MedicationScheduleStatusEnum.Scheduled.ToString(),
                        LastStatsUpdate = null,
                        CreatedAt = nowUtc,
                        ScheduledAt = scheduledAtUtc,
                        Dosage = dosage,
                        Timezone = userTimezone
                    });
                    logger.LogDebug($"Nova entidade criada: {candidates.Last()}");
                }
            }

            if (candidates.Count == 0) return false;

            var wantedTimes = candidates.Select(c => c.ScheduledAt).ToList();
            var already = await medicationsHistoryRepository.CheckScheduleAvaiablity(medicationId: medicationId, schedule: wantedTimes);

            if (already.Count > 0)
            {
                logger.LogWarning($"Já existe um agendamento para essa medicação {medicationId} para esses horários");
                return false;
            }

            logger.LogInformation($"Criando {candidates.Count} novos agendamentos");
            await medicationsHistoryRepository.CreateMedicationSchedule(candidates);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError("Erro ao gravar os agendamentos da medicação");
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<bool> UpdateMedicationScheduleStatus(Guid userId, Guid medSchedId, string status)
    {
        try
        {
            logger.LogInformation($"Alterando o agendamento {medSchedId} do usuário {userId} para o status {status}");

            var statusEnum = EnumHelper.TryParseEnum<MedicationScheduleStatusEnum>(status, true);

            if (statusEnum == null)
            {
                logger.LogWarning("Esse valor de Status não é válido!");
                return false;
            }

            var medSchedule = await medicationsHistoryRepository.Get(userId: userId, id: medSchedId);

            if (medSchedule == null
                || medSchedule.Count == 0)
            {
                logger.LogWarning("Não foi encotnrado nenhuma medicação para esse ID e usuário");
                return false;
            }

            var sched = medSchedule.FirstOrDefault();

            sched!.Status = status.ToString();
            sched.LastStatsUpdate = DateTime.UtcNow;

            await medicationsHistoryRepository.UpdateMedicationSchedule(medSchedule);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError("Erro ao tentar atualizar o status dos agendamentos");
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<bool> UpdateMedicationScheduleDosage(Guid userId, Guid medicationId, int dosage)
    {
        try
        {
            logger.LogInformation($"Alterando a dosagem (novo valor: {dosage}) dos agendamentos criados para a medicação {medicationId} do usuário {userId}");

            var medSched = await medicationsHistoryRepository.Get(userId: userId, medicationId: medicationId);

            if (medSched == null 
                || medSched.Count == 0)
            {
                logger.LogWarning("Não foi possível encontrar nenhuma medicação com esses parâmetros");
                return false;
            }

            foreach (var sched in medSched)
            {
                sched.Dosage = dosage;
            }

            await medicationsHistoryRepository.UpdateMedicationSchedule(medSched);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError("Erro ao tentar atualizar a dosagem dos agendamentos");
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<bool> CancellAllMedicationSchedules(Guid userId, Guid medicationId)
    {
        try
        {
            logger.LogInformation($"Cancelando todas as medicações de ID {medicationId} para o usuário {userId}");
            var medSchedule = await medicationsHistoryRepository.Get(userId: userId, medicationId: medicationId);

            if (medSchedule == null 
                || medSchedule.Count == 0)
            {
                logger.LogWarning("Não foi encontrado nenhum agendamento para essa medicação");
                return false;
            }

            var medCancelled = new List<MedicationHistoryEntity>();
            foreach(var med in medSchedule)
            {
                med.Status = MedicationScheduleStatusEnum.Cancelled.ToString();
                medCancelled.Add(med);
            }

            logger.LogInformation($"Cancelando {medCancelled.Count} medicações");
            await medicationsHistoryRepository.UpdateMedicationSchedule(medCancelled);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError($"Erro ao tentar cancelar todos os agendamentos da medicação {medicationId} - user: {userId}");
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<NextMedicationSchedule> GetNextMedicationHistory(Guid userId, DateTime? nextScheduledDate = null)
    {
        try
        {
            logger.LogInformation($"Buscando a próxima medicação agendada para o usuário {userId}");

            var nextDate = nextScheduledDate;

            if (nextScheduledDate == null)
            {
                var nearestScheduledDate = await medicationsHistoryRepository.GetNextScheduledDate(userId: userId);

                if (nearestScheduledDate == null)
                {
                    logger.LogWarning("Não foi encontrada nenhuma data agendada");
                    return new NextMedicationSchedule();
                }

                nextDate = nearestScheduledDate;
            }

            var medAlarms = await medicationsHistoryRepository.Get(userId: userId, scheduledDate: nextDate!.Value);

            var medIds = medAlarms
                .Select(m => m.MedicationId)
                .ToList();

            var medsDetails = await medicationService.GetMedicationsFromList(ids: medIds);

            var nextMedSched = new List<NextMedicationSchedDetails>();
            var index = 0;
            var alarmIds = medAlarms
                .Select(meds => meds.Id)
                .ToList();

            foreach (var detail in medsDetails)
            {
                nextMedSched.Add(new NextMedicationSchedDetails
                {
                    AlarmId = alarmIds[index],
                    MedicationId = detail.Id,
                    MedicationName = detail.Name,
                    Dosage = detail.Dosage ?? 1,
                });
                index++;
            }

            return new NextMedicationSchedule
            {
                UserId = userId,
                ScheduledAt = nextDate.Value,
                NextMedicationDetails = nextMedSched,
            };
        }
        catch (Exception ex)
        {
            logger.LogError("Erro ao buscar a próxima medicação agendada do uusário");
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<List<MedicationHistoryWithMedDetails>> GetMedicationHistoryWithMedDetails(Guid userId)
    {
        try
        {
            logger.LogInformation($"Buscando o histórico de medicações com os detalhes das medicações para o usuario {userId}");

            var result = await medicationsHistoryRepository.Get(userId: userId, includeMedDetails: true);

            if (result == null
                || result.Count == 0)
            {
                logger.LogWarning("Não foi encontrado nenhum historico para esse usuario");
                return new List<MedicationHistoryWithMedDetails>();
            }

            return result.Select(e => new MedicationHistoryWithMedDetails
            {
                History = new MedicationHistoryDetails
                {
                    UserId = e.UserId,
                    MedicationId = e.Id,
                    Status = e.Status,
                    ScheduledAt = e.ScheduledAt,
                    Dosage = e.Dosage,
                    Timezone = e.Timezone ?? string.Empty
                },
                MedicationName = e.Medication?.Name ?? string.Empty
            }).ToList();
        }
        catch (Exception ex)
        {
            logger.LogError("Erro ao buscar o histórico do usuário + detalhes das medicações");
            throw new Exception(ex.Message, ex);
        }
    }
    #region Métodos Privados
    private async Task<string> GetUserTimezone(Guid userId)
    {
        var users = await userProfileService.GetUserProfiles(id: new List<Guid> { userId });

        if (users == null
            || users.Count == 0) return string.Empty;

        var user = users.FirstOrDefault();

        return user.Timezone ?? string.Empty;
    }
    #endregion
}
