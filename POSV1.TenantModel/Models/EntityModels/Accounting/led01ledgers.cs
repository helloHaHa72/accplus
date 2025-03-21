using BaseAppSettings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Models.EntityModels.Accounting;
public partial class led01ledgers : Auditable
{
    public led01ledgers()
    {
        this.vou02voucher_summary = new HashSet<vou02voucher_summary>();
        this.vou03voucher_details = new HashSet<vou03voucher_details>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int led01uin { get; set; }

    public int led01led05uin { get; set; }
    public string led01title { get; set; }
    public string led01code { get; set; }
    public string led01desc { get; set; }
    public decimal led01open_bal { get; set; }
    public decimal led01balance { get; set; }
    public decimal led01prev_bal { get; set; }
    public bool led01status { get; set; }
    public bool led01deleted { get; set; }
    public int? led01led03uin { get; set; }
    public int? led01related_id { get; set; }
    public bool? led01isdefaultled { get; set; }
    public DateTime? led01date { get; set; }
    public virtual led05ledger_types led05ledger_types { get; set; }
    public virtual led03general_ledgers led03general_ledgers { get; set; }
    public virtual ICollection<vou02voucher_summary> vou02voucher_summary { get; set; }
    public virtual ICollection<vou03voucher_details> vou03voucher_details { get; set; }

    [NotMapped]
    public bool AddDr => led05ledger_types is null ? true : led05ledger_types.led05add_dr;
    [NotMapped]
    public decimal DisplayDr => AddDr ? (led01balance > 0 ? led01balance : 0) : (led01balance < 0 ? Math.Abs(led01balance) : 0);

    [NotMapped]
    public decimal DisplayCr => !AddDr ? (led01balance < 0 ? Math.Abs(led01balance) : 0) : (led01balance > 0 ? led01balance : 0);
}
