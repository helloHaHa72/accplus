using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POSV1.TenantModel.Models.EntityModels.Inventory;

namespace POSV1.TenantModel.Models.ModelConfig
{
    public class cus01customersEntityConfiguration : IEntityTypeConfiguration<cus01customers>
    {
        public void Configure(EntityTypeBuilder<cus01customers> builder)
        {
            builder
                .HasMany(e => e.sal01sales)
                .WithOne(x => x.cus01customers)
                .HasForeignKey(x => x.sal01cus01uin)
                .OnDelete(DeleteBehavior.Restrict);
            builder
              .HasMany(e => e.tran02transaction_summaries)
              .WithOne(e => e.cus01customers)
              .HasForeignKey(e => e.tran02cus01uin)
              .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(e => e.CustomerType)
                .WithMany()
                .HasForeignKey(e => e.cus01customerTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(e => e.BranchData)
                .WithMany(e => e.Customer)
                .HasForeignKey(e => e.BranchCode)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}