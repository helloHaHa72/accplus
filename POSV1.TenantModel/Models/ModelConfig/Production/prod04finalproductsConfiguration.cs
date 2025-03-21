using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantModel.Models.EntityModels.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Models.ModelConfig.Production
{
    public class prod04finalproductsConfiguration : IEntityTypeConfiguration<prod04finalproducts>
    {
        public void Configure(EntityTypeBuilder<prod04finalproducts> builder)
        {
            // Configure relationship with prod01production
            builder.HasOne(fp => fp.Production)
                .WithMany(p => p.FinalProducts)
                .HasForeignKey(fp => fp.prod4productionuin)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure relationship with pro02products
            builder.HasOne(fp => fp.Product)
                .WithMany(p => p.FinalProducts)
                .HasForeignKey(fp => fp.prod04productuin)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure relationship with pro03units
            builder.HasOne(fp => fp.Unit)
                .WithMany(p => p.FinalProducts)
                .HasForeignKey(fp => fp.prod04unituin)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
