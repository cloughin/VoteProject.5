using System;
using System.Diagnostics;
using DB.Vote;
using static System.String;

namespace Vote.banneradimage
{
  public partial class BannerAdImageDefaultPage : VotePage
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      var query = Request.Url.Query;
      if (IsNullOrWhiteSpace(query))
        Utility.Signal404();
      var info = Server.UrlDecode(query.Substring(1))?.Split('.');
      Debug.Assert(info != null, nameof(info) + " != null");
      if (info.Length < 1) Utility.Signal404();
      var adType = info[0];
      var stateCode = info.Length >= 2 ? info[1] : Empty;
      var electionKey = info.Length >= 3 ? info[2] : Empty;
      var officeKey = info.Length >= 4 ? info[3] : Empty;

      var blob = BannerAds.GetAdImage(adType, stateCode, electionKey, officeKey);
      if (blob == null) Utility.Signal404();
      AdImageDefaultPage.ServeImage(blob);
    }
  }
}