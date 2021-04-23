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
using System.IO;

namespace Vote
{
  public partial class BannerNew : System.Web.UI.UserControl
  {
    //#region WebControls
    ////protected System.Web.UI.HtmlControls.HtmlTableCell BannerTagLine;
    //#endregion

    ////protected void Page_Load(object sender, EventArgs e)
    //protected void Page_PreRender(object sender, EventArgs e)
    //{
    //  //Moved from Page_Load because if design is changed Page_Load is performed before
    //  //the database is updated with the new design. Page_PreRender is performed after
    //  //the database is updated.
    //  try
    //  {
    //    #region commented out DesignCode
    //    //string DesignCode = string.Empty;

    //    //if (db.Is_Page_Public())
    //    //  DesignCode = db.Domain_DesignCode_This();
    //    //else
    //    //{
    //    //  if (! string.IsNullOrEmpty(db.QueryString("Design")))
    //    //    DesignCode = db.QueryString("Design");//passed for Sample Page from /Master/Default.aspx
    //    //  else
    //    //    DesignCode = "VOTE-USA";
    //    //}

    //    //if (! string.IsNullOrEmpty(db.QueryString("Design")))
    //    //  DesignCode = db.QueryString("Design");//passed for Sample Page from /Master/Default.aspx
    //    //else
    //    //  DesignCode = db.Domain_DesignCode_This();

    //    #region-- debug uncomment
    //    //db.LogDebugWriteLine("Banner.ascx DesignCode: " + DesignCode);
    //    #endregion-- debug uncomment

    //    #endregion commented out DesignCode

    //    try
    //    {
    //      #region tablePage or tableAdmin
    //      if (VotePage.IsPublicPage)
    //        All.Attributes["Class"] = "tablePage";//width 1000px
    //      else
    //        All.Attributes["Class"] = "tableAdmin";//width 700px
    //      #endregion tablePage or tableAdmin

    //      #region Is Banner & Tagline Visible ?
    //      All.Visible = false;

    //      #region-- debug uncomment
    //      //if (db.Is_Include_Banner_This())
    //      //  db.LogDebugWriteLine("Banner.ascx IsIncludedBannerAllPages: true");
    //      //else
    //      //  db.LogDebugWriteLine("Banner.ascx IsIncludedBannerAllPages: false");
    //      //db.LogDebugWriteLine("Banner.ascx DesignCode: " + DesignCode);
    //      //db.LogDebugWriteLine("Banner.ascx PathImageBanner_OrEmpty: " + db.PathImageBanner_OrEmpty(DesignCode));
    //      #endregion-- debug uncomment

    //      if (VotePage.IsPublicPage)
    //    {
    //      if (
    //        (db.Is_Include_Banner_This()
    //        && (db.DesignCode_Domain_This() != string.Empty)
    //        //&& (db.PathImageBanner_OrEmpty(db.DesignCode_Domain_This()) != string.Empty))
    //        && (db.Url_ImageBannerOrEmpty(db.DesignCode_Domain_This()) != string.Empty))
    //        )
    //        All.Visible = true;
    //    }
    //    else
    //      All.Visible = true;//MASTER and DESIGN Pages
    //    #endregion Is Banner & Tagline Visible ?
    //    }
    //    catch (Exception /*ex*/)
    //    {
    //      db.Log_Page_Not_Found_404("Banner.aspx:#1");
    //      throw;
    //    }

    //    if (All.Visible)
    //    {
    //      try
    //      {
    //        #region Visible

    //        #region-- debug uncomment
    //        //db.LogDebugWriteLine("Banner.ascx HyperLinkBanner.ImageUrl: " + db.Url_Image_Banner(DesignCode));
    //        #endregion-- debug uncomment

    //        HyperLinkBanner.ImageUrl = db.Url_ImageBannerOrEmpty(db.DesignCode_Domain_This());
    //        // If we are running on the localhost (127.0.0.1), we set the banner tooltip as confirmation
    //        //if (db.ServerVariables("LOCAL_ADDR") == "127.0.0.1")
    //        //  HyperLinkBanner.ToolTip = "localhost as " + db.ServerVariables("HTTP_HOST");
    //        //HyperLinkBanner.NavigateUrl = db.Url_Vote_XX_org_Default();
    //        if (db.Is_Domain_SingleState())
    //          HyperLinkBanner.NavigateUrl = db.Url_Vote_XX_org_Default();
    //        else
    //          HyperLinkBanner.NavigateUrl = UrlManager.SiteUri.ToString();
    //        HyperLinkBanner.Target = "_self";
    //      }
    //      catch (Exception /*ex*/)
    //      {
    //        db.Log_Page_Not_Found_404("Banner.aspx:#2");
    //        throw;
    //      }

    //      try
    //      {
    //        if (db.Url_ImageTagLineOrEmpty(db.DesignCode_Domain_This()) != string.Empty)
    //        {
    //          HyperLinkBannerTagLine.Visible = true;
    //          HyperLinkBannerTagLine.ImageUrl = db.Url_ImageTagLineOrEmpty(db.DesignCode_Domain_This());
    //          //HyperLinkBannerTagLine.NavigateUrl = 
    //          //db.Http() + db.Url_Home_Organization(db.Domains_OrganizationCode_This());
    //          //HyperLinkBannerTagLine.NavigateUrl = db.Url_Vote_XX_org_Default();
    //          if (db.Is_Domain_SingleState())
    //            HyperLinkBannerTagLine.NavigateUrl = db.Url_Vote_XX_org_Default();
    //          else
    //            HyperLinkBannerTagLine.NavigateUrl = UrlManager.SiteUri.ToString();
    //          HyperLinkBannerTagLine.Target = "_self";

    //        }
    //        else
    //        {
    //          HyperLinkBannerTagLine.Visible = false;
    //        }
    //      }
    //      catch (Exception /*ex*/)
    //      {
    //        db.Log_Page_Not_Found_404("Banner.aspx:#3");
    //        throw;
    //      }

    //      #endregion Visible
    //    }
    //  }
    //  //}
    //  catch (Exception ex)
    //  {
    //    db.Log_Page_Not_Found_404("Banner.aspx:ex.Message" + ex.Message);
    //    if (!VotePage.IsDebugging) VotePage.SafeTransferToError500();
    //  }

    //}
  }
}