using System;
using DB.Vote;

namespace Vote
{
  public static class UpdateOfficesAlternateOfficeLevel
  {
    private static OfficeClass ComputeAlternateOfficeClass(OfficesRow row)
    {
      // check for Governor
      if (row.OfficeLevel.ToOfficeClass() == OfficeClass.StateWide)
      {
        if (row.OfficeKey.SafeSubstring(2).ToLowerInvariant()
          .StartsWith("gov", StringComparison.Ordinal))
          return OfficeClass.USGovernors;
        if (row.OfficeKey.SafeSubstring(2, 2).ToLowerInvariant() == "lt" ||
          row.OfficeKey.SafeSubstring(2, 10).ToLowerInvariant() == "lieutenant")
          return OfficeClass.USLtGovernor;
      }

      // DC specials
      var officeKey = row.OfficeKey;
      if (officeKey.StartsWith("DCStateSenate", StringComparison.OrdinalIgnoreCase))
        officeKey = "DCStateSenate";
      else if (officeKey.StartsWith("DCBoardOfEducation",
        StringComparison.OrdinalIgnoreCase))
        officeKey = "DCBoardOfEducation";
      switch (officeKey)
      {
        case "DCMayor":
          return OfficeClass.DCMayor;

        case "DCStateSenate":
        case "DCChairmanOfTheCouncil":
        case "DCAtLargeMemberOfTheCouncil":
          return OfficeClass.DCCouncil;

        case "DCBoardOfEducation":
        case "DCStateBoardOfEducationStudentRepresentative":
        case "DCPresidentOfTheBoardOfEducation":
          return OfficeClass.DCBoardOfEducation;

        case "DCUnitedStatesSenator":
          return OfficeClass.DCShadowSenator;

        case "DCUnitedStatesRepresentative":
          return OfficeClass.DCShadowRepresentative;
      }

      return OfficeClass.Undefined;
    }

    public static void Update()
    {
      string message;

      try
      {
        VotePage.LogInfo("UpdateOfficesAlternateOfficeLevel", "Started");

        var alternateOfficeLevelsUpdated = 0;

        foreach (var row in Offices.GetAllAlternateOfficeLevelUpdateData())
        {
          var newAlternateOfficeLevel = ComputeAlternateOfficeClass(row)
            .ToInt();

          if (newAlternateOfficeLevel == row.AlternateOfficeLevel) continue;

          Offices.UpdateAlternateOfficeLevel(newAlternateOfficeLevel, row.OfficeKey);
          alternateOfficeLevelsUpdated++;
        }

        message = $"{alternateOfficeLevelsUpdated} AlternateOfficeLevels updated";
      }
      catch (Exception ex)
      {
        VotePage.LogException("UpdateOfficesAlternateOfficeLevel", ex);
        message = $"Exception: {ex.Message} [see exception log for details]";
      }

      VotePage.LogInfo("UpdateOfficesAlternateOfficeLevel", message);
    }
  }
}