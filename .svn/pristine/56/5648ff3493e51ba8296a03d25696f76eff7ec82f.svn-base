using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Vote;

namespace VoteNew
{
  public partial class forVoters : NewVotePublicPage
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      this.IncludeCss("~/css/MainCommon.css");
      this.IncludeCss("~/css/SecondaryCommon.css");

      HtmlGenericControl linkParagraph;
      HtmlAnchor link;

      linkParagraph = new HtmlGenericControl("p");
      NationalOfficeHolderLinks.Controls.Add(linkParagraph);
      link = new HtmlAnchor();
      linkParagraph.Controls.Add(link);
      link.HRef = "#";
      link.InnerText = "President and Vice President";

      linkParagraph = new HtmlGenericControl("p");
      NationalOfficeHolderLinks.Controls.Add(linkParagraph);
      link = new HtmlAnchor();
      linkParagraph.Controls.Add(link);
      link.HRef = "#";
      link.InnerText = "U.S. Senate";

      linkParagraph = new HtmlGenericControl("p");
      NationalOfficeHolderLinks.Controls.Add(linkParagraph);
      link = new HtmlAnchor();
      linkParagraph.Controls.Add(link);
      link.HRef = "#";
      link.InnerText = "U.S. House of Representatives";

      if (DomainData.IsValidStateCode) // Single state
      {
        string stateCode = DomainData.FromQueryStringOrDomain;
        string stateName = StateCache.GetStateName(stateCode);
        StateOfficeHolderTitle.InnerText = 
          string.Format("Current {0} Office Holders", stateName);
      }
      else
      {
        StateOfficeHolderTitle.InnerText = "Current State Office Holders";
      }
    }
  }
}