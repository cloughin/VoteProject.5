using System;
using System.Web;
using System.Web.UI;

namespace Vote.Admin
{
  public partial class KeepAlive : Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      Response.Cache.SetCacheability(HttpCacheability.NoCache);
      Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
      Response.Cache.SetNoStore();
      Response.Cache.SetNoServerCaching();
    }
  }
}