using FastEndpoints;
using FluentValidation;
using JetBrains.Annotations;
using VerstaTest.Domain.Models;

namespace VerstaTest.App.Requests;

[UsedImplicitly]
public sealed record PatchOrderRequest
{
  public DeliveryAddress? FromAddress  { get; init; }
  public DeliveryAddress? ToAddress    { get; init; }
  public double?          WeightKg       { get; init; }
  public DateTime?        DeliveryDate { get; init; }

  internal sealed class PatchOrderRequestValidator : Validator<PatchOrderRequest>
  {
    public PatchOrderRequestValidator()
    {
      RuleFor( _ => _ )
        .Must( _ => _.FromAddress != null ||
                    _.ToAddress   != null ||
                    _.WeightKg.HasValue     ||
                    _.DeliveryDate.HasValue )
        .WithMessage( "Должно быть указано хотя бы одно поле для обновления" );

      When( _ => _.FromAddress != null, () =>
                                        {
                                          RuleFor( _ => _.FromAddress! )
                                            .ChildRules( address =>
                                                         {
                                                           address.RuleFor( a => a.City )
                                                                  .NotNull()
                                                                  .NotEmpty()
                                                                  .WithMessage( "Город отправления обязателен" )
                                                                  .MaximumLength( 100 );

                                                           address.RuleFor( a => a.Address )
                                                                  .NotNull()
                                                                  .NotEmpty()
                                                                  .WithMessage( "Адрес отправления обязателен" )
                                                                  .MaximumLength( 500 );
                                                         } );
                                        } );

      When( _ => _.ToAddress != null, () =>
                                      {
                                        RuleFor( _ => _.ToAddress! )
                                          .ChildRules( address =>
                                                       {
                                                         address.RuleFor( a => a.City )
                                                                .NotNull()
                                                                .NotEmpty()
                                                                .WithMessage( "Город доставки обязателен" )
                                                                .MaximumLength( 100 );

                                                         address.RuleFor( a => a.Address )
                                                                .NotNull()
                                                                .NotEmpty()
                                                                .WithMessage( "Адрес доставки обязателен" )
                                                                .MaximumLength( 500 );
                                                       } );
                                      } );

      When( _ => _.WeightKg.HasValue, () =>
                                    {
                                      RuleFor( _ => _.WeightKg!.Value )
                                        .GreaterThan( 0 )
                                        .WithMessage( "Вес должен быть больше 0" )
                                        .LessThanOrEqualTo( 100 )
                                        .WithMessage( "Вес не должен превышать 100 кг" );
                                    } );

      When( _ => _.DeliveryDate.HasValue, () =>
                                          {
                                            RuleFor( _ => _.DeliveryDate!.Value )
                                              .GreaterThan( DateTime.UtcNow )
                                              .WithMessage( "Дата доставки должна быть в будущем" )
                                              .LessThan( DateTime.UtcNow.AddYears( 1 ) )
                                              .WithMessage( "Дата доставки не может быть более чем через год" );
                                          } );

      When( _ => _.FromAddress != null && _.ToAddress != null, () =>
                                                               {
                                                                 RuleFor( _ => _ )
                                                                   .Must( _ => _.FromAddress != _.ToAddress )
                                                                   .WithMessage( "Адрес отправления и адрес доставки не должны совпадать" );
                                                               } );
    }
  }
}