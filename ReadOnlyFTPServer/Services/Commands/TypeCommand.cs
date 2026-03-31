using Microsoft.Extensions.Logging;

namespace ReadOnlyFTPServer.Services.Commands;

/// <summary>
/// Purpose: Sets the transfer type (e.g., ASCII or Binary). 
/// Input: The type code (A for ASCII, I for Image/Binary).
/// Output: 200 (Command okay).
/// </summary>
public class TypeCommand : IFtpCommand
{
    public string Command => "TYPE";
    public TypeCommand(ILogger<TypeCommand> _) { }

    public async Task ExecuteAsync(FtpCommandContext context, string arg)
    {
        await context.Writer.WriteLineAsync("200 Type set");
    }
}
