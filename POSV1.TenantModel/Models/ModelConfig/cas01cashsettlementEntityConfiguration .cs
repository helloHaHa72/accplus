using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Models.ModelConfig
{
    public class cas01cashsettlementEntityConfiguration : IEntityTypeConfiguration<cas01cashsettlement>
    {
        public void Configure(EntityTypeBuilder<cas01cashsettlement> builder)
        {

            // Primary Key
            builder.HasKey(c => c.cas01uin);

            // Foreign Key - Purchase
            builder
                .HasOne(c => c.pur01purchases)
                .WithMany(p => p.CashSettlements)
                .HasForeignKey(c => c.cas01purchaseuin)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            // Foreign Key - Sale
            builder
                .HasOne(c => c.sal01sales)
                .WithMany(s => s.CashSettlements)
                .HasForeignKey(c => c.cas01saleuin)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            builder
                .HasOne(c => c.cus01customers)
                .WithMany(p => p.CashSettlements)
                .HasForeignKey(c => c.cas01customeruin)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            builder
                .HasOne(c => c.ven01vendors)
                .WithMany(s => s.CashSettlements)
                .HasForeignKey(c => c.cas01vendoruin)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        }
    }
}
