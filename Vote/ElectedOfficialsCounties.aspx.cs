using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Vote
{
  public partial class ElectedOfficialsCountiesPage : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      // Obsolete page
      string electionKey = db.ElectionKey_New_Format(VotePage.QueryElection);
      Response.RedirectPermanent("/Officials.aspx?" + Request.QueryString);
    }
  }
}