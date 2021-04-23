using System;
using static System.String;

namespace DB.Vote
{
  public partial class VolunteersView
  {
    public static void DeleteByEmail(string email, int commandTimeout = -1)
    {
      PartiesEmails.DeleteByPartyEmailIsVolunteer(email, true, commandTimeout);
    }

    public static VolunteersViewTable GetAllDataSorted(string stateCode, string partyCode,
      int commandTimeout = -1)
    {
      string whereClause;
      switch (stateCode)
      {
        case "":
          switch (partyCode)
          {
            case " ":
              whereClause = Empty;
              break;

            default:
              whereClause = $" WHERE PartyKey LIKE '__{partyCode}'";
              break;
          }
          break;

        default:
          switch (partyCode)
          {
            case " ":
              whereClause = $" WHERE StateCode='{stateCode}'";
              break;

            default:
              whereClause = $" WHERE PartyKey='{stateCode}{partyCode}'";
              break;
          }
          break;
      }
      var cmdText = SelectAllCommandText + whereClause +
        " ORDER BY StateName,PartyKey,LastName,FirstName";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      return FillTable(cmd, VolunteersViewTable.ColumnSet.All);
    }

    public static void Insert(
      string email, string password, string partyKey, string firstName,
      string lastName, string phone, int commandTimeout = -1)
    {
      const string cmdText = "INSERT INTO PartiesEmails (PartyEmail,PartyPassword,PartyKey," +
        "PartyContactFName,PartyContactLName,PartyContactPhone,PartyContactTitle,IsVolunteer,DateStamp) " +
        "VALUES (@PartyEmail,@PartyPassword,@PartyKey,@PartyContactFirstName,@PartyContactLastName," +
        "@PartyContactPhone,@PartyContactTitle,@IsVolunteer,@DateStamp)";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "PartyEmail", email);
      VoteDb.AddCommandParameter(cmd, "PartyPassword", password);
      VoteDb.AddCommandParameter(cmd, "PartyKey", partyKey);
      VoteDb.AddCommandParameter(cmd, "PartyContactFirstName", firstName);
      VoteDb.AddCommandParameter(cmd, "PartyContactLastName", lastName);
      VoteDb.AddCommandParameter(cmd, "PartyContactPhone", phone);
      VoteDb.AddCommandParameter(cmd, "PartyContactTitle", Empty);
      VoteDb.AddCommandParameter(cmd, "IsVolunteer", true);
      VoteDb.AddCommandParameter(cmd, "DateStamp", DateTime.UtcNow);
      VoteDb.ExecuteNonQuery(cmd);
    }

    // ReSharper disable once UnusedMethodReturnValue.Global
    public static int UpdatePartyKey(string newValue, string email)
    {
      return PartiesEmails.UpdatePartyKey(newValue, email);
    }

    // ReSharper disable once UnusedMethodReturnValue.Global
    public static int UpdateColumn(Column column, object newValue, string email)
    {
      var cmdText = "UPDATE PartiesEmails SET {0}=@newValue WHERE PartyEmail=@PartyEmail";

      PartiesEmails.Column partiesEmailsColumn;
      switch (column)
      {
        case Column.Email:
          partiesEmailsColumn = PartiesEmails.Column.PartyEmail;
          break;

        case Column.FirstName:
          partiesEmailsColumn = PartiesEmails.Column.PartyContactFirstName;
          break;

        case Column.LastName:
          partiesEmailsColumn = PartiesEmails.Column.PartyContactLastName;
          break;

        case Column.PartyKey:
          partiesEmailsColumn = PartiesEmails.Column.PartyKey;
          break;

        case Column.Password:
          partiesEmailsColumn = PartiesEmails.Column.PartyPassword;
          break;

        case Column.Phone:
          partiesEmailsColumn = PartiesEmails.Column.PartyContactPhone;
          break;

        default:
          throw new Exception("VolunteersView invalid column");
      }

      cmdText = Format(cmdText, PartiesEmails.GetColumnName(partiesEmailsColumn));
      var cmd = VoteDb.GetCommand(cmdText, -1);
      VoteDb.AddCommandParameter(cmd, "PartyEmail", email);
      VoteDb.AddCommandParameter(cmd, "newValue", newValue);
      return VoteDb.ExecuteNonQuery(cmd);
    }
  }
}