using BaseAppSettings;
using System.ComponentModel.DataAnnotations;

namespace POSV1.TenantModel.Models;
public class Config : Auditable
{
    [Key]
    public int id { get; set; }
    public enumConfigSettingsKeys key { get; set; }
    public object value { get; set; }
}
