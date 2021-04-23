using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB.Vote;

namespace Vote.Admin
{
  public partial class VideosPage : SecureAdminPage, IAllowEmptyStateCode
  {
    #region public

    public static void FormatVideos(InstructionalVideosTable table, Control parent)
    {
      var videosContainer = new HtmlDiv().AddTo(parent, "videos-container");
      foreach (var video in table)
      {
        var videoContainer = new HtmlDiv().AddTo(videosContainer, "video-container");
        var h4 = new HtmlH4().AddTo(videoContainer) as HtmlH4;
        new HtmlAnchor
        {
          InnerText = video.Title,
          HRef = "video.aspx?video=" + video.Id,
          Target = "video"
        }.AddTo(h4);
        if (!string.IsNullOrWhiteSpace(video.Description))
          new HtmlP
            {
              InnerHtml =
                HttpUtility.HtmlEncode(video.Description).ReplaceNewLinesWithParagraphs(false)
            }
            .AddTo(videoContainer);
      }
    }

    #endregion public

    #region Event handlers and overrides

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        Page.Title = "Instructional Videos for Admins";
        H1.InnerHtml = "Instructional Videos for Admins";
        var table = InstructionalVideos.GetAdminData();
        FormatVideos(table, VideosPlaceHolder);
      }
    }

    #endregion Event handlers and overrides
  }
}