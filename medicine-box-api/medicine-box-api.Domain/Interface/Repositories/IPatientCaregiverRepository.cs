using medicine_box_api.Domain.Dtos.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace medicine_box_api.Domain.Interface.Repositories;
public interface IPatientCaregiverRepository
{
    Task<List<PatientCaregiversEntity>> Get(Guid? patientId = null, Guid? caregiverId = null);
    Task Update(PatientCaregiversEntity entity);
}
