using UserManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagement.Api.Extentions
{
    public static class MigrationsExtension
    {
        public static void MigrateIfDbNotCreated(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                if (!scope.ServiceProvider.
                    GetRequiredService<ApplicationDbContext>().
                    Database.CanConnect())
                {
                    scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
                }
            }
        }
    }
}
