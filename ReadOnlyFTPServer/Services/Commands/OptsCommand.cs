using Microsoft.Extensions.Logging;

namespace ReadOnlyFTPServer.Services.Commands;

/// <summary>
/// Purpose: Sets options for specific FTP commands.
/// Input: The command and its options (e.g., "UTF8 ON").
/// Output: 200 (Command okay) or 501 (Option not understood).
/// </summary>
public class OptsCommand : IFtpCommand
{
    private readonly ILogger<OptsCommand> _logger;

    public OptsCommand(ILogger<OptsCommand> logger)
    {
        _logger = logger;
    }

    public string Command => "OPTS";

    public async Task ExecuteAsync(FtpCommandContext context, string arg)
    {
        var parts = arg.Split(' ', 2);
        var subCommand = parts[0].ToUpperInvariant();
        var subArg = parts.Length > 1 ? parts[1].ToUpperInvariant() : "";

        if (subCommand == "UTF8" && (subArg == "ON" || subArg == ""))
        {
            _logger.LogInformation("Client requested UTF8 mode.");
            await context.Writer.WriteLineAsync("200 Always in UTF8 mode.");
        }
        else
        {
            _logger.LogWarning("Unknown OPTS subcommand: {SubCommand} {SubArg}", subCommand, subArg);
            await context.Writer.WriteLineAsync("501 Option not understood");
        }
    }
}
