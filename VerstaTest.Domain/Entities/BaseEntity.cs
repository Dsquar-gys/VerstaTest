using VerstaTest.Domain.Dto;

namespace VerstaTest.Domain.Entities;

public abstract class BaseEntity<TDto> where TDto : BaseDto
{
  public Guid     Id        { get; init; } = Guid.NewGuid();
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

  public abstract TDto ToDto();
}