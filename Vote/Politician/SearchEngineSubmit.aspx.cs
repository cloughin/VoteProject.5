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
using DB.Vote;

namespace Vote.Politician
{
  public partial class SearchEngineSubmit1 : VotePage
  {
    //private void Page_Load(object sender, System.EventArgs e)
    //{
    //  if (!IsPostBack)
    //  {
    //    #region ViewState["PoliticianKey"] ViewState["Page2Submit"] ViewState["IssueKey"]
    //    //string politicianKey = db.PoliticianKey_ViewState();
    //    //ViewState["PoliticianKey"] = politicianKey;
    //    ViewState["PoliticianKey"] = db.PoliticianKey_ViewState();

    //    ViewState["Page2Submit"] = "Intro";
    //    if (!string.IsNullOrEmpty(GetQueryString("Page")))
    //      ViewState["Page2Submit"] = GetQueryString("Page");

    //    ViewState["IssueKey"] = string.Empty;
    //    if (!string.IsNullOrEmpty(QueryIssue))
    //      ViewState["IssueKey"] = QueryIssue;
    //    #endregion ViewState["PoliticianKey"] ViewState["Page2Submit"] ViewState["IssueKey"]

    //    #region Redirect if Bad Entry
    //    if (ViewState["PoliticianKey"].ToString() == string.Empty)
    //      HandleFatalError("The PoliticianKey is missing");

    //    if (
    //      (ViewState["Page2Submit"].ToString() != "Intro")
    //      && (string.IsNullOrEmpty(ViewState["IssueKey"].ToString()))
    //      )
    //    {
    //      //Issue.aspx and PoliticianIssue.aspx
    //      HandleFatalError("The IssueKey is missing");
    //    }
    //    #endregion
    //    try
    //    {
    //      #region Politician Name, Office, Election
    //      PoliticianName.Text =
    //        Politicians.GetFormattedName(ViewState["PoliticianKey"].ToString());
    //      PoliticianOffice.Text = 
    //        db.Politician_Current_Office_And_Status(ViewState["PoliticianKey"].ToString());
    //      PoliticianElection.Text = 
    //        db.Politician_Current_Election(ViewState["PoliticianKey"].ToString());
    //      #endregion Politician Name, Office, Election

    //      #region Submission Url TextBox
    //      if (ViewState["Page2Submit"].ToString() == "Intro")
    //      {
    //        TextBoxURL.Text =  UrlManager.GetIntroPageUri(ViewState["PoliticianKey"].ToString()).ToString();
    //      }
    //      else if (ViewState["Page2Submit"].ToString() == "Issue")
    //      {
    //        TextBoxURL.Text = db.Url_Issue_Or_PoliticianIssue(
    //          ViewState["IssueKey"].ToString()
    //          , ViewState["PoliticianKey"].ToString());
    //      }
    //      else if (ViewState["Page2Submit"].ToString() == "PoliticianIssue")
    //      {
    //        TextBoxURL.Text = db.Url_Issue_Or_PoliticianIssue(
    //          ViewState["IssueKey"].ToString()
    //          , ViewState["PoliticianKey"].ToString());
    //      }
    //      #endregion Submission URL TextBox

    //      //Javascripts to Launch a new page to View Page Changes / or submit to search engines
    //      //ButtonSearchEngineSubmit.NavigateUrl = "javascript:postGooglePage()";

    //      #region View the Page You are Submitting
    //      if (ViewState["Page2Submit"].ToString() == "Intro")
    //        HyperLinkViewPageSubmitted.NavigateUrl = 
    //          UrlManager.GetIntroPageUri(
    //            ViewState["PoliticianKey"].ToString()).ToString();
    //      else//Issue.aspx
    //        HyperLinkViewPageSubmitted.NavigateUrl = 
    //          db.Url_Issue_Or_PoliticianIssue(
    //            ViewState["IssueKey"].ToString()
    //            , ViewState["PoliticianKey"].ToString());
    //      #endregion View the Page You are Submitting

    //      #region Add, Change or Delete Your Introduction Page Content and Picture
    //      //PoliticianKey is username of politician
    //      HyperLinkIntroPage.NavigateUrl =
    //          db.Url_Politician_Intro();
    //      //ViewState["PoliticianKey"].ToString()
    //      #endregion Add, Change or Delete Your Introduction Page Content and Picture

    //      #region Enter Your Views and Positions on Issues
    //      //if (ViewState["Page2Submit"].ToString() == "Intro")
    //      //  HyperLinkMoreIssues.NavigateUrl = 
    //      //    UrlManager.GetIntroPageUri(ViewState["PoliticianKey"].ToString()).ToString();
    //      //else//Issue.aspx
    //      string IssueKey = "ALLPersonal";
    //      //string IssueKey = "ALLBio";
    //      if (!string.IsNullOrEmpty(ViewState["IssueKey"].ToString()))
    //        IssueKey = ViewState["IssueKey"].ToString();

    //        HyperLinkMoreIssues.NavigateUrl 
    //          = db.Url_Politician_IssueQuestions(
    //          IssueKey
    //          );
    //      #endregion Enter Your Views and Positions on Issues

    //      Msg.Text = db.Msg("Follow the instructions below to submit the page defined by the URL"
    //      + " in the text box to the Google Search Engine.");

    //    }
    //    catch (Exception ex)
    //    {
    //      Msg.Text = db.Fail(ex.Message);
    //      db.Log_Error_Admin(ex);
    //    }
    //  }
    //}
  }
}
