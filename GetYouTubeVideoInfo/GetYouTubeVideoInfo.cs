using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;

namespace GetYouTubeVideoInfo
{
  public partial class GetYouTubeVideoInfo : ServiceBase
  {
    public GetYouTubeVideoInfo()
    {
      InitializeComponent();
    }

    private TcpListener _TcpListener;

    private static void Log(string message)
    {
      try
      {
        var sw =
          new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\GetYoutubeVideoInfoLog.txt",
            true);
        sw.WriteLine(DateTime.UtcNow + ": " + message);
        sw.Flush();
        sw.Close();
      }
      catch
      {
      }
    }

    private static void Log(Exception ex)
    {
      Log(ex.Source.Trim() + ": " + ex.Message.Trim());
    }

    protected override void OnStart(string[] args)
    {
      try
      {
        _TcpListener = new TcpListener(IPAddress.Parse("50.16.212.183"), 7734);
      }
      catch (Exception ex)
      {
        Log(ex);
      }
      Log("GetYouTubeVideoInfo service started.");
    }

    protected override void OnStop()
    {
      Log("GetYouTubeVideoInfo service stopped.");
    }
  }
}
