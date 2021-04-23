using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using MySql.Data.MySqlClient;
using Vote;
using static System.String;

namespace DB.Vote
{
  public partial class EmailTemplates
  {
    public static EmailTemplatesTable GetAvailableTemplateData(string ownerType,
      string owner, bool allPublic, int commandTimeout = -1)
    {
      var cmdText =
        "SELECT Id,Name,OwnerType,Owner,IsPublic,CreateTime,ModTime,LastUsedTime," +
        "Requirements,Subject,Body FROM EmailTemplates" +
        " WHERE (OwnerType=@OwnerType AND Owner=@Owner) {0}" +
        " ORDER BY Owner,OwnerType,Name";

      var publicClause = allPublic ? " OR IsPublic=1" : Empty;
      cmdText = Format(cmdText, publicClause);

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "OwnerType", ownerType);
      VoteDb.AddCommandParameter(cmd, "Owner", owner);
      return FillTable(cmd, EmailTemplatesTable.ColumnSet.All);
    }

    public static SimpleListItem[] GetStarterDocuments(int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT Id,Name FROM EmailTemplates WHERE OwnerType='*' ORDER BY Name";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return new List<SimpleListItem> {new SimpleListItem(Empty, "blank email")}.Union(
          table.Rows.OfType<DataRow>().Select(row =>
            new SimpleListItem(row.Id().ToString(CultureInfo.InvariantCulture),
              row.Name()))).ToArray();
      }
    }

    public static void Upsert(string name, string ownerType, string owner, bool isPublic,
      DateTime createTime, DateTime modTime, string subject, string body, string emailType,
      bool isNew = false, int commandTimeout = -1)
    {
      if (IsNullOrWhiteSpace(emailType)) emailType = null;

      const string cmdTemplate = "INSERT INTO EmailTemplates" +
        " (Name,OwnerType,Owner,IsPublic,CreateTime,ModTime,Requirements,Subject,Body,EmailTypeCode)" +
        " VALUES (@Name,@OwnerType,@Owner,@IsPublic,@CreateTime,@ModTime,@Requirements,@Subject,@Body,@EmailTypeCode)" +
        " ON DUPLICATE KEY UPDATE IsPublic=VALUES(IsPublic)," +
        " ModTime=VALUES(ModTime),Requirements=VALUES(Requirements),Subject=VALUES(Subject),Body=VALUES(Body),EmailTypeCode=VALUES(EmailTypeCode)";

      var cmdText = cmdTemplate;
      if (isNew) cmdText += ",SelectRecipientOptions=NULL,EmailOptions=NULL";

      var requirements = GetTemplateRequirementsString(subject, body);

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "Name", name);
      VoteDb.AddCommandParameter(cmd, "OwnerType", ownerType);
      VoteDb.AddCommandParameter(cmd, "Owner", owner);
      VoteDb.AddCommandParameter(cmd, "IsPublic", isPublic);
      VoteDb.AddCommandParameter(cmd, "CreateTime", createTime);
      VoteDb.AddCommandParameter(cmd, "ModTime", modTime);
      VoteDb.AddCommandParameter(cmd, "Requirements", requirements);
      VoteDb.AddCommandParameter(cmd, "Subject", subject);
      VoteDb.AddCommandParameter(cmd, "Body", body);
      VoteDb.AddCommandParameter(cmd, "EmailTypeCode", emailType);
      VoteDb.ExecuteScalar(cmd);
    }
  }
}