using medicine_box_api.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace medicine_box_api.Domain.Interface.Services;
public interface IMedicationsHistoryService
{
    Task<List<MedicationHistoryDetails>> GetMedicationsHistory(Guid? userId = null, Guid? medicationId = null);
    Task<bool> UpsertMedicationSchedule(Guid userId, Guid medicationId);
    Task<bool> UpdateMedicationScheduleStatus(Guid userId, Guid medSchedId, string status);
    Task<bool> UpdateMedicationScheduleDosage(Guid userId, Guid medicationId, int dosage);
    Task<bool> CancellAllMedicationSchedules(Guid userId, Guid medicationId);
    Task<NextMedicationSchedule> GetNextMedicationHistory(Guid userId, DateTime? nextScheduledDate = null);
    Task<List<MedicationHistoryWithMedDetails>> GetMedicationHistoryWithMedDetails(Guid userId);
}
