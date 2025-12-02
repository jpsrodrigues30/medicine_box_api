using medicine_box_api.Domain.Dtos;
using medicine_box_api.Domain.Dtos.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace medicine_box_api.Domain.Interface.Repositories;
public interface IMedicationsHistoryRepository
{
    Task<List<MedicationHistoryEntity>> Get(Guid? userId = null, Guid? id = null, Guid? medicationId = null, DateTime? scheduledDate = null, bool includeMedDetails = false);
    Task CreateMedicationSchedule(List<MedicationHistoryEntity> medSchedule);
    Task UpdateMedicationSchedule(List<MedicationHistoryEntity> medSchedule);
    Task<List<MedicationScheduleDate>> CheckScheduleAvaiablity(Guid medicationId, List<DateTime> schedule);
    Task<DateTime?> GetNextScheduledDate(Guid userId);
}
