using System;
using System.Collections.Generic;
using System.Data;
using DB;
using DB.Vote;
using Vote.Reports;
using static System.String;

namespace Vote.Admin
{
  public partial class Officials : SecureAdminPage
  {
    #region legacy

    public static string Fail(string msg) => $"<span class=\"MsgFail\">****FAILURE**** {msg}</span>";

    #endregion legacy

    private void Page_Title()
    {
      PageTitle.Text = Offices.GetElectoralClassDescription(StateCode,
        CountyCode, LocalKey);
    }

    private void Visible_Controls()
    {
      TableIncumbentStatusRadioButtons.Visible =
        Offices.IsElectoralClassStateCountyOrLocal(StateCode, CountyCode, LocalKey);
      TablePoliticianData.Visible = IsMasterUser;
    }

    private void ShowOfficialsReport()
    {
      OfficialsReport.GetReport(Report.SignedInReportUser, StateCode, CountyCode, LocalKey)
        .AddTo(ReportPlaceHolder);
    }

    public override IEnumerable<string> NonStateCodesAllowed => new[] {"U1", "U2", "U3", "U4"};

    public static string GetElectionsWinnersIdentifiedList(string stateCode)
    {
      var dateLastGeneralElection = Elections.GetLatestPastGeneralElectionDateByStateCode(stateCode);
      var electionsWinnersIdentified = Empty;

      if (dateLastGeneralElection != null)
      {
        var tableElections =
          Elections.GetWinnersIdentified(stateCode, dateLastGeneralElection.Value);
        foreach (DataRow rowElection in tableElections.Rows)
        {
          electionsWinnersIdentified += "<br>";
          if (rowElection.IsWinnersIdentified())
            electionsWinnersIdentified +=
              "Winners <span style=color:green>HAVE</span> been Identified";
          else
            electionsWinnersIdentified +=
              "Winners have <span style=color:red><strong>NOT</strong></span> been Identified";
          electionsWinnersIdentified += $" - {rowElection.ElectionDescription()}";
        }
      }
      return electionsWinnersIdentified;
    }

    private void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        H1.InnerHtml = Page.Title = "Incumbents";

        try
        {
          Visible_Controls();

          var stateCode = GetStateCode();
          HyperLink_View_Public_Report.NavigateUrl =
            UrlManager.GetOfficialsPageUri(stateCode).ToString();

          Page_Title();

          Label_Report_Elections_Winners_Identified.Text =
            GetElectionsWinnersIdentifiedList(StateCode);

          ShowOfficialsReport();
        }
        catch (Exception ex)
        {
          Msg.Text = Fail(ex.Message);
          LogAdminError(ex);
        }
      }
    }
  }
}