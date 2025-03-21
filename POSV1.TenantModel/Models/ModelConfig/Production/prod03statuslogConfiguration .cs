using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantModel.Models.EntityModels.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Models.ModelConfig.Production
{
    public class prod03statuslogConfiguration : IEntityTypeConfiguration<prod03statuslog>
    {
        public void Configure(EntityTypeBuilder<prod03statuslog> builder)
        {
            builder.HasOne(sl => sl.Production)
                .WithMany(p => p.StatusLogs)
                .HasForeignKey(sl => sl.prod03productionuin)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
