using medicine_box_api.Domain.Dtos;
using medicine_box_api.Domain.Dtos.Entities;
using medicine_box_api.Domain.Dtos.Enum;
using medicine_box_api.Domain.Interface.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace medicine_box_api.Infraestructure.Repositories;
public class MedicationsHistoryRepository : IMedicationsHistoryRepository
{
    protected readonly dbContext _db;
    protected readonly DbSet<MedicationHistoryEntity> _dbSet;

    public MedicationsHistoryRepository(dbContext db)
    {
        _db = db;
        _dbSet = db.Set<MedicationHistoryEntity>();
    }

    public async Task<List<MedicationHistoryEntity>> Get(Guid? userId = null, Guid? id = null, Guid? medicationId = null, DateTime? scheduledDate = null, bool includeMedDetails = false)
    {
        var query = _dbSet
            .AsQueryable();

        if (userId != null)
            query = query.Where(x => x.UserId == userId.Value);

        if (id != null)
            query = query.Where(x => x.Id == id.Value);

        if (medicationId != null)
            query = query.Where(x => x.MedicationId == medicationId.Value);

        if (scheduledDate != null)
            query = query.Where(x => x.ScheduledAt == scheduledDate.Value);

        if (includeMedDetails)
            query = query.Include(x => x.Medication);

        return await query
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task CreateMedicationSchedule(List<MedicationHistoryEntity> medSchedule)
    {
        await _dbSet.AddRangeAsync(medSchedule);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateMedicationSchedule(List<MedicationHistoryEntity> medSchedule)
    {
        _dbSet.UpdateRange(medSchedule);
        await _db.SaveChangesAsync();
    }

    public async Task<List<MedicationScheduleDate>> CheckScheduleAvaiablity(Guid medicationId, List<DateTime> schedule)
    {
        return await _dbSet
            .Where(x => x.MedicationId == medicationId)
            .Where(x => schedule.Contains(x.ScheduledAt))
            .Where(x => x.Status == MedicationScheduleStatusEnum.Scheduled.ToString())
            .Select(x => new MedicationScheduleDate
            {
                MedicationId = x.MedicationId,
                ScheduledAt = x.ScheduledAt,
                Status = x.Status,
            })
            .ToListAsync();
    }

    public async Task<DateTime?> GetNextScheduledDate(Guid userId)
    {
        return await _dbSet
            .Where(x => x.UserId == userId)
            .Where(x => x.Status == MedicationScheduleStatusEnum.Scheduled.ToString())
            .OrderBy(x => x.ScheduledAt)
            .Select(x => x.ScheduledAt)
            .FirstOrDefaultAsync();
    }
}
