using Microsoft.Extensions.Logging;

namespace ReadOnlyFTPServer.Services.Commands;

/// <summary>
/// Purpose: Returns the system type of the server.
/// Input: None.
/// Output: 215 (UNIX Type: L8).
/// </summary>
public class SystCommand : IFtpCommand
{
    public SystCommand(ILogger<SystCommand> logger)
    {
    }

    public string Command => "SYST";

    public async Task ExecuteAsync(FtpCommandContext context, string arg)
    {
        await context.Writer.WriteLineAsync("215 UNIX Type: L8");
    }
}
