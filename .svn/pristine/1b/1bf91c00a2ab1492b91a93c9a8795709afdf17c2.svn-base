using System;
using DB.Vote;

namespace Vote.Master
{
  [PageInitializers]
  public partial class UpdateVolunteersPage : SecurePage, ISuperUser
  {
    private string GetVolunteerToEdit()
    {
      return VolunteerToEdit.Value;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        const string title = "Update Volunteers";
        Page.Title = title;
        H1.InnerHtml = title;

        StateCache.Populate(ReportState, "all states", string.Empty);
        Parties.PopulateNationalParties(ReportParty, true, null, true, "any party");
      }
    }
  }
}