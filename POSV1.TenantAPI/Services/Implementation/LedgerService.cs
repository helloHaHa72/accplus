using POSV1.TenantAPI.Models;
using POSV1.TenantModel.Models;
using POSV1.TenantModel.Models.EntityModels.Accounting;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Repo.Interface.Accounting;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantModel.Repo.Interface;
using POSV1.TenantAPI.Models.EntityModels.ERP;
using POSV1.TenantAPI.Models.EntityModels.Production;
using System.Text.RegularExpressions;

namespace POSV1.TenantAPI.Services
{
    public class LedgerService : IledgerService
    {
        private readonly IConfiguration _configuration;
        private readonly IGLedgersRepo _gledgersRepo;
        private readonly ILedgersRepo _ledgersRepo;
        private readonly ICustomersRepo _customersRepo;
        private readonly IVendorRepo _vendorRepo;

        public LedgerService(
            IConfiguration configuration,
            IGLedgersRepo gLedgersRepo,
            ILedgersRepo ledgersRepo,
             ICustomersRepo customersRepo,
            IVendorRepo vendorRepo)
        {
            _configuration = configuration;
            _gledgersRepo = gLedgersRepo;
            _ledgersRepo = ledgersRepo;
            _customersRepo = customersRepo;
            _vendorRepo = vendorRepo;
        }

        public void OnProductCreated(pro02products createdProduct)
        {
            var gLedger = _configuration["GeneralLedgerConfigurations:DefaultLedgerForNewProducts"];
            var GLDetail = _gledgersRepo.GetList().FirstOrDefault(x => x.led03title == gLedger);
            if (GLDetail == null)
            {
                throw new Exception("General ledger not found while generating the ledger of that product");
            }

            var ledger_code = "p_" + createdProduct.pro02code;

            var ledgerEntity = new led01ledgers
            {
                led01code = ledger_code,
                led01led03uin = GLDetail.led03uin,
                led01related_id = createdProduct.pro02uin,
                led01led05uin = (int)EnumLedgerTypes.Income,
                led01title = "Cash",
                led01desc = "IS Income",
                led01status = true,
                led01deleted = false,
                CreatedName = "Admin",
                DateCreated = DateTime.Now,
                UpdatedName = "Admin",
                DateUpdated = DateTime.Now,
                DeletedName = "",
                led01open_bal = (createdProduct.pro02last_cp * (decimal)createdProduct.pro02opening_qty)
            };

            _ledgersRepo.Insert(ledgerEntity);
            _ledgersRepo.Save();
        }

        public void OnCustomerCreated(cus01customers createdCustomer)
        {
            var gLedger = _configuration["GeneralLedgerConfigurations:DefaultLedgerForCustomers"];
            var GLDetail = _gledgersRepo.GetList().FirstOrDefault(x => x.led03title == gLedger);
            if (GLDetail == null)
            {
                throw new Exception("general ledger not found");
            }
            var ledger_code = "c_" + createdCustomer.cus01led_code;

            var ledgerEntity = new led01ledgers
            {
                led01code = ledger_code,
                led01led03uin = GLDetail.led03uin,
                led01related_id = createdCustomer.cus01uin,
                led01led05uin = (int)EnumLedgerTypes.Income,
                //led01title = "Receivable",
                led01title = createdCustomer.cus01name_eng,
                led01desc = "Amount to be received",

                led01status = true,
                led01deleted = false,
                CreatedName = "Admin",
                DateCreated = DateTime.Now,
                UpdatedName = "Admin",
                DateUpdated = DateTime.Now,
                DeletedName = "",

                led01balance = 0,
                led01open_bal = createdCustomer.cus01opening_bal
            };

            _ledgersRepo.Insert(ledgerEntity);
            _ledgersRepo.Save();

        }

        public void OnVendorCreated(ven01vendors createdVendor)
        {
            var gLedger = _configuration["GeneralLedgerConfigurations:DefaultLedgerForVendors"];
            var GLDetail = _gledgersRepo.GetList().FirstOrDefault(x => x.led03title == gLedger);
            if (GLDetail is null)
            {
                throw new Exception("Invalid GL Pointed for vendor group ledgers.");
            }
            var ledger_code = "v_" + createdVendor.ven01led_code;

            var ledgerEntity = new led01ledgers
            {
                led01code = ledger_code,
                led01led03uin = GLDetail.led03uin,
                led01related_id = createdVendor.ven01uin,
                led01led05uin = (int)EnumLedgerTypes.Expenses,
                led01title = createdVendor.ven01name_eng,
                //led01title = "Payable",
                led01desc = "Amount to be paid",
                led01status = true,
                led01deleted = false,
                CreatedName = "Admin",
                DateCreated = DateTime.Now,
                UpdatedName = "Admin",
                DateUpdated = DateTime.Now,
                DeletedName = "",

                led01balance = 0,
                led01open_bal = (decimal)createdVendor.ven01opening_bal,
            };

            _ledgersRepo.Insert(ledgerEntity);
            _ledgersRepo.Save();
        }

        //public void OnEmployeeCreated(emp01employees createdEmployee)
        //{
        //    var gLedger = _configuration["GeneralLedgerConfigurations:DefaultLedgerForEmployees"];
        //    var GLDetail = _gledgersRepo.GetList().FirstOrDefault(x => x.led03title == gLedger);
        //    if (GLDetail == null)
        //    {
        //        throw new Exception("general ledger not found");
        //    }
        //    var ledger_code = "e_" + createdEmployee.emp01led_code;

        //    var ledgerEntity = new led01ledgers
        //    {
        //        led01code = ledger_code,
        //        led01led03uin = GLDetail.led03uin,
        //        led01related_id = createdEmployee.emp01uin,
        //        led01led05uin = (int)EnumLedgerTypes.Expenses,
        //        //led01title = "Payable",
        //        led01title = createdEmployee.emp01name,
        //        led01desc = "Amount to be paid",
        //        led01status = true,
        //        led01deleted = false,
        //        CreatedName = "Admin",
        //        DateCreated = DateTime.Now,
        //        UpdatedName = "Admin",
        //        DateUpdated = DateTime.Now,
        //        DeletedName = "",

        //        led01balance = 0,
        //        led01open_bal = 0,
        //    };

        //    _ledgersRepo.Insert(ledgerEntity);
        //    _ledgersRepo.Save();
        //}

        public async Task<List<LedgerDto>> FetchDefaultLedgerDate(string data)
        {
            try
            {
                string gLedger;
                if (data.ToLower() == "bank")
                {
                    gLedger = _configuration["GeneralLedgerConfigurations:DefaultLedgerForBank"];
                }
                else if (data.ToLower() == "cash")
                {
                    gLedger = _configuration["GeneralLedgerConfigurations:DefaultLedgerForCash"];
                }
                else
                {
                    return null;
                }

                //var GLdata = await _gledgersRepo.GetList

                // Fetch the GL details and include the related led01ledgers
                var GLDetails = await _gledgersRepo.GetList()
                    .Where(x => x.led03title.ToLower() == data)
                    .Include(x => x.led01ledgers)
                    .ToListAsync();

                // Map the led01ledgers to the LedgerDto
                var ledgerDtos = GLDetails
                    .SelectMany(x => x.led01ledgers)
                    .Select(ledger => new LedgerDto
                    {
                        LedgerName = ledger.led01title,
                        Code = ledger.led01code,
                        OpenBalance = ledger.led01open_bal,
                        Balance = ledger.led01balance,
                        PrevBalance = ledger.led01prev_bal
                    })
                    .ToList();

                return ledgerDtos;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}");
            }
        }



        public async Task<bool> CreateLedgerOnSaleDisc(sal01sales data)
        {
            try
            {
                var gLedger = _configuration["GeneralLedgerConfigurations:DefaultLedgerForDiscountGiven"];
                var GLDetail = _gledgersRepo.GetList().FirstOrDefault(x => x.led03title == gLedger);
                if (GLDetail == null)
                {
                    throw new Exception("general ledger not found while creating ledger for sales disc");
                }
                var code = await _ledgersRepo.GetList().Select(x => x.led01uin).OrderByDescending(x => x).FirstOrDefaultAsync();

                var ledgerEntity = new led01ledgers
                {
                    led01code = "d_" + code,
                    led01led03uin = GLDetail.led03uin,
                    led01related_id = data.sal01uin,
                    led01led05uin = (int)EnumLedgerTypes.Expenses,
                    led01title = "Discount_Given",
                    led01desc = "Discount given to customer",
                    led01status = true,
                    led01deleted = false,
                    CreatedName = "Admin",
                    DateCreated = DateTime.Now,
                    UpdatedName = "",

                    led01balance = data.sal01disc_amt,
                    led01open_bal = 0,
                };

                _ledgersRepo.Insert(ledgerEntity);
                _ledgersRepo.Save();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> CreateLedgerOnPurchaseDisc(pur01purchases data)
        {
            try
            {
                var gLedger = _configuration["GeneralLedgerConfigurations:DefaultLedgerForDiscountTaken"];
                var GLDetail = _gledgersRepo.GetList().FirstOrDefault(x => x.led03title == gLedger);
                if (GLDetail == null)
                {
                    throw new Exception("general ledger not found while creating ledger for purchase disc");
                }

                var code = await _ledgersRepo.GetList().Select(x => x.led01uin).OrderByDescending(x => x).FirstOrDefaultAsync();

                var ledgerEntity = new led01ledgers
                {
                    led01code = "d_" + code,
                    led01led03uin = GLDetail.led03uin,
                    led01related_id = data.pur01uin,
                    led01led05uin = (int)EnumLedgerTypes.Income,
                    led01title = "Discount_Taken",
                    led01desc = "Discount taken from vendor",
                    led01status = true,
                    led01deleted = false,
                    CreatedName = "Admin",
                    led01date = data.pur01date,
                    DateCreated = DateTime.Now,
                    UpdatedName = "",

                    led01balance = data.pur01disc_amt,
                    led01open_bal = 0,
                };

                _ledgersRepo.Insert(ledgerEntity);
                _ledgersRepo.Save();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid data");
            }
        }


        public async Task<bool> UpdateLedgerOnSale(sal01sales salesData)
        {
            try
            {
                var customerDetail = _customersRepo.GetDetail(salesData.sal01cus01uin);
                if (customerDetail == null)
                {
                    throw new Exception("customer not found");
                }

                var ledgerId = "c_" + customerDetail.cus01led_code;
                var ledgerDetail = await _ledgersRepo.GetList().Where(x => x.led01code == ledgerId).FirstOrDefaultAsync();
                if (ledgerDetail == null)
                {
                    throw new Exception("ledger not found");
                }

                foreach (var item in salesData.sal02items)
                {
                    ledgerDetail.led01balance += salesData.sal01net_amt;
                    ledgerDetail.DateUpdated = DateTime.Now;

                    _ledgersRepo.Update(ledgerDetail);
                    await _ledgersRepo.SaveAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateLedgerOnPurhcase(pur01purchases purchaseData)
        {
            try
            {
                var vendorDetail = _vendorRepo.GetDetail(purchaseData.pur01ven01uin);
                if (vendorDetail == null)
                {
                    throw new Exception("Vendor not found");
                }

                var ledgerId = "v_" + vendorDetail.ven01led_code;
                var ledgerDetail = await _ledgersRepo.GetList().Where(x => x.led01code == ledgerId).FirstOrDefaultAsync();
                if (ledgerDetail == null)
                {
                    throw new Exception("ledger not found");
                }

                foreach (var item in purchaseData.pur02items)
                {
                    ledgerDetail.led01balance += purchaseData.pur01net_amt;
                    ledgerDetail.DateUpdated = DateTime.Now;

                    _ledgersRepo.Update(ledgerDetail);
                    await _ledgersRepo.SaveAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<bool> UpdateLedgerOnAdditionalCharge(List<ChargeData> data)
        {
            try
            {
                foreach (var charge in data)
                {
                    // Extract ledger code inside square brackets
                    var match = Regex.Match(charge.Title, @"\[(.*?)\]");
                    if (!match.Success)
                    {
                        throw new Exception("Invalid ledger title format!");
                    }

                    string ledgerCode = match.Groups[1].Value;

                    var ledgerDetail = await _ledgersRepo.GetList()
                        .Where(x => x.led01code == ledgerCode)
                        .FirstOrDefaultAsync();

                    if (ledgerDetail == null)
                    {
                        throw new Exception($"Invalid ledger: {ledgerCode}");
                    }

                    ledgerDetail.led01balance += charge.Amount;
                    ledgerDetail.DateUpdated = DateTime.UtcNow;

                    _ledgersRepo.Update(ledgerDetail);
                }

                await _ledgersRepo.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating ledger: {ex.Message}");
            }
        }


        public async Task<IEnumerable<VMUserStatement>> GetStatement(string ledgerCode)
        {
            try
            {
                var ledgerWithVouchers = await _ledgersRepo.GetList()
                .Include(x => x.led05ledger_types)
                .Include(x => x.vou03voucher_details)
                .Where(x => x.led01code.Contains(ledgerCode))
                .OrderByDescending(x => x.DateCreated)
                .AsNoTracking()
                .FirstOrDefaultAsync();

                if (ledgerWithVouchers == null)
                {
                    return Enumerable.Empty<VMUserStatement>();
                }

                var statements = ledgerWithVouchers.vou03voucher_details.Select(voucher => new VMUserStatement
                {
                    VoucherNo = voucher.vou03vou02full_no,
                    Description = voucher.vou03description,
                    Debit = voucher.vou03dr,
                    Credit = voucher.vou03cr,
                    Balance = voucher.vou03balance,
                    ChequeNo = voucher.vou03chq,
                    Date = voucher.DateCreated
                });

                return statements;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> UpdateLedgerOnPurchaseReturn(pur01purchasereturns returnData)
        {
            try
            {
                // Fetch vendor details
                var vendorDetail = _vendorRepo.GetDetail(returnData.pur01ven01uin);
                if (vendorDetail == null)
                {
                    throw new Exception("Vendor not found");
                }

                // Generate ledger ID and fetch ledger details
                var ledgerId = "v_" + vendorDetail.ven01led_code;
                var ledgerDetail = await _ledgersRepo.GetList()
                                                    .Where(x => x.led01code == ledgerId)
                                                    .FirstOrDefaultAsync();
                if (ledgerDetail == null)
                {
                    throw new Exception("Ledger not found");
                }

                // Update ledger balance for each return item
                foreach (var item in returnData.pur02returnitems)
                {
                    ledgerDetail.led01balance -= item.pur02net_amt; // Subtract the net amount of the return
                    ledgerDetail.DateUpdated = DateTime.Now;

                    _ledgersRepo.Update(ledgerDetail);
                    await _ledgersRepo.SaveAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        //public async Task<bool> UpdateLedgerOnEmpTxn(tran02transaction_details txnData, decimal BalanceCalculator)
        //{
        //    try
        //    {
        //        var employeeDetail = await _employeesRepo.GetDetailAsync(txnData.tran02emp01uin);
        //        var ledgerId = "e_" + employeeDetail.emp01led_code;
        //        var ledgerDetail = await _ledgersRepo.GetList().Where(x => x.led01code == ledgerId).FirstOrDefaultAsync();
        //        if (ledgerDetail == null)
        //        {
        //            throw new Exception("ledger not found");
        //        }

        //        ledgerDetail.led01balance = ledgerDetail.led01balance + BalanceCalculator;
        //        ledgerDetail.DateUpdated = DateTime.Now;

        //        _ledgersRepo.Update(ledgerDetail);
        //        await _ledgersRepo.SaveAsync();

        //        return true;
        //    }

        //    catch (Exception ex)
        //    {
        //        throw new Exception("Invalid data");
        //    }
        //}

    }
}
