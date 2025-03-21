using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POSV1.TenantModel.Models.EntityModels.Accounting;
using POSV1.TenantModel.Models.EntityModels.Inventory;

namespace POSV1.TenantModel.Models
{
    public class vou01voucher_typesEntityConfiguration : IEntityTypeConfiguration<vou01voucher_types>
    {
        public void Configure(EntityTypeBuilder<vou01voucher_types> builder)
        {
            builder
                .HasMany(e => e.vou02voucher_summary)
                .WithOne(e => e.vou01voucher_types)
                .HasForeignKey(e => e.vou02vou01uin)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
