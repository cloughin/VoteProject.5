using System;
using DB.Vote;

namespace Vote.Party
{
  public partial class VideosPage : SecurePartyPage, IAllowEmptyPartyKey
  {
    #region private

    #endregion private

    #region Event handlers and overrides

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        Page.Title = "Instructional Videos";
        H1.InnerHtml = "Instructional Videos";
        var table = InstructionalVideos.GetVolunteersData();
        Admin.VideosPage.FormatVideos(table, VideosPlaceHolder);
      }
    }

    #endregion Event handlers and overrides
  }
}