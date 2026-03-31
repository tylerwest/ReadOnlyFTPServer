using Microsoft.Extensions.Logging;

namespace ReadOnlyFTPServer.Services.Commands;

/// <summary>
/// Purpose: Changes the current working directory to the parent directory.
/// Input: None.
/// Output: 200 (Directory changed to parent) or 530 (Not logged in).
/// </summary>
public class CdupCommand : IFtpCommand
{
    public CdupCommand(ILogger<CdupCommand> logger)
    {
    }

    public string Command => "CDUP";

    public async Task ExecuteAsync(FtpCommandContext context, string arg)
    {
        if (!context.LoggedIn)
        {
            await context.Writer.WriteLineAsync("530 Not logged in");
            return;
        }

        var newPath = context.ResolvePath("..");
        context.CurrentDir = newPath;
        await context.Writer.WriteLineAsync("200 Directory changed to parent");
    }
}
