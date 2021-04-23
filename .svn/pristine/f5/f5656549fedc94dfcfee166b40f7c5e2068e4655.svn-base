using System;
using System.Data;
using System.Web.UI.WebControls;

namespace Vote.Master
{
  public partial class FixIssuesPage : SecurePage, ISuperUser
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsMasterUser)
        HandleSecurityException();

      Title = H1.InnerText = "Fix Issue Keys";

      try
      {
        PageTitle.Text = "Fix IssueKey in Answers, Questions, SitemapIssuesElections & SitemapIssuesPoliticians";
      }
      catch (Exception ex)
      {
        Msg.Text = db.Fail(ex.Message);
        db.Log_Error_Admin(ex);
      }
    }

    protected void Button_Bad_Issue_Keys_Click(object sender, EventArgs e)
    {
      try
      {
        #region IssueKeys_Good Table
        var sqlIssueKeysGood = string.Empty;
        sqlIssueKeysGood += "SELECT IssueKey";
        sqlIssueKeysGood += " FROM Issues";
        sqlIssueKeysGood += " WHERE IssueLevel = 'A' ";
        sqlIssueKeysGood += " OR IssueLevel = 'B'";
        sqlIssueKeysGood += " ORDER BY IssueLevel,IssueKey";

        var issueKeysGoodTable = db.Table(sqlIssueKeysGood);
        var index = 0;
        foreach (DataRow issueKeysGoodRow in issueKeysGoodTable.Rows)
        {
          ListBox_Good_IssueKeys.Items.Add(new ListItem());
          ListBox_Good_IssueKeys.Items[index].Text = issueKeysGoodRow["IssueKey"].ToString();
          index++;
        }
        #endregion IssueKeys_Good Table

        db.ExecuteSql("TRUNCATE TABLE IssueKeysBad");

        #region IssueKeys_Bad_Answers Table
        var sqlAnswers = string.Empty;
        sqlAnswers += "SELECT IssueKey";
        sqlAnswers += " FROM Answers";
        sqlAnswers += " WHERE SUBSTRING(IssueKey,1,1) = 'A'";
        sqlAnswers += " OR SUBSTRING(IssueKey,1,1) = 'B'";
        sqlAnswers += " GROUP BY IssueKey";
        sqlAnswers += " ORDER BY IssueKey";
        var answersTable = db.Table(sqlAnswers);
        foreach (DataRow answersRow in answersTable.Rows)
        {
          var rowsIssues = db.Rows("SELECT IssueKey FROM Issues WHERE IssueKey = "
            + db.SQLLit(answersRow["IssueKey"].ToString()));
          if (rowsIssues == 0)
          {
            var rowsIssuesKeyBad = db.Rows("SELECT IssueKey FROM IssueKeysBad WHERE IssueKey ="
              + db.SQLLit(answersRow["IssueKey"].ToString()));
            if (rowsIssuesKeyBad == 0)
            {
              var sqlInsertIssuesKeyBad = "INSERT INTO IssueKeysBad(IssueKey)"
              + "VALUES(" + db.SQLLit(answersRow["IssueKey"].ToString())+")";
              db.ExecuteSql(sqlInsertIssuesKeyBad);
            }
          }
        }
        #endregion IssueKeys_Bad_Answers Table

        #region IssueKeys_Bad_Questions Table
        var sqlQuestions = string.Empty;
        sqlQuestions += "SELECT IssueKey";
        sqlQuestions += " FROM Questions";
        sqlQuestions += " WHERE SUBSTRING(IssueKey,1,1) = 'A'";
        sqlQuestions += " OR SUBSTRING(IssueKey,1,1) = 'B'";
        sqlQuestions += " GROUP BY IssueKey";
        sqlQuestions += " ORDER BY IssueKey";
        var questionsTable = db.Table(sqlQuestions);
        foreach (DataRow questionsRow in questionsTable.Rows)
        {
          var rowsIssues = db.Rows("SELECT IssueKey FROM Issues WHERE IssueKey = "
            + db.SQLLit(questionsRow["IssueKey"].ToString()));
          if (rowsIssues == 0)
          {
            var rowsIssuesKeyBad = db.Rows("SELECT IssueKey FROM IssueKeysBad WHERE IssueKey ="
              + db.SQLLit(questionsRow["IssueKey"].ToString()));
            if (rowsIssuesKeyBad == 0)
            {
              var sqlInsertIssuesKeyBad = "INSERT INTO IssueKeysBad(IssueKey)"
              + "VALUES(" + db.SQLLit(questionsRow["IssueKey"].ToString()) + ")";
              db.ExecuteSql(sqlInsertIssuesKeyBad);
            }
          }
        }
        #endregion IssueKeys_Bad_Questions Table

        #region Show Bad IssueKeys
        const string sqlIssuesKeyBad = "SELECT IssueKey FROM IssueKeysBad ORDER BY IssueKey";
        var issuesKeyBadTable = db.Table(sqlIssuesKeyBad);
        index = 0;
        foreach (DataRow issuesKeyBadRow in issuesKeyBadTable.Rows)
        {
          ListBox_Bad_IssueKeys.Items.Add(new ListItem());
          ListBox_Bad_IssueKeys.Items[index].Text = issuesKeyBadRow["IssueKey"].ToString();
          index++;
        }
        #endregion Show Bad IssueKeys

        Msg.Text = db.Ok("Answers, & Questions Tables have been checked for bad IssueKeys");
      }
      catch (Exception ex)
      {
        Msg.Text = db.Fail(ex.Message);
        db.Log_Error_Admin(ex);
      }

    }

    protected void ListBox_Bad_IssueKeys_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        TextBox_IssueKey_From.Text = ListBox_Bad_IssueKeys.SelectedValue;
      }
      catch (Exception ex)
      {
        Msg.Text = db.Fail(ex.Message);
        db.Log_Error_Admin(ex);
      }

    }

    protected void ListBox_Good_IssueKeys_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        TextBox_IssueKey_To.Text = ListBox_Good_IssueKeys.SelectedValue;
      }
      catch (Exception ex)
      {
        Msg.Text = db.Fail(ex.Message);
        db.Log_Error_Admin(ex);
      }

    }

    protected void Button_Make_IssueKey_Change_Click(object sender, EventArgs e)
    {
      try
      {
      }
      catch (Exception ex)
      {
        Msg.Text = db.Fail(ex.Message);
        db.Log_Error_Admin(ex);
      }

    }
  }
}
