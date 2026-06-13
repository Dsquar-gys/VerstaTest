using FastEndpoints;
using FluentValidation;
using JetBrains.Annotations;

namespace VerstaTest.App.Requests;

[PublicAPI]
public record GetPageRequest
{
  public int Page     { get; init; } = 1;
  public int PageSize { get; init; } = 20;

  public class PageRequestValidator : Validator<GetPageRequest>
  {
    public PageRequestValidator()
    {
      RuleFor( _ => _.Page ).GreaterThan( 0 );
      RuleFor( _ => _.PageSize ).InclusiveBetween( 1, 100 );
    }
  }
}