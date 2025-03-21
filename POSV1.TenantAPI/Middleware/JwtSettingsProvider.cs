using Microsoft.Extensions.Options;
using POSV1.TenantAPI.Startup;

namespace POSV1.TenantAPI.Middleware;

public class JwtSettingsProvider
{
    private readonly IOptionsMonitor<JwtSettings> _optionsMonitor;
    private JwtSettings _currentSettings;

    public JwtSettingsProvider(IOptionsMonitor<JwtSettings> optionsMonitor)
    {
        _optionsMonitor = optionsMonitor;
        _currentSettings = optionsMonitor.CurrentValue;

        // Listen for changes and update the settings atomically
        _optionsMonitor.OnChange(updatedSettings =>
        {
            Interlocked.Exchange(ref _currentSettings, updatedSettings);
        });
    }

    public JwtSettings GetSettings()
    {
        return _currentSettings; // Thread-safe access
    }
}

