using Microsoft.EntityFrameworkCore;

namespace VerstaTest.App;

public static class Extensions
{
  public static async Task MigrateDbContext<TContext>( this WebApplication app ) where TContext : DbContext
  {
    using var scope          = app.Services.CreateScope();
    var       services       = scope.ServiceProvider;
    var       crawlerContext = services.GetRequiredService<TContext>();
    await crawlerContext.Database.MigrateAsync();
  }
}