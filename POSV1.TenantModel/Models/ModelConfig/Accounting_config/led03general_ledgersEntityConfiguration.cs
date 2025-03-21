using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POSV1.TenantModel.Models.EntityModels.Accounting;

namespace POSV1.TenantModel.Models.ModelConfig.Accounting_config
{
    public class led03general_ledgersEntityConfiguration: IEntityTypeConfiguration<led03general_ledgers>
    {
        public void Configure(EntityTypeBuilder<led03general_ledgers> builder)
        {
            builder
                .HasMany(e => e.led01ledgers)
                .WithOne(x => x.led03general_ledgers)
                .HasForeignKey(x => x.led01led03uin)
                .OnDelete(DeleteBehavior.Restrict);
            builder
              .HasMany(e => e.led03child)
              .WithOne(e => e.led03parent)
              .HasForeignKey(e => e.led03led03uin)
              .OnDelete(DeleteBehavior.Restrict);
           
        }
    }
}
