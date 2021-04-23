using System;
using System.Globalization;
using System.Linq;
using DB.Vote;
using static System.String;

namespace Vote.Master
{
  public partial class VideosPage : SecurePage, ISuperUser
  {
    #region private

    private void PopulateVideosList()
    {
      var videos = InstructionalVideos.GetAllData();
      AllVideos.InnerHtml = Join(Empty,
        videos
          .OrderBy(v => v.Title, StringComparer.OrdinalIgnoreCase)
          .Select(v =>
          {
            var p = new HtmlP {InnerText = v.Title};
            p.Attributes.Add("data-id", v.Id.ToString(CultureInfo.InvariantCulture));
            p.Attributes.Add("data-url", v.Url);
            p.Attributes.Add("data-description", v.Description);
            p.Attributes.Add("data-embedcode", v.EmbedCode);
            return p.RenderToString();
          }));
      AdminVideos.InnerHtml = Join(Empty,
        videos
          .Where(v => v.AdminOrder != 0)
          .OrderBy(v => v.AdminOrder)
          .Select(v =>
          {
            var p = new HtmlP {InnerText = v.Title};
            p.Attributes.Add("data-id", v.Id.ToString(CultureInfo.InvariantCulture));
            return p.RenderToString();
          }));
      VolunteersVideos.InnerHtml = Join(Empty,
        videos
          .Where(v => v.VolunteersOrder != 0)
          .OrderBy(v => v.VolunteersOrder)
          .Select(v =>
          {
            var p = new HtmlP {InnerText = v.Title};
            p.Attributes.Add("data-id", v.Id.ToString(CultureInfo.InvariantCulture));
            return p.RenderToString();
          }));
    }

    #endregion private

    #region Event handlers and overrides

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        Page.Title = "Instructional Videos Maintenance";
        H1.InnerHtml = "Instructional Videos Maintenance";
        PopulateVideosList();
      }
    }

    #endregion Event handlers and overrides
  }
}