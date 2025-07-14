using Skopia.Infrastructure.Configurations;
using Skopia.Infrastructure.Data;

namespace Skopia.Tests.Infrastructure
{
    public class SkopiaDbContextFactoryTests
    {
        [Fact(DisplayName = "CreateDbContext deve retornar instância válida")]
        public void CreateDbContext_ShouldReturnDbContextInstance()
        {
            // Arrange
            var factory = new SkopiaDbContextFactory();

            // Act
            var context = factory.CreateDbContext([]);

            // Assert
            Assert.NotNull(context);
            Assert.IsType<SkopiaDbContext>(context);
        }
    }
}