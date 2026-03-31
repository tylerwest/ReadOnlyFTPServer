using Microsoft.Extensions.Logging;

namespace ReadOnlyFTPServer.Services.Commands;

/// <summary>
/// Purpose: Returns the last modified time of a file.
/// Input: The name of the file.
/// Output: 213 (UTC timestamp) or 550 (File not found).
/// </summary>
public class MdtmCommand : IFtpCommand
{
    public MdtmCommand(ILogger<MdtmCommand> logger)
    {
    }

    public string Command => "MDTM";

    public async Task ExecuteAsync(FtpCommandContext context, string arg)
    {
        if (!context.LoggedIn)
        {
            await context.Writer.WriteLineAsync("530 Not logged in");
            return;
        }

        var filePath = context.ResolvePath(arg);
        if (File.Exists(filePath))
        {
            var date = File.GetLastWriteTimeUtc(filePath);
            await context.Writer.WriteLineAsync($"213 {date:yyyyMMddHHmmss}");
        }
        else
        {
            await context.Writer.WriteLineAsync("550 File not found");
        }
    }
}
