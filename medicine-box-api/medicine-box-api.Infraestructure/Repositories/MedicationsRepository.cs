using medicine_box_api.Domain.Dtos.Entities;
using medicine_box_api.Domain.Interface.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace medicine_box_api.Infraestructure.Repositories;
public class MedicationsRepository : IMedicationsRepository
{
    protected readonly dbContext _db;
    protected readonly DbSet<MedicationEntity> _dbSet;

    public MedicationsRepository(dbContext db)
    {
        _db = db;
        _dbSet = db.Set<MedicationEntity>();
    }

    public async Task<List<MedicationEntity>> Get(List<Guid>? id = null, Guid? userId = null)
    {
        var query = _dbSet
            .Where(x => x.DeletedAt == null);

        if (id != null 
            && id.Count > 0)
            query = query.Where(x => id.Contains(x.Id));

        if (userId != null)
            query = query.Where(x => x.UserId == userId.Value);

        return await query
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<MedicationEntity>> GetByList(List<Guid> ids)
    {
        var query = _dbSet
            .Where(x => ids.Contains(x.Id))
            .AsNoTracking()
            .ToListAsync();

        return await query;
    }

    public async Task Create(MedicationEntity med)
    {
        await _dbSet.AddAsync(med);
        await _db.SaveChangesAsync();
    }

    public async Task Update(MedicationEntity med)
    {
        _dbSet.Update(med);
        await _db.SaveChangesAsync();
    }

    public async Task Delete(MedicationEntity med)
    {
        med.DeletedAt = DateTime.UtcNow;
        
        _dbSet.Update(med);
        await _db.SaveChangesAsync();
    }

    public async Task<List<MedicationEntity>> GetActiveMeds(Guid userId)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var query = _dbSet
            .Where(x => x.DeletedAt == null)
            .Where(x => x.UserId == userId)
            .Where(x => x.StartDate <= today)
            .Where(x => !x.EndDate.HasValue || x.EndDate.Value >= today)
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt);

        return await query.ToListAsync();
    }
}
