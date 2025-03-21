using BaseAppSettings;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POSV1.TenantModel.Models.EntityModels.Accounting;
public partial class led05ledger_types : Auditable
{
    public led05ledger_types()
    {
        led01ledgers = new HashSet<led01ledgers>();
        led03general_ledgers = new HashSet<led03general_ledgers>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int led05uin { get; set; }

    public string led05title { get; set; }

    public string led05title_nep { get; set; }

    public bool led05add_dr { get; set; }

    public virtual ICollection<led01ledgers> led01ledgers { get; set; }

    public virtual ICollection<led03general_ledgers> led03general_ledgers { get; set; }
}
