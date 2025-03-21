using BaseAppSettings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace POSV1.TenantModel.Models.EntityModels.Accounting;
public partial class led03general_ledgers : Auditable
{
    public led03general_ledgers()
    {
        this.led01ledgers = new HashSet<led01ledgers>();
        this.led03child = new HashSet<led03general_ledgers>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int led03uin { get; set; }

    public int led03led05uin { get; set; }

    public string led03title { get; set; }

    public string led03code { get; set; }

    public string led03desc { get; set; }

    public string led03balance { get; set; }

    public bool led03status { get; set; }

    public bool led03deleted { get; set; }

    public int? led03led03uin { get; set; }

    public virtual led05ledger_types led05ledger_types { get; set; }

    public virtual led03general_ledgers led03parent { get; set; }

    public virtual ICollection<led01ledgers> led01ledgers { get; set; }

    public virtual ICollection<led03general_ledgers> led03child { get; set; }

    public static void SeedGeneralLedgers(MainDbContext context)
    {
        var defaultLedgers = new List<(string Title, string Type, string DefaultLedger)>
            {
                ("Cash", "Assets", "DefaultLedgerForCash"),
                ("Bank", "Assets", "DefaultLedgerForBank"),
                ("AccountsReceivable", "Assets", "DefaultLedgerForCustomers"),
                ("AccountsPayable", "Liabilities", "DefaultLedgerForVendors"),
                ("Income", "Income", "DefaultLedgerForNewProducts"),
                ("Expense", "Expense", "DefaultLedgerForSales"),
                ("D_Income", "Income", "DefaultLedgerForDiscountTaken"),
                ("SalariesExpense", "Expenses", "DefaultLedgerForEmployees"),
                ("D_Expense", "Expenses", "DefaultLedgerForDiscountGiven")
            };

        var existingTitles = new HashSet<string>(context.led03general_ledgers.Select(l => l.led03title));
        var ledgerTypes = context.led05ledger_types.ToDictionary(l => l.led05title, l => l.led05uin);
        var random = new Random();

        foreach (var ledger in defaultLedgers)
        {
            if (!existingTitles.Contains(ledger.Title))
            {
                var randomCode = ledger.Title.Substring(0, 3).ToUpper() + new string(Enumerable.Range(0, 3).Select(_ => (char)('A' + random.Next(26))).ToArray());
                var newLedger = new led03general_ledgers
                {
                    led03title = ledger.Title,
                    led03code = randomCode,
                    led03desc = $"Auto-generated ledger for {ledger.DefaultLedger}",
                    led03balance = "0",
                    led03status = true,
                    led03deleted = false,
                    led03led05uin = ledgerTypes.ContainsKey(ledger.Type) ? ledgerTypes[ledger.Type] : ledgerTypes.Values.FirstOrDefault()
                };

                context.led03general_ledgers.Add(newLedger);
            }
        }
        context.SaveChanges();
    }
}
