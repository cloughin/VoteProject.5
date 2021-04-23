using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB;
using DB.VoteLog;

namespace Vote.Reports
{
  public static class LogsReports 
  {
    public sealed class LoginsReport : TableBasedReport
    {
      #region Private

      private readonly LoginsReportDataManager _DataManager =
        new LoginsReportDataManager();

      #region Private classes

      private sealed class LoginsReportDataManager : ReportDataManager<DataRow>
      {
        public void GetData(string userName, DateTime startDate, DateTime endDate)
        {
          DataTable = LogLogins.GetDataByUserNameDateStampRange(userName,
              startDate, endDate);
        }
      }

      #endregion Private classes

      private Control GenerateReport(string userName, DateTime startDate, DateTime endDate)
      {
        _DataManager.GetData(userName, startDate, endDate);
        var data = _DataManager.GetDataSubset()
          .GroupBy(r => r.DateStamp().Date)
          .ToList();

        StartNewHtmlTable();
        CurrentHtmlTable.AddCssClasses("tableAdmin");
        var tr = new HtmlTableRow().AddTo(CurrentHtmlTable);
        new HtmlTableCell { InnerHtml = "Logins" }.AddTo(tr, "H");
        tr = new HtmlTableRow().AddTo(CurrentHtmlTable);
        var reportTd = new HtmlTableCell().AddTo(tr, "T");
        var reportTable = new HtmlTable().AddTo(reportTd);
        tr = new HtmlTableRow().AddTo(reportTable, "trReportDetail");
        new HtmlTableCell { Align = "center", InnerHtml = "First Login" }
          .AddTo(tr, "tdReportDetailHeading");
        new HtmlTableCell { Align = "center", InnerHtml = "Last Login" }
          .AddTo(tr, "tdReportDetailHeading");
        new HtmlTableCell { Align = "center", InnerHtml = "Hours" }
          .AddTo(tr, "tdReportDetailHeading");

        var totalDuration = TimeSpan.MinValue;
        var days = 0;
        var totalHours = 0;
        foreach (var dateGroup in data)
        {
          var date = dateGroup.ToList();
          var firstLogin = date.First().DateStamp();
          var lastLogin = date.Last().DateStamp();
          var duration = lastLogin - firstLogin;
          var hours = duration.Hours;
          tr = new HtmlTableRow().AddTo(reportTable, "trReportDetail");
          new HtmlTableCell { InnerText = firstLogin.ToString(CultureInfo.InvariantCulture) }
            .AddTo(tr, "tdReportDetail");
          new HtmlTableCell { InnerText = lastLogin.ToString(CultureInfo.InvariantCulture) }
            .AddTo(tr, "tdReportDetail");
          new HtmlTableCell { InnerText = hours.ToString(CultureInfo.InvariantCulture) }
            .AddTo(tr, "tdReportDetail");
          totalDuration = totalDuration + duration;
          days++;
          totalHours += hours;
        }

        tr = new HtmlTableRow().AddTo(reportTable, "trReportDetail");
        new HtmlTableCell { InnerText = "Total" }
          .AddTo(tr, "tdReportDetail");
        new HtmlTableCell { InnerText = $"Days: {days}"}
          .AddTo(tr, "tdReportDetail");
        new HtmlTableCell { InnerText = totalHours.ToString(CultureInfo.InvariantCulture) }
          .AddTo(tr, "tdReportDetail");

        return ReportContainer;
      }

      #endregion Private

      public static Control GetReport(string userName, DateTime startDate, DateTime endDate)
      {
        var reportObject = new LoginsReport();
        return reportObject.GenerateReport(userName, startDate, endDate);
      }
    }

    public abstract class BillingReport : TableBasedReport
    {
      protected class DateCount
      {
        public DateTime DateStamp;
        public int Count;
      }

      protected Control GenerateReport(IEnumerable<DateCount> dateList, string reportHeading,
        string columnHeading, int centsPerTransaction)
      {
        StartNewHtmlTable();
        CurrentHtmlTable.AddCssClasses("tableAdmin");
        var tr = new HtmlTableRow().AddTo(CurrentHtmlTable);
        new HtmlTableCell { InnerHtml = reportHeading }.AddTo(tr, "H");
        tr = new HtmlTableRow().AddTo(CurrentHtmlTable);
        var reportTd = new HtmlTableCell().AddTo(tr, "T");
        var reportTable = new HtmlTable().AddTo(reportTd);
        tr = new HtmlTableRow().AddTo(reportTable, "HTMLTr");
        new HtmlTableCell { Align = "center", InnerHtml = "Date" }
          .AddTo(tr, "tdReportDetailHeading");
        new HtmlTableCell { Align = "center", InnerHtml = columnHeading }
          .AddTo(tr, "tdReportDetailHeading");

        var totalTransactions = 0;
        foreach (var dateCount in dateList)
        {
          tr = new HtmlTableRow().AddTo(reportTable, "HTMLTr");
          new HtmlTableCell { InnerText = dateCount.DateStamp.ToString("MM/dd/yyyy") }
            .AddTo(tr, "tdReportDetail");
          new HtmlTableCell { InnerText = dateCount.Count.ToString(CultureInfo.InvariantCulture) }
            .AddTo(tr, "tdReportDetail");
          totalTransactions += dateCount.Count;
        }

        var total = totalTransactions * centsPerTransaction;
        tr = new HtmlTableRow().AddTo(reportTable, "HTMLTr");
        new HtmlTableCell { InnerText = "Total" }
          .AddTo(tr, "tdReportDetail");
        new HtmlTableCell { InnerText =
              $"{totalTransactions} * .{centsPerTransaction} = {total} cents" }
          .AddTo(tr, "tdReportDetail");

        return ReportContainer;
      }
    }

    public sealed class PoliticianAnswersReport : BillingReport
    {
      //private Control GenerateReport(string userName, DateTime startDate, DateTime endDate)
      //{
      //  var table1 =
      //    LogPoliticianAnswers.GetBillingDataByUserNameDateStampRange(userName,
      //      startDate, endDate);
      //  var table2 =
      //    LogDataChange.GetBillingDataByUserNameTableNameDateStampRange(
      //      userName, "Answers", startDate, endDate);

      //  var dateList = table1.Select(row => row.DateStamp.Date)
      //    .Concat(table2.Select(row => row.DateStamp.Date))
      //    .GroupBy(date => date)
      //    .Select(g => new DateCount { DateStamp = g.Key, Count = g.Count() });

      //  return GenerateReport(dateList, "Politician Answers", "Answers", 30);
      //}

      private Control GenerateReport(string userName, DateTime startDate, DateTime endDate)
      {
        var dateList = LogDataChange.GetBillingDataByUserNameTableNameDateStampRange(
            userName, "Answers", startDate, endDate).Select(row => row.DateStamp.Date)
          .GroupBy(date => date)
          .Select(g => new DateCount { DateStamp = g.Key, Count = g.Count() });

        return GenerateReport(dateList, "Politician Answers", "Answers", 30);
      }

      public static Control GetReport(string userName, DateTime startDate, DateTime endDate)
      {
        var reportObject = new PoliticianAnswersReport();
        return reportObject.GenerateReport(userName, startDate, endDate);
      }
    }

    public sealed class PoliticianAddsReport : BillingReport
    {
      //private Control GenerateReport(string userName, DateTime startDate, DateTime endDate)
      //{
      //  var table =
      //    LogPoliticianAdds.GetBillingDataByUserNameDateStampRange(userName,
      //      startDate, endDate);
      //  var dateList = table.GroupBy(row => row.DateStamp.Date)
      //    .Select(
      //      group => new DateCount { DateStamp = group.Key, Count = group.Count() });

      //  return GenerateReport(dateList, "Politician Adds", "Politician Adds", 20);
      //}

      private Control GenerateReport(string userName, DateTime startDate, DateTime endDate)
      {
        var dateList = LogDataChange.GetBillingDataByUserNameTableNameDateStampRange(
            userName, "Politicians", startDate, endDate)
          .Where(row => row.ColumnName == "*INSERT")
          .Select(row => row.DateStamp.Date)
          .GroupBy(date => date)
            .Select(g => new DateCount { DateStamp = g.Key, Count = g.Count() });

        return GenerateReport(dateList, "Politicians Adds", "Adds", 10);
      }

      public static Control GetReport(string userName, DateTime startDate, DateTime endDate)
      {
        var reportObject = new PoliticianAddsReport();
        return reportObject.GenerateReport(userName, startDate, endDate);
      }
    }

    public sealed class PoliticianChangesReport : BillingReport
    {
      //private Control GenerateReport(string userName, DateTime startDate, DateTime endDate)
      //{
      //  var table1 =
      //    LogPoliticianChanges.GetBillingDataByUserNameDateStampRange(userName,
      //      startDate, endDate);
      //  var table2 =
      //    LogDataChange.GetBillingDataByUserNameTableNameDateStampRange(
      //      userName, "Politicians", startDate, endDate);

      //  var dateList = table1.Select(row => row.DateStamp.Date)
      //    .Concat(table2.Select(row => row.DateStamp.Date))
      //    .GroupBy(date => date)
      //      .Select(g => new DateCount { DateStamp = g.Key, Count = g.Count() });

      //  return GenerateReport(dateList, "Politician Changes", "Politician Changes", 10);
      //}

      private Control GenerateReport(string userName, DateTime startDate, DateTime endDate)
      {
        var dateList = LogDataChange.GetBillingDataByUserNameTableNameDateStampRange(
            userName, "Politicians", startDate, endDate)
          .Where(row => row.ColumnName != "*INSERT")
          .Select(row => row.DateStamp.Date)
          .GroupBy(date => date)
            .Select(g => new DateCount { DateStamp = g.Key, Count = g.Count() });

        return GenerateReport(dateList, "Politicians Changes", "Changes", 10);
      }

      public static Control GetReport(string userName, DateTime startDate, DateTime endDate)
      {
        var reportObject = new PoliticianChangesReport();
        return reportObject.GenerateReport(userName, startDate, endDate);
      }
    }

    public sealed class ElectionPoliticianAddsAndDeletesReport : BillingReport
    {
      //private Control GenerateReport(string userName, DateTime startDate, DateTime endDate)
      //{
      //  var table =
      //    LogElectionPoliticianAddsDeletes.GetBillingDataByUserNameDateStampRange
      //      (userName, startDate, endDate);
      //  var dateList = table.GroupBy(row => row.DateStamp.Date)
      //    .Select(
      //      group => new DateCount { DateStamp = group.Key, Count = group.Count() });

      //  return GenerateReport(dateList, "Election Politician Adds and Deletes",
      //    "Election Politician Adds Deletes", 15);
      //}

      private Control GenerateReport(string userName, DateTime startDate, DateTime endDate)
      {
        var dateList = LogDataChange.GetBillingDataByUserNameTableNameDateStampRange(
            userName, "ElectionsPoliticians", startDate, endDate)
          .Select(row => row.DateStamp.Date)
          .GroupBy(date => date)
            .Select(g => new DateCount { DateStamp = g.Key, Count = g.Count() });

        return GenerateReport(dateList, "ElectionsPolitician Changes", "Changes", 10);
      }

      public static Control GetReport(string userName, DateTime startDate, DateTime endDate)
      {
        var reportObject = new ElectionPoliticianAddsAndDeletesReport();
        return reportObject.GenerateReport(userName, startDate, endDate);
      }
    }

    public sealed class ElectionOfficeChangesReport : BillingReport
    {
      //private Control GenerateReport(string userName, DateTime startDate, DateTime endDate)
      //{
      //  var table =
      //    LogElectionOfficeChanges.GetBillingDataByUserNameDateStampRange(
      //      userName, startDate, endDate);
      //  var dateList = table.GroupBy(row => row.DateStamp.Date)
      //    .Select(
      //      group => new DateCount { DateStamp = group.Key, Count = group.Count() });

      //  return GenerateReport(dateList, "Election Office Changes", "Election Office Changes", 15);
      //}

      private Control GenerateReport(string userName, DateTime startDate, DateTime endDate)
      {
        var dateList = LogDataChange.GetBillingDataByUserNameTableNameDateStampRange(
            userName, "ElectionsOffices", startDate, endDate)
          .Select(row => row.DateStamp.Date)
          .GroupBy(date => date)
            .Select(g => new DateCount { DateStamp = g.Key, Count = g.Count() });

        return GenerateReport(dateList, "ElectionsOffices Changes", "Changes", 10);
      }

      public static Control GetReport(string userName, DateTime startDate, DateTime endDate)
      {
        var reportObject = new ElectionOfficeChangesReport();
        return reportObject.GenerateReport(userName, startDate, endDate);
      }
    }

    public sealed class OfficeChangesReport : BillingReport
    {
      //private Control GenerateReport(string userName, DateTime startDate, DateTime endDate)
      //{
      //  var table =
      //    LogOfficeChanges.GetBillingDataByUserNameDateStampRange(userName,
      //      startDate, endDate);
      //  var dateList = table.GroupBy(row => row.DateStamp.Date)
      //    .Select(
      //      group => new DateCount { DateStamp = group.Key, Count = group.Count() });

      //  return GenerateReport(dateList, "Office Changes", "Office Changes", 10);
      //}

      private Control GenerateReport(string userName, DateTime startDate, DateTime endDate)
      {
        var dateList = LogDataChange.GetBillingDataByUserNameTableNameDateStampRange(
            userName, "Offices", startDate, endDate)
          .Select(row => row.DateStamp.Date)
          .GroupBy(date => date)
            .Select(g => new DateCount { DateStamp = g.Key, Count = g.Count() });

        return GenerateReport(dateList, "Offices Changes", "Changes", 10);
      }

      public static Control GetReport(string userName, DateTime startDate, DateTime endDate)
      {
        var reportObject = new OfficeChangesReport();
        return reportObject.GenerateReport(userName, startDate, endDate);
      }
    }

    public sealed class OfficeOfficialAddsDeletesReport : BillingReport
    {
      //private Control GenerateReport(string userName, DateTime startDate, DateTime endDate)
      //{
      //  var table =
      //    LogOfficeOfficialAddsDeletes.GetBillingDataByUserNameDateStampRange(
      //      userName, startDate, endDate);
      //  var dateList = table.GroupBy(row => row.DateStamp.Date)
      //    .Select(
      //      group => new DateCount { DateStamp = group.Key, Count = group.Count() });

      //  return GenerateReport(dateList, "Office Officials Adds", "Office Official Adds Deletes", 15);
      //}

      private Control GenerateReport(string userName, DateTime startDate, DateTime endDate)
      {
        var dateList = LogDataChange.GetBillingDataByUserNameTableNameDateStampRange(
            userName, "OfficesOfficials", startDate, endDate)
          .Where(row => row.ColumnName == "*INSERT")
          .Select(row => row.DateStamp.Date)
          .GroupBy(date => date)
            .Select(g => new DateCount { DateStamp = g.Key, Count = g.Count() });

        return GenerateReport(dateList, "OfficesOfficials Adds", "Adds", 10);
      }

      public static Control GetReport(string userName, DateTime startDate, DateTime endDate)
      {
        var reportObject = new OfficeOfficialAddsDeletesReport();
        return reportObject.GenerateReport(userName, startDate, endDate);
      }
    }

    public sealed class LogOfficeOfficialsChangesReport : BillingReport
    {
      //private Control GenerateReport(string userName, DateTime startDate, DateTime endDate)
      //{
      //  var table =
      //    LogOfficeOfficialChanges.GetBillingDataByUserNameDateStampRange(
      //      userName, startDate, endDate);
      //  var dateList = table.GroupBy(row => row.DateStamp.Date)
      //    .Select(
      //      group => new DateCount { DateStamp = group.Key, Count = group.Count() });

      //  return GenerateReport(dateList, "OfficesOfficials Changes", "Changes",
      //    15);
      //}

      private Control GenerateReport(string userName, DateTime startDate, DateTime endDate)
      {
        var dateList = LogDataChange.GetBillingDataByUserNameTableNameDateStampRange(
            userName, "OfficesOfficials", startDate, endDate)
          .Where(row => row.ColumnName != "*INSERT")
          .Select(row => row.DateStamp.Date)
          .GroupBy(date => date)
            .Select(g => new DateCount { DateStamp = g.Key, Count = g.Count() });

        return GenerateReport(dateList, "OfficesOfficials Changes", "Changes", 10);
      }

      public static Control GetReport(string userName, DateTime startDate, DateTime endDate)
      {
        var reportObject = new LogOfficeOfficialsChangesReport();
        return reportObject.GenerateReport(userName, startDate, endDate);
      }
    }

    public sealed class PoliticiansImagesOriginalReport : BillingReport
    {
      //private Control GenerateReport(string userName, DateTime startDate, DateTime endDate)
      //{
      //  var table1 =
      //    LogPoliticiansImagesOriginal.GetBillingDataByUserNameDateStampRange(
      //      userName, startDate, endDate);
      //  var table2 =
      //    LogDataChange.GetBillingDataByUserNameTableNameDateStampRange(
      //      userName, "PoliticiansImagesBlobs", startDate, endDate);

      //  var dateList = table1.Select(row => row.ProfileOriginalDate.Date)
      //    .Concat(table2.Select(row => row.DateStamp.Date))
      //    .GroupBy(date => date)
      //    .Select(g => new DateCount { DateStamp = g.Key, Count = g.Count() });

      //  return GenerateReport(dateList, "Picture Uploads", "Picture Uploads", 60);
      //}

      private Control GenerateReport(string userName, DateTime startDate, DateTime endDate)
      {
        var dateList =
          LogDataChange.GetBillingDataByUserNameTableNameDateStampRange(
            userName, "PoliticiansImagesBlobs", startDate, endDate)
            .Select(row => row.DateStamp.Date)
          .GroupBy(date => date)
          .Select(g => new DateCount { DateStamp = g.Key, Count = g.Count() });

        return GenerateReport(dateList, "Picture Uploads", "Picture Uploads", 60);
      }

      public static Control GetReport(string userName, DateTime startDate, DateTime endDate)
      {
        var reportObject = new PoliticiansImagesOriginalReport();
        return reportObject.GenerateReport(userName, startDate, endDate);
      }
    }

    public sealed class AdminDataUpdatesReport : BillingReport
    {
      private Control GenerateReport(string userName, DateTime startDate, DateTime endDate)
      {
        var table = LogAdminData.GetBillingDataByUserNameDateStampRange(
          userName, startDate, endDate);
        var dateList = table.GroupBy(row => row.DateStamp.Date)
          .Select(
            group => new DateCount { DateStamp = group.Key, Count = group.Count() });

        return GenerateReport(dateList, "Admin Data Updates", "Admin Data Updates", 20);
      }

      public static Control GetReport(string userName, DateTime startDate, DateTime endDate)
      {
        var reportObject = new AdminDataUpdatesReport();
        return reportObject.GenerateReport(userName, startDate, endDate);
      }
    }

  }
}
