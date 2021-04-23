using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Vote;

namespace VoteNew
{
  public partial class Default : NewVotePublicPage
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      this.IncludeCss("~/css/MainCommon.css");
      this.IncludeCss("~/css/DefaultNew.css");

      if (!IsPostBack)
      {
        AddressEntry.SetHeaderImage("/images/getstartedtitle.png");
        if (DomainData.IsValidStateCode) // Single state
        {
          if (DomainDesign.IncludeTitleForSingleState)
            TitleTag.Text =
              DomainDesign.GetSubstitutionText("TitleTagDefaultPageSingleStateDomain");
          if (DomainDesign.IncludeMetaDescriptionForSingleState)
            MetaDescriptionTag.Content =
              DomainDesign.GetSubstitutionText("MetaDescriptionTagDefaultPageSingleStateDomain");
          if (DomainDesign.IncludeMetaKeywordsForSingleState)
            MetaKeywordsTag.Content =
              DomainDesign.GetSubstitutionText("MetaKeywordsTagDefaultPageSingleStateDomain");
        }
        else // use the All states domain
        {
          if (DomainDesign.IncludeTitleForAllStates)
            TitleTag.Text =
              DomainDesign.GetSubstitutionText("TitleTagDefaultPageAllStatesDomain");
          if (DomainDesign.IncludeMetaDescriptionForAllStates)
            MetaDescriptionTag.Content =
              DomainDesign.GetSubstitutionText("MetaDescriptionTagDefaultPageAllStatesDomain");
          if (DomainDesign.IncludeMetaKeywordsForAllStates)
            MetaKeywordsTag.Content =
              DomainDesign.GetSubstitutionText("MetaKeywordsTagDefaultPageAllStatesDomain");
        }
      }
    }
  }
}