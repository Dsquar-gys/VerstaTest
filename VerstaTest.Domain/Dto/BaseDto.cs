namespace VerstaTest.Domain.Dto;

public abstract record BaseDto
{
  public required Guid     Id        { get; init; }
  public required DateTime CreatedAt { get; init; }
}