using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace VerstaTest.Infrastructure.ContextFactories;

/// <summary>
///   Фабрика для миграций
/// </summary>
[UsedImplicitly]
internal class VerstaTestDbContextFactory : IDesignTimeDbContextFactory<VerstaTestDbContext>
{
  public VerstaTestDbContext CreateDbContext( string[] args )
  {
    var optionsBuilder = new DbContextOptionsBuilder<VerstaTestDbContext>();

    optionsBuilder
      .UseNpgsql( "Host=localhost;Port=5432;Database=CrawlerTaskSourceDB;Username=crawler;Password=aE–f<ksWc=Yu!4s" );

    return new VerstaTestDbContext( optionsBuilder.Options );
  }
}