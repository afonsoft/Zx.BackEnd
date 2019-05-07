using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zx.BackEnd.Data.Entity;

namespace Zx.BackEnd.Data.Context
{
    internal class PontoDeVendaConfiguration : IEntityTypeConfiguration<PontoDeVenda>
    {
        public void Configure(EntityTypeBuilder<PontoDeVenda> builder)
        {
            builder
                .Property(q => q.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder
                .Property(q => q.TradingName)
                .HasMaxLength(100)
                .IsRequired();

            builder
                .Property(q => q.OwnerName)
                .HasMaxLength(100)
                .IsRequired();

            builder
                .Property(q => q.Document)
                .HasMaxLength(20)
                .IsRequired();

            builder.HasOne(x => x.Address);
            builder.HasOne(x => x.CoverageArea);

            builder
                .OwnsOne(q => q.Address, end =>
                {


                    end.Property(q => q.Type)
                        .HasColumnName("AddressType")
                        .HasColumnType("int")
                        .IsRequired();

                    end.Property(q => q.Coordinates)
                        .HasColumnName("AddressCoordinates")
                        .HasMaxLength(8000)
                        .IsRequired();
                });

            builder
                .OwnsOne(q => q.CoverageArea, area =>
                {

                    area.Property(q => q.Coordinates)
                        .HasColumnName("CoverageArea")
                        .HasMaxLength(8000)
                        .IsRequired();

                    area.Property(q => q.Type)
                        .HasColumnName("CoverageAreaType")
                        .HasColumnType("int")
                        .IsRequired();
                });
        }
    }
}
