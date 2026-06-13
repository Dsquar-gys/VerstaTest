using System.Text.Json;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using VerstaTest.App;
using VerstaTest.Infrastructure;

try
{
  var builder = WebApplication.CreateBuilder( args );

  builder.Host
         .UseSerilog( ( context,
                        _,
                        configuration ) =>
                      {
                        configuration.ReadFrom
                                     .Configuration( context.Configuration )
                                     .Enrich.FromLogContext()
                                     .WriteTo.Console( theme: AnsiConsoleTheme.Code )
                                     .Filter.ByExcluding( logEvent => logEvent.Properties.TryGetValue( "RequestPath",
                                                                        out var path ) &&
                                                                      path.ToString().Contains( "/health" ) );
                      } );

  builder.Configuration.AddEnvironmentVariables();
  
  builder.Services.AddCors(options =>
                           {
                             options.AddPolicy("AllowFrontend", policy =>
                                                                {
                                                                  policy.WithOrigins("http://localhost:3000")
                                                                        .AllowAnyHeader()
                                                                        .AllowAnyMethod();
                                                                });
                           });

  builder.Services.AddDbContext<VerstaTestDbContext>( options =>
                                                        options.UseNpgsql( builder.Configuration
                                                                             .GetConnectionString( "VerstaTestDb" ) )
                                                               .LogTo( Log.Logger.Debug )
                                                               .EnableDetailedErrors() );

  builder.Services
         .AddFastEndpoints()
         .SwaggerDocument( o =>
                           {
                             o.MaxEndpointVersion      = 1;
                             o.UseOneOfForPolymorphism = true;
                             o.ShortSchemaNames        = true;
                             o.DocumentSettings = s =>
                                                  {
                                                    s.Version      = "v1";
                                                    s.Title        = "Crawler API";
                                                    s.DocumentName = "v1";
                                                  };
                           } )
         .AddHealthChecks();

  var app = builder.Build();
  
  app.UseCors("AllowFrontend");

  app.UseHealthChecks( "/health",
                       new HealthCheckOptions
                       {
                         Predicate = _ => true,
                         ResponseWriter = async ( context,
                                                  report ) =>
                                          {
                                            context.Response.ContentType = "application/json";

                                            var result = JsonSerializer.Serialize( new
                                              {
                                                status = report.Status.ToString(),
                                                checks =
                                                  report.Entries
                                                        .Select( e => new
                                                                      {
                                                                        name        = e.Key,
                                                                        status      = e.Value.Status.ToString(),
                                                                        description = e.Value.Description,
                                                                        exception   = e.Value.Exception?.Message,
                                                                        duration    = e.Value.Duration
                                                                      } ),
                                                totalDuration =
                                                  report.TotalDuration
                                              } );

                                            await context.Response.WriteAsync( result );
                                          }
                       } );

  await app.MigrateDbContext<VerstaTestDbContext>();

  app.UseFastEndpoints( c =>
                        {
                          c.Errors.UseProblemDetails();
                          c.Endpoints.RoutePrefix = "api";
                        } );
  app.UseOpenApi( c => c.Path = "/openapi/{documentName}.json" );
  app.MapScalarApiReference( options => options.AddDocument( "v1", "/openapi/v1.json" ) );

  if ( app.Environment.IsDevelopment() ) app.MapOpenApi();

  app.UseHttpsRedirection();

  await app.RunAsync();
}
catch ( Exception e )
{
  Log.Fatal( e, "Критическая ошибка сервиса" );
}
finally
{
  Log.CloseAndFlush();
}