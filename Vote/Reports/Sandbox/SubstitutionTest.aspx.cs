using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Vote.Sandbox
{
  public partial class SubstitutionTest : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      var sub = new Substitutions()
      {
        StateCode = "AL",
        //PoliticianKey = tempEmailRow.PoliticianKey(),
        ElectionKey = "AL20121106GA",
        OfficeKey = "ALUSHouse4",
        IssueKey = "ALLBio",
        //PartyKey = tempEmailRow.PartyKey(),
        //PartyEmail = tempEmailRow.Email(),
      }.Substitute("##Vote-XX.org/Issue##");
    }
  }
}