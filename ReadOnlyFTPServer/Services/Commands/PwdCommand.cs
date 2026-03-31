using Microsoft.Extensions.Logging;

namespace ReadOnlyFTPServer.Services.Commands;

/// <summary>
/// Purpose: Prints the current working directory.
/// Input: None.
/// Output: 257 (The current path in quotes).
/// </summary>
public class PwdCommand : IFtpCommand
{
    public string Command => "PWD";
    public PwdCommand(ILogger<PwdCommand> logger) { }

    public async Task ExecuteAsync(FtpCommandContext context, string arg)
    {
        var rel = context.GetRelativePath(context.CurrentDir);
        await context.Writer.WriteLineAsync($"257 \"{rel}\"");
    }
}
