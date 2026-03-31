using Microsoft.Extensions.Logging;

namespace ReadOnlyFTPServer.Services.Commands;

/// <summary>
/// Purpose: No operation. Used to prevent timeout.
/// Input: None.
/// Output: 200 (OK).
/// </summary>
public class NoopCommand : IFtpCommand
{
    public NoopCommand(ILogger<NoopCommand> logger)
    {
    }

    public string Command => "NOOP";

    public async Task ExecuteAsync(FtpCommandContext context, string arg)
    {
        await context.Writer.WriteLineAsync("200 OK");
    }
}
