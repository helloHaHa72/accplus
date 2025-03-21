using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel
{
    public enum EnumPaymentStatus
    {
        FullCredit = 1,
        Credit = 2,
        Paid = 3,
    }

    public enum EnumPaymentType
    {
        Paid = 1,
        Received = 2,
    }

    public enum EnumVatType
    {
        Payable = 1,
        Receivable = 2
    }

    public enum EnumEstimateStatus
    {
        Created = 1,
        Printed = 2,
        GivenToCustomer = 3
    }

    public enum EnumEmployeeType
    {
        Employee = 1,
        HeadLabour = 2,
        Labour = 3
    }

    public enum JobProcessingEnum
    {
        Created = 1,
        Processing = 2,
        Completed = 3,
        Failed = 4,
        NoSourceFile = 5
    }

    public enum EnumWageType
    {
        PerDay = 1,
        PerMonth = 2,
        PerPiece = 3,
        PerTrip = 4,
    }

    public enum EnumProductionStatus
    {
        NotStarted = 1,
        Ongoing = 2,
        Completed = 3,
        Cancelled = 4,
    }

    public enum EnumConfigSettings
    {
        ApplySalesVat,
        AutoApproveVoucher
    }

    public enum EnumMainHeading
    {
        PRIMARY_SETUP = 1,
        CHARTS_OF_ACCOUNT,
        //HR_SETUP,
        INVENTORY_SETUP,
        TRANSACTION,
        ACCOUNTING_TRANSACTION,
        INVENTORY_TRANSACTION,
        REPORT,
        //DAY_BOOK_BRICKX_FACTORY,
        DAY_BOOK_GENERAL,
        DAY_BOOK_EXPENSE,
        PAYABLE_RECEIVABLE_REPORT,
        SETTINGS,
        SUPERADMIN_SETTINGS
    }

    public enum EnumSubCategory
    {
        // Primary SetUp
        USER_ROLES = 1,
        USER_MANAGEMENT,
        MENU_INFO,

        // Charts of Account
        GENERAL_LEDGER,
        LEDGER,

        // HR SetUp
        //EMPLOYEE,
        //EMPLOYEES_SALARY,

        // Inventory Set Up
        PRODUCT_CATEGORY,
        PRODUCT_UNIT,
        PRODUCTS,
        VENDORS,
        CUSTOMER,
        CUSTOMERS_TYPE,
        VEHICLE_TYPE,
        VEHICLE,

        // HR Transaction
        //ADVANCE_RECORD,
        //PRODUCTION_RECORD,
        //SHIFTING_TO_DOCK,
        //SHIFTING_TO_COUNTER,
        //PAYROLL,
        //EMPLOYEES_SETTLEMENT,
        //FUEL,

        // Accounting Transaction
        JOURNAL_VOUCHERS,
        INCOME_VOUCHERS,
        EXPENSE_VOUCHERS,

        // Inventory Transaction
        PURCHASE,
        PURCHASE_RETURN,
        //SALES,
        GENERAL_SALES,
        SALES_RETURN,
        PRODUCTION,
        CASH_SETTLEMENT,

        // Accounting Report
        UNAPPROVED_VOUCHERS,
        APPROVED_VOUCHERS,
        LEDGER_STATEMENT,
        TRIAL_BALANCE,
        BALANCE_SHEET,
        PROFIT_LOSS_REPORT,

        // Inventory Reports
        CUSTOMER_SUMMARY,
        VENDOR_SUMMARY,
        ITEMWISE_TRANSACTION,
        STOCK_SUMMARY_REPORT,
        STOCK_STATEMENT_REPORT,
        SALES_REPORT,
        PURCHASE_REPORT,
        VAT_REPORT,
        //VEHICLE_LOG_BOOK,
        INVENTORY_LOGBOOK,

        // HR Report
        //FUEL_BOOK_REPORT,

        // Settings
        TAX,
        //WAGES,
        GENERAL_SETTINGS
    }
}
