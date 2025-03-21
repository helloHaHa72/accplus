using Microsoft.VisualBasic;
using POSV1.TenantModel.Models;

namespace POSV1.TenantAPI.Models
{
    public partial class VMEmployeeWithTxnDetails
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public EnumGender _Gender { get; set; }
        public string Gender => _Gender.ToString("g");
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Nationality { get; set; }
        public bool IsHead {  get; set; }
        public IList<VMEmpAdvanceRecord> AdvanceRecords { get; set; }
        public IList<VMEmpProductionRecord> ProductionRecords { get; set; }
        public IList<VMEmpShiftingToDockRecord> ShiftingToDockRecords { get; set; }
        public IList<VMEmpShiftingToCounterRecord> ShiftingToCounterRecords { get; set; }
        public IList<VMEmpMonthlyPayroll> MonthlyPayrolls { get; set; }
        public IList<VMEmpSettlement> Settements { get; set; }
        public IList<VMEmpVehicleLogBook> VehicleLogBook { get; set; }
        public IList<VMLabour> Labour { get; set; }
    }

    public partial class VMEmpBaseTxnRecord
    {

        public int ID { get; set; }
        public DateTime _TxnDate { get; set; }
        public string TxnDate => _TxnDate.ToString("yyyy/MM/dd");
        public decimal Rate { get; set; }
        public decimal Qty { get; set; }
        public string? Ref_No { get; set; }

    }

    public partial class VMEmpAdvanceRecord
    {
        public int ID { get; set; }
        public DateTime _TxnDate { get; set; }
        public string TxnDate => _TxnDate.ToString("yyyy/MM/dd");
        public decimal Amount { get; set; }
        public string? Ref_No { get; set; }
    }

    public partial class VMEmpProductionRecord
    {
        public int ID { get; set; }
        public DateTime _TxnDate { get; set; }
        public string Production_Date => _TxnDate.ToString("yyyy/MM/dd");
        public decimal Rate { get; set; }
        public decimal Qty { get; set; }
        public string? Ref_No { get; set; }
    }
    public partial class VMEmpShiftingToDockRecord
    {
        public int ID { get; set; }
        public DateTime _TxnDate { get; set; }
        public string Shifting_Date => _TxnDate.ToString("yyyy/MM/dd");
        public decimal Qty { get; set; }
        public decimal Rate { get; set; }
        public string? Ref_No { get; set; }
    }
    public partial class VMEmpShiftingToCounterRecord
    {
        public int ID { get; set; }
        public DateTime _TxnDate { get; set; }
        public string Shifting_Date => _TxnDate.ToString("yyyy/MM/dd");
        public decimal Qty { get; set; }
        public decimal Rate { get; set; }
        public string? Ref_No { get; set; }
    }
    public partial class VMEmpMonthlyPayroll
    {
        public int ID { get; set; }
        public DateTime _TxnDate { get; set; }
        public string Txn_Date => _TxnDate.ToString("yyyy/MM/dd");
        public decimal Salary { get; set; }
        public decimal PaidAmount { get; set; }
        public string? Ref_No { get; set; }
    }
    public partial class VMEmpSettlement
    {
        public int ID { get; set; }
        public DateTime _TxnDate { get; set; }
        public string Settlement_Date => _TxnDate.ToString("yyyy/MM/dd");
        public string Remarks { get; set; }
        public string Verified_By { get; set; }
    }

    public partial class VMEmpVehicleLogBook
    {
        public int? VehicleId { get; set; }
        public string VehicleNo { get; set; }
        public string Destination { get; set; }
        public DateTime _TxnDate { get; set; }
        public string TxnDate => _TxnDate.ToString("yyyy/MM/dd hh/mm tt");
        public double Txn_Qty { get; set; }
        public string Remarks { get; set; }
    }
    
}
