using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using DB.Vote;

namespace Vote.Admin
{
  public partial class UpdateOfficesPage
  {
    #region Private

    private enum MasterOnlySubTab
    {
      LockClasses,
      ChangeKey,
      Consolidate
    }

    private List<MasterOnlySubTab> _MasterOnlySubTabs;

    #region DataItem object

    [PageInitializer]
    private class MasterOnlyTabItem : OfficesDataItem
    {
      private const string GroupName = "MasterOnly";
      public MasterOnlySubTab SubTab { get; private set; }

      protected MasterOnlyTabItem(UpdateOfficesPage page) : base(page, GroupName)
      {
      }

      private static MasterOnlyTabItem[] GetTabInfo(UpdateOfficesPage page)
      {
        var masterOnlyTabInfo = new MasterOnlyTabItem[]
        {
          new MasterOnlyClassesToLockTabItem(page)
          {
            Column = "ClassesToLock",
            Description = "Office classes to Lock",
            SubTab = MasterOnlySubTab.LockClasses
          },
          new MasterOnlyChangeKeyTabItem(page)
          {
            Column = "ChangeKeyPrefix",
            Description = "Jurisdictional Prefix",
            SubTab = MasterOnlySubTab.ChangeKey
          },
          new MasterOnlyChangeKeyTabItem(page)
          {
            Column = "OldKey",
            Description = "Old Office Key",
            SubTab = MasterOnlySubTab.ChangeKey
          },
          new MasterOnlyChangeKeyTabItem(page)
          {
            Column = "NewKey",
            Description = "New Office Key",
            SubTab = MasterOnlySubTab.ChangeKey
          },
          new MasterOnlyConsolidateTabItem(page)
          {
            Column = "ConsolidatePrefix",
            Description = "Jurisdictional Prefix",
            SubTab = MasterOnlySubTab.Consolidate
          },
          new MasterOnlyConsolidateTabItem(page)
          {
            Column = "Key1",
            Description = "Office Key 1",
            SubTab = MasterOnlySubTab.Consolidate
          },
          new MasterOnlyConsolidateTabItem(page)
          {
            Column = "Key2",
            Description = "Office Key 2",
            SubTab = MasterOnlySubTab.Consolidate
          }
        };

        foreach (var item in masterOnlyTabInfo) item.InitializeItem(page);

        InitializeGroup(page, GroupName);

        return masterOnlyTabInfo;
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(UpdateOfficesPage page)
        // ReSharper restore UnusedMember.Local
      {
        page._MasterOnlyTabInfo = GetTabInfo(page);

        page._MasterOnlySubTabs = new List<MasterOnlySubTab>
        {
          MasterOnlySubTab.LockClasses,
          MasterOnlySubTab.ChangeKey,
          MasterOnlySubTab.Consolidate
        };

        if (!IsMasterUser) page.TabMasterItem.Visible = false;
      }
    }

    private class MasterOnlyClassesToLockTabItem : MasterOnlyTabItem
    {
      internal MasterOnlyClassesToLockTabItem(UpdateOfficesPage page)
        : base(page)
      {
      }

      public override void LoadControl() => Page.CreateLockedOfficeClassCheckBoxes();
    }

    private class MasterOnlyChangeKeyTabItem : MasterOnlyTabItem
    {
      internal MasterOnlyChangeKeyTabItem(UpdateOfficesPage page)
        : base(page)
      {
      }

      public override void ResetControl() => LoadControl();

      public override void LoadControl()
      {
        switch (Column)
        {
          case "ChangeKeyPrefix":
            DataControl.SetValue(Page.JurisdictionalKey);
            break;

          default:
            DataControl.SetValue(GetDefaultValue());
            break;
        }
      }
    }

    private class MasterOnlyConsolidateTabItem : MasterOnlyTabItem
    {
      internal MasterOnlyConsolidateTabItem(UpdateOfficesPage page)
        : base(page)
      {
      }

      public override void ResetControl() => LoadControl();

      public override void LoadControl()
      {
        switch (Column)
        {
          case "ConsolidatePrefix":
            DataControl.SetValue(Page.JurisdictionalKey);
            break;

          default:
            DataControl.SetValue(GetDefaultValue());
            break;
        }
      }
    }

    private MasterOnlyTabItem[] _MasterOnlyTabInfo;

    #endregion DataItem object

    private void ChangeOfficeKey()
    {
      try
      {
        var oldKeyItem = _MasterOnlyTabInfo.Single(i => i.Column == "OldKey");
        var newKeyItem = _MasterOnlyTabInfo.Single(i => i.Column == "NewKey");
        var jurisdictionKey = JurisdictionalKey;

        var success = true;

        success &= DataItemBase.ValidateRequired(oldKeyItem);
        var oldKeyOffice = oldKeyItem.DataControl.GetValue().Trim();

        success &= DataItemBase.ValidateRequired(newKeyItem);
        var newKeyOffice = newKeyItem.DataControl.GetValue().Trim();
        if (!string.IsNullOrWhiteSpace(newKeyOffice))
        {
          // get rid of all non-alphanumerics
          newKeyOffice = Regex.Replace(newKeyOffice, @"[^\dA-Z]", string.Empty,
            RegexOptions.IgnoreCase);
          // get rid of leading numerics
          newKeyOffice = Regex.Replace(newKeyOffice, @"^\d+", string.Empty);
          var maxLength = Offices.OfficeKeyMaxLength - jurisdictionKey.Length;
          if (newKeyOffice.Length > maxLength)
          {
            newKeyItem.Feedback.PostValidationError(newKeyItem.DataControl, newKeyItem.Description +
              " is too long by " + (newKeyOffice.Length - maxLength) + " characters.");
            success = false;
          }
          if (newKeyOffice.Length == 0)
          {
            newKeyItem.Feedback.PostValidationError(newKeyItem.DataControl, newKeyItem.Description +
              " consists entirely of non-key characters.");
            success = false;
          }
        }

        if (success && (oldKeyOffice == newKeyOffice))
        {
          newKeyItem.Feedback.PostValidationError(newKeyItem.DataControl, newKeyItem.Description +
            " is identical to the Old Office Key.");
          success = false;
        }

        var oldOfficeKey = jurisdictionKey + oldKeyOffice;
        var newOfficeKey = jurisdictionKey + newKeyOffice;
        var caseChangeOnly = oldOfficeKey.IsEqIgnoreCase(newOfficeKey);

        if (success && !caseChangeOnly)
        {
          // Make sure the new office key doesn't already exist
          var existsInTables = new List<string>();
          if (Offices.OfficeKeyExists(newOfficeKey)) existsInTables.Add(Offices.TableName);
          if (ElectionsOffices.OfficeKeyExists(newOfficeKey))
            existsInTables.Add(ElectionsOffices.TableName);
          if (ElectionsPoliticians.OfficeKeyExists(newOfficeKey))
            existsInTables.Add(ElectionsPoliticians.TableName);
          if (OfficesOfficials.OfficeKeyExists(newOfficeKey))
            existsInTables.Add(OfficesOfficials.TableName);
          if (ElectionsIncumbentsRemoved.OfficeKeyExists(newOfficeKey))
            existsInTables.Add(ElectionsIncumbentsRemoved.TableName);
          if (Politicians.OfficeKeyExists(newOfficeKey)) existsInTables.Add(Politicians.TableName);

          if (existsInTables.Count > 0)
          {
            newKeyItem.Feedback.PostValidationError(newKeyItem.DataControl, newKeyItem.Description +
              " already exists in the following tables: " + string.Join(", ", existsInTables));
            success = false;
          }
        }

        if (!success) return;

        // do the replacement
        var updateCount = 0;

        updateCount += Offices.UpdateOfficeKey(newOfficeKey, oldOfficeKey);
        updateCount += ElectionsOffices.UpdateOfficeKeyByOfficeKey(newOfficeKey, oldOfficeKey);
        updateCount += ElectionsPoliticians.UpdateOfficeKeyByOfficeKey(newOfficeKey, oldOfficeKey);
        updateCount += OfficesOfficials.UpdateOfficeKeyByOfficeKey(newOfficeKey, oldOfficeKey);
        updateCount += ElectionsIncumbentsRemoved.UpdateOfficeKeyByOfficeKey(newOfficeKey,
          oldOfficeKey);
        updateCount += Politicians.UpdateOfficeKeyByOfficeKey(newOfficeKey, oldOfficeKey);

        var msg = $"{updateCount} instances of the old office key {oldOfficeKey} were found.";
        if (updateCount > 0)
          msg += $" They were all changed to the new office key {newOfficeKey}.";
        FeedbackMasterOnly.AddInfo(msg);
        ResetMasterOnlySubTab(MasterOnlySubTab.ChangeKey);
      }
      catch (Exception ex)
      {
        FeedbackMasterOnly.PostValidationError(ControlMasterOnlyNewKey,
          "The office key could not be changed: " + ex.Message);
      }
    }

    private void Consolidate()
    {
      try
      {
        var key1Item = _MasterOnlyTabInfo.Single(i => i.Column == "Key1");
        var key2Item = _MasterOnlyTabInfo.Single(i => i.Column == "Key2");
        var jurisdictionKey = JurisdictionalKey;

        var success = true;

        success &= DataItemBase.ValidateRequired(key1Item);
        var key1Office = key1Item.DataControl.GetValue().Trim();

        success &= DataItemBase.ValidateRequired(key2Item);
        var key2Office = key2Item.DataControl.GetValue().Trim();

        if (success && key1Office.IsEqIgnoreCase(key2Office))
        {
          key2Item.Feedback.PostValidationError(key2Item.DataControl, key2Item.Description +
            " is identical to " + key1Item.Description);
          success = false;
        }

        var officeKey1 = jurisdictionKey + key1Office;
        var officeKey2 = jurisdictionKey + key2Office;

        if (!success) return;

        // do the consolidation
        var updateCount = 0;

        if (Offices.OfficeKeyExists(officeKey1))
          updateCount += Offices.DeleteByOfficeKey(officeKey2);
        else
          updateCount += Offices.UpdateOfficeKey(officeKey1, officeKey2);
        foreach (var row in ElectionsOffices.GetDataByOfficeKey(officeKey2))
          if (ElectionsOffices.ElectionKeyOfficeKeyExists(row.ElectionKey, officeKey1))
            updateCount += ElectionsOffices.DeleteByElectionKeyOfficeKey(row.ElectionKey, officeKey2);
          else
            updateCount += ElectionsOffices.UpdateOfficeKeyByElectionKeyOfficeKey(officeKey1,
              row.ElectionKey, officeKey2);
        foreach (var row in ElectionsPoliticians.GetDataByOfficeKey(officeKey2))
          if (ElectionsPoliticians.ElectionKeyOfficeKeyPoliticianKeyExists(row.ElectionKey,
            officeKey1, row.PoliticianKey))
            updateCount += ElectionsPoliticians.DeleteByElectionKeyOfficeKeyPoliticianKey(
              row.ElectionKey, officeKey2, row.PoliticianKey);
          else
            updateCount += ElectionsPoliticians.UpdateOfficeKeyByElectionKeyOfficeKeyPoliticianKey(
              officeKey1, row.ElectionKey, officeKey2, row.PoliticianKey);
        foreach (var row in OfficesOfficials.GetDataByOfficeKey(officeKey2))
          if (OfficesOfficials.OfficeKeyPoliticianKeyExists(officeKey1, row.PoliticianKey))
            updateCount += OfficesOfficials.DeleteByOfficeKeyPoliticianKey(officeKey2,
              row.PoliticianKey);
          else
            updateCount += OfficesOfficials.UpdateOfficeKeyByOfficeKeyPoliticianKey(officeKey1,
              officeKey2, row.PoliticianKey);
        foreach (var row in ElectionsIncumbentsRemoved.GetDataByOfficeKey(officeKey2))
          if (ElectionsIncumbentsRemoved.ElectionKeyOfficeKeyPoliticianKeyExists(row.ElectionKey,
            officeKey1, row.PoliticianKey))
            updateCount += ElectionsIncumbentsRemoved.DeleteByElectionKeyOfficeKeyPoliticianKey(
              row.ElectionKey, officeKey2, row.PoliticianKey);
          else
            updateCount += ElectionsIncumbentsRemoved
              .UpdateOfficeKeyByElectionKeyOfficeKeyPoliticianKey(
                officeKey1, row.ElectionKey, officeKey2, row.PoliticianKey);
        updateCount += Politicians.UpdateOfficeKeyByOfficeKey(officeKey1, officeKey2);

        var msg = $"{updateCount} instances of the second office key {officeKey2} were found.";
        if (updateCount > 0)
          msg += $" They were all changed to the first office key {officeKey1}.";
        FeedbackMasterOnly.AddInfo(msg);
        ResetMasterOnlySubTab(MasterOnlySubTab.Consolidate);
      }
      catch (Exception ex)
      {
        FeedbackMasterOnly.PostValidationError(ControlMasterOnlyNewKey,
          "The office keys could not be consolidated: " + ex.Message);
      }
    }

    private void CreateLockedOfficeClassCheckBoxes()
    {
      ControlMasterOnlyClassesToLock.Items.Clear();
      var options = StateCode == "US"
        ? GetOfficeClassesOptions.IncludeUSPresident
        : GetOfficeClassesOptions.IncludeUSHouse |
        GetOfficeClassesOptions.IncludeState |
        GetOfficeClassesOptions.IncludeCounty |
        GetOfficeClassesOptions.IncludeLocal;

      foreach (var officeClass in
        Offices.GetOfficeClasses(options))
      {
        var listItem = new ListItem();
        ControlMasterOnlyClassesToLock.Items.Add(listItem);
        var allIdentified = OfficesAllIdentified.GetIsOfficesAllIdentified(
          StateCode, officeClass.ToInt(), CountyCode, LocalCode);
        listItem.Text = GetOfficeClassDescription(officeClass);
        listItem.Value = officeClass.ToInt()
          .ToString(CultureInfo.InvariantCulture);
        listItem.Selected = allIdentified;
      }
    }

    private string GetOfficeClassDescription(OfficeClass officeClass,
      bool shortDesc = false) => (shortDesc
      ? Offices.GetShortLocalizedOfficeClassDescription(officeClass, StateCode,
        CountyCode, LocalCode)
      : Offices.GetLocalizedOfficeClassDescription(officeClass, StateCode,
        CountyCode, LocalCode)) + " Offices";

    private void ResetMasterOnlySubTab(MasterOnlySubTab subTab)
    {
      foreach (var item in _MasterOnlyTabInfo) if (item.SubTab == subTab) item.ResetControl();
    }

    private void UpdateLockedClasses()
    {
      // Get all OfficesAllIdentified rows for the state and make a dictionary of
      // only the true values
      var dictionary = OfficesAllIdentified.GetDataByStateCode(StateCode)
        .Where(row => row.IsOfficesAllIdentified)
        .ToDictionary(row => row.OfficeLevel.ToOfficeClass(),
          row => null as object);

      // now get any items that don't match the dictionary
      var items = ControlMasterOnlyClassesToLock.Items.OfType<ListItem>()
        .Where(
          item =>
            dictionary.ContainsKey(Offices.GetValidatedOfficeClass(item.Value)) !=
            item.Selected);

      foreach (var item in items)
      {
        var officeClass = Offices.GetValidatedOfficeClass(item.Value);
        OfficesAllIdentified.UpdateIsOfficesAllIdentified(item.Selected, StateCode,
          officeClass.ToInt());
      }

      FeedbackMasterOnly.AddInfo("The locked office classes were updated");
    }

    #endregion Private

    #region Event handlers and overrides

    protected void ButtonMasterOnly_OnClick(object sender, EventArgs e)
    {
      switch (MasterOnlyReloading.Value)
      {
        case "reloading":
        {
          MasterOnlyReloading.Value = string.Empty;
          //      ControlMasterOnlyElectionDesc.Enabled = false;
          //      MasterOnlyDateWasChanged.Value = String.Empty;
          _MasterOnlyTabInfo.LoadControls();
          //      SetElectionHeading(HeadingMasterOnly);
          FeedbackMasterOnly.AddInfo("Master-only data loaded.");
        }
          break;

        case "":
        {
          // normal update
          int subTabIndex;
          _MasterOnlyTabInfo.ClearValidationErrors();
          if (int.TryParse(ContainerMasterOnlySubTabIndex.Value, out subTabIndex))
            switch (_MasterOnlySubTabs[subTabIndex])
            {
              case MasterOnlySubTab.LockClasses:
                UpdateLockedClasses();
                break;

              case MasterOnlySubTab.ChangeKey:
                ChangeOfficeKey();
                break;

              case MasterOnlySubTab.Consolidate:
                Consolidate();
                break;
            }
        }
          break;

        default:
          throw new VoteException("Unknown reloading option");
      }
    }

    #endregion Event handlers and overrides
  }
}