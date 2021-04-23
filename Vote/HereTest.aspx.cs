using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Vote
{
  public partial class HereTestPage : PublicPage
  {
    protected HereTestPage()
    {
      NoUrlEdit = true;
      NoIndex = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      Title = "Here.com Evaluation";
    }
  }
}