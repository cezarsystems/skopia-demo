using Microsoft.EntityFrameworkCore;
using Skopia.Application.Services;
using Skopia.Domain.Models;
using Skopia.Infrastructure.Data;

namespace Skopia.Tests.Services
{
    public class UserServiceTests
    {
        private DbContextOptions<SkopiaDbContext> _dbOptions;

        public UserServiceTests()
        {
            _dbOptions = new DbContextOptionsBuilder<SkopiaDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact(DisplayName = "Exists deve retornar true se usuário existir")]
        public async Task Exists_ShouldReturnTrue_WhenUserExists()
        {
            using var context = new SkopiaDbContext(_dbOptions);
            context.Users.Add(new UserModel { Id = 1, Name = "Teste", Role = "user" });
            await context.SaveChangesAsync();

            var service = new UserService(context);
            var result = await service.Exists(1);

            Assert.True(result);
        }

        [Fact(DisplayName = "Exists deve retornar false se usuário não existir")]
        public async Task Exists_ShouldReturnFalse_WhenUserNotExists()
        {
            using var context = new SkopiaDbContext(_dbOptions);
            var service = new UserService(context);

            var result = await service.Exists(999);

            Assert.False(result);
        }

        [Fact(DisplayName = "IsManager deve retornar true para usuário com role 'mgr'")]
        public async Task IsManager_ShouldReturnTrue_WhenRoleIsMgr()
        {
            using var context = new SkopiaDbContext(_dbOptions);
            context.Users.Add(new UserModel { Id = 2, Name = "Manager", Role = "mgr" });
            await context.SaveChangesAsync();

            var service = new UserService(context);
            var result = await service.IsManager(2);

            Assert.True(result);
        }

        [Fact(DisplayName = "IsManager deve retornar false se não for 'mgr'")]
        public async Task IsManager_ShouldReturnFalse_WhenRoleIsNotMgr()
        {
            using var context = new SkopiaDbContext(_dbOptions);
            context.Users.Add(new UserModel { Id = 3, Name = "Usuário", Role = "user" });
            await context.SaveChangesAsync();

            var service = new UserService(context);
            var result = await service.IsManager(3);

            Assert.False(result);
        }

        [Fact(DisplayName = "IsManager deve retornar false se usuário não existir")]
        public async Task IsManager_ShouldReturnFalse_WhenUserNotExists()
        {
            using var context = new SkopiaDbContext(_dbOptions);
            var service = new UserService(context);

            var result = await service.IsManager(999);

            Assert.False(result);
        }
    }
}