using System;
using System.Net;
using System.Web;
using static System.String;

namespace Vote
{
  public partial class DSTTest : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (IsPostBack)
      {
        TextBoxResult.Text = Empty;
        var address = TextBoxAddress.Text.Trim();
        if (!IsNullOrWhiteSpace(address))
        {
          var url =
            $"http://ec2-35-171-163-158.compute-1.amazonaws.com/street2coordinates/{HttpUtility.UrlEncode(address)}";
          var result = new WebClient().DownloadString(url);
          TextBoxResult.Text = result;
        }
      }
    }
  }
}