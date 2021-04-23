using System;
using Vote;

namespace DB.Vote
{
  public partial class ElectionsDefaultsRow
  {
  }

  public partial class ElectionsDefaults
  {
    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global

    public static void CreateEmptyRow(string electionKey, int commandTimeout = -1)
    {
      var defaultElectionKey = Elections.GetDefaultElectionKeyFromKey(electionKey);
      if (!DefaultElectionKeyExists(defaultElectionKey))
      {
        var stateRow = States.GetData(Elections.GetStateCodeFromKey(electionKey))[0];
        Insert(defaultElectionKey, stateRow.ElectionAdditionalInfo.SafeString(),
          stateRow.BallotInstructions.SafeString(), VotePage.DefaultDbDate, VotePage.DefaultDbDate,
          VotePage.DefaultDbDate, VotePage.DefaultDbDate, VotePage.DefaultDbDate,
          VotePage.DefaultDbDate, VotePage.DefaultDbDate, VotePage.DefaultDbDate,
          VotePage.DefaultDbDate, commandTimeout);
      }
    }

    public static void RemoveOrphanedRow(string electionKey, int commandTimeout = -1)
    {
      var defaultElectionKey = Elections.GetDefaultElectionKeyFromKey(electionKey);
      const string cmdText = "SELECT COUNT(*) FROM Elections WHERE ElectionKey LIKE @ElectionKey";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "ElectionKey", defaultElectionKey + "%");
      var count = Convert.ToInt32(VoteDb.ExecuteScalar(cmd));
      if (count == 0)
        DeleteByDefaultElectionKey(defaultElectionKey);
    }

    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }
}