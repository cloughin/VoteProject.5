using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB;
using DB.Vote;
using static System.String;

namespace Vote.Reports
{
  public sealed class CountyJurisdictionsReport : TableBasedReport
  {
    #region Private

    private readonly LocalJurisdictionsReportDataManager _DataManager =
      new LocalJurisdictionsReportDataManager();

    #region Private classes

    private sealed class LocalJurisdictionsReportDataManager : ReportDataManager<DataRow>
    {
      public void GetData(string stateCode)
      {
        DataTable = Counties.GetJurisdictionsReportData(stateCode);
      }
    }

    #endregion Private classes

    private static Control MakeContactEmailLink(string contact, string email)
    {
      var result = new PlaceHolder();
      if (!IsNullOrWhiteSpace(contact))
        new LiteralControl(contact).AddTo(result);
      if (!IsNullOrWhiteSpace(email))
      {
        if (result.Controls.Count > 0)
          new HtmlBreak().AddTo(result);
        new HtmlAnchor
        {
          InnerText = email,
          HRef = "mailto:" + email
        }.AddTo(result);
      }
      if (result.Controls.Count == 0)
        new HtmlNbsp().AddTo(result);
      return result;
    }

    private static Control MakeWebAddressLink(string url)
    {
      if (IsNullOrWhiteSpace(url))
        return new HtmlNbsp();
      return new HtmlAnchor
      {
        HRef = VotePage.NormalizeUrl(url),
        Target = "_blank",
        InnerText = url
      };
    }

    private static Control MakeUpdateJurisdictionsLink(DataRow row)
    {
      return new HtmlAnchor
      {
        HRef = SecureAdminPage.GetUpdateJurisdictionsPageUrl(row.StateCode(),
         row.CountyCode()),
        Target = "_blank",
        InnerText = row.County()
      };
    }

    //private static Control MakeEmailLink(string email)
    //{
    //  if (IsNullOrWhiteSpace(email))
    //    return new HtmlNbsp();
    //  return new HtmlAnchor
    //  {
    //    InnerHtml = email,
    //    HRef = "mailto:" + email
    //  };
    //}

    private Control GenerateReport(string stateCode)
    {
      _DataManager.GetData(stateCode);

      StartNewHtmlTable();
      CurrentHtmlTable.AddCssClasses("local-jurisdictions-report simple-table-report");

      var tr = new HtmlTableRow().AddTo(CurrentHtmlTable, "th");
      new HtmlTableCell { InnerText = "County Name" }.AddTo(tr, "th");
      new HtmlTableCell { InnerText = "Web Address" }.AddTo(tr, "th");
      new HtmlTableCell { InnerText = "Main Contact and Email" }.AddTo(tr, "th");
      new HtmlTableCell {InnerText = "Alternate Contact and Email"}.AddTo(tr, "th");

      var oddEven = "odd";
      var emptyWebAddresses = 0;
      var emptyMainContact = 0;
      var emptyMailEmail = 0;
      var emptyAltContact = 0;
      var emptyAltEmail = 0;
      var total = 0;
      foreach (var row in _DataManager.GetDataSubset())
      {
        tr = new HtmlTableRow().AddTo(CurrentHtmlTable, oddEven);
        oddEven = oddEven == "odd"
          ? "even"
          : "odd";
          MakeUpdateJurisdictionsLink(row).AddTo(new HtmlTableCell().AddTo(tr));
        MakeWebAddressLink(row.Url()).AddTo(new HtmlTableCell().AddTo(tr));
        MakeContactEmailLink(row.Contact(), row.ContactEmail()).AddTo(new HtmlTableCell().AddTo(tr));
        MakeContactEmailLink(row.AltContact(), row.AltEmail()).AddTo(new HtmlTableCell().AddTo(tr));

        if (IsNullOrWhiteSpace(row.Url())) emptyWebAddresses++;
        if (IsNullOrWhiteSpace(row.Contact())) emptyMainContact++;
        if (IsNullOrWhiteSpace(row.ContactEmail())) emptyMailEmail++;
        if (IsNullOrWhiteSpace(row.AltContact())) emptyAltContact++;
        if (IsNullOrWhiteSpace(row.AltEmail())) emptyAltEmail++;
        total++;
      }

      tr = new HtmlTableRow().AddTo(CurrentHtmlTable, oddEven);
      new HtmlTableCell { InnerHtml = $"<b>TOTAL EMPTY ENTRIES</b> (of {total})" }.AddTo(tr);
      new HtmlTableCell { InnerText = emptyWebAddresses.ToString() }.AddTo(tr);
      new HtmlTableCell { InnerText = emptyMainContact + "/" + emptyMailEmail }.AddTo(tr);
      new HtmlTableCell { InnerText = emptyAltContact + "/" + emptyAltEmail }.AddTo(tr);

      return ReportContainer;
    }

    #endregion Private

    public static Control GetReport(string stateCode)
    {
      var reportObject = new CountyJurisdictionsReport();
      return reportObject.GenerateReport(stateCode);
    }
  }
}