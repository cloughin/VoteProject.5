using System;
using System.Collections.Generic;
using DB.Vote;
using static System.String;

namespace Vote.Admin
{
  public partial class UpdateJurisdictionsPage
  {
    #region Private

    private void PopulateDistrictsToRemoveDropdown(IEnumerable<SimpleListItem> list)
    {
      Utility.PopulateFromList(ControlRemoveMultiCountyDistrictsLocalKey, list);
    }

    #region DataItem object

    [PageInitializer]
    private class RemoveMultiCountyDistrictsTabItem : JurisdictionsDataItem
    {
      private const string GroupName = "RemoveMultiCountyDistricts";

      private RemoveMultiCountyDistrictsTabItem(UpdateJurisdictionsPage page)
        : base(page, GroupName)
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

      private static RemoveMultiCountyDistrictsTabItem[] GetTabInfo(
        UpdateJurisdictionsPage page)
      {
        var removeMultiCountyDistrictsTabInfo = new []
        {
          new RemoveMultiCountyDistrictsTabItem(page)
          {
            Column = "LocalKey",
            Description = "Local District to Remove"
          }
        };

        foreach (var item in removeMultiCountyDistrictsTabInfo) item.InitializeItem(page);

        InitializeGroup(page, GroupName);

        return removeMultiCountyDistrictsTabInfo;
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(UpdateJurisdictionsPage page)
        // ReSharper restore UnusedMember.Local
      {
        page._RemoveMultiCountyDistrictsTabInfo = GetTabInfo(page);
        if (page.AdminPageLevel != AdminPageLevel.County)
        {
          page.TabRemoveMultiCountyDistrictsItem.Visible = false;
        }
      }
    }

    private RemoveMultiCountyDistrictsTabItem[] _RemoveMultiCountyDistrictsTabInfo;

    #endregion DataItem object

    #endregion Private

    #region Event handlers and overrides

    protected void ButtonRemoveMultiCountyDistricts_OnClick(object sender, EventArgs e)
    {
      switch (RemoveMultiCountyDistrictsReloading.Value)
      {
        case "reloading":
          {
            RemoveMultiCountyDistrictsReloading.Value = Empty;
            _RemoveMultiCountyDistrictsTabInfo.ClearValidationErrors();
            var items =
              FormatMultiCountyLocalsList(LocalDistricts.GetLocalsForRemove(StateCode, CountyCode));
            PopulateDistrictsToRemoveDropdown(items);
            _RemoveMultiCountyDistrictsTabInfo.LoadControls();
            FeedbackRemoveMultiCountyDistricts.AddInfo("Remove Multi-County Districts settings loaded.");
          }
          break;

        case "":
          {
            try
            {
              // normal update
              _RemoveMultiCountyDistrictsTabInfo.ClearValidationErrors();
              var localKey = ControlRemoveMultiCountyDistrictsLocalKey.GetValue();
              if (IsNullOrWhiteSpace(localKey))
                throw new VoteException("No district was selected");
              var lic = LocalIdsCodes.GetDataByStateCodeLocalKey(StateCode, localKey);
              if (lic.Count != 1)
                throw new VoteException("Missing LocalIdsCodes row");
              if (lic[0].LocalType != LocalIdsCodes.LocalTypeVote)
                throw new VoteException("Invalid LocalType");
              TigerPlacesCounties.DeleteByStateCodeCountyCodeTigerCodeTigerType(StateCode,
                CountyCode, lic[0].LocalId, lic[0].LocalType);

              _RemoveMultiCountyDistrictsTabInfo.ClearValidationErrors();
              var items =
                FormatMultiCountyLocalsList(LocalDistricts.GetLocalsForRemove(StateCode, CountyCode));
              PopulateDistrictsToRemoveDropdown(items);
              _RemoveMultiCountyDistrictsTabInfo.LoadControls();
              NavigateJurisdictionUpdatePanel.Update();
              NavigateJurisdiction.Initialize();
              FeedbackRemoveMultiCountyDistricts.AddInfo("Multi-County District removed.");
            }
            catch (Exception ex)
            {
              FeedbackRemoveMultiCountyDistricts.HandleException(ex);
            }
          }
          break;

        default:
          throw new VoteException($"Unknown reloading option: '{RemoveMultiCountyDistrictsReloading.Value}'");
      }
    }

    #endregion Event handlers and overrides
  }
}