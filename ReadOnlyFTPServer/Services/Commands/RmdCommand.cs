using Microsoft.Extensions.Logging;

namespace ReadOnlyFTPServer.Services.Commands;

/// <summary>
/// Purpose: Removes an empty directory on the server.
/// Status: Disabled (Read-only server).
/// Output: 550 (Read-only server).
/// </summary>
public class RmdCommand : IFtpCommand
{
    public RmdCommand(ILogger<RmdCommand> logger)
    {
    }

    public string Command => "RMD";
    public async Task ExecuteAsync(FtpCommandContext context, string arg) => await context.Writer.WriteLineAsync("550 Read-only server");
}
