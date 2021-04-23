using System;
using System.Web.UI;

namespace Vote.Controls
{
  public partial class MissionSidebar : UserControl
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
        //if (DomainData.IsValidStateCode) // Single state
        //  MissionSidebarPullQuote.Visible = false;
        //else // use the All states domain
        MissionSidebarForVoters.Visible = false;
    }
  }
}