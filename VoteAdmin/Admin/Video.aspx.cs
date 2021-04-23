using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using DB.Vote;

namespace Vote.Admin
{
  public partial class VideoPage : SecureAdminPage, IAllowEmptyStateCode
  {
    #region private

    private void FormatVideo(InstructionalVideosRow video)
    {
      if (video == null)
      {
        Response.StatusCode = 404;
        return;
      }
      Debug.Assert(video != null, "video != null");
      var title = HttpUtility.HtmlEncode(video.Title);
      Page.Title = title + " | Instructional Video | Vote-USA.org";
      H1.InnerHtml = title;
      Description.InnerHtml =
        HttpUtility.HtmlEncode(video.Description).ReplaceNewLinesWithParagraphs(false);
      Video.InnerHtml = ScaleEmbedCode(video.EmbedCode);
    }

    #endregion

    #region public

    public static string ScaleEmbedCode(string code, int maxWidth = 940)
    {
      var regexObj = new Regex(@" height=""(?<height>\d+)""| width=""(?<width>\d+)""",
        RegexOptions.IgnoreCase);
      var matches = regexObj.Matches(code);
      var width = 0;
      var height = 0;
      foreach (Match match in matches)
        if (match.Groups["width"].Captures.Count == 1)
          int.TryParse(match.Groups["width"].Value, out width);
        else if (match.Groups["height"].Captures.Count == 1)
          int.TryParse(match.Groups["height"].Value, out height);
      if (width > maxWidth)
      {
        var scale = Convert.ToDouble(maxWidth) / Convert.ToDouble(width);
        width = maxWidth;
        height = Convert.ToInt32(Math.Round(Convert.ToDouble(height) * scale));
        code = regexObj.Replace(code, m => m.Groups["width"].Captures.Count == 1
          ? " width=\"" + width + "\""
          : " height=\"" + height + "\"");
      }
      return code;
    }

    #endregion public

    #region Event handlers and overrides

    protected void Page_Load(object sender, EventArgs e)
    {
      // get id
      int.TryParse(GetQueryString("video"), out var id);
      var video = InstructionalVideos.GetDataById(id).FirstOrDefault(v => v.AdminOrder != 0);
      FormatVideo(video);
    }

    #endregion Event handlers and overrides
  }
}