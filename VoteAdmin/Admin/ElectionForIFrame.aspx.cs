using System;
using DB.Vote;
using Vote.Reports;

namespace Vote.Admin
{
  public partial class ElectionForIFramePage : SecureAdminPage
  {
    private readonly string _ElectionKey = QueryElection;
    private string _ElectionDescription;
    private string _StateCode;

    private void PopulateMetaTags()
    {
      Title = $"Admin Election Report - {_ElectionDescription}";
    }

    private void FillInReport()
    {
      var report = AdminElectionReport.GetReport(_ElectionKey);
      report.AddTo(ElectionPlaceHolder);
    }

    private void FillInAdditionalInfo()
    {
      var additionalInfo =
        PageCache.Elections.GetElectionAdditionalInfo(_ElectionKey);
      if (string.IsNullOrWhiteSpace(additionalInfo))
        additionalInfo = ElectionsDefaults.GetElectionAdditionalInfo(
          Elections.GetDefaultElectionKeyFromKey(_ElectionKey));

      if (string.IsNullOrWhiteSpace(additionalInfo))
        AdditionalInfo.Visible = false;
      else
      {
        AdditionalInfo.InnerHtml = additionalInfo.ReplaceBreakTagsWithSpaces().ReplaceNewLinesWithParagraphs();
        AdditionalInfo.Visible = true;
      }
    }

    private void FillInTitles()
    {
      var classDescription = StateCache.IsValidStateCode(_StateCode)
        ? Elections.GetElectoralClassDescription(_ElectionKey)
        : string.Empty;
      ElectionTitle.InnerText = $"{classDescription} Admin Election Report";
      ElectionSubTitle.InnerText = _ElectionDescription;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      _ElectionDescription = PageCache.Elections.GetElectionDesc(_ElectionKey);
      // This could be a county election with no county offices, just local links,
      // in which case the county Elections row won't exist. In that case,
      // use the state description
      if (string.IsNullOrWhiteSpace(_ElectionDescription))
        _ElectionDescription = PageCache.Elections.GetElectionDesc(Elections.GetStateElectionKeyFromKey(_ElectionKey));
      _StateCode = Elections.GetStateCodeFromKey(_ElectionKey);

      PopulateMetaTags();
      FillInTitles();
      FillInAdditionalInfo();
      FillInReport();

      Master.FindControl("Body").AddCssClasses("open-all-accordions");
    }
  }
}