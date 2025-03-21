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
    public class add04chargepurchaserelConfiguration : IEntityTypeConfiguration<add04chargepurchaserel>
    {
        public void Configure(EntityTypeBuilder<add04chargepurchaserel> builder)
        {

            // Configure foreign key to add02purchaseadditionalcharges
            builder.HasOne(cpr => cpr.PurchaseAdditionalCharges)
                .WithMany(pac => pac.ChargePurchaseRelations)
                .HasForeignKey(cpr => cpr.add04puraddchargeuin)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure foreign key to pur01purchases
            builder.HasOne(cpr => cpr.Purchase)
                .WithMany(p => p.ChargePurchaseRelations)
                .HasForeignKey(cpr => cpr.add04purchaseuin)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
