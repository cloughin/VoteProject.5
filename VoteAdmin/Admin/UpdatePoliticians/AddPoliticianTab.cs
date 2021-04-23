using System;
using System.Collections.Generic;
using System.Web.UI;
using DB.Vote;
using static System.String;

namespace Vote.Admin
{
  public partial class UpdatePoliticiansPage
  {
    #region Private

    private static void DeleteAllPoliticianReferences(string politicianKey)
    {
      Answers2.DeleteByPoliticianKey(politicianKey);
      ElectionsIncumbentsRemoved.DeleteByPoliticianKey(politicianKey);
      ElectionsIncumbentsRemoved.UpdateRunningMateKeyByRunningMateKey(Empty, politicianKey);
      ElectionsPoliticians.DeleteByPoliticianKey(politicianKey);
      ElectionsPoliticians.UpdateRunningMateKeyByRunningMateKey(Empty, politicianKey);
      OfficesOfficials.DeleteByPoliticianKey(politicianKey);
      OfficesOfficials.UpdateRunningMateKeyByRunningMateKey(Empty, politicianKey);
      PoliticiansImagesBlobs.DeleteByPoliticianKey(politicianKey);
      PoliticiansImagesData.DeleteByPoliticianKey(politicianKey);
      TempEmail.DeleteByPoliticianKey(politicianKey);
    }

    private static void FormatReferences(ICollection<string> referenceList, string tableName,
      int references)
    {
      if (references > 0)
        referenceList.Add($"{tableName} ({references})");
    }

    #region DataItem object

    [PageInitializer]
    // ReSharper disable once UnusedMember.Local
    // Invoked via Reflection
    private class AddCandidatesTabItem : DataItemBase
    {
      // The rest of this is in the ManagePoliticiansPanel control
      // ReSharper disable once UnusedMember.Local
      internal static void Initialize(TemplateControl page)
      {
        InitializeGroup(page, "AddCandidates");
      }

      protected override bool Update(object newValue) => false;
    }

    #endregion DataItem object

    #endregion Private

    #region Event handlers and overrides

    protected void ButtonAddCandidates_OnClick(object sender, EventArgs e)
    {
      switch (AddCandidatesReloading.Value)
      {
        case "reloading":
        {
          DeletePoliticianOverrideContainer.AddCssClasses("hidden");
          DeletePoliticianOverride.Checked = false;
          AddCandidatesReloading.Value = Empty;
          _ManagePoliticiansPanel.LoadControls();
          _ManagePoliticiansPanel.ClearAddNewCandidate(true);
          break;
        }

        case "deleting":
        {
          AddCandidatesReloading.Value = Empty;
          DeletePoliticianOverrideContainer.AddCssClasses("hidden");
          var keyToDelete = AddCandidatesKeyToDelete.GetValue();

          // check for meaningful usages
          var referenceList = new List<string>();
          FormatReferences(referenceList, "Answers", Answers2.CountByPoliticianKey(keyToDelete));
          FormatReferences(referenceList, "ElectionsPoliticians",
            ElectionsPoliticians.CountByPoliticianKey(keyToDelete));
          FormatReferences(referenceList, "ElectionsPoliticians:RunningMateKey",
            ElectionsPoliticians.CountByRunningMateKey(keyToDelete));
          FormatReferences(referenceList, "OfficesOfficials",
            OfficesOfficials.CountByPoliticianKey(keyToDelete));
          FormatReferences(referenceList, "OfficesOfficials:RunningMateKey",
            OfficesOfficials.CountByRunningMateKey(keyToDelete));

          if (referenceList.Count > 0 && !DeletePoliticianOverride.Checked)
          {
            FeedbackAddCandidates.AddError(
              "Cannot delete because the PoliticianKey is referenced in the following tables. Check the box to override.");
            DeletePoliticianOverrideContainer.RemoveCssClass("hidden");
            foreach (var @ref in referenceList)
              FeedbackAddCandidates.AddError(@ref);
            return;
          }

          // delete
          DeletePoliticianOverride.Checked = false;
          var name = PageCache.Politicians.GetPoliticianName(keyToDelete);
          if (referenceList.Count > 0)
            DeleteAllPoliticianReferences(keyToDelete);
          Politicians.DeleteByPoliticianKey(keyToDelete);
          FeedbackAddCandidates.AddInfo($"Politician {name} ({keyToDelete}) deleted.");
          AddCandidatesKeyToDelete.Value = Empty;
          //_DeleteDistrictsTabInfo.ClearValidationErrors();
          //PopulateLocalDistrictDropdown();
          //_DeleteDistrictsTabInfo.LoadControls();

          break;
        }

        case "":
        {
          //// normal update
          //_ManagePoliticiansPanel.Update();
          //_ManagePoliticiansPanel.ClearAddNewCandidate();
          break;
        }

        default:
          throw new VoteException($"Unknown reloading option: '{AddCandidatesReloading.Value}'");
      }
    }

    #endregion Event handlers and overrides
  }
}