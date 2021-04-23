using System;

namespace Vote.Politician
{
  public partial class DefaultPage : VotePage
  {
    #region Dead code

    protected override void OnPreInit(EventArgs e)
    {
      //if (string.IsNullOrWhiteSpace(Request.QueryString["noredirect"]))
      {
        var query = Request.QueryString.ToString();
        var url = "/politician/main.aspx";
        if (!string.IsNullOrWhiteSpace(query))
          url += "?" + query;
        Response.Redirect(url);
      }
      base.OnPreInit(e);
    }

    //private void Page_Load(object sender, System.EventArgs e)
    //{
    //  if (!IsPostBack)
    //  {
    //    #region ViewState["PoliticianKey"]
    //    string politicianKey = db.PoliticianKey_ViewState();
    //    ViewState["PoliticianKey"] = politicianKey;
    //    #endregion ViewState["PoliticianKey"]

    //    #region Redirect if Bad Entry
    //    if (ViewState["PoliticianKey"].ToString() == string.Empty)
    //      db.HandleFatalError("The PoliticianKey is missing");
    //    #endregion

    //    try
    //    {
    //      //Set OfficeKey and StateCode for all other pages
    //      ViewState["OfficeKey"] = 
    //        PageCache.GetTemporary().Politicians.GetOfficeKey(politicianKey);
    //      //for Search Engine Submission of Issue Questions
    //      //ViewState["IssueKey"] = db.Issues_List_Office(ViewState["OfficeKey"].ToString());
    //      //ViewState["IssueKey"] = "ALLBio";

    //      //SiteCss.Attributes["HREF"] = db.PathStyleSheetCustomGet();// Set Custom Style Sheet if it exists at: /css/DomainDesignCode.css

    //      //LabelTitle.Text = db.Name_Office_Election_Politician(politicianKey);
    //      //PoliticianName.Text = db.Politician_Name(politicianKey);
    //      //PoliticianOffice.Text = db.Politician_Current_Office_And_Status(politicianKey);
    //      //PoliticianElection.Text = db.Politician_Current_Election(politicianKey);
    //      PoliticianName.Text = db.GetPoliticianName(politicianKey);
    //      PoliticianOffice.Text = db.Politician_Current_Office_And_Status(politicianKey);
    //      PoliticianElection.Text = db.Politician_Current_Election(politicianKey);

    //      #region Photo
    //      CandidateImage.ImageUrl = InsertNoCacheIntoUrl(
    //        db.Url_Image_Politician_Or_NoPhoto(politicianKey, db.Image_Size_200_Profile, 
    //        db.Image_Size_200_Profile));
    //      #endregion Photo

    //      #region HyperLinks - Introduce Yourself

    //      #region Add, Change or Delete Your Introduction Page Content and Picture
    //      //PoliticianKey is username of politician
    //      HyperLinkEnterIntro.NavigateUrl
    //        = db.Url_Politician_Intro();
    //      #endregion Add, Change or Delete Your Introduction Page Content and Picture

    //      #region View Your Introduction Page that We Provide to Voters
    //      HyperLinkViewIntro.NavigateUrl = InsertNoCacheIntoUrl(
    //        UrlManager.GetIntroPageUri(politicianKey).ToString());
    //      #endregion View Your Introduction Page that We Provide to Voters

    //      #region Submit Your Introduction Page to the Google Search Engine
    //      HyperLinkSearchEngineIntroPage.NavigateUrl 
    //        = db.Url_Politician_SearchEngineSubmit_Intro_Page();
    //      #endregion Submit Your Introduction Page to the Google Search Engine

    //      #endregion HyperLinks - Introduce Yourself

    //      #region HyperLinks - Provide Your Views and Positions on Issues

    //      #region Add, Change or Delete Your Views and Positions on Issues
    //      HyperLinkEnterViews.NavigateUrl
    //        = db.Url_Politician_IssueQuestions(
    //            "ALLPersonal"
    //            //"ALLBio"
    //        //, politicianKey
    //            );
    //      #endregion Add, Change or Delete Your Views and Positions on Issues

    //      #region View the Pages of Your Positions and Views that We Provide to Voters
    //      HyperlinkViewPositions.NavigateUrl =
    //        db.Url_Issue_Or_PoliticianIssue(
    //        //ViewState["IssueKey"].ToString()
    //        "ALLBio"
    //        , politicianKey
    //        );
    //      #endregion View the Pages of Your Positions and Views that We Provide to Voters

    //      #region Submit Your Pages of Positions and Views to the Google Search Engine
    //      HyperLinkSearchEngineComparePage.NavigateUrl
    //        = db.Url_Politician_SearchEngineSubmitCompare(
    //        politicianKey
    //        //, ViewState["IssueKey"].ToString()
    //        , "ALLBio"
    //        );
    //      #endregion Submit Your Pages of Positions and Views to the Google Search Engine

    //      #endregion HyperLinks - Provide Your Views and Positions on Issues

    //    }

    //    catch (Exception ex)
    //    {
    //      #region
    //      Msg.Text = db.Fail(ex.Message);
    //      db.Log_Error_Admin(ex);
    //      #endregion
    //    }
    //  }
    //}

    #endregion Dead code
  }
}