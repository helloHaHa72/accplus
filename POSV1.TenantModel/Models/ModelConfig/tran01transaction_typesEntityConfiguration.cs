using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantModel.Models.EntityModels.Inventory;

namespace POSV1.TenantModel.Models.ModelConfig
{
    public class tran01transaction_typesEntityConfiguration : IEntityTypeConfiguration<tran01transaction_types>
    {
        public void Configure(EntityTypeBuilder<tran01transaction_types> builder)
        {
            builder
                .HasMany(e => e.tran02transaction_summaries)
                .WithOne(e => e.tran01transaction_types)
                .HasForeignKey(e => e.tran02tran01uin)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
