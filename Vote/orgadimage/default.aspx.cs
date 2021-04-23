using System;
using System.Diagnostics;
using DB.Vote;
using static System.String;

namespace Vote.orgadimage
{
  public partial class OrgAdImageDefaultPage : VotePage
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      var query = Request.Url.Query;
      if (IsNullOrWhiteSpace(query))
        Utility.Signal404();
      var info = Server.UrlDecode(query.Substring(1))?.Split('.');
      Debug.Assert(info != null, nameof(info) + " != null");
      var orgId = 0;
      if (info.Length < 1 || !int.TryParse(info[0], out orgId)) Utility.Signal404();

      var blob = Organizations.GetAdImage(orgId);
      if (blob == null) Utility.Signal404();
      AdImageDefaultPage.ServeImage(blob);
    }
  }
}