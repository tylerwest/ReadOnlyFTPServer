using Microsoft.Extensions.Logging;

namespace ReadOnlyFTPServer.Services.Commands;

/// <summary>
/// Purpose: Identifies the user to the server. Starts the login sequence.
/// Input: The username string.
/// Output: 331 (User name okay, need password) or 530 (Not logged in/Denied).
/// </summary>
public class UserCommand : IFtpCommand
{
    private readonly ILogger<UserCommand> _logger;

    public UserCommand(ILogger<UserCommand> logger)
    {
        _logger = logger;
    }

    public string Command => "USER";

    public async Task ExecuteAsync(FtpCommandContext context, string arg)
    {
        context.PendingUser = arg;
        context.LoggedIn = false;

        if (string.Equals(arg, "anonymous", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogWarning("Anonymous login attempt blocked for user: {User}", arg);
            await context.Writer.WriteLineAsync("530 Anonymous logins disabled");
            context.PendingUser = null;
        }
        else
        {
            await context.Writer.WriteLineAsync("331 Username OK, need password");
        }
    }
}
