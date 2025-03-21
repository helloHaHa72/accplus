using BaseAppSettings;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using POSV1.MasterDBModel.AuthModels;
using POSV1.TenantModel.Models;
using POSV1.TenantModel.Models.EntityModels.Accounting;
using POSV1.TenantModel.Models.EntityModels.CloudR2;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Models.EntityModels.Production;
using POSV1.TenantModel.Models.EntityModels.Settings;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace POSV1.TenantModel
{

    public class MainDbContext : IdentityDbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {

        }

        //public virtual DbSet<AspNetUserOTP> aspNetUserOTP { get; set; }
        public virtual DbSet<Setting> settings { get; set; }
        //public virtual DbSet<con01menuInfo> con01menuInfo { get; set; }
        //public virtual DbSet<con02action> con02actions { get; set; }
        //public virtual DbSet<RoleActions> roleActions { get; set; }
        public virtual DbSet<cfg01configurations> cfg01configurations { get; set; }
        public virtual DbSet<ConfigurationSetting> ConfigurationSettings { get; set; }
        public virtual DbSet<MainSetup> MainSetups { get; set; }
        public virtual DbSet<OrgBranch> BranchDatas { get; set; }
        public virtual DbSet<UserBranch> UserBranches { get; set; }
        public virtual DbSet<add01additionalcharges> Add01Additionalcharges { get; set; }
        public virtual DbSet<add02purchaseadditionalcharges> Add02Purchaseadditionalcharges { get; set; }
        public virtual DbSet<add03purchaseadditionalchargesdetail> Add03Purchaseadditionalchargesdetails { get; set; }
        public virtual DbSet<add04chargepurchaserel> Add04Chargepurchaserels { get; set; }
        //public virtual DbSet<lab01labour> lab01labour { get; set; }

        #region Inventory Section
        public virtual DbSet<pro01categories> pro01categories { get; set; }
        public virtual DbSet<pro02products> pro02products { get; set; }
        public virtual DbSet<pro03units> pro03units { get; set; }
        public virtual DbSet<un01units> un01units { get; set; }
        public virtual DbSet<cus01customers> cus01customers { get; set; }
        public virtual DbSet<ven01vendors> ven01vendors { get; set; }
        public virtual DbSet<sal01sales> sal01sales { get; set; }
        public virtual DbSet<sal02items> sal02items { get; set; }
        public virtual DbSet<pur01purchases> pur01purchases { get; set; }
        public virtual DbSet<pur02items> pur02items { get; set; }
        public virtual DbSet<tran04transaction_out_details> tran04transaction_out_details { get; set; }
        public virtual DbSet<tran02transaction_summaries> tran02transaction_summaries { get; set; }
        public virtual DbSet<tran01transaction_types> tran01transaction_types { get; set; }

        #endregion

        #region Accounting Section
        public virtual DbSet<led01ledgers> led01ledgers => Set<led01ledgers>();
        public virtual DbSet<led03general_ledgers> led03general_ledgers => Set<led03general_ledgers>();
        public virtual DbSet<led05ledger_types> led05ledger_types => Set<led05ledger_types>();
        public virtual DbSet<vou01voucher_types> vou01voucher_types => Set<vou01voucher_types>();
        public virtual DbSet<vou02voucher_summary> vou02voucher_summary => Set<vou02voucher_summary>();
        public virtual DbSet<vou03voucher_details> vou03voucher_details => Set<vou03voucher_details>();
        public virtual DbSet<vou04file_attachments> vou04file_attachments => Set<vou04file_attachments>();
        public virtual DbSet<vou05voucher_log> vou05voucher_log => Set<vou05voucher_log>();
        public virtual DbSet<ta01taxsettlement> ta01taxsettlement => Set<ta01taxsettlement>();
        #endregion

        public virtual DbSet<prod01production> Prod01Productions => Set<prod01production>();
        public virtual DbSet<prod02consumerawproduct> Prod02Consumerawproducts => Set<prod02consumerawproduct>();
        public DbSet<cas01cashsettlement> cas01cashsettlement { get; set; }
        public DbSet<AccessList> AccessLists { get; set; }
        public DbSet<UserPermissionList> UserPermissionLists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Assembly assemblyWithConfigurations = GetType().Assembly; //get whatever assembly you want
            modelBuilder.ApplyConfigurationsFromAssembly(assemblyWithConfigurations);

            modelBuilder.Entity<Setting>()
                .HasKey(e => new { e.id, e.modules });

            modelBuilder.Entity<UserBranch>()
                .HasOne(ub => ub.Branch)
                .WithMany(b => b.UserBranches)
                .HasForeignKey(ub => ub.BranchCode)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<add02purchaseadditionalcharges>()
                .HasMany(p => p.AdditionalChargesDetails)
                .WithOne(d => d.PurchaseAdditionalCharges)
                .HasForeignKey(d => d.add02uin)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CloudR2Purchase>(entity =>
            {
                entity.HasOne(mc => mc.Purchase)
                    .WithMany(cr2 => cr2.CloudR2Purchase)
                    .HasForeignKey(cr2 => cr2.PurchaseId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<led05ledger_types>().HasData(
                new led05ledger_types
                {
                    led05uin = 1,
                    led05title = "Assets",
                    led05title_nep = " सम्पत्ति",
                    led05add_dr = true
                },
                new led05ledger_types
                {
                    led05uin = 2,
                    led05title = "Liabilities",
                    led05title_nep = " देयता",
                    led05add_dr = false,
                },
                new led05ledger_types
                {
                    led05uin = 3,
                    led05title = "Income",
                    led05title_nep = " आम्दानी",
                    led05add_dr = false
                },
                new led05ledger_types
                {
                    led05uin = 4,
                    led05title = "Expenses",
                    led05title_nep = "व्यय ",
                    led05add_dr = true
                }
            );

            modelBuilder.Entity<led01ledgers>().HasData(
                new led01ledgers
                {
                    led01uin = -1,
                    led01code = "SalesDiscount", // You can replace this with the appropriate code if needed
                    //led01led03uin = _gledgersRepo.GetList().FirstOrDefault(x => x.led03title == "Discount")?.led03uin,
                    led01related_id = 0, // Adjust this based on your requirements for the related ID
                    led01led05uin = (int)EnumLedgerTypes.Expenses,
                    led01title = "Discount Given",
                    led01desc = "Discount detail",
                    led01status = true,
                    led01deleted = false,
                    CreatedName = "SYSTEM",
                    //DateCreated = DateTime.Now,

                    led01balance = 0,
                    led01open_bal = 0, // Default opening balance can be set as required
                }
            );

            modelBuilder.Entity<led01ledgers>().HasData(
                new led01ledgers
                {
                    led01uin = -2,
                    led01code = "PurchaseDiscount", // You can replace this with the appropriate code if needed
                    //led01led03uin = _gledgersRepo.GetList().FirstOrDefault(x => x.led03title == "Discount")?.led03uin,
                    led01related_id = 0, // Adjust this based on your requirements for the related ID
                    led01led05uin = (int)EnumLedgerTypes.Expenses,
                    led01title = "Discount Received",
                    led01desc = "Discount detail",
                    led01status = true,
                    led01deleted = false,
                    CreatedName = "SYSTEM",
                    //DateCreated = DateTime.Now,

                    led01balance = 0,
                    led01open_bal = 0, // Default opening balance can be set as required
                }
            );


            modelBuilder.Entity<OrgBranch>().HasData(
                new OrgBranch
                {
                    //Id = 1,
                    BranchName = "Head Office",
                    BranchCode = "HO-001",
                    IsDefault = true,
                    CreatedName = "SYSTEM",
                    DateCreated = DateTime.UtcNow
                }
            );

            modelBuilder.Entity<MainSetup>().HasData(
                new MainSetup
                {
                    Id = -1,
                    OrgName = "Default Org",
                    Server = ".",
                    DbName = "POS_V1",
                    DbPassword = "asdf",
                    CreatedName = "SYSTEM",
                    //DateCreated = DateTime.UtcNow
                }
            );
        }

        //public void SeedDatas()
        //{
        //    var transactionTypes = new List<tran03transaction_types>
        //    {
        //        new tran03transaction_types { tran03name = "Advance Record", tran03description = "Record of advance taken by the employees" },
        //        new tran03transaction_types { tran03name = "Production", tran03description = "Record of the item production " },
        //        new tran03transaction_types { tran03name = "Shifting To Dock", tran03description = "Record of Shifting from production to dock" },
        //        new tran03transaction_types { tran03name = "Shifting To Counter", tran03description = "Recrod of Shifting from dock to sales store" },
        //        new tran03transaction_types {tran03name ="Monthly Payroll", tran03description ="payroll of all the employee"},
        //        new tran03transaction_types { tran03name = "Settlement", tran03description = "Description" },
        //    };

        //    tran03transaction_types.AddRange(transactionTypes);
        //    SaveChanges();
        //}

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var insertedEntries = this.ChangeTracker.Entries()
                                   .Where(x => x.State == EntityState.Added)
                                   .Select(x => x.Entity);

            foreach (var insertedEntry in insertedEntries)
            {
                var auditableEntity = insertedEntry as Auditable;
                //If the inserted object is an Auditable. 
                if (auditableEntity != null)
                {
                    auditableEntity.DateCreated = DateTime.UtcNow;
                }
            }

            var modifiedEntries = this.ChangeTracker.Entries()
                       .Where(x => x.State == EntityState.Modified)
                       .Select(x => x.Entity);

            foreach (var modifiedEntry in modifiedEntries)
            {
                //If the inserted object is an Auditable. 
                var auditableEntity = modifiedEntry as Auditable;
                if (auditableEntity != null)
                {
                    auditableEntity.DateUpdated = DateTimeOffset.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
        //public async Task<T> Delete(T entity)
        //{
        //    //If the type we are trying to delete is auditable, then we don't actually delete it but instead set it to be updated with a delete date. 
        //    if (typeof(Auditable).IsAssignableFrom(typeof(T)))
        //    {
        //        (entity as Auditable).DateDeleted = DateTimeOffset.UtcNow;
        //        _dbSet.Attach(entity);
        //        _context.Entry(entity).State = EntityState.Modified;
        //    }
        //    else
        //    {
        //        _dbSet.Remove(entity);
        //    }

        //    return entity;
        //}
    }
}
