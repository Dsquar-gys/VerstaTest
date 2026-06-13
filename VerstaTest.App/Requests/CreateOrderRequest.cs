using FastEndpoints;
using FluentValidation;
using JetBrains.Annotations;
using VerstaTest.Domain.Models;

namespace VerstaTest.App.Requests;

[UsedImplicitly]
public sealed record CreateOrderRequest
{
  public required DeliveryAddress FromAddress  { get; init; }
  public required DeliveryAddress ToAddress    { get; init; }
  public required double          WeightKg     { get; init; }
  public required DateTime        DeliveryDate { get; init; }

  internal sealed class CreateOrderRequestValidator : Validator<CreateOrderRequest>
  {
    public CreateOrderRequestValidator()
    {
      RuleFor( _ => _.FromAddress )
        .NotNull()
        .WithMessage( "Адрес отправления обязателен" )
        .ChildRules( address =>
                     {
                       address.RuleFor( a => a.City )
                              .NotNull()
                              .NotEmpty()
                              .WithMessage( "Город отправления обязателен" )
                              .MaximumLength( 100 )
                              .WithMessage( "Город отправления не должен превышать 100 символов" );

                       address.RuleFor( a => a.Address )
                              .NotNull()
                              .NotEmpty()
                              .WithMessage( "Адрес отправления обязателен" )
                              .MaximumLength( 500 )
                              .WithMessage( "Адрес отправления не должен превышать 500 символов" );
                     } );

      RuleFor( _ => _.ToAddress )
        .NotNull()
        .WithMessage( "Адрес доставки обязателен" )
        .ChildRules( address =>
                     {
                       address.RuleFor( a => a.City )
                              .NotNull()
                              .NotEmpty()
                              .WithMessage( "Город доставки обязателен" )
                              .MaximumLength( 100 )
                              .WithMessage( "Город доставки не должен превышать 100 символов" );

                       address.RuleFor( a => a.Address )
                              .NotNull()
                              .NotEmpty()
                              .WithMessage( "Адрес доставки обязателен" )
                              .MaximumLength( 500 )
                              .WithMessage( "Адрес доставки не должен превышать 500 символов" );
                     } );

      RuleFor( _ => _.WeightKg )
        .GreaterThan( 0 )
        .WithMessage( "Вес должен быть больше 0" )
        .LessThanOrEqualTo( 100 )
        .WithMessage( "Вес не должен превышать 100 кг" );

      RuleFor( _ => _.DeliveryDate )
        .GreaterThan( DateTime.UtcNow )
        .WithMessage( "Дата доставки должна быть в будущем" )
        .LessThan( DateTime.UtcNow.AddYears( 1 ) )
        .WithMessage( "Дата доставки не может быть более чем через год" );

      RuleFor( _ => _ )
        .Must( _ => _.FromAddress != _.ToAddress )
        .WithMessage( "Адрес отправления и адрес доставки не должны совпадать" );
    }
  }
}