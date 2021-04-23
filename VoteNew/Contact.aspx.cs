using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vote;

namespace VoteNew
{
  public partial class Contact : NewVotePublicPage
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      this.IncludeCss("~/css/MainCommon.css");
      this.IncludeCss("~/css/SecondaryCommon.css");

      //EmailForm.ToEmailAddress = "info@Vote-USA.org";
      //EmailForm.ToEmailAddress = "curt.loughin@businessol.com";
      EmailForm.ToEmailAddress = "mgr@Vote-USA.org";

      EmailForm.SetItems("Privacy issues", "Donation issues");
    }
  }
}