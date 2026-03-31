using Microsoft.Extensions.Logging;

namespace ReadOnlyFTPServer.Services.Commands;

/// <summary>
/// Purpose: Deletes a file on the server.
/// Status: Disabled (Read-only server).
/// Output: 550 (Read-only server).
/// </summary>
public class DeleCommand : IFtpCommand
{
    public DeleCommand(ILogger<DeleCommand> logger)
    {
    }

    public string Command => "DELE";
    public async Task ExecuteAsync(FtpCommandContext context, string arg) => await context.Writer.WriteLineAsync("550 Read-only server");
}
