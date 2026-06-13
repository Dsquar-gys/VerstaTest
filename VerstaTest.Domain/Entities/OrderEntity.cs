using VerstaTest.Domain.Dto;
using VerstaTest.Domain.Models;

namespace VerstaTest.Domain.Entities;

public class OrderEntity : BaseEntity<OrderDto>
{
  public required DeliveryAddress FromAddress  { get; set; }
  public required DeliveryAddress ToAddress    { get; set; }
  public required double          WeightKg     { get; set; }
  public required DateTime        DeliveryDate { get; set; }

  public override OrderDto ToDto()
  {
    return new OrderDto
           {
             Id           = Id,
             CreatedAt    = CreatedAt,
             FromAddress  = FromAddress,
             ToAddress    = ToAddress,
             WeightKg     = WeightKg,
             DeliveryDate = DeliveryDate
           };
  }
}