using System;
using System.Globalization;
using System.Web.UI.WebControls;
using DB.Vote;

namespace Vote.Master
{
  public partial class SitemapPage : VotePage
  {
    /*
    private static long Domain_Urls(string domainDataCode)
    {
      return Sitemap.GetUrlsDefault(domainDataCode, 0) +
        Sitemap.GetUrlsElection(domainDataCode, 0) +
        Sitemap.GetUrlsOfficials(domainDataCode, 0) +
        Sitemap.GetUrlsIntro(domainDataCode, 0) +
        Sitemap.GetUrlsPoliticianIssue(domainDataCode, 0) +
        Sitemap.GetUrlsIssue(domainDataCode, 0) +
        Sitemap.GetUrlsBallot(domainDataCode, 0);
    }

    private void TotalUrlsRunTimeAllDomains()
    {
      //string SQL = string.Empty;
      //SQL += " SELECT DomainDataCode,RunTimeSeconds";
      //SQL += " FROM Sitemap";
      //DataTable Table_SiteMap = db.Table(SQL);
      var runTimeTable = Sitemap.GetAllRunTimeData();
      long urls = 0;
      long seconds = 0;
      foreach (var runtimeRow in runTimeTable)
      {
        urls += Domain_Urls(runtimeRow.DomainDataCode);
        //Seconds += Sitemap.GetRunTimeSeconds(runtimeRow["DomainDataCode"].ToString(), 0);
        seconds += runtimeRow.RunTimeSeconds;
      }

      Label_Urls_Total_All_Domains.Text = urls.ToString(CultureInfo.InvariantCulture);
      var runTime = TimeSpan.FromSeconds(seconds);
      //Label_Run_Time_All_Domains.Text = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
      //                Run_Time.Hours,
      //                Run_Time.Minutes,
      //                Run_Time.Seconds);
      Label_Run_Time_All_Domains.Text = db.Str_Run_Time(runTime);
    }

    private void Load_Selected_Domain_Controls()
    {
      if (!string.IsNullOrEmpty(DropDownList_Domain.SelectedValue))
      {
        Label_Domain_Code.Text = DropDownList_Domain.SelectedValue;
        Label_Domain.Text = DropDownList_Domain.SelectedItem.Text;

        RadioButtonList_Election_Directories.SelectedValue =
          Sitemap.GetElectionDirectories(DropDownList_Domain.SelectedValue);
        //= Sitemap_ElectionDirectories(
        //DropDownList_Domain.SelectedValue
        //);

        RadioButtonList_Politician_Elections.SelectedValue =
          Sitemap.GetPoliticianElections(DropDownList_Domain.SelectedValue);
        //= Sitemap_PoliticianElections(
        //DropDownList_Domain.SelectedValue
        //);

        //RadioButtonList_Ballots.SelectedValue
        //  = Sitemap_Str(
        //  DropDownList_Domain.SelectedValue
        //  , "Ballots"
        //  );

        //if (Sitemap_Bool(
        //  DropDownList_Domain.SelectedValue
        //  , "IsCurrentlyElected")
        //  )
        //  CheckBox_Is_Currently_Elected.Checked = true;
        //else
        //  CheckBox_Is_Currently_Elected.Checked = false;

        CheckBox_Must_Have_Picture.Checked =
          Sitemap.GetMustHavePicture(DropDownList_Domain.SelectedValue, false);

        CheckBox_Must_Have_Statement.Checked =
          Sitemap.GetMustHaveStatement(DropDownList_Domain.SelectedValue, false);

        TextBox_Minium_Candidates_Per_Page.Text =
          Sitemap.GetMinimumCandidates(DropDownList_Domain.SelectedValue, 0)
            .ToString(CultureInfo.InvariantCulture);

        TextBox_Minium_Answers_Per_Page.Text =
          Sitemap.GetMinimumAnswers(DropDownList_Domain.SelectedValue, 0)
            .ToString(CultureInfo.InvariantCulture);

        //TextBox_Factor.Text
        //  = Sitemap_FactorIssue(
        //  DropDownList_Domain.SelectedValue
        //  ).ToString();

       // Domain Settings

        Label_Sitemap.Text = "Sitemap" + DropDownList_Domain.SelectedValue + ".xml";

        Label_Last_Created.Text =
          Sitemap.GetLastCreated(DropDownList_Domain.SelectedValue)
            .ToString();

        // Urls

        // Urls Pages

        Label_Urls_Default.Text =
          Sitemap.GetUrlsDefault(DropDownList_Domain.SelectedValue, 0)
            .ToString(CultureInfo.InvariantCulture);

        Label_Urls_Election.Text =
          Sitemap.GetUrlsElection(DropDownList_Domain.SelectedValue, 0)
            .ToString(CultureInfo.InvariantCulture);

        Label_Urls_Officials.Text =
          Sitemap.GetUrlsOfficials(DropDownList_Domain.SelectedValue, 0)
            .ToString(CultureInfo.InvariantCulture);

        Label_Urls_Intro.Text =
          Sitemap.GetUrlsIntro(DropDownList_Domain.SelectedValue, 0)
            .ToString(CultureInfo.InvariantCulture);

        Label_Urls_PoliticianIssue.Text =
          Sitemap.GetUrlsPoliticianIssue(DropDownList_Domain.SelectedValue, 0)
            .ToString(CultureInfo.InvariantCulture);

        Label_Urls_Issue.Text =
          Sitemap.GetUrlsIssue(DropDownList_Domain.SelectedValue, 0)
            .ToString(CultureInfo.InvariantCulture);

        Label_Urls_Ballot.Text =
          Sitemap.GetUrlsBallot(DropDownList_Domain.SelectedValue, 0)
            .ToString(CultureInfo.InvariantCulture);

        // Total Domain Urls

        //Label_Urls_Total.Text = Urls.ToString();
        Label_Urls_Total.Text = Domain_Urls(DropDownList_Domain.SelectedValue)
          .ToString(CultureInfo.InvariantCulture);

        // Total Domain Run Time

        var seconds = Sitemap.GetRunTimeSeconds(DropDownList_Domain.SelectedValue, 0);

        var runTime = TimeSpan.FromSeconds(seconds);
        //Label_Run_Time.Text = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
        //                Run_Time.Hours,
        //                Run_Time.Minutes,
        //                Run_Time.Seconds);
        Label_Run_Time.Text = db.Str_Run_Time(runTime);

        // Priority

        TextBox_Priority_Default.Text =
          Sitemap.GetPriorityDefault(DropDownList_Domain.SelectedValue, 0)
            .ToString(CultureInfo.InvariantCulture);

        TextBox_Priority_Election.Text =
          Sitemap.GetPriorityElection(DropDownList_Domain.SelectedValue, 0)
            .ToString(CultureInfo.InvariantCulture);

        TextBox_Priority_Officials.Text =
          Sitemap.GetPriorityOfficials(DropDownList_Domain.SelectedValue, 0)
            .ToString(CultureInfo.InvariantCulture);

        TextBox_Priority_Intro.Text =
          Sitemap.GetPriorityIntro(DropDownList_Domain.SelectedValue, 0)
            .ToString(CultureInfo.InvariantCulture);

        TextBox_Priority_PoliticianIssue.Text =
          Sitemap.GetPriorityPoliticianIssue(DropDownList_Domain.SelectedValue, 0)
            .ToString(CultureInfo.InvariantCulture);

        TextBox_Priority_Issue.Text =
          Sitemap.GetPriorityIssue(DropDownList_Domain.SelectedValue, 0)
            .ToString(CultureInfo.InvariantCulture);

        TextBox_Priority_Ballot.Text =
          Sitemap.GetPriorityBallot(DropDownList_Domain.SelectedValue, 0)
            .ToString(CultureInfo.InvariantCulture);

        // Frequency

        RadioButtonList_Frequency_Default.SelectedValue =
          Sitemap.GetFrequencyDefault(DropDownList_Domain.SelectedValue);
        //= Sitemap_FrequencyDefault(
        //DropDownList_Domain.SelectedValue
        //);

        RadioButtonList_Frequency_Election.SelectedValue =
          Sitemap.GetFrequencyElection(DropDownList_Domain.SelectedValue);
        //= Sitemap_FrequencyElection(
        //DropDownList_Domain.SelectedValue
        //);

        RadioButtonList_Frequency_Officials.SelectedValue =
          Sitemap.GetFrequencyOfficials(DropDownList_Domain.SelectedValue);
        //= Sitemap_FrequencyOfficials(
        //DropDownList_Domain.SelectedValue
        //);

        RadioButtonList_Frequency_Intro.SelectedValue =
          Sitemap.GetFrequencyIntro(DropDownList_Domain.SelectedValue);
        //= Sitemap_FrequencyIntro(
        //DropDownList_Domain.SelectedValue
        //);

        RadioButtonList_Frequency_PoliticianIssue.SelectedValue =
          Sitemap.GetFrequencyPoliticianIssue(DropDownList_Domain.SelectedValue);
        //= Sitemap_FrequencyPoliticianIssue(
        //DropDownList_Domain.SelectedValue
        //);

        RadioButtonList_Frequency_Issue.SelectedValue =
          Sitemap.GetFrequencyIssue(DropDownList_Domain.SelectedValue);
        //= Sitemap_FrequencyIssue(
        //DropDownList_Domain.SelectedValue
        //);

        RadioButtonList_Frequency_Ballot.SelectedValue =
          Sitemap.GetFrequencyBallot(DropDownList_Domain.SelectedValue);
        //= Sitemap_FrequencyBallot(
        //DropDownList_Domain.SelectedValue
        //);

        // Parameters

        RadioButtonList_Election_Directories.SelectedValue =
          Sitemap.GetElectionDirectories(DropDownList_Domain.SelectedValue);

        Msg.Text =
          db.Msg("These are the values for Sitemap: Sitemap" +
            DropDownList_Domain.SelectedValue + ".xml for Domain: " +
            DropDownList_Domain.SelectedItem.Text);
      }
      else
      {
        // clear all controls

        TextBox_Priority_Default.Text = string.Empty;
        TextBox_Priority_Election.Text = string.Empty;
        TextBox_Priority_Officials.Text = string.Empty;
        TextBox_Priority_Intro.Text = string.Empty;
        TextBox_Priority_PoliticianIssue.Text = string.Empty;
        TextBox_Priority_Issue.Text = string.Empty;
        TextBox_Priority_Ballot.Text = string.Empty;

        Label_Domain_Code.Text = string.Empty;
        Label_Domain.Text = string.Empty;
        Label_Sitemap.Text = string.Empty;
        Label_Last_Created.Text = string.Empty;

        Label_Urls_Default.Text = string.Empty;
        Label_Urls_Election.Text = string.Empty;
        Label_Urls_Officials.Text = string.Empty;
        Label_Urls_Intro.Text = string.Empty;
        Label_Urls_PoliticianIssue.Text = string.Empty;
        Label_Urls_Issue.Text = string.Empty;
        Label_Urls_Ballot.Text = string.Empty;
        Label_Urls_Total.Text = string.Empty;
        Label_Run_Time.Text = string.Empty;
        Label_Urls_Total_All_Domains.Text = string.Empty;
        Label_Run_Time_All_Domains.Text = string.Empty;

        RadioButtonList_Election_Directories.SelectedValue = "Last";
        RadioButtonList_Politician_Elections.SelectedValue = "Last";

        Msg.Text = db.Msg("Select a Domain or Create Sitemaps");
      }
    }

    private void Update_TextBox_Priority(string sitemapColumn,
      TextBox textBoxPriority, Label labelPage)
    {
      //if (!db.Is_Digit(TextBox_Priority.Text.Trim()))
      if ((!db.Is_Valid_Integer(textBoxPriority.Text.Trim())) ||
        ((Convert.ToInt16(textBoxPriority.Text.Trim()) < 0) ||
          (Convert.ToInt16(textBoxPriority.Text.Trim()) > 10)))
        throw new ApplicationException(
          "Priority must be a integer number between 0 and 10.");

      var column = Sitemap.GetColumn(sitemapColumn);
      //Sitemap_Update_Int(
      //  Label_Domain_Code.Text
      //, Sitemap_Column
      //, Convert.ToUInt16(TextBox_Priority.Text)
      //);
      Sitemap.UpdateColumn(column, Convert.ToUInt16(textBoxPriority.Text),
        Label_Domain_Code.Text);

      Msg.Text =
        db.Ok(labelPage.Text + " Urls PRIORITY was updated to " +
          textBoxPriority.Text);
    }

    private void Update_RadioButtonList_Frequency(string sitemapColumn,
      RadioButtonList radioButtonListFrequency, Label labelPage)
    {
      var column = Sitemap.GetColumn(sitemapColumn);
      Sitemap.UpdateColumn(column, radioButtonListFrequency.SelectedValue,
        Label_Domain_Code.Text);
      //Sitemap_Update_Str(
      //  Label_Domain_Code.Text
      //, Sitemap_Column
      //, RadioButtonList_Frequency.SelectedValue
      //);

      Msg.Text =
        db.Ok(labelPage.Text + " Urls FREQUENCY was updated to " +
          radioButtonListFrequency.SelectedValue);
    }

    private void Check_Domain_Selected()
    {
      if (string.IsNullOrEmpty(DropDownList_Domain.SelectedValue))
        throw new ApplicationException(
          "You need to select a domain to have ALL Domains set.");
    }

    private void Button_All(string priorityStr, TextBox textBoxPriority, string frequencyStr,
      RadioButtonList radioButtonListFrequency, Label labelPage)
    {
      if (priorityStr == null) throw new ArgumentNullException("priorityStr");
      if (textBoxPriority == null) throw new ArgumentNullException("textBoxPriority");
      //string sql = string.Empty;
      //sql += " UPDATE Sitemap SET ";
      ////sql += " SET " + Include_Str + " = ";
      ////if (CheckBox_Include.Checked)
      ////  sql += "1";
      ////else
      ////  sql += "0";
      //sql += " " + Priority_Str + " = "
      //  + TextBox_Priority.Text.Trim();
      //sql += "," + Frequency_Str + " ="
      //  + db.SQLLit(RadioButtonList_Frequency.SelectedValue);
      //db.ExecuteSQL(sql);

      var column = Sitemap.GetColumn(priorityStr);
      Sitemap.UpdateColumnAllRows(column, int.Parse(textBoxPriority.Text.Trim()));
      column = Sitemap.GetColumn(frequencyStr);
      Sitemap.UpdateColumnAllRows(column, radioButtonListFrequency.SelectedValue);

      Msg.Text =
        db.Ok("ALL PARAMETERS for ALL DOMAINS for " + labelPage.Text +
          "  have been updated with these values.");
    }

    protected void TextBox_Priority_Default_TextChanged(object sender, EventArgs e)
    {
      try
      {
        Update_TextBox_Priority("PriorityDefault", TextBox_Priority_Default,
          Label_Page_Default);
      }
      catch (Exception ex)
      {
        Msg.Text = db.Fail(ex.Message);
        db.Log_Error_Admin(ex);
      }
    }

    protected void TextBox_Priority_Election_TextChanged(object sender, EventArgs e)
    {
      try
      {
        Update_TextBox_Priority("PriorityElection", TextBox_Priority_Election,
          Label_Page_Election);
      }
      catch (Exception ex)
      {
        Msg.Text = db.Fail(ex.Message);
        db.Log_Error_Admin(ex);
      }
    }

    protected void TextBox_Priority_Officials_TextChanged(object sender, EventArgs e)
    {
      try
      {
        Update_TextBox_Priority("PriorityOfficials", TextBox_Priority_Officials,
          Label_Page_Officials);
      }
      catch (Exception ex)
      {
        Msg.Text = db.Fail(ex.Message);
        db.Log_Error_Admin(ex);
      }
    }

    protected void TextBox_Priority_Intro_TextChanged(object sender, EventArgs e)
    {
      try
      {
        Update_TextBox_Priority("PriorityIntro", TextBox_Priority_Intro,
          Label_Page_Intro);
      }
      catch (Exception ex)
      {
        Msg.Text = db.Fail(ex.Message);
        db.Log_Error_Admin(ex);
      }
    }

    protected void TextBox_Priority_PoliticianIssue_TextChanged(object sender,
      EventArgs e)
    {
      try
      {
        Update_TextBox_Priority("PriorityPoliticianIssue",
          TextBox_Priority_PoliticianIssue, Label_Page_PoliticianIssue);
      }
      catch (Exception ex)
      {
        Msg.Text = db.Fail(ex.Message);
        db.Log_Error_Admin(ex);
      }
    }

    protected void TextBox_Priority_Issue_TextChanged(object sender, EventArgs e)
    {
      try
      {
        Update_TextBox_Priority("PriorityIssue", TextBox_Priority_Issue,
          Label_Page_Issue);
      }
      catch (Exception ex)
      {
        Msg.Text = db.Fail(ex.Message);
        db.Log_Error_Admin(ex);
      }
    }

    protected void TextBox_Priority_Ballot_TextChanged(object sender, EventArgs e)
    {
      try
      {
        Update_TextBox_Priority("PriorityBallot", TextBox_Priority_Ballot,
          Label_Page_Ballot);
      }
      catch (Exception ex)
      {
        Msg.Text = db.Fail(ex.Message);
        db.Log_Error_Admin(ex);
      }
    }

    protected void RadioButtonList_Frequency_Default_SelectedIndexChanged(
      object sender, EventArgs e)
    {
      try
      {
        Update_RadioButtonList_Frequency("FrequencyDefault",
          RadioButtonList_Frequency_Default, Label_Page_Default);
      }
      catch (Exception ex)
      {
        Msg.Text = db.Fail(ex.Message);
        db.Log_Error_Admin(ex);
      }
    }

    protected void RadioButtonList_Frequency_Election_SelectedIndexChanged(
      object sender, EventArgs e)
    {
      try
      {
        Update_RadioButtonList_Frequency("FrequencyElection",
          RadioButtonList_Frequency_Election, Label_Page_Election);
      }
      catch (Exception ex)
      {
        Msg.Text = db.Fail(ex.Message);
        db.Log_Error_Admin(ex);
      }
    }

    protected void RadioButtonList_Frequency_Officials_SelectedIndexChanged(
      object sender, EventArgs e)
    {
      try
      {
        Update_RadioButtonList_Frequency("FrequencyOfficials",
          RadioButtonList_Frequency_Officials, Label_Page_Officials);
      }
      catch (Exception ex)
      {
        Msg.Text = db.Fail(ex.Message);
        db.Log_Error_Admin(ex);
      }
    }

    protected void RadioButtonList_Frequency_Intro_SelectedIndexChanged(
      object sender, EventArgs e)
    {
      try
      {
        Update_RadioButtonList_Frequency("FrequencyIntro",
          RadioButtonList_Frequency_Intro, Label_Page_Intro);
      }
      catch (Exception ex)
      {
        Msg.Text = db.Fail(ex.Message);
        db.Log_Error_Admin(ex);
      }
    }

    protected void RadioButtonList_Frequency_PoliticianIssue_SelectedIndexChanged(
      object sender, EventArgs e)
    {
      try
      {
        Update_RadioButtonList_Frequency("FrequencyPoliticianIssue",
          RadioButtonList_Frequency_PoliticianIssue, Label_Page_PoliticianIssue);
      }
      catch (Exception ex)
      {
        Msg.Text = db.Fail(ex.Message);
        db.Log_Error_Admin(ex);
      }
    }

    protected void RadioButtonList_Frequency_Issue_SelectedIndexChanged(
      object sender, EventArgs e)
    {
      try
      {
        Update_RadioButtonList_Frequency("FrequencyIssue",
          RadioButtonList_Frequency_Issue, Label_Page_Issue);
      }
      catch (Exception ex)
      {
        Msg.Text = db.Fail(ex.Message);
        db.Log_Error_Admin(ex);
      }
    }

    protected void RadioButtonList_Frequency_Ballot_SelectedIndexChanged(
      object sender, EventArgs e)
    {
      try
      {
        Update_RadioButtonList_Frequency("FrequencyBallot",
          RadioButtonList_Frequency_Ballot, Label_Page_Ballot);
      }
      catch (Exception ex)
      {
        Msg.Text = db.Fail(ex.Message);
        db.Log_Error_Admin(ex);
      }
    }

    protected void Button_All_Default_Click(object sender, EventArgs e)
    {
      try
      {
        Check_Domain_Selected();
        Button_All("PriorityDefault", TextBox_Priority_Default, "FrequencyDefault",
          RadioButtonList_Frequency_Default, Label_Page_Default);
      }
      catch (Exception ex)
      {
        Msg.Text = db.Fail(ex.Message);
        db.Log_Error_Admin(ex);
      }
    }

    protected void Button_All_Election_Click(object sender, EventArgs e)
    {
      try
      {
        Check_Domain_Selected();
        Button_All("PriorityElection", TextBox_Priority_Election, "FrequencyElection",
          RadioButtonList_Frequency_Election, Label_Page_Election);
      }
      catch (Exception ex)
      {
        Msg.Text = db.Fail(ex.Message);
        db.Log_Error_Admin(ex);
      }
    }

    protected void Button_All_Officials_Click(object sender, EventArgs e)
    {
      try
      {
        Check_Domain_Selected();
        Button_All("PriorityOfficials", TextBox_Priority_Officials, "FrequencyOfficials",
          RadioButtonList_Frequency_Officials, Label_Page_Officials);
      }
      catch (Exception ex)
      {
        Msg.Text = db.Fail(ex.Message);
        db.Log_Error_Admin(ex);
      }
    }

    protected void Button_All_Intro_Click(object sender, EventArgs e)
    {
      try
      {
        Check_Domain_Selected();
        Button_All("PriorityIntro", TextBox_Priority_Intro, "FrequencyIntro",
          RadioButtonList_Frequency_Intro, Label_Page_Intro);
      }
      catch (Exception ex)
      {
        Msg.Text = db.Fail(ex.Message);
        db.Log_Error_Admin(ex);
      }
    }

    protected void Button_All_PoliticianIssue_Click(object sender, EventArgs e)
    {
      try
      {
        Check_Domain_Selected();
        Button_All("PriorityPoliticianIssue", TextBox_Priority_PoliticianIssue,
          "FrequencyPoliticianIssue", RadioButtonList_Frequency_PoliticianIssue,
          Label_Page_PoliticianIssue);
      }
      catch (Exception ex)
      {
        Msg.Text = db.Fail(ex.Message);
        db.Log_Error_Admin(ex);
      }
    }

    protected void Button_All_Issue_Click(object sender, EventArgs e)
    {
      try
      {
        Check_Domain_Selected();
        Button_All("PriorityIssue", TextBox_Priority_Issue, "FrequencyIssue",
          RadioButtonList_Frequency_Issue, Label_Page_Issue);
      }
      catch (Exception ex)
      {
        Msg.Text = db.Fail(ex.Message);
        db.Log_Error_Admin(ex);
      }
    }

    protected void Button_All_Ballot_Click(object sender, EventArgs e)
    {
      try
      {
        Check_Domain_Selected();
        Button_All("PriorityBallot", TextBox_Priority_Ballot, "FrequencyBallot",
          RadioButtonList_Frequency_Ballot, Label_Page_Ballot);
      }
      catch (Exception ex)
      {
        Msg.Text = db.Fail(ex.Message);
        db.Log_Error_Admin(ex);
      }
    }

    protected void Button_Election_Directories_Click(object sender, EventArgs e)
    {
      try
      {
        Check_Domain_Selected();

        //string sql = string.Empty;
        //sql += " UPDATE Sitemap";
        //sql += " SET";
        //sql += " ElectionDirectories = ";
        //sql += db.SQLLit(RadioButtonList_Election_Directories.SelectedValue);
        //db.ExecuteSQL(sql);

        Sitemap.UpdateElectionDirectoriesAllRows(
          RadioButtonList_Election_Directories.SelectedValue);

        Msg.Text =
          db.Ok("ALL domains for Election Directories" + " have been updated with " +
            RadioButtonList_Election_Directories.SelectedValue);
      }
      catch (Exception ex)
      {
        Msg.Text = db.Fail(ex.Message);
        db.Log_Error_Admin(ex);
      }
    }

    protected void Button_Politician_Elections_Click(object sender, EventArgs e)
    {
      try
      {
        Check_Domain_Selected();

        //string sql = string.Empty;
        //sql += " UPDATE Sitemap";
        //sql += " SET";
        //sql += " PoliticianElections = ";
        //sql += db.SQLLit(RadioButtonList_Politician_Elections.SelectedValue);
        //db.ExecuteSQL(sql);

        Sitemap.UpdatePoliticianElectionsAllRows(
          RadioButtonList_Politician_Elections.SelectedValue);

        Msg.Text =
          db.Ok("ALL domains for parameter " + " have been updated with " +
            RadioButtonList_Politician_Elections.SelectedValue);
      }
      catch (Exception ex)
      {
        Msg.Text = db.Fail(ex.Message);
        db.Log_Error_Admin(ex);
      }
    }

    protected void Button_Must_Have_Picture_Click(object sender, EventArgs e)
    {
      try
      {
        Check_Domain_Selected();

        //string sql = string.Empty;
        //sql += " UPDATE Sitemap";
        //sql += " SET";
        //sql += " MustHavePicture = ";
        //if (CheckBox_Must_Have_Picture.Checked)
        //  sql += "1";
        //else
        //  sql += "0";
        //db.ExecuteSQL(sql);

        Sitemap.UpdateMustHavePictureAllRows(CheckBox_Must_Have_Picture.Checked);

        //sql = string.Empty;
        //sql += " UPDATE Sitemap";
        //sql += " SET";
        //sql += " MustHaveStatement = ";
        //if (CheckBox_Must_Have_Statement.Checked)
        //  sql += "1";
        //else
        //  sql += "0";
        //db.ExecuteSQL(sql);

        Sitemap.UpdateMustHaveStatementAllRows(CheckBox_Must_Have_Statement.Checked);

        Msg.Text = db.Ok("ALL domains for parameters have been updated.");
      }
      catch (Exception ex)
      {
        Msg.Text = db.Fail(ex.Message);
        db.Log_Error_Admin(ex);
      }
    }

    protected void Button_Minimum_Candidates_Click(object sender, EventArgs e)
    {
      try
      {
        Check_Domain_Selected();

        if (!db.Is_Digits(TextBox_Minium_Candidates_Per_Page.Text.Trim()))
          throw new ApplicationException(
            "Minimum Number of Candidates must be digits.");
        if (!db.Is_Digits(TextBox_Minium_Answers_Per_Page.Text.Trim()))
          throw new ApplicationException("Minimun Number of Answers must be digits.");

        //string sql = string.Empty;
        //sql += " UPDATE Sitemap";
        //sql += " SET";
        //sql += " MinimumCandidates = ";
        //sql += TextBox_Minium_Candidates_Per_Page.Text.ToString();
        //db.ExecuteSQL(sql);

        Sitemap.UpdateMinimumCandidatesAllRows(
          int.Parse(TextBox_Minium_Candidates_Per_Page.Text.Trim()));

        //sql = string.Empty;
        //sql += " UPDATE Sitemap";
        //sql += " SET";
        //sql += " MinimumAnswers = ";
        //sql += TextBox_Minium_Answers_Per_Page.Text.ToString();
        //db.ExecuteSQL(sql);

        Sitemap.UpdateMinimumAnswersAllRows(
          int.Parse(TextBox_Minium_Answers_Per_Page.Text.Trim()));

        Msg.Text = db.Ok("ALL domains for parameters have been updated.");
      }
      catch (Exception ex)
      {
        Msg.Text = db.Fail(ex.Message);
        db.Log_Error_Admin(ex);
      }
    }

    protected void Button_Set_Or_Compute_Priority_Click(object sender, EventArgs e)
    {
      try
      {
        Check_Domain_Selected();

        if (!db.Is_Single_Digit(TextBox_Factor.Text.Trim()))
          throw new ApplicationException("Factor must be a single digit.");

        //string sql = string.Empty;
        //sql += " UPDATE Sitemap";
        //sql += " SET";
        //sql += " ComputePriorityIssue = ";
        //if (CheckBox_Compute_Priority_Issue.Checked)
        //  sql += "1";
        //else
        //  sql += "0";
        //db.ExecuteSQL(sql);

        //string sql = string.Empty;
        //sql += " UPDATE Sitemap";
        //sql += " SET";
        //sql += " FactorIssue = ";
        //sql += TextBox_Factor.Text.ToString();
        //db.ExecuteSQL(sql);

        Sitemap.UpdateFactorIssueAllRows(int.Parse(TextBox_Factor.Text.Trim()));

        Msg.Text =
          db.Fail("ALL domains for factor have been updated." +
            " BUT THIS FACTOR IS NO LONGER USED.");
      }
      catch (Exception ex)
      {
        Msg.Text = db.Fail(ex.Message);
        db.Log_Error_Admin(ex);
      }
    }

    protected void CheckBox_Must_Have_Picture_CheckedChanged(object sender,
      EventArgs e)
    {
      Sitemap.UpdateMustHavePicture(CheckBox_Must_Have_Picture.Checked,
        DropDownList_Domain.SelectedValue);
      Msg.Text = db.Ok("CheckBox_Must_Have_Picture updated.");
    }

    protected void CheckBox_Must_Have_Statement_CheckedChanged(object sender,
      EventArgs e)
    {
      Sitemap.UpdateMustHaveStatement(CheckBox_Must_Have_Statement.Checked,
        DropDownList_Domain.SelectedValue);
      Msg.Text = db.Ok("CheckBox_Must_Have_Statement updated.");
    }

    protected void TextBox_Factor_TextChanged(object sender, EventArgs e)
    {
      if (!db.Is_Single_Digit(TextBox_Factor.Text.Trim()))
        throw new ApplicationException("Priority must be a single digit.");

      //Sitemap_Update_FactorIssue(
      //  DropDownList_Domain.SelectedValue
      //  , Convert.ToUInt16(TextBox_Factor.Text.Trim())
      //);

      Sitemap.UpdateFactorIssue(Convert.ToUInt16(TextBox_Factor.Text.Trim()),
        DropDownList_Domain.SelectedValue);

      Msg.Text =
        db.Fail("Issue.aspx Page FACTOR was updated to be " +
          TextBox_Factor.Text.Trim() + " BUT THE FACTOR IS NO LONGER USED.");
    }

    protected void RadioButtonList_Election_Directories_SelectedIndexChanged(
      object sender, EventArgs e)
    {
      //Sitemap_Update_Str(
      //"ElectionDirectories"
      //, RadioButtonList_Election_Directories.SelectedValue
      //);
      //Sitemap_Update_ElectionDirectories(
      //    DropDownList_Domain.SelectedValue
      //    , RadioButtonList_Election_Directories.SelectedValue
      //    );
      Sitemap.UpdateElectionDirectories(
        RadioButtonList_Election_Directories.SelectedValue,
        DropDownList_Domain.SelectedValue);

      Msg.Text =
        db.Ok("Election.aspx Pages was updated for ELECTIONS to include " +
          RadioButtonList_Election_Directories.SelectedValue);
    }

    protected void RadioButtonList_Politician_Elections_SelectedIndexChanged(
      object sender, EventArgs e)
    {
      //Sitemap_Update_PoliticianElections(
      //DropDownList_Domain.SelectedValue
      //, RadioButtonList_Politician_Elections.SelectedValue
      //);
      Sitemap.UpdatePoliticianElections(
        RadioButtonList_Politician_Elections.SelectedValue,
        DropDownList_Domain.SelectedValue);

      Msg.Text =
        db.Ok("PoliticianIssue.aspx Pages were updated to include " +
          RadioButtonList_Politician_Elections.SelectedValue);
    }

    protected void TextBox_Minium_Candidates_Per_Page_TextChanged(object sender,
      EventArgs e)
    {
      if (!db.Is_Single_Digit(TextBox_Minium_Candidates_Per_Page.Text.Trim()))
        throw new ApplicationException("Minimum candidates must be a single digit.");

      //Sitemap_Update_MinimumCandidates(
      //  DropDownList_Domain.SelectedValue
      //  , Convert.ToUInt16(TextBox_Minium_Candidates_Per_Page.Text.Trim())
      //);

      Sitemap.UpdateMinimumCandidates(
        Convert.ToUInt16(TextBox_Minium_Candidates_Per_Page.Text.Trim()),
        DropDownList_Domain.SelectedValue);

      Msg.Text =
        db.Ok("Minimum candidates per page was updated to be " +
          TextBox_Minium_Candidates_Per_Page.Text.Trim());
    }

    protected void TextBox_Minium_Answers_Per_Page_TextChanged(object sender,
      EventArgs e)
    {
      if (!db.Is_Digits(TextBox_Minium_Answers_Per_Page.Text.Trim()))
        throw new ApplicationException("Minimum answers per page must be all digits.");

      //Sitemap_Update_MinimumAnswers(
      //  DropDownList_Domain.SelectedValue
      //  , Convert.ToUInt16(TextBox_Minium_Answers_Per_Page.Text.Trim())
      //);

      Sitemap.UpdateMinimumAnswers(
        Convert.ToUInt16(TextBox_Minium_Answers_Per_Page.Text.Trim()),
        DropDownList_Domain.SelectedValue);

      Msg.Text =
        db.Ok("Minimum answers per page was updated to be " +
          TextBox_Minium_Answers_Per_Page.Text.Trim());
    }

    private static void Sitemaps_52_Domains()
    {
      // Note

      //This method must NOT use any form controls
      //because it is run as a scheduled batch job.

      // sql Sitemap

      var sitemapTable = Sitemap.GetDomainDataSorted();
      foreach (var sitemapRow in sitemapTable)
        SitemapManager.UpdateSitemapVirtualPage(sitemapRow.DomainDataCode);
    }

    protected void Button_Create_Sitemap_Click(object sender, EventArgs e)
    {
      try
      {
        SitemapManager.UpdateSitemapVirtualPage(DropDownList_Domain.SelectedValue);

        Load_Selected_Domain_Controls();

        Msg.Text = db.Ok("The Sitemap has been created.");
      }
      catch (Exception ex)
      {
        Msg.Text = db.Fail(ex.Message);
        db.Log_Error_Admin(ex);
      }
    }

    protected void Button_Create_Sitemaps_Click(object sender, EventArgs e)
    {
      try
      {
        Server.ScriptTimeout = 6000; //6000 sec = 100 min = 1.66 hours

        Sitemaps_52_Domains();

        SitemapManager.Index_Sitemap();

        Load_Selected_Domain_Controls();

        TotalUrlsRunTimeAllDomains();

        Msg.Text = db.Ok("The 52 Statemaps have been created.");
      }
      catch (Exception ex)
      {
        Msg.Text = db.Fail(ex.Message);
        db.Log_Error_Admin(ex);
      }
    }

    protected void DropDownListState_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        Load_Selected_Domain_Controls();
      }
      catch (Exception ex)
      {
        Msg.Text = db.Fail(ex.Message);
        db.Log_Error_Admin(ex);
      }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        if (!SecurePage.IsMasterUser)
          SecurePage.HandleSecurityException();

        try
        {
          Load_Selected_Domain_Controls();
          TotalUrlsRunTimeAllDomains();
        }
        catch (Exception ex)
        {
          Msg.Text = db.Fail(ex.Message);
          db.Log_Error_Admin(ex);
        }
      }
    }
     * */
  }
}