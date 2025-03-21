namespace POSV1.TenantAPI.Models
{
    public enum EnumHRSettingKey
    {
        Production_Rate = 1,
        ShiftingToDock_Rate = 2,
        ShiftingToCounter_Rate = 3,

    }

    //public enum EnumApplicationUserType
    //{
    //    SuperAdmin = 1,
    //    GeneralAdmin = 2,
    //    GeneralUser = 3,
    //    SystemAdmin = 4,
    //    SystemOperator,
    //    SystemReporter,
    //    AccountingAdmin,
    //    AccountingOperator,
    //    AccountingReporter,
    //    InventoryAdmin,
    //    InventoryOperator,
    //    InventoryReporter,
    //    HrAdmin,
    //    HrOperator,
    //    HrReporter,
    //    SalesAdmin,
    //    PurchaseAdmin
    //}

    public enum EnumFileProcessingType
    {
        Purchase,
        Sales,
        PurchaseReturn,
        SalesReturn,
        CashSettlement,
    }

    public enum TransactionType
    {
        Purchase = 1,
        Sale = 2,
        PurchaseReturn = 3,
        SaleReturn = 4
    }

    //public enum EnumPaymentStatus
    //{
    //    FullCredit,
    //    Credit,
    //    Paid,
    //}
}
