using System;
using DB.Vote;
using Vote.Reports;
using static System.String;

namespace Vote.Party
{
  public partial class Default : SecurePartyPage
  {
    #region legacy

    public static string Fail(string msg)
    {
      return "<span class=" + "\"" + "MsgFail" + "\"" + ">"
        + "****FAILURE**** " + msg + "</span>";
    }

    #endregion legacy

    protected void GenerateOfficialsReport()
    {
      PartiesOfficialsReport.GetReport(PartyKey, StateCode, out var politicianCount)
        .AddTo(ReportPlaceHolder);
      ReportLabel.Text = politicianCount + " Party Elected Officials";
      ReportContainer.Visible = true;
    }

    protected void GenerateElectionReport()
    {
      var electionKey = Elections.GetNextViewableElectionForParty(PartyKey, StateCode);
      PartiesElectionReport.GetReport(PartyKey, electionKey, out var politicianCount)
        .AddTo(ReportPlaceHolder);
      ReportLabel.Text = politicianCount + " Party Candidates";
      ReportContainer.Visible = true;
    }

    protected void Button_Elected_Representatives_Click(object sender, EventArgs e)
    {
      try
      {
        GenerateOfficialsReport();
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    protected void Button_Election_Candidates_Click(object sender, EventArgs e)
    {
      try
      {
        GenerateElectionReport();
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        var partyName = Parties.GetPartyName(PartyKey);
        Page.Title = partyName;
        H1.InnerHtml = partyName;

        // Set temp nocache
        SetNoCacheForState();

        try
        {
          var electionKey = Elections.GetNextViewableElectionForParty(PartyKey, StateCode);

          if (!IsNullOrEmpty(electionKey))
          {
            Table_Election_Candidates.Visible = true;
            Label_Election_Candidates.Text = Elections.GetElectionDesc(electionKey);
          }
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