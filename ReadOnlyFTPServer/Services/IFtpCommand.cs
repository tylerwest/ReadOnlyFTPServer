namespace ReadOnlyFTPServer.Services;

public interface IFtpCommand
{
    string Command { get; }
    Task ExecuteAsync(FtpCommandContext context, string arg);
}
