using System;
using System.Collections.Generic;
using DB.Vote;
using Vote.Reports;
using static System.String;

namespace Vote.Admin
{
  public partial class ElectionPage : SecureAdminPage
  {
    private readonly string _ElectionKey = QueryElection;
    private string _ElectionDescription;
    private string _StateCode;

    public override IEnumerable<string> NonStateCodesAllowed => new[] { "U1", "U2", "U3", "U4", "PP", "US" };

    private void PopulateMetaTags()
    {
      Title = $"Admin Election Report - {_ElectionDescription}";
    }

    private void FillInReport()
    {
      var report = AdminElectionReport.GetReport(_ElectionKey);
      report.AddTo(ElectionPlaceHolder);
    }

    private void FillInTitles()
    {
      var classDescription = StateCache.IsValidStateCode(_StateCode)
        ? Elections.GetElectoralClassDescription(_ElectionKey)
        : Empty;
      ElectionTitle.InnerText = $"{classDescription} Admin Election Report";
      ElectionSubTitle.InnerText = _ElectionDescription;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      _ElectionDescription = PageCache.Elections.GetElectionDesc(_ElectionKey);
      // This could be a county election with no county offices, just local links,
      // in which case the county Elections row won't exist. In that case,
      // use the state description
      if (IsNullOrWhiteSpace(_ElectionDescription))
        _ElectionDescription = PageCache.Elections.GetElectionDesc(Elections.GetStateElectionKeyFromKey(_ElectionKey));
      _StateCode = Elections.GetStateCodeFromKey(_ElectionKey);
      if (AdminPageLevel == AdminPageLevel.Local)
        FormatMultiCountiesMessage(MuliCountyMessage);

      PopulateMetaTags();
      FillInTitles();
      FillInReport();

      var iframe = GetQueryString("iframe").IsEqIgnoreCase("y")
        ? " for-iframe"
        : Empty;

      Master.FindControl("Body").AddCssClasses($"open-all-accordions{iframe}");
    }
  }
}