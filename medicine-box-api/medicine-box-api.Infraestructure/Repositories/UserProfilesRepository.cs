using medicine_box_api.Domain.Dtos.Entities;
using medicine_box_api.Domain.Interface.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace medicine_box_api.Infraestructure.Repositories;
public class UserProfilesRepository : IUserProfilesRepository
{
    protected readonly dbContext _db;
    protected readonly DbSet<UserProfileEntity> _dbSet;

    public UserProfilesRepository(dbContext db)
    {
        _db = db;
        _dbSet = db.Set<UserProfileEntity>();
    }

    public async Task<List<UserProfileEntity>> Get(List<Guid>? id = null, string? role = null)
    {
        var query = _dbSet
            //.Where(x => x.DeletedAt == null)
            .AsNoTracking();

        if (id != null && id.Count > 0)
            query = query.Where(x => id.Contains(x.Id));

        if (role != null)
            query = query.Where(x => x.Role == role);

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task Create(UserProfileEntity user)
    {
        await _dbSet.AddAsync(user);
        await _db.SaveChangesAsync();
    }
}
