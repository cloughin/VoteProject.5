using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB;
using DB.Vote;

namespace Vote.Reports
{
  internal sealed class IncumbentsReport : Report
  {
    #region Private

    private readonly IncumbentsReportDataManager _DataManager =
      new IncumbentsReportDataManager();

    private const int ImageSize = 25;
    private HtmlTable _MainHtmlTable;

    private sealed class NotRunningMate : ReportDataManager<DataRow>.FilterBy
    {
      public override bool Filter(DataRow row)
      {
        return !row.IsRunningMate();
      }
    }

    private sealed class IncumbentsReportDataManager : ReportDataManager<DataRow>
    {
      public void GetData(string officeKey)
      {
        DataTable = Offices.GetIncumbentReportData(officeKey);
      }

      public DataRow GetRunningMate(string politicianKey)
      {
        return DataTable.Rows.Cast<DataRow>()
          .FirstOrDefault(row => row.RunningMateKey()
            .IsEqIgnoreCase(politicianKey) && row.IsRunningMate());
      }
    }

    private Control GenerateReport(string officeKey)
    {
      _DataManager.GetData(officeKey);
      var incumbents = _DataManager.GetDataSubset(new NotRunningMate());

      _MainHtmlTable = new HtmlTable
        {
          CellSpacing = 0,
          CellPadding = 0,
          Border = 1,
          Width = "100%"
        };

      var tr = new HtmlTableRow().AddTo(_MainHtmlTable);
      new HtmlTableCell {InnerHtml = "ID (PoliticianKey)"}.AddTo(tr, "TLargeBold");
      new HtmlTableCell {InnerHtml = "Edit State Admin Info"}.AddTo(tr,
        "TLargeBold");
      if (SecurePage.IsMasterUser)
        new HtmlTableCell {InnerHtml = "Edit Politician Intro Page"}.AddTo(tr,
          "TLargeBold");

      if (incumbents.Count == 0)
      {
        tr = new HtmlTableRow().AddTo(_MainHtmlTable, "trReportDetail");
        new HtmlTableCell {InnerHtml = "No Incumbents Identified", ColSpan = 2}
          .AddTo(tr, "TextBoxInput");
      }
      else
        foreach (var incumbent in incumbents)
        {
          var partyCode = incumbent.PartyCode();
          ReportOneIncumbent(incumbent, partyCode);

          if (!incumbent.IsRunningMateOffice()) continue;

          var runningMate = _DataManager.GetRunningMate(incumbent.RunningMateKey());
          if (runningMate != null)
            ReportOneIncumbent(runningMate, partyCode);
          else
          {
            tr = new HtmlTableRow().AddTo(_MainHtmlTable, "trReportDetail");
            new HtmlTableCell {InnerHtml = "(No Running Mate Identified)"}.AddTo(
              tr, "TLargeBoldColor");
          }
        }

      return _MainHtmlTable;
    }

    private void ReportOneIncumbent(DataRow incumbent, string partyCode)
    {
      var politicianKey = incumbent.PoliticianKey();

      var tr = new HtmlTableRow().AddTo(_MainHtmlTable, "trReportDetail");
      new HtmlTableCell {InnerHtml = politicianKey}.AddTo(tr, "TLargeBoldColor");

      var td = new HtmlTableCell().AddTo(tr, "TLargeBoldColor");
      CreateAdminPoliticianAnchor(incumbent,
        FormatNameWithParty(incumbent, partyCode))
        .AddTo(td);

      td = new HtmlTableCell().AddTo(tr);
      if (!SecurePage.IsMasterUser) return;

      CreatePoliticianImageAnchor(
        SecurePoliticianPage.GetUpdateIntroPageUrl(politicianKey), politicianKey,
        ImageSize,
        Politicians.GetFormattedName(politicianKey) + " Intro Page Data Entry",
        "intro")
        .AddTo(td);
      new HtmlNbsp().AddTo(td);
    }

    private static string FormatNameWithParty(DataRow politician, string partyCode)
    {
      var result = Politicians.FormatName(politician, true);
      if (!string.IsNullOrWhiteSpace(partyCode))
        result += " (" + partyCode + ")";
      return result;
    }

    #endregion Private

    #region Public

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnassignedField.Global

    #endregion ReSharper disable

    public static Control GetReport(string officeKey)
    {
      var reportObject = new IncumbentsReport();
      return reportObject.GenerateReport(officeKey);
    }

    #region ReSharper restore

    // ReSharper restore UnassignedField.Global
    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Public
  }
}