using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB;
using DB.Vote;
using static System.String;

namespace Vote.Reports
{
  public class PartiesSummaryReport : TableBasedReport 
  {
    #region Private

    private readonly PartiesSummaryReportDataManager _DataManager =
      new PartiesSummaryReportDataManager();

    #region Private classes

    private sealed class PartiesSummaryReportDataManager : ReportDataManager<DataRow>
    {
      public void GetData(string stateCode)
      {
        DataTable = Parties.GetPartiesSummaryReportData(stateCode);
      }
    }

    private sealed class MajorPartyFilter : ReportDataManager<DataRow>.FilterBy
    {
      private readonly bool _IsMajor;

      public MajorPartyFilter(bool isMajor)
      {
        _IsMajor = isMajor;
      }

      public override bool Filter(DataRow row)
      {
        return row.IsPartyMajor() == _IsMajor;
      }
    }

    protected void CreateHeadingRow(bool isMajor)
    {
      var tr = new HtmlTableRow().AddTo(CurrentHtmlTable, "trReportDetailHeading");
      new HtmlTableCell { InnerHtml = "Order" }.AddTo(tr, "tdReportDetailHeading");
      new HtmlTableCell { InnerHtml = "Party<br>Key" }.AddTo(tr, "tdReportDetailHeading");
      new HtmlTableCell { InnerHtml = "Ballot<br>Code" }.AddTo(tr, "tdReportDetailHeading");
      new HtmlTableCell { InnerHtml = isMajor ? "Major Parties" : "Minor Parties" } 
        .AddTo(tr, "tdReportDetailHeading");
      new HtmlTableCell { InnerHtml = "Web Address" }.AddTo(tr, "tdReportDetailHeading");
    }

    protected void ReportParty(DataRow row)
    {
      var tr = new HtmlTableRow().AddTo(CurrentHtmlTable, "trReportDetail");
      new HtmlTableCell { InnerText = row.PartyOrder().ToString() }
        .AddTo(tr, "tdReportDetail");
      new HtmlTableCell { InnerText = row.PartyKey() }
        .AddTo(tr, "tdReportDetail");
      new HtmlTableCell { InnerText = row.PartyCode() }
        .AddTo(tr, "tdReportDetail");
      var td = new HtmlTableCell().AddTo(tr, "tdReportDetail");
      new HtmlAnchor
      {
        HRef = SecureAdminPage.GetPartiesPageUrl(row.StateCode(), row.PartyKey()),
        InnerText = row.PartyName(),
        Target = "edit"
      }.AddTo(td);
      td = new HtmlTableCell().AddTo(tr, "tdReportDetail");
      var url = row.PartyUrl();
      if (IsNullOrWhiteSpace(url))
        td.InnerHtml = "&nbsp;";
      else
        new HtmlAnchor
        {
          HRef = VotePage.NormalizeUrl(url),
          InnerText = row.PartyUrl(),
          Title = "Party Wensite",
          Target = "view"
        }.AddTo(td);
    }

    #endregion Private classes

    private Control GenerateReport(string stateCode)
    {
      ReportUser = ReportUser.Admin;
      _DataManager.GetData(stateCode);

      StartNewHtmlTable();
      CurrentHtmlTable.RemoveCssClass("tableAdmin");

      foreach (var isMajor in new [] { true, false})
      {
        var data = _DataManager.GetDataSubset(new MajorPartyFilter(isMajor));
        CreateHeadingRow(isMajor);
        foreach (var party in data)
          ReportParty(party);
      }

      return ReportContainer;
    }

    #endregion Private

    public static Control GetReport(string stateCode)
    {
      var reportObject = new PartiesSummaryReport();
      return reportObject.GenerateReport(stateCode);
    }
  }
}
