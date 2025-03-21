using Microsoft.EntityFrameworkCore;
using POSV1.TenantModel;
using System;

namespace POSV1.TenantAPI.Services
{
    public class MigrationStatusService
    {
        private readonly MainDbContext _context;
        private readonly ILogger<MigrationStatusService> _logger;

        public MigrationStatusService(MainDbContext context, ILogger<MigrationStatusService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public MigrationStatusDto GetMigrationStatus()
        {
            var appliedMigrations = _context.Database.GetAppliedMigrations().ToList();

            var allMigrations = _context.Database.GetMigrations().ToList();

            var pendingMigrations = allMigrations.Except(appliedMigrations).ToList();

            return new MigrationStatusDto
            {
                AppliedMigrations = appliedMigrations,
                PendingMigrations = pendingMigrations
            };
        }
    }

    public class MigrationStatusDto
    {
        public List<string> AppliedMigrations { get; set; }
        public List<string> PendingMigrations { get; set; }
    }
}
