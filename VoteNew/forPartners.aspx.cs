using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vote;

namespace VoteNew
{
  public partial class forPartners : NewVotePublicPage
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      this.IncludeCss("~/css/MainCommon.css");
      this.IncludeCss("~/css/SecondaryCommon.css");
    }
  }
}