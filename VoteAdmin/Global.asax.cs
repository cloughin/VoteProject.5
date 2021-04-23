using System;
using System.ComponentModel;
using System.Web;

namespace Vote
{
  /// <summary>
  /// Summary description for Global.
  /// </summary>
  public class Global : HttpApplication
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
// ReSharper disable NotAccessedField.Local
    private IContainer components;
    // ReSharper restore NotAccessedField.Local

    protected Global()
    {
      InitializeComponent();
    }

    protected void Application_Start(object sender, EventArgs e)
    {
#if !DEBUG
      // prime the Tiger lookup (with an arbitrary location) because of long start time
      TigerLookup.LookupAll(38.976657, -77.373288);
#endif
    }

    protected void Session_Start(object sender, EventArgs e)
    {
#if DEBUG
      GlobalAsax.ProcessVoteConfigForSessionStart(this);
#endif
    }

    protected void Application_AcquireRequestState(object sender, EventArgs e)
    {
#if DEBUG
      GlobalAsax.ProcessVoteConfigForBeginRequest(this);
#endif
    }

    protected void Application_BeginRequest(object sender, EventArgs e) { }

    protected void Application_PostMapRequestHandler(object sender, EventArgs e) {}

    protected void Application_EndRequest(object sender, EventArgs e) {}

    protected void Application_AuthenticateRequest(object sender, EventArgs e) {}

    protected void Application_Error(object sender, EventArgs e)
    {
      GlobalAsax.OnApplicationError(this);
    }

    protected void Session_End(object sender, EventArgs e) {}

    protected void Application_End(object sender, EventArgs e) {}

    #region Web Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      components = new Container();
    }

    #endregion
  }
}