using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace DB.Vote
{
  public partial class IssuesRow
  {
  }

  public partial class Issues
  {
    public static DataTable GetIssuePageIssues(string electionKey, string officeKey,
      int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT CONCAT(GetIssueLevel(@OfficeKey), GetIssueState(@OfficeKey), 'IssuesList')" +
        "  AS IssueKey, 'List of Issues' AS Issue, 0 AS IssueOrder" +
        " UNION SELECT 'ALLBio' AS IssueKey, 'Biographical' AS Issue, 1 AS IssueOrder" +
        " UNION SELECT IssueKey, Issue, 2 AS IssueOrder FROM Issues WHERE IssueKey='ALLPersonal'" +
        " UNION SELECT i.IssueKey, i.Issue, i.IssueOrder FROM Issues i" +
        " INNER JOIN Answers a ON a.IssueKey=i.IssueKey" +
        "  TRIM(a.YouTubeUrl)<>'' AND NOT a.YouTubeUrl IS NULL AND (a.YouTubeAutoDisable IS NULL OR a.YouTubeAutoDisable='')" +
        " TRIM(a.YouTubeUrl) <> ''" +
        ")" +
        " INNER JOIN Questions q ON q.IssueKey=a.IssueKey AND" +
        "  q.QuestionKey=a.QuestionKey AND q.IsQuestionOmit=0" +
        " INNER JOIN ElectionsPoliticians ep" +
        " ON ep.PoliticianKey = a.PoliticianKey AND" +
        "  ep.ElectionKey=@ElectionKey AND ep.OfficeKey=@OfficeKey" +
        " WHERE i.StateCode=GetIssueState(@OfficeKey)" +
        "  AND i.IssueLevel=GetIssueLevel(@OfficeKey)  AND i.IsIssueOmit=0" +
        " GROUP BY i.IssueKey ORDER BY IssueOrder";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
        VoteDb.AddCommandParameter(cmd, "OfficeKey", officeKey);
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    //public static DataTable GetIssuePageBioData(string electionKey,
    //  string officeKey, int commandTimeout = -1)
    //{
    //  const string columns =
    //    " ep.OrderOnBallot as OrderOnBallot,ep.IsIncumbent,ep.RunningMateKey," + 
    //      "pt.PartyCode,pt.PartyName,pt.PartyUrl,o.IsRunningMateOffice," +
    //      //"p.Accomplishments," +
    //      "p.AddOn,p.BallotPediaWebAddress," +
    //      "p.BloggerWebAddress," +
    //      //"p.Civic," +
    //      "p.DateOfBirth," +
    //      //"p.Education," +
    //      "p.EmailAddr AS Email,p.FacebookWebAddress,p.FlickrWebAddress," +
    //      "p.FName as FirstName," +
    //      //"p.GeneralStatement," +
    //      "p.GooglePlusWebAddress," +
    //      //"p.LDSEmailAddr AS LDSEmail,p.LDSWebAddr as LDSWebAddress," +
    //      "p.LinkedInWebAddress,p.LName as LastName," +
    //      //"p.Military," +
    //      "p.MName as MiddleName,p.Nickname,p.PartyKey," +
    //      //"p.Personal," +
    //      "p.PinterestWebAddress," +
    //      //"p.Political," +
    //      "p.PoliticianKey," +
    //      //"p.Profession," +
    //      //"p.Religion," +
    //      "p.RSSFeedWebAddress,p.StateEmailAddr AS StateEmail," +
    //      "p.StateWebAddr as StateWebAddress,p.Suffix,p.TwitterWebAddress," +
    //      "p.VimeoWebAddress,p.WebAddr as WebAddress,p.WebstagramWebAddress," +
    //      "p.WikipediaWebAddress,p.YouTubeWebAddress";

    //  const string cmdTemplate =
    //    "SELECT {0},0 AS IsRunningMate FROM ElectionsPoliticians ep" +
    //      " INNER JOIN Politicians p ON p.PoliticianKey = ep.PoliticianKey" +
    //      " LEFT JOIN Parties pt ON pt.PartyKey = p.PartyKey" +
    //      " INNER JOIN Offices o ON o.OfficeKey = ep.OfficeKey" +
    //      " WHERE ep.ElectionKey=@ElectionKey AND ep.OfficeKey=@OfficeKey" +
    //      " UNION SELECT {0},1 AS IsRunningMate FROM ElectionsPoliticians ep" +
    //      " INNER JOIN Politicians p ON p.PoliticianKey = ep.RunningMateKey" +
    //      " LEFT JOIN Parties pt ON pt.PartyKey = p.PartyKey" +
    //      " INNER JOIN Offices o ON o.OfficeKey = ep.OfficeKey" +
    //      " WHERE ep.ElectionKey=@ElectionKey AND ep.OfficeKey=@OfficeKey" + 
    //      " ORDER BY IsRunningMate, OrderOnBallot, LastName, FirstName";

    //  var cmdText = string.Format(cmdTemplate, columns);
    //  var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
    //  using (var cn = VoteDb.GetOpenConnection())
    //  {
    //    cmd.Connection = cn;
    //    VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
    //    VoteDb.AddCommandParameter(cmd, "OfficeKey", officeKey);
    //    var table = new DataTable();
    //    DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
    //    adapter.Fill(table);
    //    return table;
    //  }
    //}

    //public static DataTable GetIssuePageIssueData(string electionKey,
    //  string officeKey, string issueKey, int commandTimeout = -1)
    //{
    //  const string columns =
    //    " ep.OrderOnBallot as OrderOnBallot,ep.IsIncumbent,ep.RunningMateKey," +
    //      "pt.PartyCode,pt.PartyName,pt.PartyUrl,o.IsRunningMateOffice," +
    //      "p.AddOn,p.BallotPediaWebAddress,p.BloggerWebAddress,p.DateOfBirth," +
    //      "p.EmailAddr AS Email,p.FacebookWebAddress,p.FlickrWebAddress," +
    //      "p.FName as FirstName,p.GooglePlusWebAddress," +
    //      //"p.LDSEmailAddr AS LDSEmail,p.LDSWebAddr as LDSWebAddress," +
    //      "p.LinkedInWebAddress,p.LName as LastName,p.MName as MiddleName," + 
    //      "p.Nickname,p.PartyKey,p.PinterestWebAddress,p.PoliticianKey," +
    //      "p.RSSFeedWebAddress,p.StateEmailAddr AS StateEmail," +
    //      "p.StateWebAddr as StateWebAddress,p.Suffix,p.TwitterWebAddress," +
    //      "p.VimeoWebAddress,p.WebAddr as WebAddress,p.WebstagramWebAddress," +
    //      "p.WikipediaWebAddress,p.YouTubeWebAddress,a.Answer," + 
    //      "a.DateStamp as AnswerDate,a.`Source` as AnswerSource,a.QuestionKey," +
    //      "a.YouTubeDate,a.YouTubeUrl,a.YouTubeDescription,a.YouTubeRunningTime," +
    //      "a.YouTubeSourceUrl,a.YouTubeSource,a.YouTubeRefreshTime,a.YouTubeAutoDisable";

    //  const string cmdTemplate =
    //    "SELECT {0},0 AS IsRunningMate FROM ElectionsPoliticians ep" +
    //      " INNER JOIN Politicians p ON p.PoliticianKey = ep.PoliticianKey" +
    //      " LEFT JOIN Parties pt ON pt.PartyKey = p.PartyKey" +
    //      " INNER JOIN Offices o ON o.OfficeKey = ep.OfficeKey" +
    //      " LEFT JOIN Answers a ON a.IssueKey=@IssueKey" +
    //      "  AND a.PoliticianKey=p.PoliticianKey" +
    //      " WHERE ep.ElectionKey=@ElectionKey AND ep.OfficeKey=@OfficeKey" +
    //      " UNION SELECT {0},1 AS IsRunningMate FROM ElectionsPoliticians ep" +
    //      " INNER JOIN Politicians p ON p.PoliticianKey = ep.RunningMateKey" +
    //      " LEFT JOIN Parties pt ON pt.PartyKey = p.PartyKey" +
    //      " INNER JOIN Offices o ON o.OfficeKey = ep.OfficeKey" +
    //      " LEFT JOIN Answers a ON a.IssueKey=@IssueKey" +
    //      "  AND a.PoliticianKey=p.PoliticianKey" +
    //      " WHERE ep.ElectionKey=@ElectionKey AND ep.OfficeKey=@OfficeKey" +
    //      " ORDER BY IsRunningMate, OrderOnBallot, LastName, FirstName";

    //  var cmdText = string.Format(cmdTemplate, columns);
    //  var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
    //  using (var cn = VoteDb.GetOpenConnection())
    //  {
    //    cmd.Connection = cn;
    //    VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
    //    VoteDb.AddCommandParameter(cmd, "OfficeKey", officeKey);
    //    VoteDb.AddCommandParameter(cmd, "IssueKey", issueKey);
    //    var table = new DataTable();
    //    DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
    //    adapter.Fill(table);
    //    return table;
    //  }
    //}

    public static DataTable GetPoliticianIssuePageIssues(string politicianKey,
      int commandTimeout = -1)
    {
      const string cmdText =
        "CALL GetPoliticianIssuePageIssues(@PoliticianKey)";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    public static DataTable GetPoliticianIssuePageIssueData(string politicianKey, string issueKey,
      int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT Answer,DateStamp as AnswerDate,`Source` as AnswerSource," +
        " QuestionKey,YouTubeDate,YouTubeUrl,YouTubeSource,YouTubeDescription,YouTubeRunningTime," +
        " YouTubeSourceUrl,YouTubeRefreshTime,YouTubeAutoDisable FROM Answers" +
        " WHERE PoliticianKey=@PoliticianKey AND IssueKey=@IssueKey";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
        VoteDb.AddCommandParameter(cmd, "IssueKey", issueKey);
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }
  }
}