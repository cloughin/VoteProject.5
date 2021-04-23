using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vote.Reports;

namespace Vote.Sandbox
{
  public partial class IssueLinks : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      var start = DateTime.UtcNow; 
      var links = IssuesListLinks.GetReport(//        "VA20121106GA", "USPresident");
        "CA20101102GA", "CAUSSenate");
      var elapsed = DateTime.UtcNow - start; // 8+
      PlaceHolder.Controls.Add(links);
    }
  }
}