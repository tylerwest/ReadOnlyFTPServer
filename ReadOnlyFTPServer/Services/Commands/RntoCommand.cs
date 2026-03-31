using Microsoft.Extensions.Logging;

namespace ReadOnlyFTPServer.Services.Commands;

/// <summary>
/// Purpose: Specifies the "rename to" path for a file or directory.
/// Status: Disabled (Read-only server).
/// Output: 550 (Read-only server).
/// </summary>
public class RntoCommand : IFtpCommand
{
    public RntoCommand(ILogger<RntoCommand> logger)
    {
    }

    public string Command => "RNTO";
    public async Task ExecuteAsync(FtpCommandContext context, string arg) => await context.Writer.WriteLineAsync("550 Read-only server");
}
