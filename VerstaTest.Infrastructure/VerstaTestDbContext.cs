using Microsoft.EntityFrameworkCore;
using VerstaTest.Domain.Entities;

namespace VerstaTest.Infrastructure;

public sealed class VerstaTestDbContext( DbContextOptions<VerstaTestDbContext> options ) : DbContext( options )
{
  public DbSet<OrderEntity> Orders { get; set; }

  protected override void OnModelCreating( ModelBuilder modelBuilder )
  {
    modelBuilder.Entity<OrderEntity>( _ =>
                                      {
                                        _.HasKey( e => e.Id );
                                        _.Property( e => e.CreatedAt ).IsRequired();
                                        _.Property( e => e.WeightKg ).IsRequired();
                                        _.Property( e => e.DeliveryDate ).IsRequired();

                                        _.ComplexProperty( e => e.FromAddress, from =>
                                                                               {
                                                                                 from.Property( a => a.City )
                                                                                   .HasColumnName( "FromCity" );
                                                                                 from.Property( a => a.Address )
                                                                                   .HasColumnName( "FromAddressLine" );
                                                                               } );

                                        _.ComplexProperty( e => e.ToAddress, to =>
                                                                             {
                                                                               to.Property( a => a.City )
                                                                                 .HasColumnName( "ToCity" );
                                                                               to.Property( a => a.Address )
                                                                                 .HasColumnName( "ToAddressLine" );
                                                                             } );
                                      } );
  }
}