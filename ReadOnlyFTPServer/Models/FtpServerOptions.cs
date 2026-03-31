namespace ReadOnlyFTPServer.Models;

public class FtpServerOptions
{
    public int Port { get; set; } = 2121;
    public string RootPath { get; set; } = "";
    public int PassivePortRangeLow { get; set; } = 40000;
    public int PassivePortRangeHigh { get; set; } = 40100;
}
