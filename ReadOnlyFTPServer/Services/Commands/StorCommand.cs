using Microsoft.Extensions.Logging;

namespace ReadOnlyFTPServer.Services.Commands;

/// <summary>
/// Purpose: Stores (uploads) a file on the server.
/// Status: Disabled (Read-only server).
/// Output: 550 (Read-only server).
/// </summary>
public class StorCommand : IFtpCommand
{
    public StorCommand(ILogger<StorCommand> logger)
    {
    }

    public string Command => "STOR";
    public async Task ExecuteAsync(FtpCommandContext context, string arg) => await context.Writer.WriteLineAsync("550 Read-only server");
}
