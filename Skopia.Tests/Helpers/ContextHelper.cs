using Skopia.Infrastructure.Data;
using System.Diagnostics;

namespace Skopia.Tests.Helpers
{
    public static class ContextHelper
    {
        public static async Task SaveChangesSafeAsync(SkopiaDbContext dbContext)
        {
            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao salvar no contexto: {ex.Message}");

                if (ex.InnerException != null)
                    Debug.WriteLine($"InnerException: {ex.InnerException.Message}");

                throw;
            }
        }
    }
}