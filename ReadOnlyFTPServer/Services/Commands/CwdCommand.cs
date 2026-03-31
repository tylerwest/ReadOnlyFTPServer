using Microsoft.Extensions.Logging;

namespace ReadOnlyFTPServer.Services.Commands;

/// <summary>
/// Purpose: Changes the current working directory on the server.
/// Input: The relative or absolute path of the directory.
/// Output: 250 (CWD successful) or 550 (Directory not found).
/// </summary>
public class CwdCommand : IFtpCommand
{
    private readonly ILogger<CwdCommand> _logger;

    public CwdCommand(ILogger<CwdCommand> logger)
    {
        _logger = logger;
    }

    public string Command => "CWD";

    public async Task ExecuteAsync(FtpCommandContext context, string arg)
    {
        if (!context.LoggedIn)
        {
            await context.Writer.WriteLineAsync("530 Not logged in");
            return;
        }

        var newPath = context.ResolvePath(arg);
        if (Directory.Exists(newPath))
        {
            context.CurrentDir = newPath;
            _logger.LogDebug("Changed directory to: {Path}", newPath);
            await context.Writer.WriteLineAsync("250 Directory changed");
        }
        else
        {
            _logger.LogWarning("Attempted to change to non-existent directory: {Path}", newPath);
            await context.Writer.WriteLineAsync("550 Directory not found");
        }
    }
}
