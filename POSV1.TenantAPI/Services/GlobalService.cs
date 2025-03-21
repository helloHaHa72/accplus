using Microsoft.EntityFrameworkCore;
using POSV1.TenantModel.Repo.Interface;

namespace POSV1.TenantAPI.Services
{
    public interface IGlobalService
    {
        string GetInvoiceNumber();
    }
    public class GlobalService : IGlobalService
    {
        private readonly ISalesRepo _salesRepo;
        private readonly IPurchaseRepo _purchaseRepo;
        public GlobalService(ISalesRepo salesRepo,
            IPurchaseRepo purchaseRepo)
        {
            _salesRepo = salesRepo;
            _purchaseRepo = purchaseRepo;
        }

        public string GetInvoiceNumber()
        {
            var latestData = _purchaseRepo
                .GetList()
                .Where(x => x.pur01invoice_no.StartsWith("pur-"))
                .OrderByDescending(x => x.pur01uin)
                .FirstOrDefault();

            int newNumber = 1;

            if (latestData != null)
            {
                string latestInvoice = latestData.pur01invoice_no;
                if (!string.IsNullOrEmpty(latestInvoice))
                {
                    var parts = latestInvoice.Split('-');
                    if (parts.Length == 3 && int.TryParse(parts[1], out int lastNumber))
                    {
                        newNumber = (lastNumber % 2000) + 1;
                    }
                }
            }

            string formattedNumber = newNumber.ToString("D4");
            string randomLetters = GenerateRandomLetters(3);

            return $"pur-{formattedNumber}-{randomLetters}";
        }

        private string GenerateRandomLetters(int length)
        {
            Random random = new();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
                                        .Select(s => s[random.Next(s.Length)])
                                        .ToArray());
        }

    }
}
