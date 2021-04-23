using System;

namespace Vote.County
{
  public partial class Default : VotePage
  {
    #region Notes
    //MASTER, ADMIN and COUNTY users set Session["UserStateCode"] at login
    //MASTER & ADMIN users set Session["UserCountyCode"] via query string
    //COUNTY Administration user sets Session["UserCountyCode"] at login
    //After Session["UserCountyCode"] is set it immediate Redirects to /Admin/Admin.aspx
    #endregion Notes

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        if (
          (string.IsNullOrEmpty(Session["UserStateCode"].ToString()))
          || (string.IsNullOrEmpty(Session["UserCountyCode"].ToString()))
          )
          HandleFatalError("The UserStateCode and/or UserCountyCode is missing");
        else
          //Response.Redirect(db.Url_Admin_Default(
          //  Session["UserStateCode"].ToString()
          //  , Session["UserCountyCode"].ToString()
          //  ));
          Response.Redirect(db.Url_Admin_Default_Login());

        //-------------------
      }
    }
  }
}
