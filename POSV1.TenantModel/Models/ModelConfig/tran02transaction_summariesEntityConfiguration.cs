using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantModel.Models.EntityModels.Inventory;

namespace POSV1.TenantModel.Models.ModelConfig
{
    public class tran02transaction_summariesEntityConfiguration : IEntityTypeConfiguration<tran02transaction_summaries>
    {
        public void Configure(EntityTypeBuilder<tran02transaction_summaries> builder)
        {
            builder
                .HasMany(e => e.tran04transaction_out_details)
                .WithOne(e => e.tran02transaction_summaries)
                .HasForeignKey(e => e.tran04tran02uin)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
