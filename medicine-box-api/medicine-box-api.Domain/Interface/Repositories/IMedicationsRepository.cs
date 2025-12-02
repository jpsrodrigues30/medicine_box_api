using medicine_box_api.Domain.Dtos.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace medicine_box_api.Domain.Interface.Repositories;
public interface IMedicationsRepository
{
    Task<List<MedicationEntity>> Get(List<Guid>? id = null, Guid? userId = null);
    Task<List<MedicationEntity>> GetByList(List<Guid> ids);
    Task Create(MedicationEntity med);
    Task Update(MedicationEntity med);
    Task Delete(MedicationEntity med);
    Task<List<MedicationEntity>> GetActiveMeds(Guid userId);
}
