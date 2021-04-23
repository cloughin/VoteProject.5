using System;
using DB.Vote;

namespace Vote
{
  public static class UpdateSingleCandidateContestWinners
  {
    public static void Update()
    {
      string message;

      try
      {
        VotePage.LogInfo("UpdateSingleCandidateContestWinners", "Started");

        message =
          $"{ElectionsPoliticians.MarkWinnersForSingleCandidatePastContests(0)} winners updated";
      }
      catch (Exception ex)
      {
        VotePage.LogException("UpdateSingleCandidateContestWinners", ex);
        message = $"Exception: {ex.Message} [see exception log for details]";
      }

      VotePage.LogInfo("UpdateSingleCandidateContestWinners", message);
    }
  }
}