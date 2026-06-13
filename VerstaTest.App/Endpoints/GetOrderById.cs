using FastEndpoints;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using VerstaTest.Domain.Dto;
using VerstaTest.Infrastructure;

namespace VerstaTest.App.Endpoints;

[PublicAPI]
public sealed class GetOrderById( VerstaTestDbContext dbContext ) : EndpointWithoutRequest<OrderDto>
{
  public override void Configure()
  {
    Get( "/orders/{id}" );
    AllowAnonymous();

    DontAutoTag();
    Description( _ => _.WithTags( "Order" ) );

    Options( _ => _.WithName( nameof( GetOrderById ) ) );
  }

  public override async Task HandleAsync( CancellationToken ct )
  {
    var id = Route<Guid>( "id" );

    var entity = await dbContext.Orders
                                .AsNoTracking()
                                .SingleOrDefaultAsync( _ => _.Id == id, ct );

    if ( entity is null )
    {
      await Send.NotFoundAsync( ct );
      return;
    }

    await Send.OkAsync( entity.ToDto(), ct );
  }
}