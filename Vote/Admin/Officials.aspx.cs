using System;
using System.Collections.Generic;
using System.Data;
using DB.Vote;
using DB.VoteLog;
using Vote.Reports;

namespace Vote.Admin
{
  public partial class Officials : SecureAdminPage
  {
    #region from db

    public static ElectoralClass Electoral_Class(string stateCode, string countyCode,
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

    public static bool Is_Electoral_Class_State_County_Local(string stateCode, string countyCode,
      string localCode)
    {
      if (
        (Electoral_Class(stateCode, countyCode, localCode) == ElectoralClass.State)
        || (Electoral_Class(stateCode, countyCode, localCode) == ElectoralClass.County)
        || (Electoral_Class(stateCode, countyCode, localCode) == ElectoralClass.Local)
      )
        return true;
      return false;
    }

    public static string Fail(string msg) =>
      $"<span class=\"MsgFail\">****FAILURE**** {msg}</span>";

    public static void Log_Error_Admin(Exception ex, string message = null)
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

    public static string SqlLit(string str)
    {
      //Enclose string in single quotes and double up any embededded single quotes
      str = "'" + str.Replace("'", "''") + "'";
      return str;
    }

    public static string DbErrorMsg(string sql, string err) =>
      $"Database Failure for SQL Statement::{sql} :: Error Msg:: {err}";

    public static DataRow Row_First(string sql)
    {
      var table = G.Table(sql);
      if (table.Rows.Count >= 1)
        return table.Rows[0];
      throw new ApplicationException(DbErrorMsg(sql, "Did not find any rows."));
    }

    public static string Elections_Winners_Identified(string stateCode)
    {
      #region date of last General Election

      var sql = " SELECT ElectionDate";
      sql += " FROM Elections";
      sql += " WHERE StateCode = " + SqlLit(stateCode);
      sql += " AND CountyCode = '' AND LocalCode=''";
      sql += " AND ElectionType = 'G'";
      sql += " AND ElectionDate < " + SqlLit(Db.DbToday);
      sql += " ORDER BY ElectionDate DESC";

      var rowGeneralElection = Row_First(sql);
      var dateLastGeneralElection = Convert.ToDateTime(rowGeneralElection["ElectionDate"].ToString());

      #endregion date of last General Election

      #region List Elections since last General Election

      sql = " SELECT ElectionDesc,IsWinnersIdentified";
      sql += " FROM Elections";
      sql += " WHERE StateCode = " + SqlLit(stateCode);
      sql += " AND CountyCode = '' AND LocalCode=''";
      sql += " AND ElectionDate >= '" + dateLastGeneralElection.ToString("yyyy/MM/dd") + "'";
      sql += " AND ElectionDate < " + SqlLit(Db.DbToday);
      sql += " AND IsViewable = 1";
      var tableElections = G.Table(sql);
      var electionsWinnersIdentified = string.Empty;
      foreach (DataRow rowElection in tableElections.Rows)
      {
        electionsWinnersIdentified += "<br>";
        if (Convert.ToBoolean(rowElection["IsWinnersIdentified"]))
          electionsWinnersIdentified +=
            "Winners <span style=color:green>HAVE</span> been Identified";
        else
          electionsWinnersIdentified +=
            "Winners have <span style=color:red><strong>NOT</strong></span> been Identified";
        electionsWinnersIdentified += " - " + rowElection["ElectionDesc"];
      }

      #endregion List Elections since last General Election

      return electionsWinnersIdentified;
    }

    #endregion from db

    private void Page_Title()
    {
      PageTitle.Text = string.Empty;
      PageTitle.Text += Offices.GetElectoralClassDescription(ViewState["StateCode"].ToString(),
        ViewState["CountyCode"].ToString(), ViewState["LocalCode"].ToString());

      PageTitle.Text += "<br>";
      PageTitle.Text += "CURRENTLY ELECTED OFFICIALS (INCUMBENTS)";
    }

    private void Visible_Controls()
    {
      TableIncumbentStatusRadioButtons.Visible =
        Is_Electoral_Class_State_County_Local(ViewState["StateCode"].ToString(),
          ViewState["CountyCode"].ToString(), ViewState["LocalCode"].ToString());

      TablePoliticianData.Visible = IsMasterUser;
    }

    private void ShowOfficialsReport() => 
      OfficialsReport.GetReport(Report.SignedInReportUser,
      ViewState["StateCode"].ToString(), ViewState["CountyCode"].ToString(),
      ViewState["LocalCode"].ToString()).AddTo(ReportPlaceHolder);

    public override IEnumerable<string> NonStateCodesAllowed => 
      new[]
      {
        "U1",
        "U2",
        "U3",
        "U4"
      };

    private void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        Page.Title = "Officials";

        ViewState["StateCode"] = G.State_Code();
        ViewState["CountyCode"] = G.County_Code();
        ViewState["LocalCode"] = G.Local_Code();
        //if (!db.Is_User_Security_Ok())
        //  HandleSecurityException();

        try
        {
          Visible_Controls();

          var stateCode = G.State_Code();
          HyperLink_View_Public_Report.NavigateUrl =
            UrlManager.GetOfficialsPageUri(stateCode)
              .ToString();

          Page_Title();

          Label_Report_Elections_Winners_Identified.Text =
            Elections_Winners_Identified(ViewState["StateCode"].ToString());

          ShowOfficialsReport();
        }
        catch (Exception ex)
        {
          Msg.Text = Fail(ex.Message);
          Log_Error_Admin(ex);
        }
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
  }
}