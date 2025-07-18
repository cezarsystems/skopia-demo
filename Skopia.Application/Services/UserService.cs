﻿using Microsoft.EntityFrameworkCore;
using Skopia.Application.Contracts;
using Skopia.Infrastructure.Data;

namespace Skopia.Application.Services
{
    public class UserService : IUserService
    {
        private readonly SkopiaDbContext _dbContext;

        public UserService(SkopiaDbContext dbContext) => _dbContext = dbContext;

        public async Task<bool> Exists(long id)
        {
            return await _dbContext.Users.AnyAsync(p => p.Id == id);
        }

        public async Task<bool> IsManager(long userId)
        {
            var user = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user?.Role == "mgr";
        }
    }
}