using System;
using System.Diagnostics;
using System.Linq;
using System.Web;
using DB.Vote;

namespace Vote.Party
{
  public partial class VideoPage : SecurePartyPage, IAllowEmptyPartyKey
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
      Video.InnerHtml = Admin.VideoPage.ScaleEmbedCode(video.EmbedCode);
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
      // get id
      int.TryParse(GetQueryString("video"), out var id);
      var video = InstructionalVideos.GetDataById(id).FirstOrDefault(v => v.VolunteersOrder != 0);
      FormatVideo(video);
    }
  }
}