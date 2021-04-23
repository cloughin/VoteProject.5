using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using System.Configuration;

namespace Vote
{
  public partial class LoginPage : VotePage
  {
    //#region Notes about Securtiy
    ////1) Login Identifies special (non anonymous) users, each with unique permissions / capabilities, in Session["UserSecurity"]:
    ////	Session["UserSecurity"] can be: MASTER, ADMIN, COUNTY, LOCAL, POLITICIAN, DESIGN, ORANIZATION.
    ////  Folder authentication is used where each user enters the folder after the domain, like:
    ////  http://vote-usa.org/MASTER or http://vote-usa.org/ORANIZATION or http://vote-usa.org/DESIGN
    ////  Then the user is directed to the Default.aspx page in that folder 
    ////
    ////2) Login Sets specific permission limitations for each for these user levels as defined in the Security Table:
    ////MASTER No restrictions set at login
    ////ADMIN restricted to Data of one State with its counties and local districts
    ////COUNTY restricted to Data of one Couunty with its local districts
    ////LOCAL restricted to Data of one local district
    ////POLITICIAN restricted to single politician's Data: Session["PoliticianKey"]
    ////DESIGN restricted to the design of one design code
    ////ORGANIZATION restricted to the information about the organization
    ////
    ////3) Data administrations MASTER, ADMIN (State), COUNTY, LOCAL are restricted to their data and lower level data
    ////   Higher level administrators get access to lower level data by passing query strings 
    ////   Lower level administrators can not get access to higher level data.
    //#endregion

    //private string Login_Folder()
    //{
    //  string URLPageRequested = FormsAuthentication.GetRedirectUrl(TextBoxUserName.Text, false);

    //  URLPageRequested = URLPageRequested.ToUpper().Trim();
    //  if (URLPageRequested.IndexOf("MASTER", 0, URLPageRequested.Length) >= 0)
    //    //Is there a Master Folder in path
    //    return "MASTER";
    //  else if (URLPageRequested.IndexOf("ADMIN", 0, URLPageRequested.Length) >= 0)
    //    //Is there Admin Folder in path
    //    return "ADMIN";
    //  else if (URLPageRequested.IndexOf("COUNTY", 0, URLPageRequested.Length) >= 0)
    //    //Is there County Folder in path
    //    return "COUNTY";
    //  else if (URLPageRequested.IndexOf("LOCAL", 0, URLPageRequested.Length) >= 0)
    //    //Is there County Folder in path
    //    return "LOCAL";
    //  else if (URLPageRequested.IndexOf("DESIGN", 0, URLPageRequested.Length) >= 0)
    //    //Is there Design Folder in path
    //    return "DESIGN";
    //  else if (URLPageRequested.IndexOf("ORGANIZATION", 0, URLPageRequested.Length) >= 0)
    //    //Is there Design Folder in path
    //    return "ORGANIZATION";
    //  else if (URLPageRequested.IndexOf("POLITICIAN", 0, URLPageRequested.Length) >= 0)
    //  //Is there  Politician Folder in path
    //  {
    //    Session["PoliticianKey"] = TextBoxUserName.Text.Trim();
    //    return "POLITICIAN";
    //  }
    //  else if (URLPageRequested.IndexOf("PARTY", 0, URLPageRequested.Length) >= 0)
    //  //Is there Admin Folder in path
    //  {
    //    Session["UserPartyKey"] = db.PartiesEmails_Str(
    //      TextBoxUserName.Text.Trim()
    //    , "PartyKey"
    //    );
    //    return "PARTY";
    //  }
    //  else
    //    return string.Empty;
    //}

    //protected string Err_Msg()
    //{
    //  return "Either you entered an Invalid Username or Password or your browser security is not allowing cookies. "
    //   + " For example, if you are using Internet Explorer go to Tools/Internet Options. "
    //   + " On the Privacy Tab make sure the privacy slider is no higher than 'Medium High'."
    //   + " For some browsers you may need to close your browser; open a new browser; and then try again.";
    //}

    //protected void Check_Security(string URLPageRequested)
    //{
    //  #region Check TextBoxes
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxUserName);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxPassword);

    //  if (TextBoxUserName.Text.ToString().Trim() == string.Empty)
    //    throw new ApplicationException("Username was empty");
    //  if (TextBoxPassword.Text.ToString().Trim() == string.Empty)
    //    throw new ApplicationException("Password was empty");
    //  #endregion Check TextBoxes
    //  string SQL = string.Empty;
    //  //Is there a Politician Folder in path
    //  if (URLPageRequested.ToUpper().IndexOf("POLITICIAN"
    //    , 0, URLPageRequested.Length) >= 0)
    //  {
    //    #region POLITICIAN
    //    SQL = "Politicians"
    //    + " WHERE PoliticianKey = " + db.SQLLit(TextBoxUserName.Text.Trim())
    //    + " AND Password = " + db.SQLLit(TextBoxPassword.Text.Trim());
    //    #endregion POLITICIAN
    //  }
    //  else if (URLPageRequested.ToUpper().IndexOf("PARTY"
    //    , 0, URLPageRequested.Length) >= 0)
    //  {
    //    #region PARTY
    //    SQL = "PartiesEmails"
    //    + " WHERE PartyEmail = " + db.SQLLit(TextBoxUserName.Text.Trim())
    //    + " AND PartyPassword = " + db.SQLLit(TextBoxPassword.Text.Trim());
    //    #endregion PARTY
    //  }
    //  else
    //  {
    //    #region MASTER, ADMIN, COUNTY, DESIGN, ORGANIZATION
    //    SQL = "Security"
    //    + " WHERE UserName = " + db.SQLLit(TextBoxUserName.Text.Trim())
    //    + " AND UserPassword = " + db.SQLLit(TextBoxPassword.Text.Trim());
    //    #endregion MASTER, ADMIN, COUNTY, DESIGN, ORGANIZATION
    //  }
    //  int Rows = db.Rows_Count_From(SQL);
    //  if (Rows != 1)
    //    throw new ApplicationException(Err_Msg());

    //  if (
    //    (Login_Folder() == "MASTER")
    //    && (db.Security_Str(TextBoxUserName.Text.Trim().ToUpper(), "UserSecurity") != "MASTER")
    //    )

    //    throw new ApplicationException("You are not allowed in the page your requested.");
    //}

    //protected void ButtonLogin_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    string URLPageRequested
    //      = FormsAuthentication.GetRedirectUrl(
    //          TextBoxUserName.Text
    //          , false
    //          );

    //    Check_Security(URLPageRequested);

    //    #region Inits
    //    Session["UserName"] = string.Empty;
    //    Session["UserSecurity"] = string.Empty;
    //    Session["UserStateCode"] = string.Empty;
    //    //Session["UserDesignCode"] = string.Empty;
    //    Session["UserOrganizationCode"] = string.Empty;
    //    Session["UserCountyCode"] = string.Empty;
    //    Session["UserLocalCode"] = string.Empty;
    //    Session["UserIssuesCode"] = string.Empty;
    //    Session["UserPartyKey"] = string.Empty;
    //    Session["PoliticianKey"] = string.Empty;
    //    #endregion Inits

    //    string SQL = string.Empty;

    //    #region commented out
    //    ////Is there a Politician Folder in path
    //    //if (URLPageRequested.ToUpper().IndexOf(
    //    //  "POLITICIAN", 0, URLPageRequested.Length) >= 0)
    //    //{
    //    //  #region POLITICIAN Security
    //    //  Session["UserName"] = TextBoxUserName.Text.Trim();
    //    //  Session["UserSecurity"] = "POLITICIAN";
    //    //  Session["PoliticianKey"] = TextBoxUserName.Text.Trim();
    //    //  #endregion POLITICIAN Security
    //    //}
    //    //if (URLPageRequested.ToUpper().IndexOf(
    //    //  "PARTY", 0, URLPageRequested.Length) >= 0)
    //    //{
    //    //  #region PARTY
    //    //  Session["UserName"] = TextBoxUserName.Text.Trim();
    //    //  Session["UserSecurity"] = "PARTY";
    //    //  Session["UserPartyKey"] = db.PartiesEmails_Str(
    //    //    TextBoxUserName.Text.Trim()
    //    //  , "PartyKey"
    //    //  );
    //    //  #endregion PARTY
    //    //}
    //    //else
    //    //{
    //    //  #region MASTER, ADMIN, COUNTY, LOCAL, DESIGN, ORGANIZATION
    //    #endregion commented out

    //    #region Assign Security

    //    Session["UserName"] = TextBoxUserName.Text.Trim();

    //    Session["UserSecurity"] = Login_Folder();

    //    if (
    //      (Session["UserSecurity"].ToString() != "POLITICIAN")
    //      && (Session["UserSecurity"].ToString() != "PARTY")
    //      )
    //    {
    //      SQL = "SELECT"
    //          + " UserStateCode"
    //          + ",UserCountyCode"
    //          + ",UserLocalCode"
    //          + ",UserDesignCode"
    //          + ",UserOrganizationCode"
    //          + ",UserIssuesCode"
    //          + " FROM Security"
    //          + " WHERE UserName = " + db.SQLLit(TextBoxUserName.Text.Trim())
    //          + " AND UserPassword = " + db.SQLLit(TextBoxPassword.Text.Trim());
    //      DataRow SecurityRow = db.Row_Optional(SQL);

    //      Session["UserStateCode"] = SecurityRow["UserStateCode"].ToString();
    //      Session["UserCountyCode"] = SecurityRow["UserCountyCode"].ToString();
    //      Session["UserLocalCode"] = SecurityRow["UserLocalCode"].ToString();

    //      //Session["UserDesignCode"] = SecurityRow["UserDesignCode"].ToString();
    //      Session["UserOrganizationCode"] = SecurityRow["UserOrganizationCode"].ToString();

    //      Session["UserIssuesCode"] = SecurityRow["UserIssuesCode"].ToString();
    //    }

    //    #endregion Assign Security

    //    //  #endregion MASTER, ADMIN, COUNTY, LOCAL, DESIGN, ORGANIZATION
    //    //}

    //    #region INSERT Row INTO LogLogins Table
    //    //string InsertSQL = "INSERT INTO LogLogins ("
    //    //  + "DateStamp"
    //    //  + ",UserName"
    //    //  + ",UserSecurity"
    //    //  + ",UserStateCode"
    //    //  //+ ",UserDesignCode"
    //    //  + ",UserOrganizationCode"
    //    //  + ",UserCountyCode"
    //    //  + ",UserLocalCode"
    //    //  + ",UserIssuesCode"
    //    //  + ",UserPartyKey"
    //    //  + ")"
    //    //  + " VALUES("
    //    //  + db.SQLLit(Db.DbNow)
    //    //  + "," + db.SQLLit(Session["UserName"].ToString())
    //    //  + "," + db.SQLLit(Session["UserSecurity"].ToString())
    //    //  + "," + db.SQLLit(Session["UserStateCode"].ToString())
    //    //  //+ "," + db.SQLLit(db.Domain_DesignCode_This())
    //    //  + "," + db.SQLLit(Session["UserOrganizationCode"].ToString())
    //    //  + "," + db.SQLLit(Session["UserCountyCode"].ToString())
    //    //  + "," + db.SQLLit(Session["UserLocalCode"].ToString())
    //    //  + "," + db.SQLLit(Session["UserIssuesCode"].ToString())
    //    //  + "," + db.SQLLit(Session["UserPartyKey"].ToString())
    //    //  + ")";
    //    //db.ExecuteSQL(InsertSQL);

    //    DB.VoteLog.LogLogins.Insert(
    //      DateTime.Now,
    //      Session["UserName"].ToString(),
    //      Session["UserSecurity"].ToString(),
    //      Session["UserStateCode"].ToString(),
    //      Session["UserCountyCode"].ToString(),
    //      Session["UserLocalCode"].ToString(),
    //      string.Empty,
    //      Session["UserOrganizationCode"].ToString(),
    //      Session["UserIssuesCode"].ToString(),
    //      Session["UserPartyKey"].ToString(),
    //      string.Empty);

    //    #endregion INSERT Row INTO LogLogins Table

    //    //Redirect to intended page
    //    FormsAuthentication.RedirectFromLoginPage(TextBoxUserName.Text, false);

    //    #region commented out
    //    //}
    //    //else
    //    //{
    //    //  string Msg = "Either you entered an Invalid Username or Password or your browser security is not allowing cookies. "
    //    //    + " For example, if you are using Internet Explorer go to Tools/Internet Options. "
    //    //    + " On the Privacy Tab make sure the privacy slider is no higher than 'Medium High'."
    //    //    + " For some browsers you may need to close your browser; open a new browser; and then try again.";
    //    //  throw new ApplicationException(Msg);
    //    //}
    //    #endregion commented out
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    //Security failed - postback
    //    Msg.Text = "Sorry your login FAILED!!! " + ex.Message;
    //    Msg.ForeColor = Color.Red;

    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }

    //}

    //private void Page_Load(object sender, System.EventArgs e)
    //{
    //  if (!IsPostBack)//first time
    //  {
    //    try
    //    {
    //      Session.Timeout = 60;//60 minutes

    //      FormsAuthentication.SignOut();

    //      string autoLoginUserName = Session["AutoLoginUserName"] as string;
    //      if (!string.IsNullOrEmpty(autoLoginUserName))
    //      {
    //        Session.Remove("AutoLoginUserName");
    //        FormsAuthentication.RedirectFromLoginPage(autoLoginUserName, false);
    //      }

    //      //string test = FormsAuthentication.GetRedirectUrl(db.User_Security(), false);

    //      Msg.Text = " Please enter your Username and Password"
    //      + " in the textboxes below and click the Login Button.";
    //    }
    //    catch (Exception ex)
    //    {
    //      db.Log_Error_Admin(ex);
    //    }
    //  }
    //}

  }
}
