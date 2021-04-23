using System;
using DB.Vote;

namespace Vote
{
  public static class UpdatePoliticianSearchKeys
  {
    public static void Update()
    {
      string message;

      try
      {
        VotePage.LogInfo("UpdatePoliticianSearchKeys", "Started");

        var alphaNamesUpdated = 0;
        var vowelStrippedNamesUpdated = 0;

        var table = Politicians.GetAllSearchKeyUpdateData(0);
        foreach (var row in table)
        {
          var newAlphaName = row.LastName.StripAccents();

          if (newAlphaName != row.AlphaName)
          {
            row.AlphaName = newAlphaName;
            alphaNamesUpdated++;
          }

          var newVowelStrippedName = row.LastName.StripVowels();

          if (newVowelStrippedName != row.VowelStrippedName)
          {
            row.VowelStrippedName = newVowelStrippedName;
            vowelStrippedNamesUpdated++;
          }
        }

        Politicians.UpdateTable(table, PoliticiansTable.ColumnSet.SearchKeyUpdate, 0);

        message =
          $"{alphaNamesUpdated} AlphaNames updated, {vowelStrippedNamesUpdated} VowelStrippedNames updated";
      }
      catch (Exception ex)
      {
        VotePage.LogException("UpdatePoliticianSearchKeys", ex);
        message = $"Exception: {ex.Message} [see exception log for details]";
      }

      VotePage.LogInfo("UpdatePoliticianSearchKeys", message);
    }
  }
}