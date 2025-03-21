using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POSV1.TenantModel.Models.EntityModels.Accounting;

namespace POSV1.TenantModel.Models.ModelConfig.Accounting_config
{
    public class led01ledgersEntityConfiguration : IEntityTypeConfiguration<led01ledgers>
    {
        public void Configure(EntityTypeBuilder<led01ledgers> builder)
        {
            builder
                .HasMany(e => e.vou03voucher_details)
                .WithOne(e => e.led01ledgers)
                .HasForeignKey(e => e.vou03led05uin)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasMany(e => e.vou02voucher_summary)
                .WithOne(e => e.led01ledgers)
                .HasForeignKey(e => e.vou02contra_led05uin)
                .OnDelete(DeleteBehavior.Restrict);

            //builder
            //    .HasOne(e => e.led02ledgers_nepali)
            //    .WithOne(x => x.led01ledgers)
            //    .HasForeignKey<led02ledgers_nepali>(e => e.led02led01uin)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
