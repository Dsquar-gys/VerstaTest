using FastEndpoints;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using VerstaTest.Infrastructure;

namespace VerstaTest.App.Endpoints;

[PublicAPI]
public sealed class DeleteOrder( VerstaTestDbContext dbContext ) : EndpointWithoutRequest
{
  public override void Configure()
  {
    Delete( "/orders/{id}" );
    AllowAnonymous();

    DontAutoTag();
    Description( _ => _.WithTags( "Order" ) );

    Options( _ => _.WithName( nameof( DeleteOrder ) ) );
  }

  public override async Task HandleAsync( CancellationToken ct )
  {
    var id = Route<Guid>( "id" );

    var deletedCount = await dbContext.Orders
                                      .Where( o => o.Id == id )
                                      .ExecuteDeleteAsync( ct );

    if ( deletedCount == 0 )
    {
      await Send.NotFoundAsync( ct );
      return;
    }

    await Send.NoContentAsync( ct );
  }
}