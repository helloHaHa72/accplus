using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POSV1.TenantModel.Models.EntityModels.Inventory;

namespace POSV1.TenantModel.Models
{
    public class vou02voucher_summaryEntityConfiguration : IEntityTypeConfiguration<vou02voucher_summary>
    {
        public void Configure(EntityTypeBuilder<vou02voucher_summary> builder)
        {
            builder
              .HasMany(e => e.vou03voucher_details)
              .WithOne(e => e.vou02voucher_summary)
              .HasForeignKey(e => e.vou03vou02full_no)
              .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasMany(e => e.vou04file_attachments)
                .WithOne(e => e.vou02voucher_summary)
                .HasForeignKey(e => e.vou04vou02full_no)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
