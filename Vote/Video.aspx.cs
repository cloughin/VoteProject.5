using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Vote
{
  public partial class VideoPage : PublicPage
  {
    public VideoPage()
    {
      NoUrlEdit = true;
      //NoIndex = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      this.IncludeCss("~/css/MainCommon.css");
      this.IncludeCss("~/css/SecondaryCommon.css");

      introBio.Visible = false;
      compareMdPrimary.Visible = false;
      if (GetQueryString("video") == "introBio")
      {
        introBio.Visible = true;
        LabelVideoDesc.Text = "This 15 minute video shows how to capture Candidates' biographical information"
        + ", campaign website, social media links for their Introduction Page."
        + " Other videos are availabe to capture candidates' pictures and their views and positions on issues.";
      }

      else if (GetQueryString("video") == "compareMdPrimary")
      {
        compareMdPrimary.Visible = true;
        LabelVideoDesc.Text = "This 25 minute video compares the information available for the April 3, 2012 Maryland Primary"
        + " of the primary websites that provide candidate information.";
      }

      //compareMdPrimary

    }
  }
}