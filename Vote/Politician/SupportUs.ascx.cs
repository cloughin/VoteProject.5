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

namespace Vote.Politician
{
  public partial class SupportUs : System.Web.UI.UserControl
  {
    protected void Page_Load(object sender, EventArgs e)
    {
    //  try
    //  {
    //    //if (!IsPostBack)
    //    //{
    //    //if (db.DomainsThis("DomainCode") == "US")
    //    All.Visible = true;
    //    //else
    //    //  All.Visible = false;
    //  }
    //  //}
    //  catch (Exception ex)
    //  {
    //    db.Log_Error_Admin(ex,"In Politician.SupportUs.aspx");
    //  }
      DonateLink.HRef = VotePage.DonateUrl;
      DonateLink2.HRef = VotePage.DonateUrl;
      DonateLink3.HRef = VotePage.DonateUrl;
    }
  }
}