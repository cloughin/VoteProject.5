using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
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
    private class SchoolDistrictItem
    {
#pragma warning disable 649
      public bool New;
      public bool Delete;
      public bool Exists;
      public bool Create;
      public string Id;
      public bool InShapefile;
#pragma warning restore 649
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

    private void LoadSchoolSubDistricts()
    {
      //  SchoolDistrictTableBody.Controls.Clear();
      var districts = SchoolDistrictDistricts
        .GetSchoolDistrictDistrictsByTigerTypeTigerCode(StateCode, TigerSchoolType,
          TigerSchoolCode).Rows.OfType<DataRow>()
        .OrderBy(r => r.SchoolDistrictDistrictCode()).ToList();
      foreach (var d in districts)
      {
        var tr = new TableRow();
        tr.AddTo(SchoolDistrictTableBody);
        new TableCell().AddTo(tr, "delete").Controls
          .Add(new HtmlInputCheckBox {Disabled = d.LocalKey() != null});
        new TableCell().AddTo(tr, "exists").Controls.Add(
          new HtmlInputCheckBox {Disabled = true, Checked = d.LocalKey() != null});
        new TableCell().AddTo(tr, "create").Controls
          .Add(new HtmlInputCheckBox {Disabled = d.LocalKey() != null});
        new TableCell().AddTo(tr, "code").Controls.Add(new HtmlInputText
        {
          Value = d.SchoolDistrictDistrictCode(),
          Disabled = d.LocalKey() != null
        });
        new TableCell().AddTo(tr, "in-shapefile").Controls
          .Add(new HtmlInputCheckBox {Checked = d.IsInShapefile()});
        new TableCell().AddTo(tr, "name").Controls
          .Add(new HtmlInputText {Value = d.Name()});
      }
      SetSubDistrictPrefix(districts.Count > 0
        ? districts[0].SchoolDistrictDistrictCode()
        : null);
      BulkAddSchoolDistrict.Disabled = districts.Count > 0;
    }

    private void LoadCountySupervisorDistricts(ICollection<SchoolDistrictItem> items)
    {
      SchoolDistrictTableBody.Controls.Clear();
      foreach (var s in items)
      {
        var tr = new TableRow();
        tr.AddTo(SchoolDistrictTableBody, s.New ? "new" : null);
        new TableCell()
          .AddTo(tr, $"delete {s.ErrorClass(nameof(SchoolDistrictItem.Delete))}").Controls
          .Add(new HtmlInputCheckBox {Disabled = s.Exists, Checked = s.Delete});
        new TableCell()
          .AddTo(tr, $"exists  {s.ErrorClass(nameof(SchoolDistrictItem.Exists))}").Controls
          .Add(new HtmlInputCheckBox {Disabled = true, Checked = s.Exists});
        new TableCell()
          .AddTo(tr, $"create  {s.ErrorClass(nameof(SchoolDistrictItem.Create))}").Controls
          .Add(new HtmlInputCheckBox {Disabled = s.Exists, Checked = s.Create});
        new TableCell().AddTo(tr, "code").Controls.Add(
          new HtmlInputText {Value = s.Id, Disabled = s.Exists}.AddCssClasses(
            s.ErrorClass(nameof(SchoolDistrictItem.Id))));
        new TableCell()
          .AddTo(tr,
            $"in-shapefile  {s.ErrorClass(nameof(SchoolDistrictItem.InShapefile))}")
          .Controls.Add(new HtmlInputCheckBox {Checked = s.InShapefile});
        new TableCell().AddTo(tr, "name").Controls.Add(
          new HtmlInputText {Value = s.Name}.AddCssClasses(
            s.ErrorClass(nameof(SchoolDistrictItem.Name))));
      }
      BulkAddSchoolDistrict.Disabled = items.Count > 0;
    }

    private void SetSubDistrictPrefix(string schoolDistrictDistrictCode)
    {
      var prefix = Empty;
      if (IsNullOrWhiteSpace(schoolDistrictDistrictCode))
      {
        var prefixes = SchoolDistrictDistricts.GetPrefixes(StateCode);
        if (prefixes.Count > 1000)
          throw new Exception(
            "All School Sub-District prefixes for this state are in use");
        if (prefixes.Count == 0) prefix = "100";
        else
        {
          var last = prefixes.Last();
          if (last == "999")
          {
            // use highest available
            for (var p = 998; p >= 0; p--)
            {
              prefix = p.ToString("D3");
              if (!prefixes.Contains(prefix)) break;
            }
          }
          else prefix = (int.Parse(last) + 1).ToString("D3");
        }
      }
      else
      {
        prefix = schoolDistrictDistrictCode.Substring(0, 3);
      }
      SchoolDistrictPrefix.Text = prefix;
      SchoolName.Value = TigerSchoolName;
    }

    #region DataItem object

    [PageInitializer]
    // ReSharper disable once ClassNeverInstantiated.Local
    // ReSharper disable once UnusedMember.Local
    private class SetupSchoolDistrictTabItem : JurisdictionsDataItem
    {
      // This class assumes SubDistrictList is the only column
      private const string GroupName = "SetupSchoolDistrict";

      private SchoolDistrictItem[] _Submitted;

      public void PrepareForUpdate()
      {
        _Submitted =
          new JavaScriptSerializer().Deserialize<SchoolDistrictItem[]>(
            Page.ControlSetupSchoolDistrictSubDistrictListValue.Value);
      }

      private static bool ValidateSchoolDistrict(DataItemBase item)
      {
        var thisItem = item as SetupSchoolDistrictTabItem;
        Debug.Assert(thisItem != null, "thisItem != null");
        var submitted = thisItem._Submitted;
        var feedback = thisItem.Feedback;
        var control = item.DataControl;

        // pre-trim names and ids
        foreach (var s in submitted)
        {
          s.Id = s.Id.Trim();
          s.Name = s.Name.Trim();
        }

        // ids must exist
        var missingIds = submitted.Where(s => s.Id == Empty).ToList();
        if (missingIds.Count > 0)
        {
          feedback.PostValidationError(control, "Every entry must have a Shapefile Id.");
          foreach (var e in missingIds) e.SetError(nameof(SchoolDistrictItem.Id));
        }

        // ids must be 5 characters, all numeric
        var invalidIds = submitted
          .Where(s => s.Id != Empty && s.Id.Length != 5 || !s.Id.IsDigits()).ToList();
        if (invalidIds.Count > 0)
        {
          feedback.PostValidationError(control,
            "Invalid Shapefile Id(s). Ids must be 5 numeric characters.");
          foreach (var e in missingIds) e.SetError(nameof(SchoolDistrictItem.Id));
        }

        // ids must be unique
        var duplicateIds = submitted.GroupBy(s => s.Id).Where(g => g.Count() > 1).ToList();
        if (duplicateIds.Count > 0)
        {
          feedback.PostValidationError(control, "Shapefile Ids must be unique.");
          foreach (var g in duplicateIds)
          foreach (var e in g) e.SetError(nameof(SchoolDistrictItem.Id));
        }

        // ids must begin with the designated prefix
        var missingPrefixes = submitted
          .Where(s => !s.Id.StartsWith(thisItem.Page.SchoolDistrictPrefix.Text)).ToList();
        if (missingPrefixes.Count > 0)
        {
          feedback.PostValidationError(control,
            "Shapefile Ids must begin with the School District Prefix.");
          foreach (var e in missingPrefixes) e.SetError(nameof(SchoolDistrictItem.Id));
        }

        // all items except deletes must have a name
        var missingNames = submitted.Where(s => !s.Delete && s.Name == Empty).ToList();
        if (missingNames.Count > 0)
        {
          feedback.PostValidationError(control, "Missing District Name(s).");
          foreach (var e in missingNames) e.SetError(nameof(SchoolDistrictItem.Name));
        }

        if (feedback.ValidationErrorCount == 0) return true;

        // there were errors
        thisItem.Page.LoadCountySupervisorDistricts(submitted);
        return false;
      }

      private SetupSchoolDistrictTabItem(UpdateJurisdictionsPage page) : base(page,
        GroupName)
      {
      }

      private static SetupSchoolDistrictTabItem[] GetTabInfo(UpdateJurisdictionsPage page)
      {
        var setupSchoolDistrictTabInfo = new[]
        {
          new SetupSchoolDistrictTabItem(page)
          {
            Column = "SubDistrictList",
            Description = "School Sub-Districts",
            Validator = ValidateSchoolDistrict
          }
        };

        foreach (var item in setupSchoolDistrictTabInfo)
          item.InitializeItem(page);

        InitializeGroup(page, GroupName);

        return setupSchoolDistrictTabInfo;
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
        page._SetupSchoolDistrictTabInfo = GetTabInfo(page);
        if (!page.IsTigerSchool)
        {
          page.TabSetupSchoolDistrictItem.Visible = false;
        }
      }

      public override void LoadControl()
      {
        Page.LoadSchoolSubDistricts();
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
            SchoolDistrictDistricts.Insert(stateCode, s.Id, Page.TigerSchoolType,
              Page.TigerSchoolCode, s.Name, s.InShapefile);
            if (s.Create)
            {
              // also create local district
              var localKey = LocalDistricts.GetAvailableLocalKey(stateCode);
              LocalDistricts.Insert(stateCode, localKey, s.Name, Empty, Empty, Empty, Empty,
                Empty, Empty, Empty, Empty, Empty, Empty, Empty, Empty, Empty, Empty, Empty,
                Empty, Empty, Empty, false, false);
              LocalIdsCodes.Insert(stateCode, LocalIdsCodes.LocalTypeSchoolDistrictDistrict,
                s.Id, localKey);
              changed = true;
            }
          }
          else if (s.Delete)
          {
            // can only delete if no local district entry
            changed |=
              SchoolDistrictDistricts.DeleteByStateCodeSchoolDistrictDistrictCode(stateCode,
                s.Id) != 0;
          }
          else
          {
            // existing item, but may update IsInShapefile or Name
            changed |=
              SchoolDistrictDistricts.UpdateNameByStateCodeSchoolDistrictDistrictCode(
                s.Name, stateCode, s.Id) != 0;
            changed |=
              SchoolDistrictDistricts
                .UpdateIsInShapefileByStateCodeSchoolDistrictDistrictCode(s.InShapefile,
                  stateCode, s.Id) != 0;
          }
        }

        LoadControl();
        return changed;
      }
    }

    private SetupSchoolDistrictTabItem[] _SetupSchoolDistrictTabInfo;

    #endregion DataItem object

    #endregion Private

    #region Event handlers and overrides

    protected void ButtonSetupSchoolDistrict_OnClick(object sender, EventArgs e)
    {
      switch (SetupSchoolDistrictReloading.Value)
      {
        case "reloading":
        {
          SetupSchoolDistrictReloading.Value = Empty;
          _SetupSchoolDistrictTabInfo.LoadControls();
          FeedbackSetupSchoolDistrict.AddInfo("School District loaded.");
        }
          break;

        case "":
        {
          if (_SetupSchoolDistrictTabInfo.Length == 1)
            _SetupSchoolDistrictTabInfo[0].PrepareForUpdate();
          _SetupSchoolDistrictTabInfo.Update(FeedbackSetupSchoolDistrict);
          NavigateJurisdictionUpdatePanel.Update();
          NavigateJurisdiction.Initialize();
        }
          break;

        default:
          throw new VoteException(
            $"Unknown reloading option: '{SetupSchoolDistrictReloading.Value}'");
      }
    }

    #endregion Event handlers and overrides
  }
}