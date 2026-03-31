using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReadOnlyFTPServer.Models;

namespace ReadOnlyFTPServer.Services;

public class UserStore
{
    private readonly IOptionsMonitor<UserStoreOptions> _monitor;
    private readonly ILogger<UserStore> _logger;

    public UserStore(IOptionsMonitor<UserStoreOptions> monitor, ILogger<UserStore> logger)
    {
        _monitor = monitor;
        _logger = logger;

        // Log when a change is detected in users.json
        _monitor.OnChange(opt =>
        {
            _logger.LogInformation("[UserStore] Hot-reload detected. Loaded {Count} users.", opt.Users.Count);
        });
    }

    public bool Validate(string username, string password)
    {
        var users = _monitor.CurrentValue.Users;
        var user = users.Find(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));
        
        return user != null && user.Password == password;
    }
}