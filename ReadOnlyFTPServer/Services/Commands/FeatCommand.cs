using Microsoft.Extensions.Logging;

namespace ReadOnlyFTPServer.Services.Commands;

/// <summary>
/// Purpose: Lists all extended features supported by the server.
/// Input: None.
/// Output: 211 (Features specification).
/// </summary>
public class FeatCommand : IFtpCommand
{
    public FeatCommand(ILogger<FeatCommand> logger)
    {
    }

    public string Command => "FEAT";

    public async Task ExecuteAsync(FtpCommandContext context, string arg)
    {
        await context.Writer.WriteLineAsync("211-Features:");
        await context.Writer.WriteLineAsync(" PASV");
        await context.Writer.WriteLineAsync(" EPSV");
        await context.Writer.WriteLineAsync(" UTF8");
        await context.Writer.WriteLineAsync(" SIZE");
        await context.Writer.WriteLineAsync(" MDTM");
        await context.Writer.WriteLineAsync(" REST STREAM");
        await context.Writer.WriteLineAsync("211 End");
    }
}
