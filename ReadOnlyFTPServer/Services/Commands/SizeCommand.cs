using Microsoft.Extensions.Logging;

namespace ReadOnlyFTPServer.Services.Commands;

/// <summary>
/// Purpose: Returns the size of a file in bytes.
/// Input: The name of the file.
/// Output: 213 (File size) or 550 (File not found).
/// </summary>
public class SizeCommand : IFtpCommand
{
    public SizeCommand(ILogger<SizeCommand> logger)
    {
    }

    public string Command => "SIZE";

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
            var size = new FileInfo(filePath).Length;
            await context.Writer.WriteLineAsync($"213 {size}");
        }
        else
        {
            await context.Writer.WriteLineAsync("550 File not found");
        }
    }
}
