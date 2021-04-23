using System;

namespace Vote.Master
{
  [PageInitializers]
  public partial class UpdateUsersPage : SecurePage, ISuperUser
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        const string title = "Update Users";
        Page.Title = title;
        H1.InnerHtml = title;

        StateCache.Populate(AddUsersUserStateCode, "<Select a State>", "");
      }
    }
  }
}