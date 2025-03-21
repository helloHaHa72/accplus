using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace POSV1.TenantModel.Models
{
    public class ConfigValuesRepo : IConfigValuesRepo
    {
        private readonly MainDbContext _db;
        public ConfigValuesRepo(MainDbContext db)
        {
            _db = db;
        }

        private cfg01configurations Create(string ModuleName, string KeyName, string Value)
        {
            cfg01configurations cfg01configurations = new cfg01configurations()
            {
                cfg01module = ModuleName,
                cfg01key = KeyName,
                cfg01value = Value,
                cfg01created_date = DateTime.Now,
                cfg01created_name = "Admin",

                cfg01updated_date = DateTime.Now,
                cfg01updated_name = "Admin"
            };
            _db.cfg01configurations.Add(cfg01configurations);
            return cfg01configurations;
        }
        private cfg01configurations Update(cfg01configurations Data)
        {
            Data.cfg01updated_date = DateTime.Now;
            Data.cfg01updated_name = "Admin";
            _db.Entry(Data).State = EntityState.Modified;// (cfg01configurations);
            _db.SaveChanges();
            return Data;
        }

        private cfg01configurations Rec(string ModuleName, string KeyName, string defaultValue = "")
        {
            cfg01configurations cfg01configurations = _db.cfg01configurations.FirstOrDefault(x => x.cfg01module == ModuleName && x.cfg01key == KeyName);
            if (cfg01configurations == null)
            {
                Create(ModuleName, KeyName, defaultValue);
                _db.SaveChanges();
                cfg01configurations = Rec(ModuleName, KeyName);
            }
            if (cfg01configurations.cfg01value == null) { cfg01configurations.cfg01value = ""; }
            return cfg01configurations;
        }
        public bool GetBool(string ModuleName, string KeyName)
        {
            var defaultValue = "False";
            cfg01configurations cfg01configurations = Rec(ModuleName, KeyName, defaultValue);
            bool s = false;
            bool.TryParse(cfg01configurations.cfg01value, out s);
            return s;
        }

        public DateTime GetDatetime(string ModuleName, string KeyName)
        {
            cfg01configurations cfg01configurations = Rec(ModuleName, KeyName);
            DateTime s = default(DateTime);
            DateTime.TryParse(cfg01configurations.cfg01value, out s);
            return s;
        }

        public int GetInt(string ModuleName, string KeyName)
        {
            cfg01configurations cfg01configurations = Rec(ModuleName, KeyName);
            int s = default(int);
            int.TryParse(cfg01configurations.cfg01value, out s);
            return s;
        }

        public long GetLong(string ModuleName, string KeyName)
        {
            cfg01configurations cfg01configurations = Rec(ModuleName, KeyName);
            long s = default(long);
            if (cfg01configurations.cfg01value == null)
            {
                cfg01configurations.cfg01value = "";
            }
            long.TryParse(cfg01configurations.cfg01value, out s);
            return s;
        }
        public decimal GetDecimal(string ModuleName, string KeyName)
        {
            cfg01configurations cfg01configurations = Rec(ModuleName, KeyName);
            decimal s = default(decimal);
            if (cfg01configurations.cfg01value == null)
            {
                cfg01configurations.cfg01value = "";
            }
            decimal.TryParse(cfg01configurations.cfg01value, out s);
            return s;
        }

        public string GetString(string ModuleName, string KeyName)
        {
            cfg01configurations cfg01configurations = Rec(ModuleName, KeyName);
            string s = cfg01configurations.cfg01value;
            return s;
        }

        public bool Update(string ModuleName, string KeyName, string Value)
        {
            cfg01configurations r1 = Rec(ModuleName, KeyName);
            r1.cfg01value = Value;

            
            Update(r1);
            return true;
        }

        public bool Update(string ModuleName, string KeyName, int Value)
        {
            cfg01configurations r1 = Rec(ModuleName, KeyName);
            r1.cfg01value = Value.ToString();
            Update(r1);
            return true;
        }

        public bool Update(string ModuleName, string KeyName, bool Value)
        {
            cfg01configurations r1 = Rec(ModuleName, KeyName);
            r1.cfg01value = Value.ToString();
            Update(r1);
            return true;
        }

        public bool Update(string ModuleName, string KeyName, long Value)
        {
            cfg01configurations r1 = Rec(ModuleName, KeyName);
            r1.cfg01value = Value.ToString();
            Update(r1);
            return true;
        }

        public bool Update(string ModuleName, string KeyName, DateTime Value)
        {
            cfg01configurations r1 = Rec(ModuleName, KeyName);
            r1.cfg01value = Value.ToString();
            Update(r1);
            return true;
        }
        public bool Update(string ModuleName, string KeyName, decimal Value)
        {
            cfg01configurations r1 = Rec(ModuleName, KeyName);
            r1.cfg01value = Value.ToString();
            Update(r1);
            return true;
        }
        public async Task<bool> SaveAsync()
        {
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
