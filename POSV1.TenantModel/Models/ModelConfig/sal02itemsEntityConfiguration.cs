using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POSV1.TenantModel.Models.EntityModels.Inventory;

namespace POSV1.TenantModel.Models.ModelConfig
{
    public class sal02itemsEntityConfiguration : IEntityTypeConfiguration<sal02items>
    {
        public void Configure(EntityTypeBuilder<sal02items> builder)
        {
            //builder.Property(t => t.sal02sub_total)
            //    .ValueGeneratedOnAddOrUpdate();
            //builder.Property(t => t.sal02net_amt)
            //    .ValueGeneratedOnAddOrUpdate();
        }
    }
}