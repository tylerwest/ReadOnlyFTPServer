using Microsoft.Extensions.Logging;

namespace ReadOnlyFTPServer.Services.Commands;

/// <summary>
/// Purpose: Specifies the "rename from" path for a file or directory.
/// Status: Disabled (Read-only server).
/// Output: 550 (Read-only server).
/// </summary>
public class RnfrCommand : IFtpCommand
{
    public RnfrCommand(ILogger<RnfrCommand> logger)
    {
    }

    public string Command => "RNFR";
    public async Task ExecuteAsync(FtpCommandContext context, string arg) => await context.Writer.WriteLineAsync("550 Read-only server");
}
