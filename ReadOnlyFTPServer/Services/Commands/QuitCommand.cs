using Microsoft.Extensions.Logging;

namespace ReadOnlyFTPServer.Services.Commands;

/// <summary>
/// Purpose: Terminates the FTP session.
/// Input: None.
/// Output: 221 (Service closing control connection).
/// </summary>
public class QuitCommand : IFtpCommand
{
    public string Command => "QUIT";
    public QuitCommand(ILogger<QuitCommand> _) { }

    public async Task ExecuteAsync(FtpCommandContext context, string arg)
    {
        await context.Writer.WriteLineAsync("221 Goodbye");
    }
}
