using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vote;

namespace VoteNew
{
  public partial class forCandidates : NewVotePublicPage
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      this.IncludeCss("~/css/MainCommon.css");
      this.IncludeCss("~/css/SecondaryCommon.css");

      EmailForm.ToEmailAddress = "mgr@Vote-USA.org";
      //EmailForm.ToEmailAddress = "curt.loughin@businessol.com";

      EmailForm.SetItems("I am a candidate -- I need my logon credentials and instructions");
    }
  }
}