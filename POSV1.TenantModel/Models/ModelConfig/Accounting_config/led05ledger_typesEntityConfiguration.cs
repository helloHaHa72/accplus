using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POSV1.TenantModel.Models.EntityModels.Accounting;

namespace POSV1.TenantModel.Models.ModelConfig.Accounting_config
{
    public class led05ledger_typesEntityConfiguration : IEntityTypeConfiguration<led05ledger_types>
    {
        public void Configure(EntityTypeBuilder<led05ledger_types> builder)
        {
            builder
                .HasMany(e => e.led01ledgers)
                .WithOne(x => x.led05ledger_types)
                .HasForeignKey(x => x.led01led05uin)
                .OnDelete(DeleteBehavior.Restrict);
            builder
              .HasMany(e => e.led03general_ledgers)
              .WithOne(e => e.led05ledger_types)
              .HasForeignKey(e => e.led03led05uin)
              .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
