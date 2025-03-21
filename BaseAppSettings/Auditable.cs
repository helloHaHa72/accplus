using System.ComponentModel.DataAnnotations;

namespace BaseAppSettings;

public enum EnumApplicationUserType
{
    SuperAdmin = 1,
    GeneralAdmin = 2,
    GeneralUser = 3,
    SystemAdmin = 4,
    SystemOperator,
    SystemReporter,
    AccountingAdmin,
    AccountingOperator,
    AccountingReporter,
    InventoryAdmin,
    InventoryOperator,
    InventoryReporter,
    HrAdmin,
    HrOperator,
    HrReporter,
    SalesAdmin,
    PurchaseAdmin
}
public enum EnumModules
{
    MasterSetup = 0,
    Accounting = 1,
    Inventory = 2,
    HR = 3
}
public class ModuleInfo
{
    public EnumModules ID { get; set; }
    public string ModuleName { get; set; }
    public bool IsActive { get; set; }
}
public interface IModules<RelatedDBContext>
{
    void SeedData(RelatedDBContext dbContext); //additional parameters if required
    ModuleInfo GetInfo();
}

public abstract class Auditable
{
    [MaxLength(255)]
    public string CreatedName { get; set; } = "";
    [MaxLength(255)]
    public string UpdatedName { get; set; } = "";
    [MaxLength(255)]
    public string DeletedName { get; set; } = "";
    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset? DateUpdated { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }
}
