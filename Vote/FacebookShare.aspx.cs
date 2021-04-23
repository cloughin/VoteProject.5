using System;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using DB.Vote;

namespace Vote
{
  public partial class FacebookSharePage : Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      var json = new JavaScriptSerializer();
      DebugLog.Insert("fsp", Request.Form.ToString());
      var choices = json.DeserializeObject(Request.Form["choices"]);
      var voteUrl = WebService.FormatShareUrl(Request.Form["url"], choices);
      Response.Redirect("https://www.facebook.com/sharer/sharer.php?p[url]=" +
        $"{HttpContext.Current.Server.UrlEncode(voteUrl)}&u={HttpContext.Current.Server.UrlEncode(voteUrl)}");
    }
  }
}