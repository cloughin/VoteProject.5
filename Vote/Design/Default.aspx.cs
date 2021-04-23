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

namespace Vote.Admin
{
  public partial class Design : VotePage
  {
    #region Dead code

    //    private void Page_Load(object sender, System.EventArgs e)
//    {
//      if (!IsPostBack)
//      {
//        // A MASTER user enters either Admin/Admin.aspx, Design/Default.aspx, or Admin/Organization.aspx from anchors on Master/Default.aspx
//        //if (
//        //  (db.User() == db.User_.Master) 
//        //  && (! string.IsNullOrEmpty(db.QueryString("Design")))
//        //  )
//        //  Session["UserDesignCode"] = db.QueryString("Design");

//        #region Security Checks
//          if (db.Domain_DesignCode_This() == string.Empty)
//            db.HandleMissingDomainDesignCode();
//        #endregion Security Checks

//        try
//        {
//          LabelDesignCode.Text = db.Domain_DesignCode_This();

//          string QueryParms = "?Design=" + db.Domain_DesignCode_This();
//          QueryParms += "&State=" + db.DomainDesigns_Str_This("DomainDataCode");

//          #region HyperLink Query strings
//          HyperLinkAllPages.NavigateUrl = "/Design/DesignAllPages.aspx"
//                + QueryParms;

//          //if (db.IsDomain_DataCode_Single_State())
//          if (db.IsDomain_DesignCode_Single_State())
//          {
//            HyperLink_Default_Single_State.Enabled = true;
//            HyperLink_Default_Single_State.NavigateUrl = 
//              "/Design/DesignDefaultPage4SingleStateDomain.aspx"
//                + QueryParms;
//            //HyperLink_Default_Single_State.NavigateUrl +=
//            //  "?Design=" + db.QueryString("Design")
//            //  //+ "&State=" + db.StateCode_Domain_This();
//            //  + "&State=" + db.DomainDesigns_Str_This("DomainDataCode");
//            HyperLink_Default_All_States.Enabled = false;
//          }
//          else
//          {
//            HyperLink_Default_All_States.Enabled = true;
//            HyperLink_Default_All_States.NavigateUrl = 
//              "/Design/DesignDefaultPage4AllStatesDomain.aspx"
//                + QueryParms;
//            //HyperLink_Default_All_States.NavigateUrl += 
//            //  "?Design=" + db.QueryString("Design");
//            HyperLink_Default_Single_State.Enabled = false;
//          }

//          HyperLinkBallot.NavigateUrl = "/Design/DesignBallotPage.aspx"
//                + QueryParms;
//          HyperLinkElected.NavigateUrl = "/Design/DesignElectedPage.aspx"
//                + QueryParms;
//          HyperLinkIntro.NavigateUrl = "/Design/DesignIntroPage.aspx"
//                + QueryParms;
//          HyperLinkPoliticianIssue.NavigateUrl = "/Design/DesignPoliticianIssuePage.aspx"
//                + QueryParms;
//          HyperLinkIssue.NavigateUrl = "/Design/DesignIssuePage.aspx"
//                + QueryParms;
//          HyperLinkReferendum.NavigateUrl = "/Design/DesignReferendumPage.aspx"
//                + QueryParms;
//          HyperLinkElection.NavigateUrl = "/Design/DesignElectionPage.aspx"
//                + QueryParms;
//          HyperLinkOfficials.NavigateUrl = "/Design/DesignElectedOfficialsPage.aspx"
//                + QueryParms;
//          HyperLinkAboutUs.NavigateUrl = "/Design/DesignAboutUsPage.aspx"
//                + QueryParms;
//          HyperLinkCandidates.NavigateUrl = "/Design/DesignCandidatesPage.aspx"
//                + QueryParms;
//          HyperLinkContactUs.NavigateUrl = "/Design/DesignContactUsPage.aspx"
//                + QueryParms;
//          HyperLinkInterns.NavigateUrl = "/Design/DesignInternsPage.aspx"
//                + QueryParms;
//          HyperLinkVoters.NavigateUrl = "/Design/DesignVotersPage.aspx"
//                + QueryParms;
//          HyperLinkArchives.NavigateUrl = "/Design/DesignArchivesPage.aspx"
//                + QueryParms;

//          //if (!string.IsNullOrEmpty(db.QueryString("Design")))
//          //{
//          //  HyperLinkAllPages.NavigateUrl += "?Design=" + db.QueryString("Design");

//          //  HyperLinkBallot.NavigateUrl += "?Design=" + db.QueryString("Design");
//          //  HyperLinkElected.NavigateUrl += "?Design=" + db.QueryString("Design");
//          //  HyperLinkIntro.NavigateUrl += "?Design=" + db.QueryString("Design");
//          //  HyperLinkPoliticianIssue.NavigateUrl += "?Design=" + db.QueryString("Design");
//          //  HyperLinkIssue.NavigateUrl += "?Design=" + db.QueryString("Design");
//          //  HyperLinkReferendum.NavigateUrl += "?Design=" + db.QueryString("Design");
//          //  HyperLinkElection.NavigateUrl += "?Design=" + db.QueryString("Design");
//          //  HyperLinkOfficials.NavigateUrl += "?Design=" + db.QueryString("Design");
//          //  HyperLinkAboutUs.NavigateUrl += "?Design=" + db.QueryString("Design");
//          //  HyperLinkCandidates.NavigateUrl += "?Design=" + db.QueryString("Design");
//          //  HyperLinkContactUs.NavigateUrl += "?Design=" + db.QueryString("Design");
//          //  HyperLinkInterns.NavigateUrl += "?Design=" + db.QueryString("Design");
//          //  HyperLinkVoters.NavigateUrl += "?Design=" + db.QueryString("Design");
//          //  HyperLinkArchives.NavigateUrl += "?Design=" + db.QueryString("Design");
//          //}
//          #endregion HyperLink Query strings

//          #region Hide Button 'Default.aspx Before State Selection' if domain for a single state
//          if (db.User() == db.User_.Master)
//          {
//            //if (db.Is_StateCode_State(DomanDataCode))
//            if (db.IsDomain_DataCode_Single_State())
//              trNoStateSelected.Visible = false;
//            else
//              trNoStateSelected.Visible = true;
//          }
//          else //ADMIN
//          {
//            if (db.Is_Domain_SingleState())
//            {
//              trNoStateSelected.Visible = false;
//              //db.LogDebugWriteLine("trNoStateSelected -false");
//            }
//            else
//            {
//              trNoStateSelected.Visible = true;
//              //db.LogDebugWriteLine("trNoStateSelected -true");
//            }
//          }

//#endregion Hide Button 'Default.aspx Befor State Selection' if domain for a single state

//          #region Controls for MASTER user
//          if (db.User() == db.User_.Master)
//          {
//            TableCache_Remove.Visible = true;
//          }
//          else
//          {
//            TableCache_Remove.Visible = false;
//          }
//          #endregion Controls for MASTER user
//        }
//        catch (Exception ex)
//        {
//          #region
//          Msg.Text = db.Fail(ex.Message);
//          db.Log_Error_Admin(ex);
//          #endregion
//        }
//      }
//    }

//    protected void ButtonDeleteCachePages_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        //db.Cache_Remove_Domain_Design(db.Domain_DesignCode_This());
//        //Msg.Text = db.Ok("All Cached Pages have been cleared for this domain design code.");
//      }
//      catch (Exception ex)
//      {
//        #region
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//        #endregion
//      }
//    }

    #endregion Dead code

  }
}
