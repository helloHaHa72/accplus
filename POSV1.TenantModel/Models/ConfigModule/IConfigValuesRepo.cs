using System;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Models
{
    public interface IConfigValuesRepo
    {
        string GetString(string ModuleName, string KeyName);
        int GetInt(string ModuleName, string KeyName);
        bool GetBool(string ModuleName, string KeyName);
        long GetLong(string ModuleName, string KeyName);
        DateTime GetDatetime(string ModuleName, string KeyName);
        decimal GetDecimal(string ModuleName, string KeyName);

        bool Update(string ModuleName, string KeyName, string Value);
        bool Update(string ModuleName, string KeyName, int Value);
        bool Update(string ModuleName, string KeyName, bool Value);
        bool Update(string ModuleName, string KeyName, long Value);
        bool Update(string ModuleName, string KeyName, DateTime Value);
        bool Update(string ModuleName, string KeyName, decimal Value);

        Task<bool> SaveAsync();
    }
}
