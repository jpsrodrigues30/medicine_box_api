using medicine_box_api.Domain.Dtos;
using medicine_box_api.Domain.Dtos.Requests;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace medicine_box_api.Domain.Interface.Services;
public interface IMedicationsService
{
    Task<List<MedicationsDetails>> GetMedications(List<Guid>? id = null, Guid? userId = null);
    Task<List<MedicationsDetails>> GetMedicationsFromList(List<Guid> ids);
    Task<Guid?> CreateNewMedication(MedicationCreateRequest request);
    Task<bool> UpdateMedication(Guid id, Guid userId, MedicationUpdateRequest request);
    Task<bool> DeleteMedication(Guid id, Guid userId);
    Task<List<MedicationsDetails>> GetActiveMedications(Guid userId);
}
