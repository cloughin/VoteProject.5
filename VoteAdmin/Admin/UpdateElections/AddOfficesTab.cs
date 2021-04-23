using System;
using System.Linq;
using DB.Vote;
using Vote.Controls;
using static System.String;

namespace Vote.Admin
{
  public partial class UpdateElectionsPage
  {
    #region Private

    #region DataItem object

    [PageInitializer]
    private class AddOfficesTabItem : ElectionsDataItem
    {
      // This class assumes OfficeList is the only column
      private const string GroupName = "AddOffices";

      public static int OfficeCount { get; private set; }

      private AddOfficesTabItem(UpdateElectionsPage page) : base(page, GroupName)
      {
      }

      private static AddOfficesTabItem[] GetTabInfo(UpdateElectionsPage page)
      {
        var addOfficesTabInfo = new[]
        {
          new AddOfficesTabItem(page) {Column = "OfficeList", Description = "Office List"}
        };

        foreach (var item in addOfficesTabInfo) item.InitializeItem(page);

        InitializeGroup(page, GroupName);

        return addOfficesTabInfo;
      }

      protected override string GetCurrentValue()
      {
        return null;
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(UpdateElectionsPage page)
        // ReSharper restore UnusedMember.Local
      {
        page._AddOfficesTabInfo = GetTabInfo(page);
        page.ShowChangeOffices = StateCache.IsValidStateCode(page.StateCode);
        if (!page.ShowChangeOffices) page.TabChangeOfficesItem.Visible = false;
      }

      public override void LoadControl()
      {
        OfficeCount = 0;
        Elections.ActualizeElection(Page.GetElectionKey());
        var table = Elections.GetAvailableElectionOfficeData(Page.GetElectionKey(),
          Page.StateCode, Page.CountyCode, Page.LocalKey);
        if (table.Count == 0)
        {
          Page.AddOfficesMessage.RemoveCssClass("hidden");
          Page.AddOfficesControl.AddCssClasses("hidden");
        }
        else
        {
          Page.AddOfficesMessage.AddCssClasses("hidden");
          Page.AddOfficesControl.RemoveCssClass("hidden");
          var relatedJurisdictions =
            OfficeControl.CreateRelatedJurisdictionsNodes("/admin/updateElections.aspx",
              "addoffices", Page.StateCode, Page.CountyCode, Page.LocalKey);
          OfficeCount = OfficeControl.PopulateOfficeTree(table,
            Page.PlaceHolderAddOfficesTree, Page.StateCode, true, false, true,
            Page.AdminPageLevel == AdminPageLevel.State, relatedJurisdictions);
        }
      }

      protected override void Log(string oldValue, string newValue)
      {
        // Logging done in update
      }

      private string ActualizeOfficeKey(string officeKey)
      {
        return Offices.ActualizeKey(officeKey, Page.CountyCode, Page.LocalKey);
      }

      protected override bool Update(object newValue)
      {
        var electionKey = Page.GetElectionKey();
        var changed = false;
        var stringValue = newValue.ToString();

        // get the list of officeKeys
        var officeKeys = stringValue
          .Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries).ToList();

        // actualize any virtuals
        foreach (var officeKey in officeKeys)
          Offices.ActualizeOffice(officeKey, Page.CountyCode, Page.LocalKey);

        // create a dictionary of office keys from the updated tree
        var officeKeyDictionary = officeKeys.ToDictionary(ActualizeOfficeKey, key => false,
          StringComparer.OrdinalIgnoreCase);

        // get the current list of office keys for the election
        var offices = ElectionsOffices.GetOfficeKeysData(electionKey);

        // delete any that aren't in the dictionary (and from 
        // ElectionsPoliticians too), and mark those that are there
        // so we know what to add
        foreach (var office in offices)
        {
          var officeKey = office.OfficeKey;
          if (officeKeyDictionary.ContainsKey(officeKey))
            officeKeyDictionary[officeKey] = true;
          else
          {
            LogElectionsOfficesDelete(electionKey, officeKey);
            ElectionsOffices.DeleteByElectionKeyOfficeKey(electionKey, officeKey);
            ElectionsPoliticians.DeleteByElectionKeyOfficeKey(electionKey, officeKey);
            changed = true;
          }
        }

        // add offices from the dictionary that aren't marked
        foreach (var officeKey in officeKeyDictionary.Where(kvp => !kvp.Value)
          .Select(kvp => kvp.Key))
        {
          LogElectionsOfficesInsert(electionKey, officeKey);
          InsertElectionOffice(electionKey, officeKey);
          changed = true;
        }
        LoadControl();
        return changed;
      }
    }

    private AddOfficesTabItem[] _AddOfficesTabInfo;

    #endregion DataItem object

    private static string CreateFederalElectionIfNeeded(string electionKey,
      string officeKey)
    {
      var officeClass = Offices.GetOfficeClass(officeKey);
      if (!officeClass.IsFederal()) return Empty;

      var federalCode = officeClass.StateCodeProxy();
      var federalElectionKey =
        Elections.GetFederalElectionKeyFromKey(electionKey, federalCode);

      if (GetAdminPageLevel() != AdminPageLevel.State ||
        !Elections.IsGeneralElection(electionKey)) return Empty;

      if (!Elections.ElectionKeyExists(federalElectionKey))
      {
        var electionDate = Elections.GetElectionDateFromKey(electionKey);
        var electionDateString = Elections.GetElectionDateStringFromKey(electionKey);
        var electionDesc =
          $"{electionDate:MMMM d, yyyy} General Election {officeClass.Description()} State-By-State";

        Elections.Insert(federalElectionKey, federalCode, Empty, Empty,
          electionDate, electionDateString, Elections.ElectionTypeGeneralElection,
          Parties.NationalPartyAll, "ALL", Empty, electionDesc, Empty,
          Empty, DefaultDbDate, Empty, false, /*0, DefaultDbDate, 0,
          DefaultDbDate, 0, DefaultDbDate, 0, DefaultDbDate, 0,*/ Empty, 0, false,
          false, DefaultDbDate, DefaultDbDate, DefaultDbDate, DefaultDbDate, DefaultDbDate,
          DefaultDbDate, DefaultDbDate, DefaultDbDate, DefaultDbDate, null);
        ElectionsDefaults.CreateEmptyRow(federalElectionKey);
      }

      return federalElectionKey;
    }

    private static void ActualizeElectionOffice(string electionKey, string officeKey)
    {
      if (!ElectionsOffices.ElectionKeyOfficeKeyExists(electionKey, officeKey))
        InsertElectionOffice(electionKey, officeKey);
    }

    private static void InsertElectionOffice(string electionKey, string officeKey)
    {
      var stateCode = Elections.GetStateCodeFromKey(electionKey);
      var countyCode = Elections.GetCountyCodeFromKey(electionKey);
      var localKey = Elections.GetLocalKeyFromKey(electionKey);
      var federalElectionKey = CreateFederalElectionIfNeeded(electionKey, officeKey);
      var officeClass = Offices.GetOfficeClass(officeKey);
      var districtCode = Offices.GetValidDistrictCode(officeKey);
      var stateElectionKey = Elections.GetStateElectionKeyFromKey(electionKey);

      ElectionsOffices.Insert(electionKey, officeKey, stateElectionKey, federalElectionKey,
        stateCode, countyCode, localKey, districtCode, officeClass.ToInt(), false);
    }

    #endregion Private

    #region Event handlers and overrides

    protected void ButtonAddOffices_OnClick(object sender, EventArgs e)
    {
      switch (AddOfficesReloading.Value)
      {
        case "reloading":
        {
          AddOfficesReloading.Value = Empty;
          _AddOfficesTabInfo.LoadControls();
          SetElectionHeading(HeadingAddOffices);
          FeedbackAddOffices.AddInfo($"{AddOfficesTabItem.OfficeCount} offices loaded.");
        }
          break;

        case "":
        {
          // normal update
          _AddOfficesTabInfo.Update(FeedbackAddOffices);
        }
          break;

        default:
          throw new VoteException(
            $"Unknown reloading option: '{AddOfficesReloading.Value}'");
      }
    }

    #endregion Event handlers and overrides
  }
}