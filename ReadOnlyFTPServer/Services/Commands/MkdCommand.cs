using Microsoft.Extensions.Logging;

namespace ReadOnlyFTPServer.Services.Commands;

/// <summary>
/// Purpose: Creates a new directory on the server.
/// Status: Disabled (Read-only server).
/// Output: 550 (Read-only server).
/// </summary>
public class MkdCommand : IFtpCommand
{
    public MkdCommand(ILogger<MkdCommand> logger)
    {
    }

    public string Command => "MKD";
    public async Task ExecuteAsync(FtpCommandContext context, string arg) => await context.Writer.WriteLineAsync("550 Read-only server");
}
