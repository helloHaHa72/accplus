using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Models.EntityModels.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Models.ModelConfig.Production
{
    public class prod02consumerawproductConfiguration : IEntityTypeConfiguration<prod02consumerawproduct>
    {
        public void Configure(EntityTypeBuilder<prod02consumerawproduct> builder)
        {
            // Foreign Key: prod02productuin → pro02products.pro02uin
            builder.HasOne(p => p.Product)
                .WithMany(p => p.RawProducts)
                .HasForeignKey(p => p.prod02productuin)
                .OnDelete(DeleteBehavior.Restrict);

            // Foreign Key: prod02unituin → un01units.un01uin (assuming units exist)
            builder.HasOne<un01units>()
                .WithMany()
                .HasForeignKey(p => p.prod02unituin)
                .OnDelete(DeleteBehavior.Restrict);

            // Foreign Key: prod02productuin → prod01production.prod01uin
            builder.HasOne(p => p.Production)
                .WithMany(p => p.RawProducts)
                .HasForeignKey(p => p.prod02productionuin)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
