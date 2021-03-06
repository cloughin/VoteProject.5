using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Vote
{
  public partial class SupportUs : System.Web.UI.UserControl
  {
    //protected void Page_Load(object sender, EventArgs e)
    protected void Page_PreRender(object sender, EventArgs e)
    {
      //Moved from Page_Load because if design is changes Page_Load is performed before
      //the database is updated with the new design. Page_PreRender is performed after
      //the database is updated.
      try
      {
        //if (!IsPostBack)
        //{
        #region Is Support Us Visible ?
        All.Visible = false;
        if (VotePage.IsPublicPage)
        {
          if (db.Is_Include_Donate_This())
            All.Visible = true;
        }
        else
          All.Visible = true;//MASTER and DESIGN Pages
        #endregion Is Support Us Visible ?

        if (All.Visible)
        {
          if (VotePage.IsPublicPage)
            All.Attributes["class"] = "tablePage";
          else
            All.Attributes["class"] = "tableAdmin";
        }
      }
      //}
      catch (Exception ex)
      {
        db.Log_Page_Not_Found_404("SupportUs.aspx:ex.Message" + ex.Message);
        if (!VotePage.IsDebugging) VotePage.SafeTransferToError500();
      }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      DonateLink.HRef = VotePage.DonateUrl;
      DonateLink2.HRef = VotePage.DonateUrl;
      DonateLink3.HRef = VotePage.DonateUrl;
    }
  }
}