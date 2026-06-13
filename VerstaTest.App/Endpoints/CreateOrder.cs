using FastEndpoints;
using JetBrains.Annotations;
using VerstaTest.App.Requests;
using VerstaTest.Domain.Dto;
using VerstaTest.Domain.Entities;
using VerstaTest.Infrastructure;

namespace VerstaTest.App.Endpoints;

[PublicAPI]
public sealed class CreateOrder( VerstaTestDbContext dbContext ) : Endpoint<CreateOrderRequest, OrderDto>
{
  public override void Configure()
  {
    Post( "/order" );
    AllowAnonymous();

    DontAutoTag();
    Description( _ => _.WithTags( "Order" ) );

    Options( _ => _.WithName( nameof( CreateOrder ) ) );
  }

  public override async Task HandleAsync( CreateOrderRequest req, CancellationToken ct )
  {
    try
    {
      var proxy = new OrderEntity
                  {
                    FromAddress  = req.FromAddress,
                    ToAddress    = req.ToAddress,
                    WeightKg     = req.WeightKg,
                    DeliveryDate = req.DeliveryDate
                  };

      dbContext.Orders.Add( proxy );
      await dbContext.SaveChangesAsync( ct );

      await Send.CreatedAtAsync<GetOrderById>( new { id = proxy.Id },
                                               proxy.ToDto(),
                                               cancellation: ct );
    }
    catch ( Exception e )
    {
      AddError( e.Message );
      await Send.ErrorsAsync( cancellation: ct );
    }
  }
}