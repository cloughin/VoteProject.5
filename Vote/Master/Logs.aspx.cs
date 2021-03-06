using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB.VoteLog;

namespace Vote.Logs
{
  public partial class LogsPage : SecurePage, ISuperUser
  {
    #region from db

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

    public static string Fail(string msg)
    {
      return "<span class=" + "\"" + "MsgFail" + "\"" + ">"
        + "****FAILURE**** " + msg + "</span>";
    }

    private static HtmlTableRow Add_Tr_To_Table_Return_Tr(HtmlTable htmlTable, string rowClass,
      string align)
    {
      //<tr Class="RowClass">
      var htmlTr = new HtmlTableRow();
      if (rowClass != string.Empty)
        htmlTr.Attributes["Class"] = rowClass;
      if (align != string.Empty)
        htmlTr.Attributes["align"] = align;
      //</tr>
      htmlTable.Rows.Add(htmlTr);
      return htmlTr;
    }

    public static HtmlTableRow Add_Tr_To_Table_Return_Tr(HtmlTable htmlTable, string rowClass)
    {
      return Add_Tr_To_Table_Return_Tr(htmlTable, rowClass, string.Empty);
    }

    private static void Add_Td_To_Tr(HtmlTableRow htmlTr, string text, string tdClass, string align,
      int colspan = 0)
    {
      //<td Class="TdClass">
      var htmlTableCell = new HtmlTableCell {InnerHtml = text};
      if (tdClass != string.Empty)
        htmlTableCell.Attributes["class"] = tdClass;
      if (align != string.Empty)
        htmlTableCell.Attributes["align"] = align;
      if (colspan != 0)
        htmlTableCell.Attributes["colspan"] = colspan.ToString(CultureInfo.InvariantCulture);
      //</td>
      htmlTr.Cells.Add(htmlTableCell);
    }

    private static void Add_Td_To_Tr(HtmlTableRow htmlTr, string text, string tdClass)
    {
      Add_Td_To_Tr(htmlTr, text, tdClass, string.Empty);
    }

    private static bool Is_Valid_Date(string date)
    {
      DateTime value;
      return DateTime.TryParse(date, out value);
    }

    #endregion from db

    private class DateCount
    {
      public DateTime DateStamp;
      public int Count;
    }

    private static HtmlTable Report(IEnumerable<DateCount> dateList, string reportHeading,
      int centsPerTransaction)
    {
      var htmlTable = new HtmlTable();

      #region Heading

      var htmlTr = Add_Tr_To_Table_Return_Tr(htmlTable, "HTMLTr");
      Add_Td_To_Tr(htmlTr, "Date", "tdReportDetailHeading", "center");
      Add_Td_To_Tr(htmlTr, reportHeading, "tdReportDetailHeading", "center");

      #endregion

      #region Report Rows of Day / Transactions

      var totalTransactions = 0;

      foreach (var dateCount in dateList)
      {
        htmlTr = Add_Tr_To_Table_Return_Tr(htmlTable, "HTMLTr");
        Add_Td_To_Tr(htmlTr, dateCount.DateStamp.ToString("MM/dd/yyyy"), "tdReportDetail");
        Add_Td_To_Tr(htmlTr, dateCount.Count.ToString(CultureInfo.InvariantCulture),
          "tdReportDetail");
        totalTransactions += dateCount.Count;
      }

      //totals
      var total = totalTransactions * centsPerTransaction;
      htmlTr = Add_Tr_To_Table_Return_Tr(htmlTable, "HTMLTr");
      Add_Td_To_Tr(htmlTr, "Total", "tdReportDetail");
      Add_Td_To_Tr(htmlTr
        , totalTransactions + " * ." + centsPerTransaction + " = " + total + " cents"
        , "tdReportDetail");

      #endregion

      return htmlTable;
    }

    protected void ButtonRunReport_Click(object sender, EventArgs e)
    {
      try
      {
        #region Textbox Checks

        if (TextBoxLoginUser.Text.Trim() == string.Empty)
          throw new ApplicationException("The Login Username is empty.");
        if (!Is_Valid_Date(TextBoxFrom.Text.Trim()))
          throw new ApplicationException("The From Date is not valid.");
        if (!Is_Valid_Date(TextBoxTo.Text.Trim()))
          throw new ApplicationException("The To Date is not valid.");

        #endregion

        #region Login Username

        LabelUserName.Text = "Login User Name: " + TextBoxLoginUser.Text.Trim() +
          " From:" + TextBoxFrom.Text.Trim() + " To: " + TextBoxTo.Text.Trim();

        #endregion

        var userName = TextBoxLoginUser.Text.Trim();

        if (CheckBoxListLogs.Items[0].Selected) //LogLogins
        {
          TableLogins.Visible = true;

          #region LogLogins

          var htmlTable = new HtmlTable();

          var tr = new HtmlTableRow().AddTo(htmlTable, "trReportDetail");
          new HtmlTableCell {Align = "center", InnerHtml = "First Login"}.AddTo(
            tr, "tdReportDetailHeading");
          new HtmlTableCell {Align = "center", InnerHtml = "Last Login"}.AddTo(
            tr, "tdReportDetailHeading");
          new HtmlTableCell {Align = "center", InnerHtml = "Hours"}.AddTo(
            tr, "tdReportDetailHeading");

          var date = Convert.ToDateTime(TextBoxFrom.Text.Trim());
          var dateEnd = Convert.ToDateTime(TextBoxTo.Text.Trim());
          var totalDuration = TimeSpan.MinValue;
          var days = 0;
          var totalHours = 0;
          while (dateEnd >= date)
          {
            var lowDate = date;
            var highDate = date.AddDays(1);
            var loginTable = LogLogins.GetDataByUserNameDateStampRange(userName,
              lowDate, highDate);
            if (loginTable.Count > 0)
            {
              var firstLogin = loginTable[0].DateStamp;
              var lastLogin = loginTable[loginTable.Count - 1].DateStamp;
              var duration = lastLogin - firstLogin;
              var hours = duration.Hours;
              tr = new HtmlTableRow().AddTo(htmlTable, "trReportDetail");
              Add_Td_To_Tr(tr, firstLogin.ToString(CultureInfo.InvariantCulture), "tdReportDetail");
              Add_Td_To_Tr(tr, lastLogin.ToString(CultureInfo.InvariantCulture), "tdReportDetail");
              Add_Td_To_Tr(tr, duration.Hours.ToString(CultureInfo.InvariantCulture),
                "tdReportDetail");
              totalDuration = totalDuration + duration;
              days++;
              totalHours += hours;
            }
            date = date.AddDays(1);
          }

          tr = Add_Tr_To_Table_Return_Tr(htmlTable, "trReportDetail");
          Add_Td_To_Tr(tr, "Total", "tdReportDetail");
          Add_Td_To_Tr(tr, "Days: " + days, "tdReportDetail");
          Add_Td_To_Tr(tr, totalHours.ToString(CultureInfo.InvariantCulture), "tdReportDetail");

          LabelLogins.Text = htmlTable.RenderToString();

          #endregion
        }

        var beginDate = Convert.ToDateTime(TextBoxFrom.Text.Trim());
        var endDate = Convert.ToDateTime(TextBoxTo.Text.Trim());
        endDate = endDate.AddDays(1);

        if (CheckBoxListLogs.Items[1].Selected) //LogPoliticianAnswers
        {
          var table1 =
            LogPoliticianAnswers.GetBillingDataByUserNameDateStampRange(userName,
              beginDate, endDate);
          var table2 =
            LogDataChange.GetBillingDataByUserNameTableNameDateStampRange(
              userName, "Answers", beginDate, endDate);

          var dateList = table1.Select(row => row.DateStamp.Date)
            .Concat(table2.Select(row => row.DateStamp.Date))
            .GroupBy(date => date)
            .Select(g => new DateCount {DateStamp = g.Key, Count = g.Count()});

          TablePoliticianAnswers.Visible = true;
          //Control report = Report(dateList, "Answers", 40);
          Control report = Report(dateList, "Answers", 30);
          LabelPoliticianAnswers.Text = report.RenderToString();
        }

        if (CheckBoxListLogs.Items[2].Selected) //LogPoliticianAdds
        {
          var table =
            LogPoliticianAdds.GetBillingDataByUserNameDateStampRange(userName,
              beginDate, endDate);
          var dateList = table.GroupBy(row => row.DateStamp.Date)
            .Select(
              group => new DateCount {DateStamp = group.Key, Count = group.Count()});
          TablePoliticianAdds.Visible = true;
          Control report = Report(dateList, "Politician Adds", 20);
          LabelPoliticianAdds.Text = report.RenderToString();
        }

        if (CheckBoxListLogs.Items[3].Selected) //LogPoliticianChanges
        {
          var table1 =
            LogPoliticianChanges.GetBillingDataByUserNameDateStampRange(userName,
              beginDate, endDate);
          var table2 =
            LogDataChange.GetBillingDataByUserNameTableNameDateStampRange(
              userName, "Politicians", beginDate, endDate);

          var dateList = table1.Select(row => row.DateStamp.Date)
            .Concat(table2.Select(row => row.DateStamp.Date))
            .GroupBy(date => date)
            .Select(g => new DateCount {DateStamp = g.Key, Count = g.Count()});

          TablePoliticianChanges.Visible = true;
          //Control report = Report(dateList, "Politician Changes", 8);
          Control report = Report(dateList, "Politician Changes", 10);
          LabelPoliticianChanges.Text = report.RenderToString();
        }

        if (CheckBoxListLogs.Items[4].Selected) //LogElectionPoliticianAddsDeletes
        {
          var table =
            LogElectionPoliticianAddsDeletes.GetBillingDataByUserNameDateStampRange
              (userName, beginDate, endDate);
          var dateList = table.GroupBy(row => row.DateStamp.Date)
            .Select(
              group => new DateCount {DateStamp = group.Key, Count = group.Count()});
          TableElectionPoliticianAddsDeletes.Visible = true;
          Control report = Report(dateList, "Election Politician Adds Deletes", 15);
          LabelElectionPoliticianAddsDeletes.Text = report.RenderToString();
        }

        if (CheckBoxListLogs.Items[6].Selected) //LogElectionOfficeChanges
        {
          var table =
            LogElectionOfficeChanges.GetBillingDataByUserNameDateStampRange(
              userName, beginDate, endDate);
          var dateList = table.GroupBy(row => row.DateStamp.Date)
            .Select(
              group => new DateCount {DateStamp = group.Key, Count = group.Count()});
          TableElectionOfficeChanges.Visible = true;
          Control report = Report(dateList, "Election Office Changes", 15);
          LabelElectionOfficeChanges.Text = report.RenderToString();
        }

        if (CheckBoxListLogs.Items[7].Selected) //LogOfficeChanges
        {
          var table =
            LogOfficeChanges.GetBillingDataByUserNameDateStampRange(userName,
              beginDate, endDate);
          var dateList = table.GroupBy(row => row.DateStamp.Date)
            .Select(
              group => new DateCount {DateStamp = group.Key, Count = group.Count()});
          TableOfficeChanges.Visible = true;
          //Control report = Report(dateList, "Office Changes", 8);
          Control report = Report(dateList, "Office Changes", 10);
          LabelOfficeChanges.Text = report.RenderToString();
        }

        if (CheckBoxListLogs.Items[8].Selected) //LogOfficeOfficialAddsDeletes
        {
          var table =
            LogOfficeOfficialAddsDeletes.GetBillingDataByUserNameDateStampRange(
              userName, beginDate, endDate);
          var dateList = table.GroupBy(row => row.DateStamp.Date)
            .Select(
              group => new DateCount {DateStamp = group.Key, Count = group.Count()});
          TableOfficeOfficialsAdds.Visible = true;
          Control report = Report(dateList, "Office Official Adds Deletes", 15);
          LabelOfficeOfficialsAdds.Text = report.RenderToString();
        }

        if (CheckBoxListLogs.Items[9].Selected) //LogOfficeOfficialChanges
        {
          var table =
            LogOfficeOfficialChanges.GetBillingDataByUserNameDateStampRange(
              userName, beginDate, endDate);
          var dateList = table.GroupBy(row => row.DateStamp.Date)
            .Select(
              group => new DateCount {DateStamp = group.Key, Count = group.Count()});
          TableOfficeOfficialsChanges.Visible = true;
          Control report = Report(dateList, "Office Official Changes", 15);
          LabelOfficeOfficialsChanges.Text = report.RenderToString();
        }

        if (CheckBoxListLogs.Items[10].Selected) //LogPoliticiansImagesOriginal
        {
          var table1 =
            LogPoliticiansImagesOriginal.GetBillingDataByUserNameDateStampRange(
              userName, beginDate, endDate);
          var table2 =
            LogDataChange.GetBillingDataByUserNameTableNameDateStampRange(
              userName, "PoliticiansImagesBlobs", beginDate, endDate);

          var dateList = table1.Select(row => row.ProfileOriginalDate.Date)
            .Concat(table2.Select(row => row.DateStamp.Date))
            .GroupBy(date => date)
            .Select(g => new DateCount {DateStamp = g.Key, Count = g.Count()});

          TablePictureUploads.Visible = true;
          Control report = Report(dateList, "Picture Uploads", 60);
          LabelPictureUploads.Text = report.RenderToString();
        }

        if (CheckBoxListLogs.Items[11].Selected) //LogAdminData
        {
          var table = LogAdminData.GetBillingDataByUserNameDateStampRange(
            userName, beginDate, endDate);
          var dateList = table.GroupBy(row => row.DateStamp.Date)
            .Select(
              group => new DateCount {DateStamp = group.Key, Count = group.Count()});
          TableAdminDataUpdates.Visible = true;
          Control report = Report(dateList, "Admin Data Updates", 20);
          LabelAdminDataUpdates.Text = report.RenderToString();
        }
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
      }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        if (!IsMasterUser)
          HandleSecurityException();

        Title = H1.InnerText = "Logs";

        try
        {
          TableLogins.Visible = false;
          TablePoliticianAnswers.Visible = false;
          TablePoliticianAdds.Visible = false;
          TablePoliticianChanges.Visible = false;
          TableElectionPoliticianAddsDeletes.Visible = false;
          TableElectionPoliticianChanges.Visible = false;
          TableElectionOfficeChanges.Visible = false;
          TableOfficeChanges.Visible = false;
          TableOfficeOfficialsAdds.Visible = false;
          TableOfficeOfficialsChanges.Visible = false;
          TablePictureUploads.Visible = false;
          TableAdminDataUpdates.Visible = false;
        }
        catch (Exception ex)
        {
          Msg.Text = Fail(ex.Message);
          Log_Error_Admin(ex);
        }
      }
    }

    protected void ButtonCheckAll_Click(object sender, EventArgs e)
    {
      try
      {
        for (var i = 0; i <= 11; i++)
        {
          CheckBoxListLogs.Items[i].Selected = true;
        }
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
      }
    }
  }
}