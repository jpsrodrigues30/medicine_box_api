using medicine_box_api.Domain.Dtos.Entities;
using medicine_box_api.Domain.Interface.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace medicine_box_api.Infraestructure.Repositories;
public class PatientCaregiverRepository : IPatientCaregiverRepository
{
    protected readonly dbContext _db;
    protected readonly DbSet<PatientCaregiversEntity> _dbSet;

    public PatientCaregiverRepository(dbContext db)
    {
        _db = db;
        _dbSet = db.Set<PatientCaregiversEntity>();
    }

    public async Task<List<PatientCaregiversEntity>> Get(Guid? patientId = null, Guid? caregiverId = null)
    {
        var query = _dbSet
            .Where(x => x.DeletedAt == null)
            .AsQueryable();

        if (patientId != null)
            query = query.Where(x => x.PatientId == patientId);

        if (caregiverId != null)
            query = query.Where(x => x.CaregiverId == caregiverId);

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task Update(PatientCaregiversEntity entity)
    {
        _dbSet.Update(entity);
        await _db.SaveChangesAsync();
    }
}
