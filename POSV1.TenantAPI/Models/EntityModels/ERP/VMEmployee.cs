using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using POSV1.TenantModel;
using POSV1.TenantModel.Models;

namespace POSV1.TenantAPI.Models
{
    public partial class VMEmployee
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public DateTime DOB { get; set; }

        public EnumGender Gender { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string Nationality { get; set; }
        //public string Ledger_code { get; set; }

        public bool IsHead {  get; set; }

        public EnumEmployeeType EmployeeType { get; set; }

        //public IList<VMLabour> Labours { get; set; }
        public IList<CreateLabourEmployee> Labours { get; set; }
    }


    public partial class VMEmployeeDetail
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public DateTime DOB { get; set; }

        public string Gender { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string Nationality { get; set; }
        //public string Ledger_code { get; set; }

        public bool IsHead { get; set; }

        public string EmployeeType { get; set; }

        //public IList<VMLabour> Labours { get; set; }
        public IList<VMLabour> Labours { get; set; }
    }


    public partial class CreateLabourEmployee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? DOB { get; set; }
        public EnumGender Gender{ get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }

    public partial class VMLabour
    {
        public int LabourId {  get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
    }

    public partial class VMEmployeeList
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public DateTime DOB { get; set; }

        public String Gender { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string Nationality { get; set; }

        public bool IsHead { get; set; }

        public string EmployeeType { get; set; }
    }

   

 
}
