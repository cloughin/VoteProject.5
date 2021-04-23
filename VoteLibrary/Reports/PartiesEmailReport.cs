using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB;
using DB.Vote;

namespace Vote.Reports
{
  public class PartiesEmailReport : TableBasedReport 
  {
    #region Private

    private readonly PartiesEmailReportReportDataManager _DataManager =
      new PartiesEmailReportReportDataManager();

    #region Private classes

    private sealed class PartiesEmailReportReportDataManager : ReportDataManager<DataRow>
    {
      public void GetData(string partyKey)
      {
        DataTable = PartiesEmails.GetPartiesEmailReportData(partyKey);
      }
    }

    protected void CreateHeadingRow()
    {
      var tr = new HtmlTableRow().AddTo(CurrentHtmlTable, "trReportDetailHeading");
      new HtmlTableCell { InnerText = "Email Address" }.AddTo(tr, "tdReportDetailHeading");
      new HtmlTableCell { InnerText = "Phone" }.AddTo(tr, "tdReportDetailHeading");
      new HtmlTableCell { InnerText = "First Name" }.AddTo(tr, "tdReportDetailHeading");
      new HtmlTableCell { InnerText = "Last Name" }.AddTo(tr, "tdReportDetailHeading");
      new HtmlTableCell { InnerText = "Title" }.AddTo(tr, "tdReportDetailHeading");
      new HtmlTableCell { InnerText = "Password" }.AddTo(tr, "tdReportDetailHeading");
    }

    protected void ReportEmail(DataRow row)
    {
      var tr = new HtmlTableRow().AddTo(CurrentHtmlTable, "trReportDetailHeading");
      var td = new HtmlTableCell().AddTo(tr, "tdReportDetail");
      new HtmlAnchor
      {
        HRef = SecureAdminPage.GetPartiesPageUrl(Parties.GetStateCodeFromKey(row.PartyKey()),
          row.PartyKey(), row.PartyEmail()),
        Target = "edit",
        InnerText = row.PartyEmail()
      }.AddTo(td);
      new HtmlTableCell { InnerText = row.PartyContactPhone() }.AddTo(tr, "tdReportDetail");
      new HtmlTableCell { InnerText = row.PartyContactFName() }.AddTo(tr, "tdReportDetail");
      new HtmlTableCell { InnerText = row.PartyContactLName() }.AddTo(tr, "tdReportDetail");
      new HtmlTableCell { InnerText = row.PartyContactTitle() }.AddTo(tr, "tdReportDetail");
      new HtmlTableCell { InnerText = row.PartyPassword() }.AddTo(tr, "tdReportDetail");
    }

    #endregion Private classes

    private Control GenerateReport(string partyKey)
    {
      _DataManager.GetData(partyKey);
      var data = _DataManager.GetDataSubset();

      StartNewHtmlTable();
      CurrentHtmlTable.RemoveCssClass("tableAdmin");

      if (data.Count == 0)
      {
        new HtmlTableCell
        {
          InnerHtml = "There are No Email Addresses for this Party."
        }.AddTo(new HtmlTableRow().AddTo(CurrentHtmlTable, "trReportGroupHeading center"),
          "tdReportGroupHeading");
      }
      else
      {
        CreateHeadingRow();
        foreach (var email in data)
          ReportEmail(email);
      }

      return ReportContainer;
    }

    #endregion Private

    public static Control GetReport(string partyKey)
    {
      var reportObject = new PartiesEmailReport();
      return reportObject.GenerateReport(partyKey);
    }
  }
}
