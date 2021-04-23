using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DB;
using DB.Vote;
using static System.String;

namespace Vote.Admin
{
  public partial class UpdateJurisdictionsPage
  {
    #region Private

    private static List<SimpleListItem> FormatMultiCountyLocalsList(IEnumerable<IGrouping<string, DataRow>> groups)
    {
      var list = groups.Select(g => new SimpleListItem
      {
        Value = g.Key,
        Text = $"{g.First().LocalDistrict()}" +
          $" ({Join(", ", g.Select(r => r.County()).OrderBy(s => s, StringComparer.OrdinalIgnoreCase))})"
      }).ToList();
      if (list.Count == 0)
      {
        list.Add(new SimpleListItem(Empty, "<no eligible districts available>"));
      }
      return list;
    }

    private void PopulateDistrictsToIncludeDropdown(IEnumerable<SimpleListItem> list)
    {
      Utility.PopulateFromList(ControlIncludeMultiCountyDistrictsLocalKey, list);
    }

    #region DataItem object

    [PageInitializer]
    private class IncludeMultiCountyDistrictsTabItem : JurisdictionsDataItem
    {
      private const string GroupName = "IncludeMultiCountyDistricts";

      private IncludeMultiCountyDistrictsTabItem(UpdateJurisdictionsPage page)
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

      private static IncludeMultiCountyDistrictsTabItem[] GetTabInfo(
        UpdateJurisdictionsPage page)
      {
        var includeMultiCountyDistrictsTabInfo = new[]
        {
          new IncludeMultiCountyDistrictsTabItem(page)
          {
            Column = "LocalKey",
            Description = "Local District to Include"
          }
       };

        foreach (var item in includeMultiCountyDistrictsTabInfo) item.InitializeItem(page);

        InitializeGroup(page, GroupName);

        return includeMultiCountyDistrictsTabInfo;
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(UpdateJurisdictionsPage page)
        // ReSharper restore UnusedMember.Local
      {
        page._IncludeMultiCountyDistrictsTabInfo = GetTabInfo(page);
        if (page.AdminPageLevel != AdminPageLevel.County)
        {
          page.TabIncludeMultiCountyDistrictsItem.Visible = false;
        }
      }
    }

    private IncludeMultiCountyDistrictsTabItem[] _IncludeMultiCountyDistrictsTabInfo;

    #endregion DataItem object

    #endregion Private

    #region Event handlers and overrides

    protected void ButtonIncludeMultiCountyDistricts_OnClick(object sender, EventArgs e)
    {
      switch (IncludeMultiCountyDistrictsReloading.Value)
      {
        case "reloading":
          {
            IncludeMultiCountyDistrictsReloading.Value = Empty;
            _IncludeMultiCountyDistrictsTabInfo.ClearValidationErrors();
            var items =
              FormatMultiCountyLocalsList(LocalDistricts.GetLocalsForInclude(StateCode, CountyCode));
            PopulateDistrictsToIncludeDropdown(items);
            _IncludeMultiCountyDistrictsTabInfo.LoadControls();
            FeedbackIncludeMultiCountyDistricts.AddInfo("Include Multi-County Districts settings loaded.");
          }
          break;

        case "":
          {
            try
            {
              // normal update
              _IncludeMultiCountyDistrictsTabInfo.ClearValidationErrors();
              var localKey = ControlIncludeMultiCountyDistrictsLocalKey.GetValue();
              if (IsNullOrWhiteSpace(localKey))
                throw new VoteException("No district was selected");
              var lic = LocalIdsCodes.GetDataByStateCodeLocalKey(StateCode, localKey);
              if (lic.Count != 1)
                throw new VoteException("Missing LocalIdsCodes row");
              if (lic[0].LocalType != LocalIdsCodes.LocalTypeVote)
                throw new VoteException("Invalid LocalType");
              TigerPlacesCounties.Insert(StateCode, CountyCode, lic[0].LocalId, lic[0].LocalType);

              _IncludeMultiCountyDistrictsTabInfo.ClearValidationErrors();
              var items =
                FormatMultiCountyLocalsList(LocalDistricts.GetLocalsForInclude(StateCode, CountyCode));
              PopulateDistrictsToIncludeDropdown(items);
              _IncludeMultiCountyDistrictsTabInfo.LoadControls();
              NavigateJurisdictionUpdatePanel.Update();
              NavigateJurisdiction.Initialize();
              FeedbackIncludeMultiCountyDistricts.AddInfo("Multi-County District included.");
            }
            catch (Exception ex)
            {
              FeedbackIncludeMultiCountyDistricts.HandleException(ex);
            }
          }
          break;

        default:
          throw new VoteException($"Unknown reloading option: '{IncludeMultiCountyDistrictsReloading.Value}'");
      }
    }

    #endregion Event handlers and overrides
  }
}