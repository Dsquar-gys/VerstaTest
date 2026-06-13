using FastEndpoints;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using VerstaTest.App.Requests;
using VerstaTest.App.Responses;
using VerstaTest.Domain.Dto;
using VerstaTest.Infrastructure;

namespace VerstaTest.App.Endpoints;

[PublicAPI]
public sealed class GetOrders( VerstaTestDbContext dbContext ) : Endpoint<GetPageRequest, GetPageResponse<OrderDto>>
{
  public override void Configure()
  {
    Get( "/orders" );
    AllowAnonymous();

    DontAutoTag();
    Description( _ => _.WithTags( "Order" ) );

    Options( _ => _.WithName( nameof( GetOrders ) ) );
  }

  public override async Task<GetPageResponse<OrderDto>> ExecuteAsync( GetPageRequest req, CancellationToken ct )
  {
    var query = dbContext.Orders.AsNoTracking();

    var totalCount = await query.CountAsync( ct );

    var items = await query.OrderByDescending( _ => _.CreatedAt )
                           .Skip( ( req.Page - 1 ) * req.PageSize )
                           .Take( req.PageSize )
                           .Select( _ => _.ToDto() )
                           .ToListAsync( ct );

    return new GetPageResponse<OrderDto>
           {
             Items      = items,
             TotalCount = totalCount,
             Page       = req.Page,
             PageSize   = req.PageSize
           };
  }
}