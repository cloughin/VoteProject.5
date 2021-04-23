using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB;
using DB.Vote;

namespace Vote.Reports
{
  internal sealed class CountiesReport : TableBasedReport
  {
    #region Private

    private readonly CountiesReportDataManager _DataManager =
      new CountiesReportDataManager();

    #region Private classes

    private sealed class CountiesReportDataManager : ReportDataManager<DataRow>
    {
      public void GetData(string stateCode)
      {
        DataTable = Counties.GetCountiesReportData(stateCode);
      }
    }

    #endregion Private classes

    private static Control MakeEmailLink(string email)
    {
      if (string.IsNullOrWhiteSpace(email))
        return new HtmlNbsp();
      return new HtmlAnchor
      {
        InnerHtml = email,
        HRef = "mailto:" + email
      };
    }

    private Control GenerateReport(string stateCode)
    {
      _DataManager.GetData(stateCode);

      StartNewHtmlTable();
      CurrentHtmlTable.AddCssClasses("counties-report simple-table-report");

      var tr = new HtmlTableRow().AddTo(CurrentHtmlTable, "th");
      new HtmlTableCell {InnerHtml = "County Name"}.AddTo(tr, "th");
      new HtmlTableCell {InnerHtml = "Contact"}.AddTo(tr, "th");
      new HtmlTableCell {InnerHtml = "Main Email"}.AddTo(tr, "th");
      new HtmlTableCell {InnerHtml = "Alternate Contact"}.AddTo(tr, "th");
      new HtmlTableCell {InnerHtml = "Alternate Email"}.AddTo(tr, "th");

      var oddEven = "odd";
      foreach (var county in _DataManager.GetDataSubset())
      {
        tr = new HtmlTableRow().AddTo(CurrentHtmlTable, oddEven);
        oddEven = oddEven == "odd"
          ? "even"
          : "odd";
        new HtmlTableCell {InnerHtml = county.County()}.AddTo(tr);
        new HtmlTableCell {InnerHtml = county.Contact()}.AddTo(tr);
        MakeEmailLink(county.ContactEmail()).AddTo(new HtmlTableCell().AddTo(tr));
        new HtmlTableCell {InnerHtml = county.AltContact()}.AddTo(tr);
        MakeEmailLink(county.AltEmail()).AddTo(new HtmlTableCell().AddTo(tr));
      }

      return ReportContainer;
    }

    #endregion Private

    public static Control GetReport(string stateCode)
    {
      var reportObject = new CountiesReport();
      return reportObject.GenerateReport(stateCode);
    }
  }
}