using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POSV1.TenantModel.Models.EntityModels.Inventory;

namespace POSV1.TenantModel.Models.ModelConfig
{
    public class sup01suppliersEntityConfiguration : IEntityTypeConfiguration<sup01suppliers>
    {
        public void Configure(EntityTypeBuilder<sup01suppliers> builder)
        {
            builder
                .HasMany(e => e.tran02transaction_summaries)
                .WithOne(e => e.sup01suppliers)
                .HasForeignKey(e => e.tran02sup01uin)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}