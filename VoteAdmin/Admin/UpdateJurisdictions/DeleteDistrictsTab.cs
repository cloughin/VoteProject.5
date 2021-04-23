using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using DB;
using DB.Vote;
using static System.String;

namespace Vote.Admin
{
  public partial class UpdateJurisdictionsPage
  {
    #region Private

    private static void DeleteAllDistictReferences(string stateCode, string localKey)
    {
      Elections.DeleteByStateCodeLocalKey(stateCode, localKey);
      ElectionsIncumbentsRemoved.DeleteByStateCodeLocalKey(stateCode, localKey);
      ElectionsOffices.DeleteByStateCodeLocalKey(stateCode, localKey);
      ElectionsPoliticians.DeleteByStateCodeLocalKey(stateCode, localKey);
      QuestionsJurisdictions.DeleteByIssueLevelStateCodeCountyOrLocal(Issues.IssueLevelLocal,
          stateCode, localKey);
      Offices.DeleteByStateCodeLocalKey(stateCode, localKey);
      //OfficesAllIdentified.DeleteByStateCodeLocalKey(stateCode, localKey);
      OfficesOfficials.DeleteByStateCodeLocalKey(stateCode, localKey);
      Referendums.DeleteByStateCodeLocalKey(stateCode, localKey);
    }

    private static void FormatReferences(ICollection<string> referenceList, string tableName,
      int references)
    {
      if (references > 0)
        referenceList.Add($"{tableName} ({references})");
    }

    private void PopulateLocalDistrictDropdown()
    {
      LocalDistricts.Populate(ControlDeleteDistrictsLocalKey, StateCode, CountyCode,
        "<select district to delete>", Empty, null, true);
    }

    #region DataItem object

    [PageInitializer]
    private class DeleteDistrictsTabItem : JurisdictionsDataItem
    {
      private const string GroupName = "DeleteDistricts";

      private DeleteDistrictsTabItem(UpdateJurisdictionsPage page) : base(page, GroupName)
      {
      }

      protected override string GetCurrentValue()
      {
        return null;
      }

      protected override bool Update(object newValue)
      {
        return false;
      }

      private static DeleteDistrictsTabItem[] GetTabInfo(UpdateJurisdictionsPage page)
      {
        var deleteDistrictsTabInfo = new[]
        {
          new DeleteDistrictsTabItem(page)
          {
            Column = "LocalKey",
            Description = "Local District to Delete"
          }
        };

        foreach (var item in deleteDistrictsTabInfo) item.InitializeItem(page);

        InitializeGroup(page, GroupName);

        return deleteDistrictsTabInfo;
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(UpdateJurisdictionsPage page)
        // ReSharper restore UnusedMember.Local
      {
        page._DeleteDistrictsTabInfo = GetTabInfo(page);
        if (page.AdminPageLevel != AdminPageLevel.County)
          page.TabDeleteDistrictsItem.Visible = false;
      }
    }

    private DeleteDistrictsTabItem[] _DeleteDistrictsTabInfo;

    #endregion DataItem object

    #endregion Private

    #region Event handlers and overrides

    protected void ButtonDeleteDistricts_OnClick(object sender, EventArgs e)
    {
      switch (DeleteDistrictsReloading.Value)
      {
        case "reloading":
        {
          DeleteDistrictOverride.AddCssClasses("hidden");
          ControlDeleteDistrictOverride.Checked = false;
          _DeleteDistrictsTabInfo.ClearValidationErrors();
          DeleteDistrictsReloading.Value = Empty;
          PopulateLocalDistrictDropdown();
          _DeleteDistrictsTabInfo.LoadControls();
          FeedbackDeleteDistricts.AddInfo("Delete Districts information loaded.");
        }
          break;

        case "":
        {
          // normal update
          DeleteDistrictOverride.AddCssClasses("hidden");
          _DeleteDistrictsTabInfo.ClearValidationErrors();
          var localKey = ControlDeleteDistrictsLocalKey.GetValue();
          var localDistrict = LocalDistricts.GetLocalDistrictByStateCodeLocalKey(StateCode,
            localKey);

          var referenceList = new List<string>();
          if (!ControlDeleteDistrictOverride.Checked)
          {
            // check for references in other counties
            var otherCounties = new List<string>();
            foreach (var oc in
              LocalIdsCodes.GetOtherCountyReferences(StateCode, CountyCode, localKey)
                .Rows.OfType<DataRow>())
              otherCounties.Add(oc.County());

            // check for meaningful usages
            FormatReferences(referenceList, "Elections",
              Elections.CountByStateCodeLocalKey(StateCode, localKey));
            FormatReferences(referenceList, "ElectionsOffices",
              ElectionsOffices.CountByStateCodeLocalKey(StateCode, localKey));
            FormatReferences(referenceList, "ElectionsPoliticians",
              ElectionsPoliticians.CountByStateCodeLocalKey(StateCode, localKey));
            FormatReferences(referenceList, "QuestionsJurisdictions",
              QuestionsJurisdictions.CountByIssueLevelStateCodeCountyOrLocal(
                Issues.IssueLevelLocal, StateCode, localKey));
            FormatReferences(referenceList, "Offices",
              Offices.CountByStateCodeLocalKey(StateCode, localKey));
            FormatReferences(referenceList, "OfficesOfficials",
              OfficesOfficials.CountByStateCodeLocalKey(StateCode, localKey));
            FormatReferences(referenceList, "Referendums",
              Referendums.CountByStateCodeLocalKey(StateCode, localKey));

            if (referenceList.Count > 0 || otherCounties.Count > 0)
            {
              DeleteDistrictOverride.RemoveCssClass("hidden");
              if (otherCounties.Count > 0)
              {
                FeedbackDeleteDistricts.PostValidationError(ControlDeleteDistrictsLocalKey,
                  "The district is referenced in the following other counties. If you delete this" +
                  " district it will be removed from the other counties also. Check the box to" +
                  " override.");
                foreach (var oc in otherCounties)
                  FeedbackDeleteDistricts.AddError(oc);
              }
              if (referenceList.Count > 0)
              {
                FeedbackDeleteDistricts.PostValidationError(ControlDeleteDistrictsLocalKey,
                  "Cannot delete because the LocalKey is referenced in the following tables. Check the box to override.");
                foreach (var @ref in referenceList)
                  FeedbackDeleteDistricts.AddError(@ref);
              }
              return;
            }
          }

          // delete
          ControlDeleteDistrictOverride.Checked = false;
          DeleteAllDistictReferences(StateCode, localKey);
          LocalDistricts.DeleteByStateCodeLocalKey(StateCode, localKey);

          // delete TigerPlacesCounties rows if type Vote -- these are not intrinsically tied
          // to county(s)
          var localIdsCodes = LocalIdsCodes.GetDataByStateCodeLocalKey(StateCode, localKey);
          Debug.Assert(localIdsCodes.Count == 1);
          if (localIdsCodes[0].LocalType == LocalIdsCodes.LocalTypeVote)
            TigerPlacesCounties.DeleteByStateCodeTigerTypeTigerCode(StateCode,
              LocalIdsCodes.LocalTypeVote, localIdsCodes[0].LocalId);
          LocalIdsCodes.DeleteByStateCodeLocalKey(StateCode, localKey);
          _DeleteDistrictsTabInfo.ClearValidationErrors();
          PopulateLocalDistrictDropdown();
          _DeleteDistrictsTabInfo.LoadControls();
          NavigateJurisdictionUpdatePanel.Update();
          NavigateJurisdiction.Initialize();

          FeedbackDeleteDistricts.AddInfo(
            $"Local District {localDistrict} ({localKey}) deleted.");
        }
          break;

        default:
          throw new VoteException(
            $"Unknown reloading option: '{DeleteDistrictsReloading.Value}'");
      }
    }

    #endregion Event handlers and overrides
  }
}