using System;
using System.Diagnostics;
using System.Web;
using DB.VoteLog;

namespace Vote.Master
{
  public partial class LatestLoggedImagePage : VotePage
  {
    internal static byte[] GetLoggedImageByDate(string politicianKey, DateTime date)
    {
      byte[] blob = null;
      //var originalTable = LogPoliticiansImagesOriginal
      //  .GetDataByPoliticianKeyProfileOriginalDate(politicianKey, date);
      //if (originalTable.Count == 1)
      //  blob = originalTable[0].ProfileOriginal;
      //else
      {
        var newOriginalsTable =
          LogDataChange.GetDataByTableNameColumnNameKeyValuesDateStamp(
            "PoliticiansImagesBlobs", "ProfileOriginal", politicianKey, date);
        if (newOriginalsTable.Count == 1)
          blob = Convert.FromBase64String(newOriginalsTable[0].NewValue);
      }
      return blob;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!long.TryParse(GetQueryString("ticks"), out var ticks)) Response.End();
      var blob = GetLoggedImageByDate(QueryId, new DateTime(ticks));
      if (blob == null) Response.End();

      var response = HttpContext.Current.Response;
      var now = DateTime.UtcNow;

      response.Cache.SetCacheability(HttpCacheability.NoCache);
      response.Cache.SetLastModified(now);
      response.Cache.SetMaxAge(new TimeSpan(0));
      response.Cache.SetExpires(now);
      response.ContentType = ImageManager.GetContentType(blob);
      Debug.Assert(blob != null, "blob != null");
      response.BinaryWrite(blob);
      Response.End();
    }
  }
}