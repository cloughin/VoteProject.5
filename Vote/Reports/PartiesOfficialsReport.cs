using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB.Vote;

namespace Vote.Reports
{
  internal sealed class PartiesOfficialsReport : PartiesReport
  {
    #region Private

    private readonly PartiesOfficialsReportDataManager _DataManager =
      new PartiesOfficialsReportDataManager();

    #region Private classes

    private sealed class PartiesOfficialsReportDataManager : ReportDataManager<DataRow>
    {
      public void GetData(string partyKey, string stateCode)
      {
        DataTable = OfficesOfficials.GetPartyIncumbentsByState(stateCode, partyKey);
      }
    }

    #endregion Private classes

    private Control GenerateReport(string partyKey, string stateCode, out int politicianCount)
    {
      _DataManager.GetData(partyKey, stateCode);
      var data = _DataManager.GetDataSubset();
      politicianCount = data.Count;

      StartNewHtmlTable();
      CurrentHtmlTable.AddCssClasses("tableAdmin");

      if (data.Count == 0)
      {
        new HtmlTableCell
        {
          InnerHtml = "No Elected Party Members"
        }.AddTo(new HtmlTableRow().AddTo(CurrentHtmlTable, "trReportGroupHeading center"),
          "tdReportGroupHeading");
      }
      else
      {
        CreateHeadingRow();
        foreach (var row in data)
          FormatPoliticianRow(row);
      }

      return ReportContainer;
    }

    #endregion Private

    public static Control GetReport(string partyKey, string stateCode, out int politicianCount)
    {
      var reportObject = new PartiesOfficialsReport();
      return reportObject.GenerateReport(partyKey, stateCode, out politicianCount);
    }
  }
}