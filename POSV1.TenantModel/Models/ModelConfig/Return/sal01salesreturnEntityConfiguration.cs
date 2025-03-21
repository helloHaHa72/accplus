using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Models.ModelConfig.Return
{
    partial class sal01salesreturnEntityConfiguration : IEntityTypeConfiguration<sal01salesreturn>
    {
        public void Configure(EntityTypeBuilder<sal01salesreturn> builder)
        {
            // Configure relationship with sal02itemsreturn
            builder
               .HasMany(e => e.sal02itemsreturn)
               .WithOne(e => e.sal01salesreturn)
               .HasForeignKey(e => e.sal02sal01uin)
               .OnDelete(DeleteBehavior.Restrict);

            // Configure relationship with cus01customers
            builder
               .HasOne(e => e.cus01customers)
               .WithMany()
               .HasForeignKey(e => e.sal01cus01uin)
               .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(e => e.sal01sales)
                .WithMany(e => e.sal01salesreturns) // Assuming sal01sales has a collection of sal01salesreturn
                .HasForeignKey(e => e.sal01_salId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
