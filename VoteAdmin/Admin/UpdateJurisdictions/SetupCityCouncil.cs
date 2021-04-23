using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web.Script.Serialization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB;
using DB.Vote;
using static System.String;

namespace Vote.Admin
{
  public partial class UpdateJurisdictionsPage
  {
    #region Private

    // ReSharper disable once ClassNeverInstantiated.Local
    private class CityCouncilItem
    {
#pragma warning disable 649
      public bool New;
      public bool Delete;
      public bool Exists;
      public bool Create;
      public string Id;
      public bool InShapefile;
#pragma warning restore 649
      public string District;
      public string Name;
      private List<string> _ErrorFields;

      public string ErrorClass(string fieldName)
      {
        if (_ErrorFields == null || !_ErrorFields.Contains(fieldName))
          return Empty;
        return "error";
      }

      public void SetError(string fieldName)
      {
        if (_ErrorFields == null) _ErrorFields = new List<string>();
        if (!_ErrorFields.Contains(fieldName)) _ErrorFields.Add(fieldName);
      }
    }

    private void LoadCityCouncilDistricts()
    {
      CityCouncilTableBody.Controls.Clear();
      var districts = CityCouncil
        .GetCityCouncilDistrictsByTigerCode(StateCode, TigerPlaceCode).Rows
        .OfType<DataRow>().OrderBy(r => r.CityCouncilCode()).ToList();
      foreach (var d in districts)
      {
        var tr = new TableRow();
        tr.AddTo(CityCouncilTableBody);
        new TableCell().AddTo(tr, "delete").Controls
          .Add(new HtmlInputCheckBox {Disabled = d.LocalKey() != null});
        new TableCell().AddTo(tr, "exists").Controls.Add(
          new HtmlInputCheckBox {Disabled = true, Checked = d.LocalKey() != null});
        new TableCell().AddTo(tr, "create").Controls
          .Add(new HtmlInputCheckBox {Disabled = d.LocalKey() != null});
        new TableCell().AddTo(tr, "code").Controls.Add(new HtmlInputText
        {
          Value = d.CityCouncilCode(),
          Disabled = d.LocalKey() != null
        });
        new TableCell().AddTo(tr, "in-shapefile").Controls
          .Add(new HtmlInputCheckBox {Checked = d.IsInShapefile()});
        new TableCell().AddTo(tr, "district").Controls
          .Add(new HtmlInputText {Value = d.District()});
        new TableCell().AddTo(tr, "name").Controls
          .Add(new HtmlInputText {Value = d.Name()});
      }
      SetCouncilPrefix2(districts.Count > 0 ? districts[0].CityCouncilCode() : null);
      BulkAddCityCouncil.Disabled = districts.Count > 0;
    }

    private void LoadCityCouncilDistricts(ICollection<CityCouncilItem> items)
    {
      CityCouncilTableBody.Controls.Clear();
      foreach (var s in items)
      {
        var tr = new TableRow();
        tr.AddTo(CityCouncilTableBody, s.New ? "new" : null);
        new TableCell().AddTo(tr, $"delete {s.ErrorClass(nameof(CityCouncilItem.Delete))}")
          .Controls.Add(new HtmlInputCheckBox {Disabled = s.Exists, Checked = s.Delete});
        new TableCell().AddTo(tr, $"exists  {s.ErrorClass(nameof(CityCouncilItem.Exists))}")
          .Controls.Add(new HtmlInputCheckBox {Disabled = true, Checked = s.Exists});
        new TableCell().AddTo(tr, $"create  {nameof(CityCouncilItem.Create)}").Controls
          .Add(new HtmlInputCheckBox {Disabled = s.Exists, Checked = s.Create});
        new TableCell().AddTo(tr, "code").Controls.Add(
          new HtmlInputText {Value = s.Id, Disabled = s.Exists}.AddCssClasses(
            s.ErrorClass(nameof(CityCouncilItem.Id))));
        new TableCell()
          .AddTo(tr, $"in-shapefile  {s.ErrorClass(nameof(CityCouncilItem.InShapefile))}")
          .Controls.Add(new HtmlInputCheckBox {Checked = s.InShapefile});
        new TableCell().AddTo(tr, "district").Controls.Add(
          new HtmlInputText {Value = s.District}.AddCssClasses(
            s.ErrorClass(nameof(CityCouncilItem.District))));
        new TableCell().AddTo(tr, "name").Controls.Add(
          new HtmlInputText {Value = s.Name}.AddCssClasses(
            s.ErrorClass(nameof(CityCouncilItem.Name))));
      }
      BulkAddCityCouncil.Disabled = items.Count > 0;
    }

    private void SetCouncilPrefix(string cityCouncilCode)
    {
      var prefix = Empty;
      if (IsNullOrWhiteSpace(cityCouncilCode))
      {
        var prefixes = CityCouncil.GetPrefixes(StateCode);
        if (prefixes.Count >= 100)
          throw new Exception("All City Council prefixes for this state are in use");
        if (prefixes.Count == 0) prefix = "10";
        else
        {
          var last = prefixes.Last();
          if (last == "99")
          {
            // use highest available
            for (var p = 98; p >= 0; p--)
            {
              prefix = p.ToString("D2");
              if (!prefixes.Contains(prefix)) break;
            }
          }
          else prefix = (int.Parse(last) + 1).ToString("D2");
        }
      }
      else
      {
        prefix = cityCouncilCode.Substring(0, 2);
      }
      CityCouncilPrefix.Text = prefix;
      PlaceName.Value = TigerPlaceName;
    }

    private static string GetNextCouncilPrefix(string prefix)
    {
      Debug.Assert(prefix != null && prefix.Length == 2);
      Debug.Assert(char.IsDigit(prefix[0]) || char.IsUpper(prefix[0]));
      Debug.Assert(char.IsDigit(prefix[1]));
      var array = prefix.ToCharArray();
      if (array[1] != '9') array[1]++;
      else if (array[0] != 'Z')
      {
        if (array[0] >= 'A')
        {
          array[0]++;
        }
        else if (array[0] == '9')
        {
          array[0] = 'A';
        }
        else array[0]++;
        array[1] = '0';
      }
      else return "00";
      return new string(array);
    }

    private void SetCouncilPrefix2(string cityCouncilCode)
    {
      string prefix;
      if (IsNullOrWhiteSpace(cityCouncilCode))
      {
        var prefixes = CityCouncil.GetPrefixes(StateCode);
        if (prefixes.Count >= 360)
          throw new Exception("All City Council prefixes for this state are in use");
        if (prefixes.Count == 0) prefix = "10";
        else
        {
          prefix = GetNextCouncilPrefix(prefixes.Last());
          while (prefixes.Contains(prefix))
            prefix = GetNextCouncilPrefix(prefix);
        }
      }
      else
      {
        prefix = cityCouncilCode.Substring(0, 2);
      }
      CityCouncilPrefix.Text = prefix;
      PlaceName.Value = TigerPlaceName;
    }


    #region DataItem object

    [PageInitializer]
    // ReSharper disable once ClassNeverInstantiated.Local
    // ReSharper disable once UnusedMember.Local
    private class SetupCityCouncilTabItem : JurisdictionsDataItem
    {
      // This class assumes CouncilList is the only column
      private const string GroupName = "SetupCityCouncil";

      private CityCouncilItem[] _Submitted;

      public void PrepareForUpdate()
      {
        _Submitted =
          new JavaScriptSerializer().Deserialize<CityCouncilItem[]>(
            Page.ControlSetupCityCouncilCouncilListValue.Value);
      }

      private static bool ValidateCityCouncil(DataItemBase item)
      {
        var thisItem = item as SetupCityCouncilTabItem;
        Debug.Assert(thisItem != null, "thisItem != null");
        var submitted = thisItem._Submitted;
        var feedback = thisItem.Feedback;
        var control = item.DataControl;

        // pre-trim districts, names and ids
        foreach (var s in submitted)
        {
          s.Id = s.Id.Trim();
          s.District = s.District.Trim();
          s.Name = s.Name.Trim();
        }

        // ids must exist
        var missingIds = submitted.Where(s => s.Id == Empty).ToList();
        if (missingIds.Count > 0)
        {
          feedback.PostValidationError(control, "Every entry must have a Shapefile Id.");
          foreach (var e in missingIds) e.SetError(nameof(CityCouncilItem.Id));
        }

        // ids must be 5 characters, all numeric
        var invalidIds = submitted
          .Where(s => s.Id != Empty && (s.Id.Length != 5 || !char.IsUpper(s.Id[0]) && !char.IsDigit(s.Id[0]) || !s.Id.Substring(1, 4).IsDigits())).ToList();
        if (invalidIds.Count > 0)
        {
          feedback.PostValidationError(control,
            //"Invalid Shapefile Id(s). Ids must be 5 numeric characters.");
            "Invalid Shapefile Id(s). Ids must be 5 alphanumeric characters, the last 4 numeric.");
          foreach (var e in missingIds) e.SetError(nameof(CityCouncilItem.Id));
        }

        // ids must be unique
        var duplicateIds = submitted.GroupBy(s => s.Id).Where(g => g.Count() > 1).ToList();
        if (duplicateIds.Count > 0)
        {
          feedback.PostValidationError(control, "Shapefile Ids must be unique.");
          foreach (var g in duplicateIds)
          foreach (var e in g) e.SetError(nameof(CityCouncilItem.Id));
        }

        // ids must begin with the designated prefix
        var missingPrefixes = submitted
          .Where(s => !s.Id.StartsWith(thisItem.Page.CityCouncilPrefix.Text)).ToList();
        if (missingPrefixes.Count > 0)
        {
          feedback.PostValidationError(control,
            "Shapefile Ids must begin with the City Council Prefix.");
          foreach (var e in missingPrefixes) e.SetError(nameof(CityCouncilItem.Id));
        }

        // all items except deletes must have a name
        var missingNames = submitted.Where(s => !s.Delete && s.Name == Empty).ToList();
        if (missingNames.Count > 0)
        {
          feedback.PostValidationError(control, "Missing District Name(s).");
          foreach (var e in missingNames) e.SetError(nameof(CityCouncilItem.Name));
        }

        // all items except deletes must have a district
        var missingDistricts =
          submitted.Where(s => !s.Delete && s.District == Empty).ToList();
        if (missingDistricts.Count > 0)
        {
          feedback.PostValidationError(control, "Missing District(s).");
          foreach (var e in missingDistricts) e.SetError(nameof(CityCouncilItem.District));
        }

        if (feedback.ValidationErrorCount == 0) return true;

        // there were errors
        thisItem.Page.LoadCityCouncilDistricts(submitted);
        return false;
      }

      private SetupCityCouncilTabItem(UpdateJurisdictionsPage page) : base(page, GroupName)
      {
      }

      private static SetupCityCouncilTabItem[] GetTabInfo(UpdateJurisdictionsPage page)
      {
        var setupCityCouncilTabInfo = new[]
        {
          new SetupCityCouncilTabItem(page)
          {
            Column = "CouncilList",
            Description = "City Council",
            Validator = ValidateCityCouncil
          }
        };

        foreach (var item in setupCityCouncilTabInfo)
          item.InitializeItem(page);

        InitializeGroup(page, GroupName);

        return setupCityCouncilTabInfo;
      }

      protected override string GetCurrentValue()
      {
        return null;
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(UpdateJurisdictionsPage page)
        // ReSharper restore UnusedMember.Local
      {
        page._SetupCityCouncilTabInfo = GetTabInfo(page);
        if (!page.IsTigerPlace)
        {
          page.TabSetupCityCouncilItem.Visible = false;
        }
      }

      public override void LoadControl()
      {
        Page.LoadCityCouncilDistricts();
      }

      protected override bool Update(object newValue)
      {
        // everything has been validated already
        var changed = false;
        var stateCode = Page.StateCode;

        foreach (var s in _Submitted)
        {
          if (s.New)
          {
            CityCouncil.Insert(stateCode, s.Id, s.District, s.Name, Page.TigerPlaceCode,
              s.InShapefile);
            if (s.Create)
            {
              // also create local district
              var localKey = LocalDistricts.GetAvailableLocalKey(stateCode);
              LocalDistricts.Insert(stateCode, localKey, s.Name, Empty, Empty, Empty, Empty,
                Empty, Empty, Empty, Empty, Empty, Empty, Empty, Empty, Empty, Empty, Empty,
                Empty, Empty, Empty, false, false);
              LocalIdsCodes.Insert(stateCode, LocalIdsCodes.LocalTypeCityCouncil, s.Id,
                localKey);
              changed = true;
            }
          }
          else if (s.Delete)
          {
            // can only delete if no local district entry
            changed |= CityCouncil.DeleteByStateCodeCityCouncilCode(stateCode, s.Id) != 0;
          }
          else
          {
            // existing item, but may update IsInShapefile, District or Name
            changed |=
              CityCouncil.UpdateDistrictByStateCodeCityCouncilCode(s.District, stateCode,
                s.Id) != 0;
            changed |=
              CityCouncil.UpdateNameByStateCodeCityCouncilCode(s.Name, stateCode, s.Id) !=
              0;
            changed |=
              CityCouncil.UpdateIsInShapefileByStateCodeCityCouncilCode(s.InShapefile,
                stateCode, s.Id) != 0;
          }
        }

        LoadControl();
        return changed;
      }
    }

    private SetupCityCouncilTabItem[] _SetupCityCouncilTabInfo;

    #endregion DataItem object

    #endregion Private

    #region Event handlers and overrides

    protected void ButtonSetupCityCouncil_OnClick(object sender, EventArgs e)
    {
      switch (SetupCityCouncilReloading.Value)
      {
        case "reloading":
        {
          SetupCityCouncilReloading.Value = Empty;
          _SetupCityCouncilTabInfo.LoadControls();
          FeedbackSetupCityCouncil.AddInfo("City Council loaded.");
        }
          break;

        case "":
        {
          if (_SetupCityCouncilTabInfo.Length == 1)
            _SetupCityCouncilTabInfo[0].PrepareForUpdate();
          _SetupCityCouncilTabInfo.Update(FeedbackSetupCityCouncil);
          NavigateJurisdictionUpdatePanel.Update();
          NavigateJurisdiction.Initialize();
        }
          break;

        default:
          throw new VoteException(
            $"Unknown reloading option: '{SetupCityCouncilReloading.Value}'");
      }
    }

    #endregion Event handlers and overrides
  }
}