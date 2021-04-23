using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB;
using DB.Vote;
using Vote.Controls;
using VoteLibrary.Utility;
using static System.String;

namespace Vote.Admin
{
  public partial class UpdateJurisdictionsPage
  {
    #region Private

    private static readonly string[] GenericDistrictWords =
    {
      "and", "area", "authority", "board", "borough", "by", "city", "county", "district",
      "dos", "east","los",  "new", "north", "of", "port", "precinct", "school", "south",
      "the", "town", "trustee", "unified", "valley", "village", "ward", "west", "zone"
    };

    private IList<string> GetDistrictList(string proposedName, out bool hasDuplicates)
    {
      // break test string into non-generic words
      var nameWords =
        proposedName.SafeString()
          .Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)
          .Where(w => !GenericDistrictWords.Contains(w, StringComparer.OrdinalIgnoreCase));

      // format a district list with potential conflicts highlighted in <em> tags
      var thereWereDuplicates = false;
      var comparer = new AlphanumericComparer();
      var districtList =
        LocalDistricts.GetLocalsForCounty(StateCode, CountyCode)
          .Rows.Cast<DataRow>()
          .OrderBy(r => r.LocalDistrict(), comparer)
          .Select(r =>
            // Use Format so line breaks can be inserted for readability
            // ReSharper disable once UseStringInterpolation
              Format("{0} ({1})",
                // Format parameter 0:
                // the district name, with any matching, non-generic words in <em> tags
                Join(" ",
                  r.LocalDistrict()
                    .Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(w =>
                    {
                      if (!nameWords.Contains(w, StringComparer.OrdinalIgnoreCase))
                        return w;
                      thereWereDuplicates = true;
                      return $"<em>{w}</em>";
                    })), r.LocalKey())).ToList();

      hasDuplicates = thereWereDuplicates;
      return districtList.Count == 0 ? null : districtList;
    }

    private bool TigerChecks(out string localType, out List<string> localIds, 
      HtmlInputCheckBox controlEntireCounty, DropDownList controlTigerDistrict,
      DropDownList controlTigerPlace, DropDownList controlSchoolDistrict,
      DropDownList controlCityCouncilDistrict, DropDownList controlCountySupervisorsDistrict, 
      FeedbackContainerControl feedback)
    {
      localType = Empty;
      localIds = new List<string>();
      if (controlEntireCounty.Checked)
      {
        localType = LocalIdsCodes.LocalTypeVote;
        localIds.Add(LocalIdsCodes.GetNextVoteIdForState(StateCode));
      }
      else
      {
        var tigerDistrict = controlTigerDistrict.GetValue();
        var tigerPlace = controlTigerPlace.GetValue();
        var schoolDistrict = controlSchoolDistrict.GetValue();
        var cityCouncilDistrict = controlCityCouncilDistrict.GetValue();
        var countySupervisorsDistrict = controlCountySupervisorsDistrict.GetValue();
        var selections = 0;
        if (!IsNullOrWhiteSpace(tigerPlace))
        {
          selections++;
          localType = LocalIdsCodes.LocalTypeTiger;
          localIds.Add(tigerPlace);
        }
        if (!IsNullOrWhiteSpace(tigerDistrict))
        {
          selections++;
          localType = LocalIdsCodes.LocalTypeTiger;
          localIds.Add(tigerDistrict);
        }
        if (!IsNullOrWhiteSpace(schoolDistrict))
        {
          selections++;
          localType = schoolDistrict.Substring(0, 1);
          localIds.Add(schoolDistrict.Substring(1));
        }
        if (!IsNullOrWhiteSpace(cityCouncilDistrict))
        {
          selections++;
          localType = LocalIdsCodes.LocalTypeCityCouncil;
          localIds.Add(cityCouncilDistrict);
        }
        if (!IsNullOrWhiteSpace(countySupervisorsDistrict))
        {
          selections++;
          localType = LocalIdsCodes.LocalTypeCountySupervisors;
          localIds.Add(countySupervisorsDistrict);
        }
        if (selections == 0)
        {
          feedback.PostValidationError(
            new[]
            {
              controlTigerDistrict, controlTigerPlace, controlSchoolDistrict,
              controlCityCouncilDistrict, controlCountySupervisorsDistrict
            },
            "At least one item must be selected");
          return true;
        }
        if (selections > 1)
        {
          feedback.PostValidationError(
            new[]
            {
              controlTigerDistrict, controlTigerPlace, controlSchoolDistrict,
              controlCityCouncilDistrict, controlCountySupervisorsDistrict
            },
            "Conflicting selections");
          return true;
        }
      }
      return false;
    }

    private void PopulateTigerDistricts(DropDownList dropDownList, string tigerType,
      string doNotDisable = null, string selected = null)
    {
      if (selected == null) selected = doNotDisable;
      var districtsTable = tigerType == TigerPlacesCounties.TigerTypeCousub
        ? TigerPlacesCounties.GetCousubsByCountyCode(StateCode, CountyCode)
        .Rows.Cast<DataRow>().ToArray()
        : TigerPlacesCounties.GetPlacesByCountyCode(StateCode, CountyCode)
        .Rows.Cast<DataRow>().ToArray();
      var districtItems = new List<SimpleListItem>
        {
          new SimpleListItem {Text = "<none>", Value = Empty},
          districtsTable
            .Select(r => new SimpleListItem {Text = r.LongName(), Value = r.TigerCode()})
            .ToArray()
        };
      Utility.PopulateFromList(dropDownList, districtItems, selected);

      // apply disabling
      foreach (var item in dropDownList.Items.OfType<ListItem>())
      {
        if (item.Value != doNotDisable)
        {
          var matchingRow = districtsTable.FirstOrDefault(r => r.TigerCode() == item.Value);
          if (!IsNullOrWhiteSpace(matchingRow?.LocalKey()))
            item.Attributes.Add("disabled", "disabled");
        }
      }
    }

    private void PopulateTigerSchoolDistricts(DropDownList dropDownList, string doNotDisable = null, 
      string selected = null)
    {
      if (selected == null) selected = doNotDisable;
      var districtsTable =
        TigerPlacesCounties.GetTigerSchoolDistrictsByCountyCode(StateCode, CountyCode)
        .Rows.Cast<DataRow>().ToArray();
      var districtItems = new List<SimpleListItem>
        {
          new SimpleListItem {Text = "<none>", Value = Empty},
          districtsTable
            .Select(r => new SimpleListItem { Text = r.Name(), Value = r.TigerType() + r.TigerCode()})
            .ToArray()
        };
      Utility.PopulateFromList(dropDownList, districtItems, selected);

      // apply disabling
      foreach (var item in dropDownList.Items.OfType<ListItem>())
      {
        if (item.Value != doNotDisable)
        {
          var matchingRow =
            districtsTable.FirstOrDefault(r => r.TigerType() + r.TigerCode() == item.Value);
          if (!IsNullOrWhiteSpace(matchingRow?.LocalKey()))
            item.Attributes.Add("disabled", "disabled");
        }
      }
    }

    private void PopulateCityCouncilDistricts(DropDownList dropDownList, string doNotDisable = null, 
      string selected = null)
    {
      if (selected == null) selected = doNotDisable;
      var districtsTable =
        CityCouncil.GetCityCouncilDistrictsByCountyCode(StateCode, CountyCode)
        .Rows.Cast<DataRow>().ToArray();

      var districtItems = new List<SimpleListItem>
        {
          new SimpleListItem {Text = "<none>", Value = Empty},
          districtsTable
            .Select(r => new SimpleListItem { Text = r.Name(), Value = r.CityCouncilCode()})
            .ToArray()
        };
      Utility.PopulateFromList(dropDownList, districtItems, selected);

      // apply disabling
      foreach (var item in dropDownList.Items.OfType<ListItem>())
      {
        if (item.Value != doNotDisable)
        {
          var matchingRow =
            districtsTable.FirstOrDefault(r => r.CityCouncilCode() == item.Value);
          if (!IsNullOrWhiteSpace(matchingRow?.LocalKey()))
            item.Attributes.Add("disabled", "disabled");
        }
      }
    }

    private void PopulateCountySupervisorsDistricts(DropDownList dropDownList, string doNotDisable = null,
      string selected = null)
    {
      if (selected == null) selected = doNotDisable;
      var districtsTable =
        CountySupervisors.GetCountySupervisorsDistrictsByCountyCode(StateCode, CountyCode)
        .Rows.Cast<DataRow>().ToArray();

      var districtItems = new List<SimpleListItem>
        {
          new SimpleListItem {Text = "<none>", Value = Empty},
          districtsTable
            .Select(r => new SimpleListItem { Text = r.Name(), Value = r.CountySupervisorsCode()})
            .ToArray()
        };
      Utility.PopulateFromList(dropDownList, districtItems, selected);

      // apply disabling
      foreach (var item in dropDownList.Items.OfType<ListItem>())
      {
        if (item.Value != doNotDisable)
        {
          var matchingRow =
            districtsTable.FirstOrDefault(r => r.CountySupervisorsCode() == item.Value);
          if (!IsNullOrWhiteSpace(matchingRow?.LocalKey()))
            item.Attributes.Add("disabled", "disabled");
        }
      }
    }

    private void PopulateTigerDistrictsForAddDistricts(bool select = false)
    {
      PopulateTigerDistricts(ControlAddDistrictsTigerDistrict, TigerPlacesCounties.TigerTypeCousub,
        null, select ? ControlAddDistrictsTigerDistrict.GetValue() : null);
      PopulateTigerDistricts(ControlAddDistrictsTigerPlace, TigerPlacesCounties.TigerTypePlace,
        null, select ? ControlAddDistrictsTigerPlace.GetValue() : null);
      PopulateTigerSchoolDistricts(ControlAddDistrictsSchoolDistrict, null,
        select ? ControlAddDistrictsSchoolDistrict.GetValue() : null);
      PopulateCityCouncilDistricts(ControlAddDistrictsCouncilDistrict, null,
        select ? ControlAddDistrictsCouncilDistrict.GetValue() : null);
      PopulateCountySupervisorsDistricts(ControlAddDistrictsSupervisorsDistrict, null,
        select ? ControlAddDistrictsSupervisorsDistrict.GetValue() : null);
    }

    private bool RefreshCurrentDistrictsList(Control currentDistricts, string proposedName = null)
    {
      currentDistricts.Controls.Clear();
      var districtList = GetDistrictList(proposedName, out var hasDuplicates);
      if (districtList == null)
        new HtmlP { InnerText = "<none>" }.AddTo(currentDistricts, "none");
      else
        foreach (var districtLine in districtList)
          new HtmlP { InnerHtml = districtLine }.AddTo(currentDistricts);
      return hasDuplicates;
    }

    #region DataItem object

    [PageInitializer]
    private class AddDistrictsTabItem : JurisdictionsDataItem
    {
      private const string GroupName = "AddDistricts";

      private AddDistrictsTabItem(UpdateJurisdictionsPage page)
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

      private static AddDistrictsTabItem[] GetTabInfo(
        UpdateJurisdictionsPage page)
      {
        var addDistrictsTabInfo = new[]
        {
          new AddDistrictsTabItem(page)
          {
            Column = "LocalDistrict",
            Description = "Local District Name",
            Validator = ValidateTitleCaseRequired
          },
          new AddDistrictsTabItem(page)
          {
            Column = "EntireCounty",
            Description = "Include District in Entire County",
            ConvertFn = ToBool
          },
          new AddDistrictsTabItem(page)
          {
            Column = "TigerDistrict",
            Description = "The Tiger District (County Subdivision) name"
          },
          new AddDistrictsTabItem(page)
          {
            Column = "TigerPlace",
            Description = "The Tiger Place name"
          },
          new AddDistrictsTabItem(page)
          {
            Column = "SchoolDistrict",
            Description = "The School District name"
          },
          new AddDistrictsTabItem(page)
          {
            Column = "CouncilDistrict",
            Description = "The Council District name"
          },
          new AddDistrictsTabItem(page)
          {
            Column = "SupervisorsDistrict",
            Description = "The Supervisors District name"
          },
          new AddDistrictsTabItem(page)
          {
            Column = "Override",
            Description = "Add Conflicted District",
            ConvertFn = ToBool
          }
        };

        foreach (var item in addDistrictsTabInfo) item.InitializeItem(page);

        InitializeGroup(page, GroupName);

        return addDistrictsTabInfo;
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(UpdateJurisdictionsPage page)
        // ReSharper restore UnusedMember.Local
      {
        page._AddDistrictsTabInfo = GetTabInfo(page);
        if (page.AdminPageLevel != AdminPageLevel.County)
          page.TabAddDistrictsItem.Visible = false;
      }
    }

    private AddDistrictsTabItem[] _AddDistrictsTabInfo;

    #endregion DataItem object

    #endregion Private

    #region Event handlers and overrides

    protected void ButtonAddDistricts_OnClick(object sender, EventArgs e)
    {
      AddDistrictsOverride.AddCssClasses("hidden");
      switch (AddDistrictsReloading.Value)
      {
        case "reloading":
        {
          AddDistrictsReloading.Value = Empty;
          _AddDistrictsTabInfo.ClearValidationErrors();
          RefreshCurrentDistrictsList(AddDistrictsCurrentDistricts);
          _AddDistrictsTabInfo.LoadControls();

          PopulateTigerDistrictsForAddDistricts();

          FeedbackAddDistricts.AddInfo("Add Single Districts information loaded.");
        }
          break;

        case "":
        {
          try
          {
            // normal update
            _AddDistrictsTabInfo.ClearValidationErrors();
            RefreshCurrentDistrictsList(AddDistrictsCurrentDistricts);
            PopulateTigerDistrictsForAddDistricts(true);

            // validate
            if (!_AddDistrictsTabInfo.FindItem("LocalDistrict").Validate())
              return;

            if (TigerChecks(out var localType, out var localIds, ControlAddDistrictsEntireCounty,
              ControlAddDistrictsTigerDistrict, ControlAddDistrictsTigerPlace,
              ControlAddDistrictsSchoolDistrict, ControlAddDistrictsCouncilDistrict,
              ControlAddDistrictsSupervisorsDistrict, FeedbackAddDistricts)) return;

            var localDistrict = ControlAddDistrictsLocalDistrict.Text;
            if (localType == LocalIdsCodes.LocalTypeVote)
            {
              var @override = ControlAddDistrictsOverride.Checked;
              var conflicted = RefreshCurrentDistrictsList(AddDistrictsCurrentDistricts,
                localDistrict);
              if (!@override && conflicted)
              {
                AddDistrictsOverride.RemoveCssClass("hidden");
                FeedbackAddDistricts.PostValidationError(AddDistrictsOverride,
                  "Potential duplicate name or code");
                return;
              }
            }

            var localKey = LocalDistricts.GetAvailableLocalKey(StateCode);

            // add
            LocalDistricts.Insert(StateCode, localKey, localDistrict,
              Empty, Empty, Empty, Empty, Empty,
              Empty, Empty, Empty, Empty, Empty,
              Empty, Empty, Empty, Empty, Empty,
              Empty, Empty, Empty, false, false);

            foreach (var localId in localIds)
            {
              LocalIdsCodes.Insert(StateCode, localType, localId, localKey);
              if (localType == LocalIdsCodes.LocalTypeVote)
                TigerPlacesCounties.Insert(StateCode, CountyCode, localId,
                  TigerPlacesCounties.TigerTypeVote);
            }

            _AddDistrictsTabInfo.ClearValidationErrors();
            RefreshCurrentDistrictsList(AddDistrictsCurrentDistricts);
            _AddDistrictsTabInfo.LoadControls();
            PopulateTigerDistrictsForAddDistricts();
            NavigateJurisdictionUpdatePanel.Update();
            NavigateJurisdiction.Initialize();
            FeedbackAddDistricts.AddInfo($"Local District {localDistrict} ({localKey}) added.");
          }
          catch (Exception ex)
          {
            FeedbackAddDistricts.HandleException(ex);
          }
        }
          break;

        default:
          throw new VoteException(
            $"Unknown reloading option: '{AddDistrictsReloading.Value}'");
      }
    }

    #endregion Event handlers and overrides
  }
}