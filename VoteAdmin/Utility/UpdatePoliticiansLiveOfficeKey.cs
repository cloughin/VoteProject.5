using System;
using System.Linq;
using DB.Vote;

namespace Vote
{
  public static class UpdatePoliticiansLiveOfficeKey
  {
    public static void Update()
    {
      string message;

      try
      {
        VotePage.LogInfo("UpdatePoliticiansLiveOfficeKey", "Started");

        var officeKeysUpdated = 0;
        var officeStatusesUpdated = 0;
        var electionKeysUpdated = 0;

        var dictionary = PoliticiansLiveOfficeKeyView.GetAllData(commandTimeout: 3600)
          .ToDictionary(row => row.PoliticianKey, row => row);
        var politiciansTable = Politicians.GetAllLiveOfficeData();

        foreach (var politiciansRow in politiciansTable)
        {
          PoliticiansLiveOfficeKeyViewRow viewRow;
          if (!dictionary.TryGetValue(politiciansRow.PoliticianKey, out viewRow))
            continue;
          var keyAndStatus =
            PoliticianOfficeStatus.FromLiveOfficeKeyAndStatus(
              viewRow.LiveOfficeKeyAndStatus);
          if (keyAndStatus.OfficeKey != politiciansRow.LiveOfficeKey)
          {
            Politicians.UpdateLiveOfficeKey(keyAndStatus.OfficeKey,
              politiciansRow.PoliticianKey);
            officeKeysUpdated++;
          }
          if (keyAndStatus.PoliticianStatus.ToString() !=
            politiciansRow.LiveOfficeStatus)
          {
            Politicians.UpdateLiveOfficeStatus(
              keyAndStatus.PoliticianStatus.ToString(), politiciansRow.PoliticianKey);
            officeStatusesUpdated++;
          }
          // ReSharper disable InvertIf
          if (keyAndStatus.ElectionKey != politiciansRow.LiveElectionKey)
          // ReSharper restore InvertIf
          {
            Politicians.UpdateLiveElectionKey(
              keyAndStatus.ElectionKey, politiciansRow.PoliticianKey);
            electionKeysUpdated++;
          }
        }

        message =
          $"{officeKeysUpdated} LiveOfficeKeys updated," +
          $" {officeStatusesUpdated} LiveOfficeStatuses updated," +
          $" {electionKeysUpdated} LiveElectionKeys updated";
      }
      catch (Exception ex)
      {
        VotePage.LogException("UpdatePoliticiansLiveOfficeKey", ex);
        message = $"Exception: {ex.Message} [see exception log for details]";
      }

      VotePage.LogInfo("UpdatePoliticiansLiveOfficeKey", message);
    }
  }
}