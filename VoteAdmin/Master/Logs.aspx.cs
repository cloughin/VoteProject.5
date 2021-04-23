using System;
using Vote.Reports;
using static System.String;

namespace Vote.Logs
{
  public partial class LogsPage : SecurePage, ISuperUser
  {
    #region legacy

    public static string Fail(string msg)
    {
      return $"<span class=\"MsgFail\">****FAILURE**** {msg}</span>";
    }

    #endregion legacy

    protected void ButtonRunReport_Click(object sender, EventArgs e)
    {
      try
      {
        if (TextBoxLoginUser.Text.Trim() == Empty)
          throw new ApplicationException("The Login Username is empty.");
        if (!TextBoxFrom.Text.Trim().IsValidDate())
          throw new ApplicationException("The From Date is not valid.");
        if (!TextBoxTo.Text.Trim().IsValidDate())
          throw new ApplicationException("The To Date is not valid.");

        LabelUserName.Text = $"Login User Name: {TextBoxLoginUser.Text.Trim()}" +
          $" From:{TextBoxFrom.Text.Trim()} To: {TextBoxTo.Text.Trim()}";

        var userName = TextBoxLoginUser.Text.Trim();

        var startDate = Convert.ToDateTime(TextBoxFrom.Text.Trim());
        var endDate = Convert.ToDateTime(TextBoxTo.Text.Trim()).AddDays(1);

        if (CheckBoxListLogs.Items[0].Selected)
          LogsReports.LoginsReport.GetReport(userName, startDate, endDate)
            .AddTo(ReportsPlaceHolder);

        if (CheckBoxListLogs.Items[1].Selected)
          LogsReports.PoliticianAnswersReport.GetReport(userName, startDate, endDate)
            .AddTo(ReportsPlaceHolder);

        if (CheckBoxListLogs.Items[2].Selected)
          LogsReports.PoliticianAddsReport.GetReport(userName, startDate, endDate)
            .AddTo(ReportsPlaceHolder);

        if (CheckBoxListLogs.Items[3].Selected)
          LogsReports.PoliticianChangesReport.GetReport(userName, startDate, endDate)
            .AddTo(ReportsPlaceHolder);

        if (CheckBoxListLogs.Items[4].Selected)
          LogsReports.ElectionPoliticianAddsAndDeletesReport
            .GetReport(userName, startDate, endDate).AddTo(ReportsPlaceHolder);

        if (CheckBoxListLogs.Items[6].Selected)
          LogsReports.ElectionOfficeChangesReport.GetReport(userName, startDate, endDate)
            .AddTo(ReportsPlaceHolder);

        if (CheckBoxListLogs.Items[7].Selected)
          LogsReports.OfficeChangesReport.GetReport(userName, startDate, endDate)
            .AddTo(ReportsPlaceHolder);

        if (CheckBoxListLogs.Items[8].Selected)
          LogsReports.OfficeOfficialAddsDeletesReport
            .GetReport(userName, startDate, endDate).AddTo(ReportsPlaceHolder);

        if (CheckBoxListLogs.Items[9].Selected)
          LogsReports.LogOfficeOfficialsChangesReport
            .GetReport(userName, startDate, endDate).AddTo(ReportsPlaceHolder);

        if (CheckBoxListLogs.Items[10].Selected)
          LogsReports.PoliticiansImagesOriginalReport
            .GetReport(userName, startDate, endDate).AddTo(ReportsPlaceHolder);

        if (CheckBoxListLogs.Items[11].Selected)
          LogsReports.AdminDataUpdatesReport.GetReport(userName, startDate, endDate)
            .AddTo(ReportsPlaceHolder);
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      Msg.Text = Empty;
      if (!IsPostBack)
      {
        if (!IsMasterUser)
          HandleSecurityException();

        Title = H1.InnerText = "Logs";
      }
    }

    protected void ButtonCheckAll_Click(object sender, EventArgs e)
    {
      try
      {
        for (var i = 0; i <= 11; i++)
          CheckBoxListLogs.Items[i].Selected = true;
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }
  }
}