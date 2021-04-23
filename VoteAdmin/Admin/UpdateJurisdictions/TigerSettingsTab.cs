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

    private void LoadTigerControlsForTigerSettings()
    {
      var tigerData =
        LocalIdsCodes.GetLocalData(StateCode, LocalKey).Rows.Cast<DataRow>().ToList();
      ControlTigerSettingsEntireCounty.SetValue(
        tigerData.Any(d => d.LocalType() == LocalIdsCodes.LocalTypeVote).ToString());
      PopulateTigerDistrictsForTigerSettings(tigerData);
    }

    private void PopulateTigerDistrictsForTigerSettings(IReadOnlyCollection<DataRow> tigerData, bool select = false)
    { 
      PopulateTigerDistricts(ControlTigerSettingsTigerDistrict, TigerPlacesCounties.TigerTypeCousub,
        tigerData.FirstOrDefault(r => r.TigerType() == TigerPlacesCounties.TigerTypeCousub)?.LocalId(),
        select ? ControlTigerSettingsTigerDistrict.GetValue() : null);
      PopulateTigerDistricts(ControlTigerSettingsTigerPlace, TigerPlacesCounties.TigerTypePlace,
        tigerData.FirstOrDefault(r => r.TigerType() == TigerPlacesCounties.TigerTypePlace)?.LocalId(),
        select ? ControlTigerSettingsTigerPlace.GetValue() : null);
      var dndSchool =
        tigerData.FirstOrDefault(r => 
        TigerPlacesCounties.TigerTypeSchoolDistricts.Contains(r.LocalType()));
      PopulateTigerSchoolDistricts(ControlTigerSettingsSchoolDistrict,
        dndSchool == null ? null : dndSchool.LocalType() + dndSchool.LocalId(),
        select ? ControlTigerSettingsSchoolDistrict.GetValue() : null);
      PopulateCityCouncilDistricts(ControlTigerSettingsCouncilDistrict,
        tigerData.FirstOrDefault(r => r.LocalType() == TigerPlacesCounties.TigerTypeCityCouncil)?.LocalId(),
        select ? ControlTigerSettingsCouncilDistrict.GetValue() : null);
      PopulateCountySupervisorsDistricts(ControlTigerSettingsSupervisorsDistrict,
        tigerData.FirstOrDefault(r => r.LocalType() == TigerPlacesCounties.TigerTypeCountySupervisors)?.LocalId(),
        select ? ControlTigerSettingsSupervisorsDistrict.GetValue() : null);
    }

    #region DataItem object

    [PageInitializer]
    private class TigerSettingsTabItem : JurisdictionsDataItem
    {
      private const string GroupName = "TigerSettings";

      private TigerSettingsTabItem(UpdateJurisdictionsPage page)
        : base(page, GroupName)
      {
      }

      private static TigerSettingsTabItem[] GetTabInfo(
        UpdateJurisdictionsPage page)
      {
        var tigerSettingsTabInfo = new[]
        {
          new TigerSettingsTabItem(page)
          {
            Column = "EntireCounty",
            Description = "Include District in Entire County",
            ConvertFn = ToBool
          },
          new TigerSettingsTabItem(page)
          {
            Column = "TigerDistrict",
            Description = "The Tiger District (County Subdivision) name"
          },
          new TigerSettingsTabItem(page)
          {
            Column = "TigerPlace",
            Description = "The Tiger Place name"
          },
          new TigerSettingsTabItem(page)
          {
            Column = "SchoolDistrict",
            Description = "The School District name"
          },
          new TigerSettingsTabItem(page)
          {
            Column = "CouncilDistrict",
            Description = "The Council District name"
          },
          new TigerSettingsTabItem(page)
          {
            Column = "SupervisorsDistrict",
            Description = "The Supervisors District name"
          }
        };

        foreach (var item in tigerSettingsTabInfo) item.InitializeItem(page);

        InitializeGroup(page, GroupName);

        return tigerSettingsTabInfo;
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(UpdateJurisdictionsPage page)
        // ReSharper restore UnusedMember.Local
      {
        page._TigerSettingsTabInfo = GetTabInfo(page);
        if (page.AdminPageLevel != AdminPageLevel.Local)
        {
          page.TabTigerSettingsItem.Visible = false;
        }
      }
    }

    private TigerSettingsTabItem[] _TigerSettingsTabInfo;

    #endregion DataItem object

    #endregion Private

    #region Event handlers and overrides

    protected void ButtonTigerSettings_OnClick(object sender, EventArgs e)
    {
      switch (TigerSettingsReloading.Value)
      {
        case "reloading":
          {
            TigerSettingsReloading.Value = Empty;
            _TigerSettingsTabInfo.ClearValidationErrors();
            LoadTigerControlsForTigerSettings();
            FeedbackTigerSettings.AddInfo("Tiger settings loaded.");
          }
          break;

        case "":
          {
            try
            {             
              // normal update
              _TigerSettingsTabInfo.ClearValidationErrors();
              var tigerData =
                LocalIdsCodes.GetLocalData(StateCode, LocalKey).Rows.Cast<DataRow>().ToList();
              PopulateTigerDistrictsForTigerSettings(tigerData, true);

              // phase 2 checks
              if (TigerChecks(out var localType, out var localIds,
                ControlTigerSettingsEntireCounty, ControlTigerSettingsTigerDistrict,
                ControlTigerSettingsTigerPlace, ControlTigerSettingsSchoolDistrict,
                ControlTigerSettingsCouncilDistrict, ControlTigerSettingsSupervisorsDistrict, 
                FeedbackTigerSettings)) return;

              // delete TigerPlacesCounties row if type Vote -- these are not intrinsically tied
              // to county(s)
              var localIdsCodes = LocalIdsCodes.GetDataByStateCodeLocalKey(StateCode,
                LocalKey);
              Debug.Assert(localIdsCodes.Count == 1);
              if (localIdsCodes[0].LocalType == LocalIdsCodes.LocalTypeVote)
                TigerPlacesCounties.DeleteByStateCodeCountyCodeTigerCodeTigerType(StateCode,
                  CountyCode, localIdsCodes[0].LocalId, LocalIdsCodes.LocalTypeVote);
              // delete existing
              LocalIdsCodes.DeleteByStateCodeLocalKey(StateCode, LocalKey);

              // add new
              foreach (var localId in localIds)
              {
                LocalIdsCodes.Insert(StateCode, localType, localId, LocalKey);
                if (localType == LocalIdsCodes.LocalTypeVote)
                  TigerPlacesCounties.Insert(StateCode, CountyCode, localId,
                    TigerPlacesCounties.TigerTypeVote);
              }

              LoadTigerControlsForTigerSettings();
              FeedbackTigerSettings.AddInfo("Tiger Settings changed.");
            }
            catch (Exception ex)
            {
              FeedbackTigerSettings.HandleException(ex);
            }
          }
          break;

        default:
          throw new VoteException($"Unknown reloading option: '{TigerSettingsReloading.Value}'");
      }
    }

    #endregion Event handlers and overrides
  }
}