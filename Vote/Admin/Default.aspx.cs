using System;
using System.Web.UI;
using DB.Vote;
using DB.VoteLog;
using Vote.Reports;

namespace Vote.Admin
{
  public partial class Default : SecureAdminPage, IAllowEmptyStateCode
  {
    #region from db

    private static ElectoralClass Electoral_Class(string stateCode, string countyCode,
      string localCode)
    {
      if (!string.IsNullOrEmpty(localCode) && !string.IsNullOrEmpty(countyCode) &&
        !string.IsNullOrEmpty(stateCode))
        return ElectoralClass.Local;

      if (!string.IsNullOrEmpty(countyCode) && !string.IsNullOrEmpty(stateCode))
        return ElectoralClass.County;

      if (!string.IsNullOrEmpty(stateCode))
        switch (stateCode)
        {
          case "PP":
          case "US":
          case "U1":
            return ElectoralClass.USPresident;
          case "U2":
            return ElectoralClass.USSenate;
          case "U3":
            return ElectoralClass.USHouse;
          case "U4":
            return ElectoralClass.USGovernors;
          default:
            return StateCache.IsValidStateCode(stateCode)
              ? ElectoralClass.State
              : ElectoralClass.All;
        }

      return ElectoralClass.Unknown;
    }

    private static bool Is_Electoral_Class_State_County_Local(string stateCode, string countyCode,
      string localCode) => 
      (Electoral_Class(stateCode, countyCode, localCode) == ElectoralClass.State)
      || (Electoral_Class(stateCode, countyCode, localCode) == ElectoralClass.County)
      || (Electoral_Class(stateCode, countyCode, localCode) == ElectoralClass.Local);

    private static string Ok(string msg) => $"<span class=\"MsgOk\">SUCCESS!!! {msg}</span>";

    private static string Fail(string msg) =>
      $"<span class=\"MsgFail\">****FAILURE**** {msg}</span>";

    private static string Name_Electoral() => 
      Offices.GetElectoralClassDescription(G.State_Code(), G.County_Code(),
      G.Local_Code());

    private static string Name_Electoral_Short() => 
      Offices.GetElectoralClassShortDescription(G.State_Code(), G.County_Code(),
      G.Local_Code());

    private static string Name_Electoral_Plus_Text(string textBeforeName, 
      string textAfterName, bool isElectoralClassNameLong)
    {
      var text = string.Empty;
      if (!string.IsNullOrEmpty(textBeforeName.Trim()))
        text += textBeforeName;

      if (isElectoralClassNameLong)
        text += Name_Electoral();
      else
        text += Name_Electoral_Short();

      if (!string.IsNullOrEmpty(textAfterName.Trim()))
        //Text += " " + Text_After_Name;
        text += textAfterName;

      return text;
    }

    private static string Name_Electoral_Plus_Text(string textAfterName) => 
      Name_Electoral_Plus_Text(string.Empty, textAfterName, true);

    private static void Log_Error_Admin(Exception ex, string message = null)
    {
      var logMessage = string.Empty;
      var stackTrace = string.Empty;
      if (ex != null)
      {
        logMessage = ex.Message;
        stackTrace = ex.StackTrace;
      }
      if (!string.IsNullOrWhiteSpace(message))
      {
        if (!string.IsNullOrWhiteSpace(logMessage))
          logMessage += " :: ";
        logMessage += message;
      }
      LogErrorsAdmin.Insert(DateTime.Now, UrlManager.GetCurrentPathUri(true).ToString(),
        logMessage, stackTrace);
    }

    private static bool Is_TextBox_Html(ITextControl textBox) => 
      (textBox.Text.IndexOf("<", StringComparison.Ordinal) >= 0)
      || (textBox.Text.IndexOf("/>", StringComparison.Ordinal) >= 0);

    private static bool Is_Str_Script(string strToCheck) => 
      strToCheck.Trim().ToUpper().IndexOf("<SCRIPT", StringComparison.Ordinal) >= 0;

    private static string SqlLit(string str)
    {
      //Enclose string in single quotes and double up any embededded single quotes
      str = "'" + str.Replace("'", "''") + "'";
      return str;
    }

    private static string DbErrorMsg(string sql, string err) =>
      $"Database Failure for SQL Statement::{sql} :: Error Msg:: {err}";

    private static string Fix_Url_Parms(string url)
    {
      //sets first parm in a query string to ? if all seperators are &'s
      if ((url.IndexOf('?', 0, url.Length) == -1) //no ?
        && (url.IndexOf('&', 0, url.Length) > -1)) //but one or more &
      {
        var loc = url.IndexOf('&', 0, url.Length);
        var lenAfter = url.Length - loc - 1;
        var urlBefore = url.Substring(0, loc);
        var urlAfter = url.Substring(loc + 1, lenAfter);
        return urlBefore + "?" + urlAfter;
      }
      return url;
    }

    //public static string Url_Add_State_County_Local_Codes()
    //{
    //  //Add only lower level codes
    //  var urlParms = string.Empty;
    //  switch (Electoral_Class(
    //    G.State_Code() //db.State_Code()
    //    , G.County_Code()
    //    , G.Local_Code()))
    //  {
    //    case ElectoralClass.State:
    //      if (!string.IsNullOrEmpty(G.State_Code()))
    //        urlParms += "&State=" + G.State_Code();
    //      break;
    //    case ElectoralClass.County:
    //      if (!string.IsNullOrEmpty(G.State_Code()))
    //        urlParms += "&State=" + G.State_Code();
    //      if (!string.IsNullOrEmpty(G.User_CountyCode()))
    //        urlParms += "&County=" + G.User_CountyCode();
    //      break;
    //    case ElectoralClass.Local:
    //      if (!string.IsNullOrEmpty(G.State_Code()))
    //        urlParms += "&State=" + G.State_Code();
    //      if (!string.IsNullOrEmpty(G.User_CountyCode()))
    //        urlParms += "&County=" + G.User_CountyCode();
    //      if (!string.IsNullOrEmpty(G.User_LocalCode()))
    //        urlParms += "&Local=" + G.User_LocalCode();
    //      break;
    //    default: //for Federal Codes U!...U4
    //      if (!string.IsNullOrEmpty(G.State_Code()))
    //        urlParms += "&State=" + G.State_Code();
    //      break;
    //  }
    //  return urlParms;
    //}

    //public static string Url_Admin_Authority()
    //{
    //  var url = "/Admin/Authority.aspx";
    //  url += Url_Add_State_County_Local_Codes();
    //  return Fix_Url_Parms(url);
    //}

    private static string Url_Admin_Parties(string stateCode, string partyKey, string partyEmail)
    {
      var url = "/Admin/Parties.aspx";
      if (!string.IsNullOrEmpty(stateCode))
        url += "&State=" + stateCode;
      if (!string.IsNullOrEmpty(partyKey))
        url += "&Party=" + partyKey;
      if (!string.IsNullOrEmpty(partyEmail))
        url += "&Email=" + partyEmail;
      return Fix_Url_Parms(url);
    }

    private static string Single_Key_Str(string tableName, string column, string keyName,
      string keyValue)
    {
      var sql = "SELECT " + column.Trim() + " FROM " + tableName.Trim()
        + " WHERE " + keyName.Trim() + " = " + SqlLit(keyValue.Trim());

      var table = G.Table(sql);
      if (table.Rows.Count == 1)
      {
        // modified to pass through nulls (Politician.Address etc)
        //return (string)table.Rows[0][Column].ToString().Trim();
        var result = table.Rows[0][column] as string;
        result = result?.Trim();
        return result;
      }
      throw new ApplicationException(DbErrorMsg(sql, "Did not find a single row"));
    }

    //public static int Single_Key_Int(string tableName, string column, string keyName,
    //  string keyValue)
    //{
    //  var sql = "SELECT " + column.Trim() + " FROM " + tableName.Trim()
    //    + " WHERE " + keyName.Trim() + " = " + SqlLit(keyValue.Trim());
    //  var table = G.Table(sql);
    //  if (table.Rows.Count == 1)
    //  {
    //    return (int) table.Rows[0][column];
    //  }
    //  throw new ApplicationException(DbErrorMsg(sql, "Did not find a single row"));
    //}

    private static bool Single_Key_Bool(string tableName, string column, string keyName,
      string keyValue)
    {
      var sql = "SELECT " + column.Trim() + " FROM " + tableName.Trim()
        + " WHERE " + keyName.Trim() + " = " + SqlLit(keyValue.Trim());
      var table = G.Table(sql);
      if (table.Rows.Count == 1)
      {
        return Convert.ToBoolean(table.Rows[0][column]);
      }
      throw new ApplicationException(DbErrorMsg(sql, "Did not find a single row"));
    }

    //public static DateTime Single_Key_Date(string tableName, string column, string keyName,
    //  string keyValue)
    //{
    //  var sql = "SELECT " + column.Trim() + " FROM " + tableName.Trim()
    //    + " WHERE " + keyName.Trim() + " = " + SqlLit(keyValue.Trim());
    //  var table = G.Table(sql);
    //  if (table.Rows.Count == 1)
    //  {
    //    return (DateTime) table.Rows[0][column];
    //  }
    //  throw new ApplicationException(DbErrorMsg(sql, "Did not find a single row"));
    //}

    private static string States_Str(string stateCode, string column) => 
      stateCode != string.Empty
      ? Single_Key_Str("States", column, "StateCode", stateCode)
      : string.Empty;

    private static void Throw_Exception_TextBox_Html(ITextControl textBox)
    {
      if (Is_TextBox_Html(textBox))
        throw new ApplicationException(
          "Text in a textbox appears to be HTML because it contains an opening or closing HTML tag (< or />). Please remove and try again.");
    }

    private static void Throw_Exception_TextBox_Script(ITextControl textBox)
    {
      if (Is_Str_Script(textBox.Text))
        throw new ApplicationException("Text in the textbox is illegal.");
    }

    private static void Throw_Exception_TextBox_Html_Or_Script(ITextControl textBox)
    {
      Throw_Exception_TextBox_Html(textBox);
      Throw_Exception_TextBox_Script(textBox);
    }

    private static bool States_Bool(string stateCode, string column) => 
      (stateCode != string.Empty) && (stateCode != "US") &&
      Single_Key_Bool("States", column, "StateCode", stateCode);

    //private static string Url_Admin_Officials(string stateCode)
    //{
    //  return Url_Admin_Officials(stateCode, string.Empty, string.Empty);
    //}

    //private static string Url_Admin_Officials(string stateCode, string countyCode,
    //  string localCode)
    //{
    //  var url = "/Admin/Officials.aspx";
    //  if (stateCode != string.Empty)
    //    url += "&State=" + stateCode;
    //  if (countyCode != string.Empty)
    //    url += "&County=" + countyCode;
    //  if (localCode != string.Empty)
    //    url += "&Local=" + localCode;
    //  return db.Fix_Url_Parms(url);
    //}

    private static bool Is_StateCode_State_By_State(string stateCode) => 
      StateCache.IsValidFederalCode(stateCode) &&
      !StateCache.IsUS(stateCode);

    private static string Str_Election_Type_Description(string type, string stateCode)
    {
      switch (type)
      {
        case "G":
          return "General Election";
        case "O":
          return "General Off-Year Election";
        case "S":
          return "Special Election";
        case "P":
          return "Primary";
        case "B":
          return "Presidential Primary";
        case "A":
          return stateCode == "US"
            ? "Remaining Presidential Party Primary Candidates"
            : "Template of Major Presidential Party Primary Candidates";
        default:
          return string.Empty;
      }
    }

    //private static string Url_Admin_Ballot()
    //{
    //  var url = "/Admin/Ballot.aspx";
    //  url += db.Url_Add_State_County_Local_Codes();
    //  url = db.Fix_Url_Parms(url);
    //  if (string.IsNullOrWhiteSpace(QueryCounty))
    //    url += "#ballot";
    //  return url;
    //}

    //private static int States_Int(string stateCode, string column)
    //{
    //  return stateCode != string.Empty && stateCode != "US"
    //    ? Single_Key_Int("States", column, "StateCode", stateCode)
    //    : 0;
    //}

    //private static DateTime States_Date(string stateCode, string column)
    //{
    //  return stateCode != string.Empty
    //    ? Single_Key_Date("States", column, "StateCode", stateCode)
    //    : DateTime.MinValue;
    //}

    private static string Url_Admin_Parties(string stateCode) => 
      Url_Admin_Parties(stateCode, string.Empty, string.Empty);

    //private static string Url_Admin_Politicians(OfficeClass officeClass)
    //{
    //  var url = db.Url_Admin_Politicians();
    //  url += "&Class=" + officeClass.ToInt();
    //  url += db.Url_Add_State_County_Local_Codes();
    //  return db.Fix_Url_Parms(url);
    //}

    //private static string Url_Admin_Offices(string stateCode, string countyCode, string localCode)
    //{
    //  var url = db.Url_Admin_Offices();
    //  if (!string.IsNullOrEmpty(stateCode))
    //    url += "&State=" + stateCode;
    //  if (!string.IsNullOrEmpty(countyCode))
    //    url += "&County=" + countyCode;
    //  if (!string.IsNullOrEmpty(localCode))
    //    url += "&Local=" + localCode;
    //  return db.Fix_Url_Parms(url);
    //}

    //private static bool Is_Electoral_Class_Local(string stateCode, string countyCode, string localCode)
    //{
    //  return db.Electoral_Class(stateCode, countyCode, localCode) == db.ElectoralClass.Local;
    //}

    //private static string Url_Admin_Offices(string stateCode, string countyCode)
    //{
    //  return Url_Admin_Offices(stateCode, countyCode, string.Empty);
    //}

    //private static bool Is_Electoral_Class_County(string stateCode, string countyCode, string localCode)
    //{
    //  return db.Electoral_Class(stateCode, countyCode, localCode) == db.ElectoralClass.County;
    //}

    //private static string Url_Admin_Offices(string stateCode)
    //{
    //  return Url_Admin_Offices(stateCode, string.Empty, string.Empty);
    //}

    //private static bool Is_Electoral_Class_State(string stateCode, string countyCode, string localCode)
    //{
    //  return db.Electoral_Class(stateCode, countyCode, localCode) == db.ElectoralClass.State;
    //}

    //private static bool Is_Electoral_Class_Federal(string stateCode, string countyCode, string localCode)
    //{
    //  return (db.Electoral_Class(stateCode, countyCode, localCode) == db.ElectoralClass.USPresident)
    //    || (db.Electoral_Class(stateCode, countyCode, localCode) == db.ElectoralClass.USSenate)
    //    || (db.Electoral_Class(stateCode, countyCode, localCode) == db.ElectoralClass.USHouse)
    //    || (db.Electoral_Class(stateCode, countyCode, localCode) == db.ElectoralClass.USGovernors);
    //}

    private static string Url_Admin_LocalDistricts(string stateCode, string countyCode,
      string localCode)
    {
      var url = "/Admin/Districts.aspx";
      if (stateCode != string.Empty)
        url += "&State=" + stateCode;
      if (countyCode != string.Empty)
        url += "&County=" + countyCode;
      if (localCode != string.Empty)
        url += "&Local=" + localCode;
      return Fix_Url_Parms(url);
    }

    #endregion from db

    #region Private

    private void CheckTextBoxsForHtmlAndIllegalInput()
    {
      Throw_Exception_TextBox_Html_Or_Script(TextBoxOfficesStatusStatewide);
      Throw_Exception_TextBox_Html_Or_Script(TextBoxOfficesStatusJudicial);
      Throw_Exception_TextBox_Html_Or_Script(TextBoxOfficesStatusCounties);
    }

    private void LoadCountyLinks()
    {
      TableCountyLinks.Visible = true;
      LabelCounties.Text =
        CountyLinks.GetDefaultCountyLinks(ViewState["StateCode"].ToString())
          .RenderToString();
    }

    private void LoadLocalLinks()
    {
      var stateCode = ViewState["StateCode"].ToString();
      var countyCode = ViewState["CountyCode"].ToString();
      TableLocalLinks.Visible = true;
      Label_County.Text =
        Offices.GetElectoralClassDescription(stateCode, countyCode) +
        " LOCAL DISTRICTS, TOWNS and CITIES (" + countyCode + ")";
      LabelLocalDistricts.Text =
        LocalLinks.GetDefaultLocalLinks(stateCode, countyCode, true, true).RenderToString();
      if (string.IsNullOrEmpty(LabelLocalDistricts.Text))
      {
        LabelLocalDistricts.Text =
          "No Local Districts, Towns or Cities have been identified for " +
          Offices.GetElectoralClassDescription(stateCode, countyCode);

        LabelLocalDesc.Text = string.Empty;
      }
      else
        LabelLocalDesc.Text =
          "Use the links above to EDIT information for any of these local districts," +
          " towns or cities in this county." +
          " Use the link below to ADD local districts, towns and cities to this county.";
    }

    private void RecordElectionStatusData()
    {
      var updateSql = "UPDATE States SET " + " OfficesStatusStatewide = " +
        SqlLit(TextBoxOfficesStatusStatewide.Text.Trim()) +
        ", OfficesStatusJudicial = " +
        SqlLit(TextBoxOfficesStatusJudicial.Text.Trim()) +
        ", OfficesStatusCounties = " +
        SqlLit(TextBoxOfficesStatusCounties.Text.Trim()) + " WHERE StateCode= " +
        SqlLit(ViewState["StateCode"].ToString());
      G.ExecuteSql(updateSql);
      Msg.Text = Ok("The Election Authority Data was Recorded.");
    }

    #endregion Private

    #region Event handlers and overrides

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        //The Session UserStateCode, UserCountyCode, UserLocalCode can be changed
        //by a higher administration level using query strings
        //This is done in db.State_Code(), db.County_Code(), db.Local_Code()
        //
        //Using ViewState variables insures these values won't
        //change on any postbacks or in different tab or browser Sessions.
        //
        //ViewState["StateCode"] can be a StateCode or U1, u2, u3 for FederalCode
        //db.QueryString("State") can't be used because Login Security does not pass
        //the State in a query string
        //
        //db.State_Code(),db.County_Code(),db.Local_Code() have to be called 
        //in that order because db.State_Code() resets CountyCode and LocalCode empty 

        Page.Title = "Home";

        if (string.IsNullOrWhiteSpace(StateCode))
        {
          UpdateControls.Visible = false;
          NoJurisdiction.CreateStateLinks("/admin/?state={StateCode}");
          NoJurisdiction.ShowHeadOptional(false);
          NoJurisdiction.Visible = true;
          return;
        }

        ViewState["StateCode"] = G.State_Code();
        ViewState["CountyCode"] = G.County_Code();
        ViewState["LocalCode"] = G.Local_Code();

        try
        {
          // All Tables Not Visible

          //TableStateElection.Visible = false;
          //TablePresidentialPrimary.Visible = false;
          //TablePresidentialPrimaryTemplate.Visible = false;
          //TableStateByStateElections.Visible = false;

          //TableStateElectionMaint.Visible = false;

          TableCountyLinks.Visible = false;
          TableElectionAuthority.Visible = false;
          TableBallotDesign.Visible = false;
          //TableSendEmails.Visible = false;
          TableLocalLinks.Visible = false;
          TableLocalLinksEdit.Visible = false;
          //TableOffices_Identified.Visible = false;
          //TableBulkCountyOfficeAdds.Visible = false;
          TableMultiCountyJudicialDistricts.Visible = false;
          TableMultiCountyOtherDistricts.Visible = false;
          TableMultiCountyPartyDistricts.Visible = false;
          TableNotes.Visible = false;
          TableMasterOnly.Visible = false;
          TableMisc.Visible = false;

          // State vs Non-State Controls
          // State
          if (StateCache.IsValidStateCode(ViewState["StateCode"].ToString()))
          {
            //TableStateElection.Visible = true;
            //TableStateElectionMaint.Visible = true;

            PageTitle.Text = string.Empty;
            PageTitle.Text += Offices.GetElectoralClassDescription(
              ViewState["StateCode"].ToString(),
              ViewState["CountyCode"].ToString(), ViewState["LocalCode"].ToString());

            PageTitle.Text += "<br>";
            PageTitle.Text += "ADMINISTRATION";

            //HyperLinkElections.Text = db.Name_Electoral_Plus_Text(" Elections");
            //HyperLinkElections.NavigateUrl = GetUpdateElectionsPageUrl(StateCode,
            //  CountyCode, LocalCode);

            // Counties & Locals Links
            switch (
              Electoral_Class(ViewState["StateCode"].ToString(),
                ViewState["CountyCode"].ToString(), ViewState["LocalCode"].ToString())
            )
            {
              case ElectoralClass.USPresident:
                break;
              case ElectoralClass.USSenate:
                break;
              case ElectoralClass.USHouse:
                break;
              case ElectoralClass.USGovernors:
                break;
              case ElectoralClass.State:
                LoadCountyLinks();

                // Bulk County Office Adds & Multi-County Districts
                if (IsSuperUser)
                {
                  //TableBulkCountyOfficeAdds.Visible = true;

                  TableMultiCountyJudicialDistricts.Visible = true;
                  TableMultiCountyOtherDistricts.Visible = true;
                  TableMultiCountyPartyDistricts.Visible = true;

                  // Bulk Addition of County Offices

                  var sql = string.Empty;
                  sql += " Offices";
                  sql += " WHERE StateCode = " +
                    SqlLit(ViewState["StateCode"].ToString());
                  sql += " AND (";
                  sql += " (OfficeLevel = 8)";
                  sql += " OR (OfficeLevel = 9)";
                  sql += " OR (OfficeLevel = 10)";
                  sql += " OR (OfficeLevel = 11)";
                  sql += " OR (OfficeLevel = 18)";
                  sql += " OR (OfficeLevel = 22)";
                  sql += " )";
                  var countyOffices = G.Rows_Count_From(sql);
                  //Label_BulkCountyOfficeAdds.Text = countyOffices +
                  //  " County offices are defined in all counties.";

                  if (countyOffices > 0)
                  {
                    //sql = string.Empty;
                    //sql += " Offices";
                    //sql += " WHERE StateCode = " +
                    //  SqlLit(ViewState["StateCode"].ToString());
                    //sql += " AND OfficeLevel = 8";
                    //var countyExecutiveOffices = G.Rows_Count_From(sql);
                    //Label_BulkCountyOfficeAdds.Text += " (" + countyExecutiveOffices +
                    //  " Executive Offices)";

                    //sql = string.Empty;
                    //sql += " Offices";
                    //sql += " WHERE StateCode = " +
                    //  SqlLit(ViewState["StateCode"].ToString());
                    //sql += " AND OfficeLevel = 9";
                    //var countyLegislative = G.Rows_Count_From(sql);
                    //Label_BulkCountyOfficeAdds.Text += " (" + countyLegislative +
                    //  " Legislative Offices)";

                    //sql = string.Empty;
                    //sql += " Offices";
                    //sql += " WHERE StateCode = " +
                    //  SqlLit(ViewState["StateCode"].ToString());
                    //sql += " AND OfficeLevel = 10";
                    //var countySchoolBoard = G.Rows_Count_From(sql);
                    //Label_BulkCountyOfficeAdds.Text += " (" + countySchoolBoard +
                    //  " SchoolBoard Offices)";

                    //sql = string.Empty;
                    //sql += " Offices";
                    //sql += " WHERE StateCode = " +
                    //  SqlLit(ViewState["StateCode"].ToString());
                    //sql += " AND OfficeLevel = 11";
                    //var countyCommission = G.Rows_Count_From(sql);
                    //Label_BulkCountyOfficeAdds.Text += " (" + countyCommission +
                    //  " Commission Offices)";

                    //sql = string.Empty;
                    //sql += " Offices";
                    //sql += " WHERE StateCode = " +
                    //  SqlLit(ViewState["StateCode"].ToString());
                    //sql += " AND OfficeLevel = 18";
                    //var countyJudicial = G.Rows_Count_From(sql);
                    //Label_BulkCountyOfficeAdds.Text += " (" + countyJudicial +
                    //  " Judicial Offices)";

                    //sql = string.Empty;
                    //sql += " Offices";
                    //sql += " WHERE StateCode = " +
                    //  SqlLit(ViewState["StateCode"].ToString());
                    //sql += " AND OfficeLevel = 22";
                    //var countyParty = G.Rows_Count_From(sql);
                    //Label_BulkCountyOfficeAdds.Text += " (" + countyParty +
                    //  " Party Offices)";
                  }

                  //HyperLinkBulkCountyOfficeAdds.NavigateUrl =
                  //  "/Admin/Office.aspx?Electoral=4";

                  //HyperLinkBulkCountyOfficeAdds.Text =
                  //  StateCache.GetStateName(ViewState["StateCode"].ToString()) +
                  //    " Bulk Additions, Updates and Report of County Offices in EACH COUNTY";

                  sql = string.Empty;

                  // JUDICIAL Districts
                  sql += " Districts";
                  sql += " WHERE StateCode = " +
                    SqlLit(ViewState["StateCode"].ToString());
                  sql += " AND OfficeLevel = 17";
                  var judicialDistricts = G.Rows_Count_From(sql);
                  Label_JudicialDistricts.Text = judicialDistricts +
                    " Multi-County JUDICIAL Districts are defined.";
                  HyperLinkJudicialDistricts.NavigateUrl =
                    "/Admin/Districts.aspx?Level=17";

                  HyperLinkJudicialDistricts.Text =
                    StateCache.GetStateName(ViewState["StateCode"].ToString()) +
                    " Add or Edit JUDICIAL Multi-County Districts (Office Level 17)";

                  // OTHER Districts
                  sql = string.Empty;
                  sql += " Districts";
                  sql += " WHERE StateCode = " +
                    SqlLit(ViewState["StateCode"].ToString());
                  sql += " AND OfficeLevel = 7";
                  var otherDistricts = G.Rows_Count_From(sql);
                  Label_MultiCountyDistricts.Text = otherDistricts +
                    " Multi-County OTHER Districts are defined.";
                  HyperLinkMultiCountyDistricts.NavigateUrl =
                    "/Admin/Districts.aspx?Level=7";

                  HyperLinkMultiCountyDistricts.Text =
                    StateCache.GetStateName(ViewState["StateCode"].ToString()) +
                    " Add or Edit OTHER Multi-County Districts like Conservation Districts (Office Level 7)";

                  // PARTY Districts
                  sql = string.Empty;
                  sql += " Districts";
                  sql += " WHERE StateCode = " +
                    SqlLit(ViewState["StateCode"].ToString());
                  sql += " AND OfficeLevel = 21";
                  var partyDistricts = G.Rows_Count_From(sql);
                  Label_PartyDistricts.Text = partyDistricts +
                    " Multi-County PARTY Districts are defined.";
                  HyperLinkPartyDistricts.NavigateUrl =
                    "/Admin/Districts.aspx?Level=21";

                  HyperLinkPartyDistricts.Text =
                    StateCache.GetStateName(ViewState["StateCode"].ToString()) +
                    " Add or Edit PARTY Multi-County Districts (Office Level 21)";
                }

                TableBallotDesign.Visible = true;

                TableElectionAuthority.Visible = true;

                break;
              case ElectoralClass.County:

                //County
                LoadCountyLinks();
                LoadLocalLinks();

                // Local Districts
                TableLocalLinksEdit.Visible = true;
                HyperLinkLocalDistricts.NavigateUrl =
                  Url_Admin_LocalDistricts(ViewState["StateCode"].ToString(),
                    ViewState["CountyCode"].ToString(), string.Empty);

                HyperLinkLocalDistricts.Text = "ADD " +
                  Offices.GetElectoralClassDescription(ViewState["StateCode"].ToString(),
                    ViewState["CountyCode"].ToString()) +
                  " Local Districts, Towns and Cities";

                TableElectionAuthority.Visible = true;

                break;
              case ElectoralClass.Local:

                LoadCountyLinks();

                LoadLocalLinks();

                TableElectionAuthority.Visible = true;

                break;
            }

            // Elected Officials
            //HyperLinkOfficials.NavigateUrl = db.Url_Admin_Officials();
            //HyperLinkOfficials.Text = //db.Name_Electoral_Plus_Text_Officials();
            //  db.Name_Electoral_Plus_Text(" Currently Elected Officials (Incumbents)");

            // Elected Offices
            //if (Is_Electoral_Class_Federal(ViewState["StateCode"].ToString(),
            //  ViewState["CountyCode"].ToString(), ViewState["LocalCode"].ToString()))
            //{
            //  //No link -> defined at state level
            //}
            //else if (Is_Electoral_Class_State(ViewState["StateCode"].ToString(),
            //  ViewState["CountyCode"].ToString(), ViewState["LocalCode"].ToString()))

            //  // State Admin
            //  HyperLinkElectedOffices.NavigateUrl =
            //    Url_Admin_Offices(ViewState["StateCode"].ToString());

            //else if (Is_Electoral_Class_County(ViewState["StateCode"].ToString(),
            //  ViewState["CountyCode"].ToString(),
            //  ViewState["LocalCode"].ToString()))

            //  // County Admin
            //  HyperLinkElectedOffices.NavigateUrl =
            //    Url_Admin_Offices(ViewState["StateCode"].ToString(),
            //      ViewState["CountyCode"].ToString());

            //else if (Is_Electoral_Class_Local(ViewState["StateCode"].ToString(),
            //  ViewState["CountyCode"].ToString(),
            //  ViewState["LocalCode"].ToString()))

            //  // Local Admiin
            //  HyperLinkElectedOffices.NavigateUrl =
            //    Url_Admin_Offices(ViewState["StateCode"].ToString(),
            //      ViewState["CountyCode"].ToString(),
            //      ViewState["LocalCode"].ToString());

            //HyperLinkElectedOffices.Text = ////db.Name_Electoral_Plus_Text_Offices();
            //  Offices.GetElectoralClassDescription(ViewState["StateCode"].ToString(),
            //    ViewState["CountyCode"].ToString(), ViewState["LocalCode"].ToString()) +
            //    " Elected Offices";
            //if (Is_Electoral_Class_Federal(ViewState["StateCode"].ToString(),
            //  ViewState["CountyCode"].ToString(), ViewState["LocalCode"].ToString()))

            //  // Federal
            //  HyperLinkElectedOffices.Text +=
            //    " are all defined, and can onlly be edited at the state level.";

            // Politicians
            // HyperLinkPoliticians.NavigateUrl
            //if (Is_Electoral_Class_Federal(ViewState["StateCode"].ToString(),
            //  ViewState["CountyCode"].ToString(), ViewState["LocalCode"].ToString()))
            //{
            //  // Federal - No link -> defined at state level
            //}
            //else
            //  HyperLinkPoliticians.NavigateUrl =
            //    Url_Admin_Politicians(OfficeClass.Undefined);

            //HyperLinkPoliticians.Text = db.Name_Electoral_Plus_Text(" Politicians");
            //if (Is_Electoral_Class_Federal(ViewState["StateCode"].ToString(),
            //  ViewState["CountyCode"].ToString(), ViewState["LocalCode"].ToString()))

            //  // Federal
            //  HyperLinkPoliticians.Text += " are edited at the state level.";

            if (
              Is_Electoral_Class_State_County_Local(
                ViewState["StateCode"].ToString(), ViewState["CountyCode"].ToString(),
                ViewState["LocalCode"].ToString()))
            {
              // Election Authority
              //if (TableElectionAuthority.Visible)
              //{
              //  HyperLinkElectionAuthority.NavigateUrl = Url_Admin_Authority();
              //  //HyperLinkElectionAuthority.Text = db.Name_Electoral(
              //  //  db.State_Code(), string.Empty, string.Empty, true)
              //  //  + " Election Authority";
              //  HyperLinkElectionAuthority.Text =
              //    Name_Electoral_Plus_Text(" Election Authority");
              //}

              // Political Parties
              HyperLinkPoliticalParties.NavigateUrl =
                Url_Admin_Parties(ViewState["StateCode"].ToString());
              HyperLinkPoliticalParties.Text =
                Offices.GetElectoralClassDescription(G.State_Code(), string.Empty, string.Empty) +
                " Political Parties";

              // Counties
              Label_Counties.Text = Name_Electoral_Plus_Text(" Counties");

              // Emails
              //Label_Emails_Date_Roster_Primary.Text = States_Date(
              //  ViewState["StateCode"].ToString(), "EmailsDateElectionRosterPrimary")
              //  .ToString(CultureInfo.InvariantCulture);
              //Label_Emails_Sent_Roster_Primary.Text = States_Int(
              //  ViewState["StateCode"].ToString(), "EmailsSentElectionRosterPrimary")
              //  .ToString(CultureInfo.InvariantCulture);
              //Label_Emails_Date_Roster_General.Text = States_Date(
              //  ViewState["StateCode"].ToString(), "EmailsDateElectionRosterGeneral")
              //  .ToString(CultureInfo.InvariantCulture);
              //Label_Emails_Sent_Roster_General.Text = States_Int(
              //  ViewState["StateCode"].ToString(), "EmailsSentElectionRosterGeneral")
              //  .ToString(CultureInfo.InvariantCulture);

              // Ballot Design
              HyperLinkBallotDesign.NavigateUrl = /*Url_Admin_Ballot()*/ GetBallotPageUrl(StateCode);
              HyperLinkBallotDesign.Text = Name_Electoral_Plus_Text(" Ballot Design and Content");
            }
          }
          else
            switch (ViewState["StateCode"].ToString())
            {
              case "US":
                //TablePresidentialPrimary.Visible = true;
                PageTitle.Text = Str_Election_Type_Description("A", "US");
                PageTitle.Text += "<br>";
                PageTitle.Text += "ADMINISTRATION";
                //HyperLinkElectionsUS.Text = Str_Election_Type_Description("A",
                //  "US");
                //HyperLinkElectionsUS.NavigateUrl = db.Url_Admin_Elections("US");
                break;
              case "PP":
                //TablePresidentialPrimaryTemplate.Visible = true;
                PageTitle.Text = Str_Election_Type_Description("A", "PP");
                PageTitle.Text += "<br>";
                PageTitle.Text += "ADMINISTRATION";
                //HyperLinkElectionsPP.Text = Str_Election_Type_Description("A",
                //  "PP");
                //HyperLinkElectionsPP.NavigateUrl = db.Url_Admin_Elections("PP");
                break;
              default:
                if (Is_StateCode_State_By_State(ViewState["StateCode"].ToString()))
                {
                  // State-By-State Reports
                  //TableStateByStateElections.Visible = true;
                  //if (db.QueryString("State") == "U1")
                  switch (ViewState["StateCode"].ToString())
                  {
                    case "U1":
                      PageTitle.Text = "U.S. President or State-By-State Elections";
                      PageTitle.Text += "<br>";
                      PageTitle.Text += "ADMINISTRATION";
                      //HyperLinkElectionsUD.Text = "US President State-By-State";
                      //HyperLinkElectionsUD.NavigateUrl = db.Url_Admin_Elections("U1");
                      //HyperLinkOfficialsUD.Text = "Current US President";
                      //HyperLinkOfficialsUD.NavigateUrl = Url_Admin_Officials("U1");
                      break;
                    case "U2":
                      PageTitle.Text =
                        "U.S. Senate Officials or State-By-State Elections";
                      PageTitle.Text += "<br>";
                      PageTitle.Text += "ADMINISTRATION";
                      //HyperLinkElectionsUD.Text = "US Senate State-By-State";
                      //HyperLinkElectionsUD.NavigateUrl = db.Url_Admin_Elections("U2");
                      //HyperLinkOfficialsUD.Text = "Current US Senate";
                      //HyperLinkOfficialsUD.NavigateUrl = Url_Admin_Officials("U2");
                      break;
                    case "U3":
                      PageTitle.Text =
                        "U.S. House of Representatives Officials or State-By-State Elections";
                      PageTitle.Text += "<br>";
                      PageTitle.Text += "ADMINISTRATION";
                      //HyperLinkElectionsUD.Text =
                      //  "US House of Representatives State-By-State";
                      //HyperLinkElectionsUD.NavigateUrl = db.Url_Admin_Elections("U3");
                      //HyperLinkOfficialsUD.Text =
                      //  "Current US House of Representatives";
                      //HyperLinkOfficialsUD.NavigateUrl = Url_Admin_Officials("U3");
                      break;
                    case "U4":
                      PageTitle.Text =
                        "Current Governors or State-By-State Elections";
                      PageTitle.Text += "<br>";
                      PageTitle.Text += "ADMINISTRATION";
                      //HyperLinkElectionsUD.Text =
                      //  "Governors State-By-State Elections";
                      //HyperLinkElectionsUD.NavigateUrl = db.Url_Admin_Elections("U4");
                      //HyperLinkOfficialsUD.Text = "Current Governors State-By-State";
                      //HyperLinkOfficialsUD.NavigateUrl = Url_Admin_Officials("U4");
                      break;
                  }
                }
                break;
            }

          // Help links
          if (IsMasterUser)
          {
            HyperLink_Interns.Visible = true;
            HyperLink_Help.Visible = false;
          }
          else
          {
            HyperLink_Help.Visible = true;
            HyperLink_Interns.Visible = false;
          }

          if (IsSuperUser
            && StateCache.IsValidStateCode(ViewState["StateCode"].ToString()))
          {
            // Master Only
            TableMasterOnly.Visible = true;
            //TableSendEmails.Visible = true;
            TableMisc.Visible = true;

            HyperLinkNames.NavigateUrl =
              "/Master/MgtReports.aspx?Report=OfficialsNames&State=" +
              ViewState["StateCode"];

            RadioButtonListUseBOLBanner.SelectedValue =
              States_Bool(ViewState["StateCode"].ToString(), "IsUseBOEBanner")
                ? "T"
                : "F";

            if (
              Electoral_Class(ViewState["StateCode"].ToString(),
                ViewState["CountyCode"].ToString(), ViewState["LocalCode"].ToString()) ==
              ElectoralClass.State)
            {
              // Status Notes of Statewide, Judicial and County Elected Offices and Incumbents
              TableNotes.Visible = true;
              TextBoxOfficesStatusStatewide.Text =
                States_Str(ViewState["StateCode"].ToString(),
                  "OfficesStatusStatewide");
              TextBoxOfficesStatusJudicial.Text =
                States_Str(ViewState["StateCode"].ToString(),
                  "OfficesStatusJudicial");
              TextBoxOfficesStatusCounties.Text =
                States_Str(ViewState["StateCode"].ToString(),
                  "OfficesStatusCounties");
            }
          }
        }
        catch (Exception ex)
        {
          Msg.Text = Fail(ex.Message);
          Log_Error_Admin(ex);
        }
      }
    }

    protected void ButtonRecordOfficesStaus_Click(object sender, EventArgs e)
    {
      try
      {
        CheckTextBoxsForHtmlAndIllegalInput();

        RecordElectionStatusData();
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
      }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
      try
      {
        if (Session["ErrNavBarAdmin"].ToString() != string.Empty)
          Msg.Text = Fail(Session["ErrNavBarAdmin"].ToString());
      }
      // ReSharper disable once EmptyGeneralCatchClause
      catch /*(Exception ex)*/
      {
      }
    }

    #endregion Event handlers and overrides
  }
}