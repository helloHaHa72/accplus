using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POSV1.TenantModel.Models.EntityModels.Inventory;

namespace POSV1.TenantModel.Models.ModelConfig
{
    public class un01unitsEntityConfiguration : IEntityTypeConfiguration<un01units>
    {

        public void Configure(EntityTypeBuilder<un01units> builder)
        {
            builder.HasMany(y => y.pro03units)
                .WithOne(x => x.un01units)
                .HasForeignKey(x => x.pro03un01uin)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(y => y.pro02products)
                .WithOne(x => x.un01units)
                .HasForeignKey(x => x.pro02un01uin)
                .OnDelete(DeleteBehavior.Restrict);

            builder
               .HasMany(e => e.sal02items)
               .WithOne(e => e.un01units)
               .HasForeignKey(e => e.sal02un01uin)
               .OnDelete(DeleteBehavior.Restrict);

            builder
               .HasMany(e => e.pur02items)
               .WithOne(e => e.un01units)
               .HasForeignKey(e => e.pur02un01uin)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}