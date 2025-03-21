﻿using System;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Models
{
    public class ConfigValuesByEnumRepo :
    RepoBaseModelCore._AbsGeneralRepositories<MainDbContext, cfg01configurations, int>,
        IConfigValuesByEnumRepo
    {
        private readonly IConfigValuesRepo _MainRepo;
        public ConfigValuesByEnumRepo(IConfigValuesRepo MainRepo, MainDbContext context) : base(context)
        {
            _MainRepo = MainRepo;
        }

        public bool GetBool(enumConfigSettingsKeys Key)
        {
            var Set1 = ConfigSettingsKeyValues._AllKeys[Key];
            return _MainRepo.GetBool(Set1.Key, Set1.Value);
        }

        public DateTime GetDatetime(enumConfigSettingsKeys Key)
        {
            var Set1 = ConfigSettingsKeyValues._AllKeys[Key];
            return _MainRepo.GetDatetime(Set1.Key, Set1.Value);
        }

        public int GetInt(enumConfigSettingsKeys Key)
        {
            var Set1 = ConfigSettingsKeyValues._AllKeys[Key];
            return _MainRepo.GetInt(Set1.Key, Set1.Value);
        }

        public long GetLong(enumConfigSettingsKeys Key)
        {
            var Set1 = ConfigSettingsKeyValues._AllKeys[Key];
            return _MainRepo.GetLong(Set1.Key, Set1.Value);

        }
        public decimal GetDecimal(enumConfigSettingsKeys Key)
        {
            var Set1 = ConfigSettingsKeyValues._AllKeys[Key];
            return _MainRepo.GetDecimal(Set1.Key, Set1.Value);

        }

        public string GetString(enumConfigSettingsKeys Key)
        {
            var Set1 = ConfigSettingsKeyValues._AllKeys[Key];
            return _MainRepo.GetString(Set1.Key, Set1.Value);
        }

        public bool Update(enumConfigSettingsKeys Key, string Value)
        {
            var Set1 = ConfigSettingsKeyValues._AllKeys[Key];
            return _MainRepo.Update(Set1.Key, Set1.Value, Value);
        }

        public bool Update(enumConfigSettingsKeys Key, int Value)
        {
            var Set1 = ConfigSettingsKeyValues._AllKeys[Key];
            return _MainRepo.Update(Set1.Key, Set1.Value, Value);
        }

        public bool Update(enumConfigSettingsKeys Key, bool Value)
        {
            var Set1 = ConfigSettingsKeyValues._AllKeys[Key];
            return _MainRepo.Update(Set1.Key, Set1.Value, Value);
        }

        public bool Update(enumConfigSettingsKeys Key, long Value)
        {
            var Set1 = ConfigSettingsKeyValues._AllKeys[Key];
            return _MainRepo.Update(Set1.Key, Set1.Value, Value);
        }

        public bool Update(enumConfigSettingsKeys Key, DateTime Value)
        {
            var Set1 = ConfigSettingsKeyValues._AllKeys[Key];
            return _MainRepo.Update(Set1.Key, Set1.Value, Value);
        }
        public bool Update(enumConfigSettingsKeys Key, decimal Value)
        {
            var Set1 = ConfigSettingsKeyValues._AllKeys[Key];
            return _MainRepo.Update(Set1.Key, Set1.Value, Value);
        }

        public async Task<bool> SaveAsync()
        {
            await _MainRepo.SaveAsync();
            return true;
        }
    }
}
