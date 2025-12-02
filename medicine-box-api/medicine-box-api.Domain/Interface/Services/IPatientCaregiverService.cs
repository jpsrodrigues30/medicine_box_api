using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace medicine_box_api.Domain.Interface.Services;
public interface IPatientCaregiverService
{
    Task<List<Guid>> GetUserPatientCaregiver(Guid userId, bool isPatient = false);
    Task<bool> RemovePatientCaregiverRelation(Guid patientId, Guid caregiverId);
}
