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

namespace Vote.Admin
{
  public partial class UpdateJurisdictionsPage
  {
    #region Private

    // ReSharper disable once ClassNeverInstantiated.Local
    private class CountySupervisor
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
          return string.Empty;
        return "error";
      }

      public void SetError(string fieldName)
      {
        if (_ErrorFields == null) _ErrorFields = new List<string>();
        if (!_ErrorFields.Contains(fieldName)) _ErrorFields.Add(fieldName);
      }
    }

    private void LoadCountySupervisorDistricts()
    {
      CountySupervisorsTableBody.Controls.Clear();
      var districts = CountySupervisors
        .GetCountySupervisorsDistrictsByCountyCode(StateCode, CountyCode).Rows
        .OfType<DataRow>().OrderBy(r => r.CountySupervisorsCode()).ToList();
      foreach (var d in districts)
      {
        var tr = new TableRow();
        tr.AddTo(CountySupervisorsTableBody);
        new TableCell().AddTo(tr, "delete").Controls.Add(new HtmlInputCheckBox
        {
          Disabled = d.LocalKey() != null
        });
        new TableCell().AddTo(tr, "exists").Controls.Add(new HtmlInputCheckBox
        {
          Disabled = true,
          Checked = d.LocalKey() != null
        });
        new TableCell().AddTo(tr, "create").Controls.Add(new HtmlInputCheckBox
        {
          Disabled = d.LocalKey() != null
        });
        new TableCell().AddTo(tr, "code").Controls.Add(new HtmlInputText
        {
          Value = d.CountySupervisorsCode(),
          Disabled = d.LocalKey() != null
        });
        new TableCell().AddTo(tr, "in-shapefile").Controls.Add(new HtmlInputCheckBox
        {
          Checked = d.IsInShapefile()
        });
        new TableCell().AddTo(tr, "name").Controls.Add(new HtmlInputText
        {
          Value = d.Name()
        });
      }
      BulkAddCountySupervisors.Disabled = districts.Count > 0;
    }

    private void LoadCountySupervisorDistricts(ICollection<CountySupervisor> supervisors)
    {
      CountySupervisorsTableBody.Controls.Clear();
      foreach (var s in supervisors)
      {
        var tr = new TableRow();
        tr.AddTo(CountySupervisorsTableBody, s.New ? "new" : null);
        new TableCell().AddTo(tr, $"delete {s.ErrorClass("Delete")}")
          .Controls.Add(new HtmlInputCheckBox
        {
          Disabled= s.Exists,
          Checked = s.Delete
        });
        new TableCell().AddTo(tr, $"exists  {s.ErrorClass("Exists")}").Controls.Add(new HtmlInputCheckBox
        {
          Disabled = true,
          Checked = s.Exists
        });
        new TableCell().AddTo(tr, $"create  {s.ErrorClass("Create")}").Controls.Add(new HtmlInputCheckBox
        {
          Disabled = s.Exists,
          Checked = s.Create
        });
        new TableCell().AddTo(tr, "code").Controls.Add(new HtmlInputText
        {
          Value = s.Id,
          Disabled = s.Exists
        }.AddCssClasses(s.ErrorClass("Id")));
        new TableCell().AddTo(tr, $"in-shapefile  {s.ErrorClass("IsInShapefile")}").Controls.Add(new HtmlInputCheckBox
        {
          Checked = s.InShapefile
        });
        new TableCell().AddTo(tr, "name").Controls.Add(new HtmlInputText
        {
          Value = s.Name
        }.AddCssClasses(s.ErrorClass("Name")));
      }
      BulkAddCountySupervisors.Disabled = supervisors.Count > 0;
    }

    #region DataItem object

    [PageInitializer]
    // ReSharper disable once ClassNeverInstantiated.Local
    // ReSharper disable once UnusedMember.Local
    private class SetupCountySupervisorsTabItem : JurisdictionsDataItem
    {
      // This class assumes SupervisorsList is the only column
      private const string GroupName = "SetupCountySupervisors";

      private CountySupervisor[] _Submitted;

      public void PrepareForUpdate()
      {
        _Submitted = new JavaScriptSerializer()
          .Deserialize<CountySupervisor[]>(Page.ControlSetupCountySupervisorsSupervisorsListValue.Value);
      }

      private static bool ValidateCountySupervisors(DataItemBase item)
      {
        var thisItem = item as SetupCountySupervisorsTabItem;
        Debug.Assert(thisItem != null, "thisItem != null");
        var submitted = thisItem._Submitted;
        var feedback = thisItem.Feedback;
        var control = item.DataControl;
        var stateCode = thisItem.Page.StateCode;
        var countyCode = thisItem.Page.CountyCode;

        // pre-trim names and ids
        foreach (var s in submitted)
        {
          s.Id = s.Id.Trim();
          s.Name = s.Name.Trim();
        }

        // ids must exist
        var missingIds = submitted.Where(s => s.Id == string.Empty).ToList();
        if (missingIds.Count > 0)
        {
          feedback.PostValidationError(control, "Every entry must have a Shapefile Id.");
          foreach (var e in missingIds) e.SetError("Id");
        }

        // ids must be 5 characters, all numeric
        var invalidIds = submitted
          .Where(s => s.Id != string.Empty && s.Id.Length != 5 || !s.Id.IsDigits())
          .ToList();
        if (invalidIds.Count > 0)
        {
          feedback.PostValidationError(control,
            "Invalid Shapefile Id(s). Ids must be 5 numeric characters.");
          foreach (var e in missingIds) e.SetError("Id");
        }

        // ids must be unique
        var duplicateIds = submitted.GroupBy(s => s.Id).Where(g => g.Count() > 1).ToList();
        if (duplicateIds.Count > 0)
        {
          feedback.PostValidationError(control, "Shapefile Ids must be unique.");
          foreach (var g in duplicateIds) foreach (var e in g) e.SetError("Id");
        }

        // except for DC, ids must begin with the county code
        if (stateCode != "DC")
        {
          var missingCounties = submitted.Where(s => !s.Id.StartsWith(countyCode)).ToList();
          if (missingCounties.Count > 0)
          {
            feedback.PostValidationError(control,
              "Shapefile Ids must begin with the CountyCode (except DC).");
            foreach (var e in missingCounties) e.SetError("Id");
          }
        }

        // all items except deletes must have a name
        var missingNames = submitted.Where(s => !s.Delete && s.Name == string.Empty)
          .ToList();
        if (missingNames.Count > 0)
        {
          feedback.PostValidationError(control, "Missing District Name(s).");
          foreach (var e in missingNames) e.SetError("Name");
        }

        if (feedback.ValidationErrorCount == 0) return true;

        // there were errors
        thisItem.Page.LoadCountySupervisorDistricts(submitted);
        return false;
      }

      private SetupCountySupervisorsTabItem(UpdateJurisdictionsPage page)
        : base(page, GroupName)
      {
      }

      private static SetupCountySupervisorsTabItem[] GetTabInfo(
        UpdateJurisdictionsPage page)
      {
        var setupCountySupervisorsTabInfo = new []
        {
          new SetupCountySupervisorsTabItem(page)
          {
            Column = "SupervisorsList",
            Description = "County Supervisors",
            Validator = ValidateCountySupervisors
          }
        };

        foreach (var item in setupCountySupervisorsTabInfo)
          item.InitializeItem(page);

        InitializeGroup(page, GroupName);

        return setupCountySupervisorsTabInfo;
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
        page._SetupCountySupervisorsTabInfo = GetTabInfo(page);
        if (page.AdminPageLevel != AdminPageLevel.County)
        {
          page.TabSetupCountySupervisorsItem.Visible = false;
        }
      }

      public override void LoadControl()
      {
        Page.LoadCountySupervisorDistricts();
      }

      protected override bool Update(object newValue)
      {
        // everything has been validated already
        var changed = false;
        var stateCode = Page.StateCode;
        var countyCode = Page.CountyCode;

        foreach (var s in _Submitted)
        {
          if (s.New)
          {
            CountySupervisors.Insert(stateCode, s.Id, s.Name, countyCode, s.InShapefile);
            if (s.Create)
            {
              // also create local district
              var localKey = LocalDistricts.GetAvailableLocalKey(stateCode);
              LocalDistricts.Insert(stateCode, localKey, s.Name, string.Empty, string.Empty,
                string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                string.Empty, false);
              LocalIdsCodes.Insert(stateCode, LocalIdsCodes.LocalTypeCountySupervisors,
                s.Id, localKey);
              changed = true;
            }
          }
          else if (s.Delete)
          {
            // can only delete if no local district entry
            changed |=
              CountySupervisors.DeleteByStateCodeCountySupervisorsCode(stateCode, s.Id) !=
              0;
          }
          else
          {
            // existing item, but may update IsInShapefile or Name
            changed |=
              CountySupervisors.UpdateNameByStateCodeCountySupervisorsCode(s.Name,
                stateCode, s.Id) != 0;
            changed |=
              CountySupervisors.UpdateIsInShapefileByStateCodeCountySupervisorsCode(
                s.InShapefile, stateCode, s.Id) != 0;
          }
        }

        LoadControl();
        return changed;
      }
    }

    private SetupCountySupervisorsTabItem[] _SetupCountySupervisorsTabInfo;

    #endregion DataItem object

    #endregion Private

    #region Event handlers and overrides

    protected void ButtonSetupCountySupervisors_OnClick(object sender, EventArgs e)
    {
      switch (SetupCountySupervisorsReloading.Value)
      {
        case "reloading":
        {
          SetupCountySupervisorsReloading.Value = string.Empty;
          _SetupCountySupervisorsTabInfo.LoadControls();
          FeedbackSetupCountySupervisors.AddInfo("County Supervisors loaded.");
        }
          break;

        case "":
        {
          if (_SetupCountySupervisorsTabInfo.Length == 1)
            _SetupCountySupervisorsTabInfo[0].PrepareForUpdate();
          _SetupCountySupervisorsTabInfo.Update(FeedbackSetupCountySupervisors);
        }
          break;

        default:
          throw new VoteException($"Unknown reloading option: '{SetupCountySupervisorsReloading.Value}'");
      }
    }

    #endregion Event handlers and overrides
  }
}