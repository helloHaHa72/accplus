using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POSV1.TenantModel.Models.EntityModels.Inventory;

namespace POSV1.TenantModel.Models.ModelConfig
{
    public class pro02productsEntityConfiguration : IEntityTypeConfiguration<pro02products>
    {

        public void Configure(EntityTypeBuilder<pro02products> builder)
        {
            builder.HasMany(y => y.pro03units)
                .WithOne(x => x.pro02products)
                .HasForeignKey(x => x.pro03pro02uin)
                .OnDelete(DeleteBehavior.Restrict);
             
            builder
                .HasMany(e => e.sal02items)
                .WithOne(e => e.pro02products)
                .HasForeignKey(e => e.sal02pro02uin)
                .OnDelete(DeleteBehavior.Restrict);

            builder
              .HasMany(e => e.pur02items)
              .WithOne(e => e.pro02products)
              .HasForeignKey(e => e.pur02pro02uin)
              .OnDelete(DeleteBehavior.Restrict);

            builder
               .HasMany(e => e.tran04transaction_out_details)
               .WithOne(e => e.pro02products)
               .HasForeignKey(e => e.tran04pro02uin)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}