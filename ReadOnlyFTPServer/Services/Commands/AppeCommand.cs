using Microsoft.Extensions.Logging;

namespace ReadOnlyFTPServer.Services.Commands;

/// <summary>
/// Purpose: Appends data to an existing file on the server.
/// Status: Disabled (Read-only server).
/// Output: 550 (Read-only server).
/// </summary>
public class AppeCommand : IFtpCommand
{
    public AppeCommand(ILogger<AppeCommand> logger)
    {
    }

    public string Command => "APPE";
    public async Task ExecuteAsync(FtpCommandContext context, string arg) => await context.Writer.WriteLineAsync("550 Read-only server");
}
