using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vote;

namespace VoteNew.Controls
{
  public partial class MissionSidebar : System.Web.UI.UserControl
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        if (DomainData.IsValidStateCode) // Single state
          MissionSidebarPullQuote.Visible = false;
        else // use the All states domain
          MissionSidebarForVoters.Visible = false;
      }
    }
  }
}