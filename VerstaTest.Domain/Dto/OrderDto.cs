using VerstaTest.Domain.Models;

namespace VerstaTest.Domain.Dto;

public record OrderDto : BaseDto
{
  public required DeliveryAddress FromAddress  { get; init; }
  public required DeliveryAddress ToAddress    { get; init; }
  public required double          WeightKg     { get; init; }
  public required DateTime        DeliveryDate { get; init; }
}