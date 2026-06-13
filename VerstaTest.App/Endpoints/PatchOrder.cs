using FastEndpoints;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using VerstaTest.App.Requests;
using VerstaTest.Domain.Dto;
using VerstaTest.Infrastructure;

namespace VerstaTest.App.Endpoints;

[PublicAPI]
public sealed class PatchOrder( VerstaTestDbContext dbContext ) : Endpoint<PatchOrderRequest, OrderDto>
{
  public override void Configure()
  {
    Patch( "/orders/{id}" );
    AllowAnonymous();

    DontAutoTag();
    Description( _ => _.WithTags( "Order" ) );

    Options( _ => _.WithName( nameof( PatchOrder ) ) );
  }

  public override async Task HandleAsync( PatchOrderRequest req, CancellationToken ct )
  {
    var id = Route<Guid>( "id" );

    var entity = await dbContext.Orders
                                .SingleOrDefaultAsync( o => o.Id == id, ct );

    if ( entity is null )
    {
      await Send.NotFoundAsync( ct );
      return;
    }

    if ( req.FromAddress is not null )
      entity.FromAddress = req.FromAddress;

    if ( req.ToAddress is not null )
      entity.ToAddress = req.ToAddress;

    if ( req.WeightKg.HasValue )
      entity.WeightKg = req.WeightKg.Value;

    if ( req.DeliveryDate.HasValue )
      entity.DeliveryDate = req.DeliveryDate.Value;

    await dbContext.SaveChangesAsync( ct );

    await Send.OkAsync( entity.ToDto(), ct );
  }
}