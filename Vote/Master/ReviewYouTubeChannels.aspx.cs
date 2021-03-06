using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.HtmlControls;
using DB;
using DB.Vote;

namespace Vote.Master
{
  public partial class ReviewYouTubeChannelsPage : SecurePage, ISuperUser
  {
    #region Private

    private void BuildTable(IEnumerable<DataRow> unverifiedVideos)
    {
      var even = true;
      foreach (var row in unverifiedVideos)
      {
        var tr = new HtmlTableRow().AddTo(BodyPlaceHolder, even ? "even" : "odd");
        tr.Attributes.Add("data-key", row.PoliticianKey());
        even = !even;

        var td = new HtmlTableCell().AddTo(tr, "name");
        var content = Politicians.FormatName(row) + " [" + row.StateCode() + "]";
        new HtmlDiv {InnerText = content}.AddTo(td).Attributes.Add("title", content);

        td = new HtmlTableCell().AddTo(tr, "video");
        var div = new HtmlDiv().AddTo(td);
        div.Attributes.Add("title", row.YouTubeWebAddress());
        new HtmlAnchor
        {
          InnerText = row.YouTubeWebAddress().GetYouTubeVideoId(),
          HRef = NormalizeUrl(row.YouTubeWebAddress()),
          Target = "view"
        }.AddTo(div);

        new HtmlTableCell {InnerHtml = "<div>&nbsp;</div>"}.AddTo(tr, "channel-url");
        new HtmlTableCell {InnerHtml = "<div>&nbsp;</div>"}.AddTo(tr, "channel-title");
        new HtmlTableCell {InnerHtml = "<div>&nbsp;</div>"}.AddTo(tr, "channel-desc");
        new HtmlTableCell {InnerHtml = "<div>&nbsp;</div>"}.AddTo(tr, "url-to-use");
      }
    }

    #endregion Private

    #region Event handlers and overrides

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        Page.Title = "Review YouTube Channels";
        H1.InnerHtml = "Review YouTube Channels";
        var unverifiedVideos = Politicians.GetUnverifiedYouTubeVideos();
        if (unverifiedVideos.Count == 0)
        {
          var tr = new HtmlTableRow().AddTo(BodyPlaceHolder);
          new HtmlTableCell
          {
            ColSpan = 6,
            InnerText = "No unverified videos found"
          }.AddTo(tr);
        }
        else BuildTable(unverifiedVideos);
      }
    }

    #endregion Event handlers and overrides
  }
}