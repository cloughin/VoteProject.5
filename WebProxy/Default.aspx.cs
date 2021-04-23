using System;
using System.Net;

namespace WebProxy
{
  public partial class Default : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      try
      {
        // ReSharper disable once AssignNullToNotNullAttribute
        Response.BinaryWrite(new WebClient().DownloadData(Server.UrlDecode(Request.QueryString.ToString())));
      }
      catch
      {
        // ignored
      }
    }
  }
}