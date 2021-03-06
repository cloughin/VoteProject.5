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

namespace Vote
{
  public partial class NavBarAdmin : System.Web.UI.UserControl
  {
    //#region Notes
    ////1)
    ////Moved from Page_Load because if design is changes Page_Load is performed before
    ////the database is updated with the new design. Page_PreRender is performed after
    ////the database is updated.
    ////2)
    ////Anchors are constructed using this concept
    ////If Session["UserSecurity"] = "MASTER" all data can be maintained; 
    ////        Session[UserStateCode"] = empty
    ////        Session[UserCountyCode"] = empty
    ////        Session["UserLocalCode"] = empty
    ////If Session["UserSecurity"] = "ADMIN" only one State can be maintained; 
    ////        Session[UserStateCode"] = StateCode
    ////        Session[UserCountyCode"] = empty
    ////        Session["UserLocalCode"] = empty
    ////If Session["UserSecurity"] = "COUNTY" only one County can be maintained; 
    ////        Session[UserStateCode"] = StateCode
    ////        Session[UserCountyCode"] = CountyCode
    ////        Session["UserLocalCode"] = empty
    ////If Session["UserSecurity"] = ""LOCAL" only one Local Districtc an be maintained; 
    ////        Session[UserStateCode"] = StateCode
    ////        Session[UserCountyCode"] = CountyCode
    ////        Session["UserLocalCode"] = LocalCode
    //#endregion Notes
    ////----------------
    //private void Navbar_Anchor_Top(ref HtmlTableRow Tr_Top_Navbar, string Anchor)
    //{
    //  db.Add_Td_To_Tr(Tr_Top_Navbar, Anchor, "tdNavbarAdminTop", "center");
    //}
    //private void Navbar_Anchor_Bottom(ref HtmlTableRow Tr_Bottom_Navbar, string Anchor)
    //{
    //  db.Add_Td_To_Tr(Tr_Bottom_Navbar, Anchor, "tdNavbarAdminBottom", "center");
    //}

    //#region Master Anchors
    //private string Anchor_Master_Default()//Master
    //{
    //  return db.Anchor(
    //    db.Url_Master_Default()
    //    , "Master"
    //    , "Master Administration for All States");
    //}

    //#region commented
    ////private string Anchor_Master_USDefaultUSPres()//US President
    ////{
    ////  return db.Anchor(
    ////    db.Url_Master_USDefault("U1")
    ////    , "US President"
    ////    , "US President");
    ////}
    ////private string Anchor_Master_USDefaultUSSenate()//US Senate
    ////{
    ////  return db.Anchor(
    ////    db.Url_Master_USDefault("U2")
    ////    , "US Senate"
    ////    , "US Senate");
    ////}
    ////private string Anchor_Master_USDefaultUSHouse()//US House
    ////{
    ////  return db.Anchor(
    ////    db.Url_Master_USDefault("U3")
    ////    , "US House"
    ////    , "US House");
    ////}
    ////private string Anchor_Master_USDefaultGovernors()//US House
    ////{
    ////  return db.Anchor(
    ////    db.Url_Master_USDefault("U4")
    ////    , "State Governors"
    ////    , "State Governors");
    ////}
    //#endregion commented

    //#endregion

    //#region Admin Anchors for State, County and Local

    //#region Notes:
    ////All links have NO query strings for StateCode, CountyCode or LocalCode.
    ////this allows lower level administrators to reset to its higher level and hides query string parameters.
    //#endregion Notes:


    //#region Utilities

    //private string DataToolTipAppend()
    //{
    //  switch (db.Electoral_Class(
    //    db.State_Code()
    //  , db.User_CountyCode()
    //  , db.User_LocalCode()
    //    )
    //    )
    //  {
    //    case db.ElectoralClass.State:
    //      return "and its counties and local districts";
    //    case db.ElectoralClass.County:
    //      return "and its local districts.";
    //    default:
    //      return string.Empty;
    //  }
    //}
    //private string AnchorToolTip(string DataType)
    //{
    //  return db.Name_Electoral()
    //    + " " + DataToolTipAppend()
    //    + " " + DataType;
    //}
    //#endregion Utilities

    //#region Default Navbar
    ////State, County or Local Default
    //private string Anchor_Admin_Default(string State_Code, string County_Code, string Local_Code)
    //{
    //  #region note
    //  //1st, 2nd, and 3rd Anchors on Admin Navbar for /Admin pages 
    //  //provides the query strings to /Admin/Default.aspx 
    //  //which then resets the user security back to the login security.
    //  #endregion note
    //  string Anchor_Text = Offices.GetElectoralClassDescription(
    //        State_Code
    //        , County_Code
    //        , Local_Code);

    //  //string Tool_Tip = "Elections, Elected Officials, Elected Office and Politicians for "
    //  //  + db.Name_Electoral(
    //  //      State_Code
    //  //      , County_Code
    //  //      , Local_Code
    //  //      , true);

    //  switch (db.Electoral_Class(
    //    State_Code
    //    , County_Code
    //    , Local_Code
    //    ))
    //  {
    //    case db.ElectoralClass.State:
    //      Anchor_Text += " and Its Counties";
    //      //Tool_Tip += " and Its Counties";
    //      break;
    //    case db.ElectoralClass.County:
    //      Anchor_Text += " and Its Local Districts";
    //      //Tool_Tip += " and Its Local Districts";
    //      break;
    //    case db.ElectoralClass.Local:
    //      break;
    //  }

    //  return db.Anchor_Admin_Default(
    //    State_Code
    //    , County_Code
    //    , Local_Code
    //    , db.Center(Anchor_Text)
    //    //, Tool_Tip
    //    );
    //}
    //private string Anchor_Admin_Default(string State_Code, string County_Code)
    //{
    //  return Anchor_Admin_Default(State_Code, County_Code, string.Empty);
    //}
    //private string Anchor_Admin_Default(string State_Code)
    //{
    //  return Anchor_Admin_Default(State_Code, string.Empty, string.Empty);
    //}

    //private string Anchor_Admin_Elections()//Elections
    //{
    //  return db.Anchor(
    //    db.Url_Admin_Elections()
    //    //, db.Name_Electoral_Plus_Text_Elections_Center()
    //  , db.Center(db.Name_Electoral_Plus_Text(
    //      " Elections"))
    //  , AnchorToolTip("administration of all upcoming and previous elections")
    //  , "_self"
    //  );
    //}
    ////private string Anchor_Admin_Offices()//Elected Offices
    //private string Anchor_Admin_Offices(string StateCode)//Elected Offices
    //{
    //  return db.Anchor(
    //    //db.Url_Admin_Offices()
    //    db.Url_Admin_Offices(OfficeClass.All.ToInt(), StateCode)
    //    //, db.Name_Electoral_Plus_Text_Offices_Center()
    //  , db.Center(db.Name_Electoral_Plus_Text(
    //      " Elected Offices"))
    //  , AnchorToolTip("administration of all elected offices")
    //  , "_offices"
    //  );
    //}
    //private string Anchor_Admin_Politicians(string StateCode)//Politicians
    //{
    //  return db.Anchor(
    //    //db.Url_Admin_Politicians()
    //    db.Url_Admin_Politicians(OfficeClass.All, StateCode)
    //    //, db.Name_Electoral_Plus_Text_Politicians_Center()
    //  , db.Center(db.Name_Electoral_Plus_Text(
    //      " Politicians"))
    //  , AnchorToolTip("add and edit politicians independent of any election")
    //  , "_politicians"
    //  );
    //}
    //private string Anchor_Admin_Officials()//Elected Officials
    //{
    //  return db.Anchor(
    //    db.Url_Admin_Officials()
    //    //, db.Name_Electoral_Plus_Text_Officials_Center()
    //  , db.Center(db.Name_Electoral_Plus_Text(
    //      " Elected Officials"))
    //  , AnchorToolTip("report to easily enter the results on an election and identify the currently elected officials (incumbents)")
    //  , "_self"
    //  );
    //}
    //private string Anchor_Admin_Authority()//Election Authority
    //{
    //  return db.Anchor(
    //    db.Url_Admin_Authority()
    //  //, db.Center(db.Name_Electoral(
    //  //    db.State_Code()
    //  //    , string.Empty
    //  //    , string.Empty, true
    //  //    )
    //  //    + " Election Authority")
    //  , db.Center("Election Authority")
    //  , Offices.GetElectoralClassDescription(
    //      db.State_Code())
    //    + " add and edit information about the election authority"
    //  , "_edit"
    //  );
    //}
    //#endregion Default Navbar

    //#region Election NavBar
    //private string Anchor_Admin_Election()//Election Home
    //{
    //  string ElectionKey = VotePage.QueryElection;
    //  //DateTime ElectionDate = db.Election_Date(db.ElectionKey_State(ElectionKey), "ElectionDate");
    //  return db.Anchor(
    //    db.Url_Admin_Election(ElectionKey)
    //    //, db.Name_Electoral_Plus_Text_Election_Center()
    //    , db.Center(db.Name_Electoral_Plus_Text(
    //        "This "
    //        , " Election"
    //        ))
    //    , db.Name_Electoral()
    //        + " main form for "
    //        + db.Elections4Navbar(db.ElectionKey_State(ElectionKey), "ElectionDesc")
    //    , "_self"
    //    );
    //}

    //private string Anchor_Admin_ElectionOffices()//Specific Office Contests
    //{
    //  string ElectionKey = VotePage.QueryElection;
    //  return db.Anchor(
    //    db.Url_Admin_Election_Offices(ElectionKey)
    //    //, db.Name_Electoral_Plus_Text_ElectionOffices_Center()
    //    , db.Center(db.Name_Electoral_Plus_Text(
    //        "Offices Contests in this "
    //        , " Election"))
    //    , db.Name_Electoral()
    //        + " specific office contests in each office category in "
    //        + db.Elections4Navbar(db.ElectionKey_State(ElectionKey), "ElectionDesc")
    //    , "_self"
    //    );
    //}
    //private string Anchor_Admin_Referendums()//Election Referendums
    //{
    //  string ElectionKey = VotePage.QueryElection;
    //  return db.Anchor(
    //    db.Ur4AdminReferendums(ElectionKey)
    //    //, db.Name_Electoral_Plus_Text_Referendum_Center()
    //    , db.Center(db.Name_Electoral_Plus_Text(
    //        "Referendumsin this "
    //        , " Election"))
    //    , db.Name_Electoral()
    //        + " referendums in "
    //        + db.Elections4Navbar(db.ElectionKey_State(ElectionKey), "ElectionDesc")
    //    , "_self"
    //    );
    //}

    //private string Anchor_Admin_ElectionResults()//Election Results
    //{
    //  return db.Anchor(
    //    db.Url_Admin_Officials()
    //    //, db.Name_Electoral_Plus_Text_ElectionResults_Center()
    //  , db.Center(db.Name_Electoral_Plus_Text(
    //      "Enter "
    //      , " Election Results"))
    //  , AnchorToolTip("report to easily enter the results on an election")
    //  , "_self"
    //  );
    //}
    ////------------------
    //#endregion Election NavBar

    //#endregion Admin Anchors for State, County and Local

    //#region Organization Anchors
    //#endregion Organization Anchors

    //#region Politician Anchors
    ////Home
    //private string Anchor_Politician_Default()
    //{
    //  return db.Anchor(
    //    //db.Url_Politician_Default(db.PoliticianKey4QueryString())
    //    //db.Url_Politician_Default(db.User_Name())
    //    //db.Url_Politician_Default(db.Politician_Id_Add_To_QueryString_For_Master_User())
    //    db.Url_Politician_Default()
    //    , db.Center("Candidate Home")
    //    , "Candidate home page of main services we provide"
    //    );

    //}
    ////Add, Change or Delete Your Introduction Page Content and Picture
    //private string Anchor_Politician_IntroPage()
    //{
    //  return db.Anchor(
    //    db.Url_Politician_Intro(db.PoliticianKey_ViewState())
    //    //UrlManager.GetIntroPageUri(db.PoliticianKey_ViewState()).ToString()
    //    , db.Center("Enter Your Introduction Information")
    //    , "Enter information and picture for your introduction page"
    //    ,"_intro"
    //    );
    //}
    ////View Your Introduction Page that We Provide to Voters
    //private string Anchor_Politician_Intro()
    //{
    //  return db.Anchor(
    //    UrlManager.GetIntroPageUri(db.PoliticianKey_ViewState()),
    //    db.Center("View Your Introduction Page"),
    //    "View Your Introduction Page that voters will see",
    //    "view");
    //}
    ////Submit Your Introduction Page to the Google Search Engine
    //private string Anchor_Politician_SearchEngineSubmitIntro()
    //{
    //  return db.Anchor(
    //    db.Url_Politician_SearchEngineSubmit_Intro_Page()
    //   , db.Center("Submit Intro to Google")
    //   , "Submit your Introduction Page to Google Search Engine"
    //   );
    //}

    ////Add, Change or Delete Your Views and Positions on Issues
    //private string Anchor_Politician_IssueQuestions()
    //{
    //  if (
    //    (VotePage.QueryIssue == string.Empty)
    //    || (VotePage.QueryIssue ==
    //          //db.Issues_List_Office(db.Politicians_Str(db.PoliticianKey_ViewState(), "OfficeKey")))
    //          db.Issues_List_Office(VotePage.GetPageCache().Politicians.GetOfficeKey(db.PoliticianKey_ViewState())))
    //    )
    //  {
    //    return db.Anchor(
    //      db.Url_Politician_IssueQuestions("ALLPersonal")
    //      //db.Url_Politician_IssueQuestions("ALLBio")
    //      , db.Center("Enter Your Issue Views")
    //      , "Enter your views and positions on various issues"
    //      );
    //  }
    //  else
    //  {
    //    return db.Anchor(
    //      db.Url_Politician_IssueQuestions(VotePage.QueryIssue)
    //      , db.Center("Enter Your Issue Views")
    //      , "Provide additional responses to issues");
    //      //+ db.Issues_Issue(db.QueryString("Issue")) + " Issue");
    //  }
    //}
    ////View the Pages of Your Positions and Views that We Provide to Voters
    //private string Anchor_Politician_Issue()
    //{
    //  string IssueKey = "ALLPersonal";
    //  //string IssueKey = "ALLBio";
    //  if (VotePage.QueryIssue != string.Empty)
    //    IssueKey = VotePage.QueryIssue;

    //  return db.Anchor(
    //    db.Url_Issue_Or_PoliticianIssue(
    //          IssueKey
    //          , db.PoliticianKey_ViewState())
    //     , db.Center("View Position Pages")
    //     , "View page comparing your positions with other candidates that voters will see."
    //        , "view"
    //     );
    //}

    ////Submit Your Pages of Positions and Views to the Google Search Engine
    //private string Anchor_Politician_SearchEngineSubmitIntroCompare()
    //{
    //  string IssueKey = "ALLPersonal";
    //  //string IssueKey = "ALLBio";
    //  if (VotePage.QueryIssue != string.Empty)
    //    IssueKey = VotePage.QueryIssue;

    //  return db.Anchor(
    //    db.Url_Politician_SearchEngineSubmitCompare(
    //        db.PoliticianKey_ViewState()
    //        , IssueKey
    //        )
    //    , db.Center("Submit Positions to Google ")
    //    , "Submit Your Positions Page to Google Search Engine");
    //}
    //#endregion


    //protected void Page_Load(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    //NavBarTop.Visible = true;

    //    //string PageInLowerCase = db.SCRIPT_NAME().ToLower();

    //    #region commented out Security Folder level
    //    //db.FolderLevel FolderLevel = db.FolderLevel.Public;
    //    //if (db.Is_Page_In_Admin_Folder())
    //    //  FolderLevel = db.FolderLevel.Admin;
    //    //if (db.Is_Page_In_Design_Folder())
    //    //  FolderLevel = db.FolderLevel.Design;
    //    //else if (db.Is_Page_In_Politician_Folder())
    //    //  FolderLevel = db.FolderLevel.Politician;
    //    //else if (db.Is_Page_Master_Folder())
    //    //  FolderLevel = db.FolderLevel.Master;
    //    #endregion commented out Security Folder level

    //    #region Init Strings
    //    string State_Code = string.Empty;
    //    string County_Code = string.Empty;
    //    string Local_Code = string.Empty;
    //    string ToolTip = string.Empty;
    //    //string Name_Electoral_Plus_Text = string.Empty;
    //    string Link2Page = string.Empty;
    //    string NavbarText = string.Empty;
    //    //string ElectionKey = string.Empty;
    //    #endregion Init Strings

    //    HtmlTableRow Tr_Top_Navbar = db.Add_Tr_To_Table_Return_Tr(NavBarTop, "trNavbarAdminTop");

    //    HtmlTableRow Tr_Bottom_Navbar = db.Add_Tr_To_Table_Return_Tr(NavBarBottom, "trNavbarAdminBottom");

    //    #region /Master/Default.aspx - Only Master Users
    //    //Master Administrators to Master Link to /Master/Default.aspx
    //    if (
    //      (SecurePage.IsMasterUser)
    //      && (db.SCRIPT_NAME().ToLower() != "/master/default.aspx")
    //      )
    //    {
    //      Navbar_Anchor_Top(ref Tr_Top_Navbar, Anchor_Master_Default());//Master
    //    }
    //    #endregion /Master/Default.aspx - Only Master Users

    //    if (SecurePage.IsAdminPage)
    //    {
    //      if (db.Is_Page_Issues())
    //      {
    //      }
    //      else
    //      {
    //        #region Note - next 1 to 3 links
    //        //The next 1 to 3 links resets the Administriation level back
    //        //to the login security if they are clicked
    //        #endregion Note - next 1 to 3 links

    //        #region /Admin/Default.aspx - Only Master and State Administrators
    //        //Master and State Administrators to State Link to /Admin/Default.aspx
    //        if (
    //          (SecurePage.IsMasterUser)
    //          //|| (SecurePage.IsSuperUser)
    //          || (SecurePage.IsStateAdminUser)
    //          )
    //        {
    //          //Either the State Administrator's State of Master's selected State
    //          //Reset security to State level
    //          if (
    //            (SecurePage.IsStateAdminUser)
    //            || (db.Is_User_Master_Restricted())
    //            )
    //            State_Code = db.State_Code_Security_Login();
    //          else
    //            State_Code = db.State_Code();

    //          if (
    //            (!string.IsNullOrEmpty(State_Code))
    //            && (StateCache.IsValidStateCode(State_Code))
    //            )
    //          {
    //            //State /Admin/Default.aspx
    //            Navbar_Anchor_Top(ref Tr_Top_Navbar
    //              , Anchor_Admin_Default(
    //                State_Code
    //                )
    //              );
    //          }
    //        }
    //        #endregion /Admin/Default.aspx - Only Master and State Administrators

    //        #region /Admin/Default.aspx - Only Master, State and County Administrators
    //        //Master, State and County Administrators to County Link to /Admin/Default.aspx
    //        if (
    //          (SecurePage.IsMasterUser)
    //          //|| (SecurePage.IsSuperUser)
    //          || (SecurePage.IsStateAdminUser)
    //          || SecurePage.IsCountyAdminUser
    //          )
    //        {
    //          if (SecurePage.IsCountyAdminUser)
    //          {
    //            State_Code = db.State_Code_Security_Login();
    //            County_Code = db.County_Code_Security_Login();
    //          }
    //          else
    //          {
    //            State_Code = db.State_Code();
    //            County_Code = db.User_CountyCode();
    //          }
    //          if (
    //            (!string.IsNullOrEmpty(State_Code))
    //            && (StateCache.IsValidStateCode(State_Code))
    //            && (!string.IsNullOrEmpty(County_Code))
    //            )
    //          {
    //            //County /Admin/Default.aspx
    //            Navbar_Anchor_Top(ref Tr_Top_Navbar
    //              , Anchor_Admin_Default(
    //                  State_Code
    //                  , County_Code
    //                  )
    //              );
    //          }
    //        }
    //        #endregion /Admin/Default.aspx - Only Master, State and County Administrators

    //        #region /Admin/Default.aspx - Only Master, State, County and Local Administrators
    //        //Master, State, County and Local Administrators Link to /Admin/Default.aspx
    //        if (
    //          (SecurePage.IsMasterUser)
    //          //|| (SecurePage.IsSuperUser)
    //          || (SecurePage.IsAdminUser)
    //          )
    //        {
    //          if (SecurePage.IsLocalAdminUser)
    //          {
    //            State_Code = db.State_Code_Security_Login();
    //            County_Code = db.County_Code_Security_Login();
    //            Local_Code = db.Local_Code_Security_Login();
    //          }
    //          else
    //          {
    //            State_Code = db.State_Code();
    //            County_Code = db.User_CountyCode();
    //            Local_Code = db.User_LocalCode();
    //          }
    //          if (
    //            (!string.IsNullOrEmpty(State_Code))
    //            && (StateCache.IsValidStateCode(State_Code))
    //            && (!string.IsNullOrEmpty(County_Code))
    //            && (!string.IsNullOrEmpty(Local_Code))
    //            )
    //          {
    //            //Local /Admin/Default.aspx
    //            Navbar_Anchor_Top(ref Tr_Top_Navbar
    //              , Anchor_Admin_Default(
    //                State_Code
    //                , County_Code
    //                , Local_Code
    //                )
    //              );
    //          }
    //        }
    //        #endregion /Admin/Default.aspx - Only Master, State and County Administrators

    //        //if (db.Is_StateCode_State(db.User_StateCode()))
    //        if (StateCache.IsValidStateCode(db.State_Code()))
    //        {
    //          #region /Admin/Elections.aspx - All Administrators
    //          if (
    //            (SecurePage.IsMasterUser)
    //            || (SecurePage.IsAdminUser)
    //            )
    //          {
    //            //Elections
    //            Navbar_Anchor_Bottom(ref Tr_Bottom_Navbar, Anchor_Admin_Elections());
    //          }
    //          #endregion /Admin/Elections.aspx - All Administrators

    //          #region /Admin/Officials.aspx, Admin/Officials.aspx, Admin/Officials.aspx
    //          if (
    //            SecurePage.IsMasterUser
    //            || SecurePage.IsAdminUser
    //            )
    //          {
    //            if (
    //              (db.SCRIPT_NAME().ToLower() == "/admin/default.aspx")
    //              || (db.SCRIPT_NAME().ToLower() == "/admin/elections.aspx")
    //              || (db.SCRIPT_NAME().ToLower() == "/admin/election.aspx")
    //              || (db.SCRIPT_NAME().ToLower() == "/admin/officials.aspx")
    //              )
    //            {
    //              Navbar_Anchor_Bottom(ref Tr_Bottom_Navbar, Anchor_Admin_Officials());//Elected Officials
    //              Navbar_Anchor_Bottom(ref Tr_Bottom_Navbar, Anchor_Admin_Offices(db.State_Code()));//Elected Offices
    //              Navbar_Anchor_Bottom(ref Tr_Bottom_Navbar, Anchor_Admin_Politicians(db.State_Code()));//Politicians
    //            }
    //          }
    //          #endregion /Admin/Officials.aspx, Admin/Officials.aspx, Admin/Officials.aspx

    //          #region /Admin/Election.aspx, Admin/ElectionCategories.aspx, Admin/ElectionOffices.aspx, Admin/Referendum.aspx
    //          if (
    //            (SecurePage.IsMasterUser)
    //            || (SecurePage.IsAdminUser)
    //            )
    //          {
    //            if (
    //              (db.SCRIPT_NAME().ToLower() == "/admin/election.aspx")
    //              //|| (db.SCRIPT_NAME().ToLower() == "/admin/electioncategories.aspx")
    //              || (db.SCRIPT_NAME().ToLower() == "/admin/electionoffices.aspx")
    //              || (db.SCRIPT_NAME().ToLower() == "/admin/referendum.aspx")
    //              || (db.SCRIPT_NAME().ToLower() == "/admin/emailcandidates.aspx")
    //              )
    //            {
    //              Navbar_Anchor_Bottom(ref Tr_Bottom_Navbar, Anchor_Admin_Election());//Election Home
    //              //Navbar_Anchor_Bottom(ref Tr_Bottom_Navbar, Anchor_Admin_ElectionCategories());//General Office Contest Categories
    //              Navbar_Anchor_Bottom(ref Tr_Bottom_Navbar, Anchor_Admin_ElectionOffices());//Specific Office Contests
    //              Navbar_Anchor_Bottom(ref Tr_Bottom_Navbar, Anchor_Admin_Referendums());//Election Referendums
    //              Navbar_Anchor_Bottom(ref Tr_Bottom_Navbar, Anchor_Admin_ElectionResults());//Election Results
    //            }
    //          }
    //          #endregion /Admin/Election.aspx, Admin/ElectionCategories.aspx, Admin/ElectionOffices.aspx, Admin/Referendum.aspx

    //          #region /Admin/Authority.aspx
    //          if (
    //           (
    //            (SecurePage.IsMasterUser)
    //             || (SecurePage.IsStateAdminUser)
    //            )
    //            //&& (db.Is_StateCode_State(db.State_Code()))
    //            && (StateCache.IsValidStateCode(db.State_Code()))
    //           )
    //          {
    //            Navbar_Anchor_Top(ref Tr_Top_Navbar, Anchor_Admin_Authority());//Election Authority
    //          }
    //          #endregion /Admin/Authority.aspx
    //        }
    //      }
    //    }

    //    //if (db.Is_Page_In_Design_Folder())
    //    //{
    //    //  Navbar_Anchor_Top(ref Tr_Top_Navbar, Anchor_Design_Default());//Design
    //    //}

    //    if (SecurePage.IsPoliticianPage)
    //    {
    //      #region Politician Folder
    //      Navbar_Anchor_Top(ref Tr_Top_Navbar, Anchor_Politician_Default());//Home
    //      Navbar_Anchor_Top(ref Tr_Top_Navbar, Anchor_Politician_IntroPage());//Enter Your Introduction Information
    //      Navbar_Anchor_Top(ref Tr_Top_Navbar, Anchor_Politician_IssueQuestions());//Enter Your Issue Views
    //      Navbar_Anchor_Top(ref Tr_Top_Navbar, Anchor_Politician_Intro());//View Your Introduction Page
    //      Navbar_Anchor_Top(ref Tr_Top_Navbar, Anchor_Politician_Issue());//View Positions Pages
    //      Navbar_Anchor_Top(ref Tr_Top_Navbar, Anchor_Politician_SearchEngineSubmitIntro());//Intro Google Submit
    //      Navbar_Anchor_Top(ref Tr_Top_Navbar, Anchor_Politician_SearchEngineSubmitIntroCompare());//Positions Google Submit
    //      #endregion Politician Folder
    //    }

    //  }
    //  catch (Exception ex)
    //  {
    //    db.Log_Error_Admin(ex, "In NavBarAdmin");
    //  }
    //}

    #region Dead code

    //private string Anchor_Master_USElection()//US Elections
    //{
    //  string ElectionKey = db.QueryString("Election");
    //  return db.Anchor(
    //    db.Url_Master_USElection(db.ElectionKey_State(ElectionKey))
    //    , "Election Home"
    //    , "Election Home");
    //}
    //private string Anchor_Master_LDSDataUpdate()
    //{
    //  return db.Anchor(
    //    db.Url_Master_LDSDataUpdate()
    //    , "LDS Data Update"
    //    , "LDS Data Update");
    //}
    //private string Anchor_Master_DefaultDesign()//Default Design
    //{
    //  return db.Anchor(
    //    db.Url_Master_DesignDefaults()
    //    , "Default Design"
    //    , "Default Design all Domains and DesignCodes");
    //}

    //private string Anchor_Admin_ElectionCategories()//General Office Contest Categories
    //{
    //  string ElectionKey = db.QueryString("Election");
    //  return db.Anchor(
    //    db.Url_Admin_ElectionCategories(ElectionKey)
    //    //, db.Name_Electoral_Plus_Text_ElectionCategories_Center()
    //    , db.Center(db.Name_Electoral_Plus_Text(
    //        "Office Categories in this "
    //        , " Election"
    //        ))
    //    , db.Name_Electoral()
    //        + " general office contest types or office categories in "
    //        + db.Elections4Navbar(db.ElectionKey_State(ElectionKey), "ElectionDesc")
    //    , "_self"
    //    );
    //}

    //private string Anchor_Admin_Candidates()//Edit Election
    //{
    //  string ElectionKey = db.QueryString("Election");
    //  return db.Anchor(
    //    db.Url_Admin_ElectionReport(ElectionKey)
    //    //, db.Name_Electoral_Plus_Text_Candidates_Center()
    //    , db.Center(db.Name_Electoral_Plus_Text(
    //        "Easy Edit This "
    //        , " Election"))
    //    , db.Name_Electoral()
    //        + " report where you can edit the candidates and offices on ballots for "
    //        + db.Elections4Navbar(db.ElectionKey_State(ElectionKey), "ElectionDesc")
    //    , "_self"
    //    );
    //}
    //#region Old Not used

    //private string AnchorElectionAuthority()//Virginia Election Authority
    //{
    //  return db.Generic_WebAddress_Anchor(
    //    db.States_Str(Session["UserStateCode"].ToString(), "URL")
    //  , StateCache.GetStateName(Session["UserStateCode"].ToString()) + " Election Authority"
    //  , StateCache.GetStateName(Session["UserStateCode"].ToString()) + " Election Authority"
    //  , "view");
    //}
    //private string Anchor_Admin_Counties4Officials()//Counties' Elected Officials
    //{
    //  return db.Anchor(
    //    Url_NavBarAdminCounties4Officials()
    //    , db.Center("Counties' Elected Officials")
    //    , "County by county reports where you can edit currently elected officials in each county");
    //}
    //private string Anchor_Admin_Counties4ElectionReports()//Edit Counties' Elections
    //{
    //  return db.Anchor(
    //    Url_NavBarAdminCounties4ElectionReports()
    //    , db.Center("Edit Counties' Elections")
    //    , "County by county reports where you can edit candidates and offices on ballots in each county for this election");
    //}
    //private string Anchor_Admin_Counties4ElectionOffices()//Counties' Specific Office Contests
    //{
    //  return db.Anchor(
    //    Url_NavBarAdminCounties4ElectionOffices("Offices")
    //    , db.Center("Counties' Specific Office Contests")
    //    , "County by county reports where you can identify the offices contests on ballots in each county for this election");
    //}
    //private string Anchor_Admin_Counties4ElectionReferendums()//Counties' Election Referendums
    //{
    //  return db.Anchor(
    //    Url_NavBarAdminCounties4ElectionOffices("Referendums")
    //    , db.Center("Counties' Election Referendums")
    //    , "County by county reports where you can identify the referendums in each county for this election");
    //}
    //private string xAnchor_Admin_MultiCountyDistricts()//Multi-County
    //{
    //  return db.Anchor(
    //    Url_NavBarAdminMultiCountylDistricts()
    //    , db.Center("Multi-County Districts")
    //    , "Multi-County Districts");
    //}
    //private string xAnchor_Admin_JudicialDistricts()//Judicial Districts
    //{
    //  return db.Anchor(
    //    xUrl_NavBarAdminJudicialDistricts()
    //    , db.Center("Judicial Districts")
    //    , "Judicial Districts");
    //}
    //private string Anchor_Admin_Issues()//Issues
    //{
    //  return db.Anchor(
    //    Url_NavBarAdminIssues()
    //    , db.Center("Issues")
    //    , "Issues at this level");
    //}
    //private string Anchor_Admin_ElectionAuthority()//Election Authority
    //{
    //  if (!string.IsNullOrEmpty(Session["UserStateCode"].ToString()))
    //    return db.Anchor_State_Election_Authority(
    //      Session["UserStateCode"].ToString());
    //  else
    //    return string.Empty;
    //}
    //private string Anchor_Admin_DefaultOrganization()//Organization
    //{
    //  return db.Anchor(
    //    db.Url_OrganizationDefault()
    //    , db.Center("Organization")
    //    , "Contact information for " + db.Organizations(Session["UserOrganizationCode"].ToString(), "Organization"));
    //}
    //#endregion Old Not used

    //private string Url_NavBarAdminCounties4Officials()
    //{
    //  string Url = db.Url_Admin_Counties_Officials();

    //  Url += db.Url_Add_State_County_Local_Codes();

    //  return db.Fix_Url_Parms(Url);
    //}
    //private string Url_NavBarAdminCounties4ElectionReports()
    //{
    //  string ElectionKey = db.QueryString("Election");
    //  string Url = db.Ur4AdminCounties4ElectionReports();

    //  Url += db.Url_Add_State_County_Local_Codes();

    //  if (!string.IsNullOrEmpty(ElectionKey))
    //    Url += "&Election=" + db.ElectionKey_State(ElectionKey);

    //  return db.Fix_Url_Parms(Url);
    //}
    //private string Url_NavBarAdminCounties4ElectionOffices(string Type)
    //{
    //  string ElectionKey = db.QueryString("Election");
    //  string Url = db.Url_Admin_Counties_ElectionOffices();

    //  Url += db.Url_Add_State_County_Local_Codes();

    //  if (!string.IsNullOrEmpty(ElectionKey))
    //    Url += "&Election=" + db.ElectionKey_State(ElectionKey);

    //  Url += "&Type=" + Type;

    //  return db.Fix_Url_Parms(Url);
    //}

    //private string Url_NavBarAdminMultiCountylDistricts()
    //{
    //  string Url = db.Url_Admin_MultiCountylDistricts(db.Office_State_District_Multi_Counties);

    //  Url += db.Url_Add_State_County_Local_Codes();

    //  return db.Fix_Url_Parms(Url);
    //}
    //private string xUrl_NavBarAdminJudicialDistricts()
    //{
    //  string Url = db.Url_Admin_MultiCountylDistricts(db.Office_State_District_Multi_Counties_Judicial);

    //  Url += db.Url_Add_State_County_Local_Codes();

    //  return db.Fix_Url_Parms(Url);
    //}

    //private string Url_NavBarAdminIssues()
    //{
    //  string Url = db.Url_Admin_xIssues();

    //  Url += db.Url_Add_State_County_Local_Codes();

    //  return db.Fix_Url_Parms(Url);
    //}

    #endregion Dead code


  }
}