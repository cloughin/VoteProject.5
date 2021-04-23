using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB.Vote;
using static System.String;

namespace Vote.Reports
{
  public sealed class PartiesElectionReport : PartiesReport
  {
    #region Private

    private readonly PartiesElectionReportDataManager _DataManager =
      new PartiesElectionReportDataManager();

    #region Private classes

    private sealed class PartiesElectionReportDataManager : ReportDataManager<DataRow>
    {
      public void GetData(string partyKey, string electionKey)
      {
        var electionKeys = new List<string>(new[] {electionKey});
        var altElectionKey = Elections.GetElectionKeyToInclude(electionKey);
        if (!IsNullOrWhiteSpace(altElectionKey)) electionKeys.Add(altElectionKey);
        DataTable = ElectionsPoliticians.GetPartyCandidatesInElectionsNew(electionKeys, partyKey);
      }
    }

    #endregion Private classes

    private Control GenerateReport(string partyKey, string electionKey, out int politicianCount)
    {
      _DataManager.GetData(partyKey, electionKey);
      var data = _DataManager.GetDataSubset();
      politicianCount = data.Count;

      StartNewHtmlTable();
      CurrentHtmlTable.AddCssClasses("tableAdmin");

      if (data.Count == 0)
      {
        new HtmlTableCell
        {
          InnerHtml = "No Party Candidates in the Election"
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

    public static Control GetReport(string partyKey, string electionKey, out int politicianCount)
    {
      var reportObject = new PartiesElectionReport();
      return reportObject.GenerateReport(partyKey, electionKey, out politicianCount);
    }
  }
}