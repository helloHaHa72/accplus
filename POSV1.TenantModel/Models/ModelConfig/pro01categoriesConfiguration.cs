using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POSV1.TenantModel.Models.EntityModels.Inventory;

namespace POSV1.TenantModel.Models.ModelConfig
{
    public class pro01categoriesConfiguration : IEntityTypeConfiguration<pro01categories>
    {
        public void Configure(EntityTypeBuilder<pro01categories> builder)
        {
            builder.HasMany(y => y.pro02products)
                .WithOne(x => x.pro01categories)
                .HasForeignKey(x => x.pro02pro01uin)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}