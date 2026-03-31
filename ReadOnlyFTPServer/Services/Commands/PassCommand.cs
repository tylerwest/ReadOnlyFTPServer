using Microsoft.Extensions.Logging;

namespace ReadOnlyFTPServer.Services.Commands;

/// <summary>
/// Purpose: Provides the password for the user identified by the previous USER command.
/// Input: The password string.
/// Output: 230 (User logged in) or 530 (Login incorrect).
/// </summary>
public class PassCommand : IFtpCommand
{
    private readonly ILogger<PassCommand> _logger;

    public PassCommand(ILogger<PassCommand> logger)
    {
        _logger = logger;
    }

    public string Command => "PASS";

    public async Task ExecuteAsync(FtpCommandContext context, string arg)
    {
        if (context.PendingUser == null)
        {
            await context.Writer.WriteLineAsync("503 Bad sequence");
            return;
        }

        if (context.UserStore.Validate(context.PendingUser, arg))
        {
            context.LoggedIn = true;
            _logger.LogInformation("User {User} logged in successfully.", context.PendingUser);
            await context.Writer.WriteLineAsync("230 Login successful");
        }
        else
        {
            _logger.LogWarning("Login failed for user: {User}", context.PendingUser);
            await context.Writer.WriteLineAsync("530 Login incorrect");
        }
    }
}
