using Microsoft.Extensions.Logging;

namespace ReadOnlyFTPServer.Services.Commands;

/// <summary>
/// Purpose: Sets a restart marker for the next data transfer command (RETR or STOR).
/// Input: The byte offset to seek to in the file.
/// Output: 350 (Restarting at specified offset).
/// </summary>
public class RestCommand : IFtpCommand
{
    private readonly ILogger<RestCommand> _logger;

    public RestCommand(ILogger<RestCommand> logger)
    {
        _logger = logger;
    }

    public string Command => "REST";

    public async Task ExecuteAsync(FtpCommandContext context, string arg)
    {
        if (long.TryParse(arg, out var offset) && offset >= 0)
        {
            context.RestartOffset = offset;
            _logger.LogInformation("Restart offset set to {Offset}", offset);
            await context.Writer.WriteLineAsync($"350 Restarting at {offset}. Send RETR or STOR.");
        }
        else
        {
            await context.Writer.WriteLineAsync("501 Invalid offset");
        }
    }
}
