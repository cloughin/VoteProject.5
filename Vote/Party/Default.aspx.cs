using System;
using DB.Vote;
using DB.VoteLog;
using Vote.Reports;

namespace Vote.Party
{
  public partial class Default : SecurePartyPage
  {
    #region from db

    public static string Fail(string msg)
    {
      return "<span class=" + "\"" + "MsgFail" + "\"" + ">"
        + "****FAILURE**** " + msg + "</span>";
    }

    public static void Log_Error_Admin(Exception ex, string message = null)
    {
      var logMessage = string.Empty;
      var stackTrace = string.Empty;
      if (ex != null)
      {
        logMessage = ex.Message;
        stackTrace = ex.StackTrace;
      }
      if (!string.IsNullOrWhiteSpace(message))
      {
        if (!string.IsNullOrWhiteSpace(logMessage))
          logMessage += " :: ";
        logMessage += message;
      }
      LogErrorsAdmin.Insert(DateTime.Now, UrlManager.GetCurrentPathUri(true).ToString(),
        logMessage, stackTrace);
    }

    #endregion from db

    protected void GenerateOfficialsReport()
    {
      int politicianCount;
      PartiesOfficialsReport.GetReport(PartyKey, StateCode, out politicianCount)
        .AddTo(ReportPlaceHolder);
      ReportLabel.Text = politicianCount + " Party Elected Officials";
      ReportContainer.Visible = true;
    }

    protected void GenerateElectionReport()
    {
      var electionKey = Elections.GetNextViewableElectionForParty(PartyKey, StateCode);
      int politicianCount;
      PartiesElectionReport.GetReport(PartyKey, electionKey, out politicianCount)
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
        Log_Error_Admin(ex);
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
        Log_Error_Admin(ex);
      }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        var partyName = Parties.GetPartyName(PartyKey);
        Page.Title = partyName;
        H1.InnerHtml = partyName;

        try
        {
          var electionKey = Elections.GetNextViewableElectionForParty(PartyKey, StateCode);

          if (!string.IsNullOrEmpty(electionKey))
          {
            Table_Election_Candidates.Visible = true;
            Label_Election_Candidates.Text = Elections.GetElectionDesc(electionKey);
          }
        }
        catch (Exception ex)
        {
          Msg.Text = Fail(ex.Message);
          Log_Error_Admin(ex);
        }
      }
    }
  }
}