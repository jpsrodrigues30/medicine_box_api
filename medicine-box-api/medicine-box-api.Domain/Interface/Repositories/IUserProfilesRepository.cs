using medicine_box_api.Domain.Dtos.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace medicine_box_api.Domain.Interface.Repositories;
public interface IUserProfilesRepository
{
    Task<List<UserProfileEntity>> Get(List<Guid>? id = null, string? role = null);
    Task Create(UserProfileEntity user);
}
