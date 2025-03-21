using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POSV1.TenantModel.Models.EntityModels.Inventory;

namespace POSV1.TenantModel.Models.ModelConfig
{
    public class sal01salesEntityConfiguration : IEntityTypeConfiguration<sal01sales>
    {
        public void Configure(EntityTypeBuilder<sal01sales> builder)
        {
            //builder.Property(t => t.sal01total)
            //   .ValueGeneratedOnAddOrUpdate();
            //builder.Property(t => t.sal01net_amt)
            //    .ValueGeneratedOnAddOrUpdate();
            builder
                .HasMany(e => e.sal02items)
                .WithOne(e => e.sal01sales)
                .HasForeignKey(e => e.sal02sal01uin)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasMany(e => e.tran04transaction_out_details)
                .WithOne(e => e.sal01sales)
                .HasForeignKey(e => e.tran04sal01uin)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(e => e.BranchData)
                .WithMany(e => e.Sale)
                .HasForeignKey(e => e.BranchCode)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
   

}