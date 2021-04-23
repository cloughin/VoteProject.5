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

namespace Vote.LocalDistrict
{
  public partial class Default : VotePage
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        if (
          (string.IsNullOrEmpty(Session["UserStateCode"].ToString()))
          || (string.IsNullOrEmpty(Session["UserCountyCode"].ToString()))
          || (string.IsNullOrEmpty(Session["UserLocalCode"].ToString()))
          )
          HandleFatalError("The UserStateCode and/or UserCountyCode and/or UserLocalCode is missing");
        else
          Response.Redirect(db.Url_Admin_Default_Login());
            //Session["UserStateCode"].ToString()
            //, Session["UserCountyCode"].ToString()
            //, Session["UserLocalCode"].ToString()
            //));
      }
    }
  }
}
