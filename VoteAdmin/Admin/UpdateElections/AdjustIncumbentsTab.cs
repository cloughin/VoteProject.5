using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB;
using DB.Vote;
using static System.String;

namespace Vote.Admin
{
  public partial class UpdateElectionsPage
  {
    #region Private

    #region DataItem object

    [PageInitializer]
    private class AdjustIncumbentsTabItem : ElectionsDataItem
    {
      private const string GroupName = "AdjustIncumbents";
      public static int OfficeCount { get; protected set; }

      protected AdjustIncumbentsTabItem(UpdateElectionsPage page) : base(page, GroupName)
      {
      }

      private static AdjustIncumbentsTabItem[] GetTabInfo(UpdateElectionsPage page)
      {
        var adjustIncumbentsTabInfo = new AdjustIncumbentsTabItem[]
        {
          new ReinstateIncumbentsListTabItem(page)
          {
            Column = "ReinstatementList",
            Description = "Reinstate Incumbents List"
          },
          new AdjustIncumbentsListTabItem(page)
          {
            Column = "List",
            Description = "Incumbents List"
          }
        };

        foreach (var item in adjustIncumbentsTabInfo) item.InitializeItem(page);

        InitializeGroup(page, GroupName);

        return adjustIncumbentsTabInfo;
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(UpdateElectionsPage page)
        // ReSharper restore UnusedMember.Local
      {
        page._AdjustIncumbentsTabInfo = GetTabInfo(page);
        page.ShowAdjustIncumbents = StateCache.IsValidStateCode(page.StateCode);
        if (!page.ShowAdjustIncumbents) page.TabAdjustIncumbentsItem.Visible = false;
      }
    }

    private class AdjustIncumbentsListTabItem : AdjustIncumbentsTabItem
    {
      internal AdjustIncumbentsListTabItem(UpdateElectionsPage page) : base(page)
      {
      }

      protected override string GetCurrentValue() => null;

      public override void LoadControl()
      {
        var incumbentsToEliminate =
          ElectionsOffices.GetOfficesWithCandidatesToEliminate(Page.GetElectionKey());
        OfficeCount = incumbentsToEliminate.Count;
        Page.ControlAdjustIncumbentsListValue.Value = Empty;
        if (OfficeCount == 0)
        {
          Page.AdjustIncumbentsMessage.RemoveCssClass("hidden");
          Page.AdjustIncumbentsControl.AddCssClasses("hidden");
          Page.AdjustIncumbentsMessage.InnerHtml =
            "No offices that require incumbents to be removed were found for this election";
        }
        else
        {
          Page.AdjustIncumbentsMessage.AddCssClasses("hidden");
          Page.AdjustIncumbentsControl.RemoveCssClass("hidden");
          PopulateIncumbentsToAdjustList(incumbentsToEliminate,
            Page.PlaceHolderAdjustIncumbentsList);
        }
      }

      protected override void Log(string oldValue, string newValue)
      {
        // Logging done in Update;
      }

      protected override bool Update(object newValue)
      {
        var changed = false;
        //Parse the value from the UI tree
        if (!(newValue is string valueStr)) return false;
        var offices = valueStr.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries)
          .Select(officeStr =>
          {
            var officeSplit = officeStr.Split(':');
            return new {OfficeKey = officeSplit[0], Incumbents = officeSplit[1].Split(',')};
          }).ToArray();

        foreach (var office in offices)
        {
          var table = OfficesOfficials.GetDataByOfficeKey(office.OfficeKey);
          var officeChanged = false;
          foreach (var row in table)
            if (!office.Incumbents.Contains(row.PoliticianKey))
            {
              if (!ElectionsIncumbentsRemoved.ElectionKeyOfficeKeyPoliticianKeyExists(
                Page.GetElectionKey(), row.OfficeKey, row.PoliticianKey))
                ElectionsIncumbentsRemoved.Insert(Page.GetElectionKey(), row.OfficeKey,
                  row.PoliticianKey, row.RunningMateKey, row.StateCode,
                  Offices.GetCountyCodeFromKey(row.OfficeKey),
                  Offices.GetLocalKeyFromKey(row.OfficeKey), row.DistrictCode,
                  row.DataLastUpdated, row.UserSecurity, row.UserName);
              row.Delete();
              officeChanged = true;
            }
          if (!officeChanged) continue;
          OfficesOfficials.UpdateTable(table);
          changed = true;
        }

        //LoadControl();
        return changed;
      }
    }

    private class ReinstateIncumbentsListTabItem : AdjustIncumbentsTabItem
    {
      internal ReinstateIncumbentsListTabItem(UpdateElectionsPage page) : base(page)
      {
      }

      protected override string GetCurrentValue() => null;

      public override void LoadControl()
      {
        var incumbentsToReinstate =
          ElectionsIncumbentsRemoved.GetOfficesWithCandidatesToReinstate(
            Page.GetElectionKey());
        Page.ControlAdjustIncumbentsReinstatementListValue.Value = Empty;
        if (incumbentsToReinstate.Count == 0)
        {
          Page.ReinstateIncumbentsMessage.AddCssClasses("hidden");
          Page.ReinstateIncumbentsControl.AddCssClasses("hidden");
        }
        else
        {
          Page.ReinstateIncumbentsMessage.RemoveCssClass("hidden");
          Page.ReinstateIncumbentsControl.RemoveCssClass("hidden");
          PopulateIncumbentsToReinstateList(incumbentsToReinstate,
            Page.PlaceHolderReinstateIncumbentsList);
        }
      }

      protected override void Log(string oldValue, string newValue)
      {
        // Logging done in Update;
      }

      protected override bool Update(object newValue)
      {
        var changed = false;
        //Parse the value from the UI tree
        if (!(newValue is string valueStr)) return false;
        var offices = valueStr.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries)
          .Select(officeStr =>
          {
            var officeSplit = officeStr.Split(':');
            return new {OfficeKey = officeSplit[0], Incumbents = officeSplit[1].Split(',')};
          }).ToArray();

        foreach (var office in offices)
        foreach (var incumbent in office.Incumbents)
        {
          var table =
            ElectionsIncumbentsRemoved.GetDataByElectionKeyOfficeKeyPoliticianKey(
              Page.GetElectionKey(), office.OfficeKey, incumbent);
          if (table.Count == 1)
          {
            var row = table[0];
            changed = true;
            if (!OfficesOfficials.OfficeKeyPoliticianKeyExists(row.OfficeKey,
              row.PoliticianKey))
              OfficesOfficials.Insert(row.OfficeKey, row.PoliticianKey, row.RunningMateKey,
                row.StateCode, Offices.GetCountyCodeFromKey(row.OfficeKey),
                Offices.GetLocalKeyFromKey(row.OfficeKey), row.DistrictCode,
                row.DataLastUpdated, row.UserSecurity, row.UserName);
            ElectionsIncumbentsRemoved.DeleteByElectionKeyOfficeKeyPoliticianKey(
              Page.GetElectionKey(), office.OfficeKey, incumbent);
          }
        }

        //LoadControl();
        return changed;
      }
    }

    private AdjustIncumbentsTabItem[] _AdjustIncumbentsTabInfo;

    #endregion DataItem object

    private static void PopulateIncumbentsToAdjustList(
      IEnumerable<IGrouping<string, DataRow>> incumbentsToEliminate, Control parent)
    {
      parent.Controls.Clear();

      new HtmlH6
      {
        InnerHtml =
          "Incumbent(s) must be removed from the following office(s)" +
          " to make room for newly elected candidate(s).<br />" +
          "<em>If any incumbents won reelection, remove them here then" +
          " mark them as winners on the </em>General Winners<em> tab.</em><br />" +
          "<em>If no incumbents were running for reelection, remove the current" +
          " office holder whose term is expiring.</em>"
      }.AddTo(parent);

      var container = new HtmlDiv().AddTo(parent, "offices");

      foreach (var incumbents in incumbentsToEliminate)
      {
        var office = incumbents.First();
        var div = new HtmlDiv().AddTo(container, "office");
        div.Attributes.Add("rel", office.OfficeKey());
        new HtmlP {InnerHtml = Offices.FormatOfficeName(office)}.AddTo(div, "office-name");
        var extra = office.ElectionPositions() - (office.Incumbents() - incumbents.Count());
        new HtmlP {InnerHtml = $"Eliminate {extra}"}.AddTo(div, "office-extra");
        foreach (var incumbent in incumbents)
        {
          var p = new HtmlP().AddTo(div);
          new HtmlInputCheckBox {Checked = true, Value = incumbent.PoliticianKey()}.AddTo(p,
            "incumbent");
          new LiteralControl(Politicians.FormatName(incumbent)).AddTo(p);
        }
      }
    }

    private static void PopulateIncumbentsToReinstateList(
      IEnumerable<IGrouping<string, DataRow>> incumbentsToReinstate, Control parent)
    {
      parent.Controls.Clear();

      var container = new HtmlDiv().AddTo(parent, "offices");

      foreach (var incumbents in incumbentsToReinstate)
      {
        var office = incumbents.First();
        var div = new HtmlDiv().AddTo(container, "office");
        div.Attributes.Add("rel", office.OfficeKey());
        new HtmlP {InnerHtml = Offices.FormatOfficeName(office)}.AddTo(div, "office-name");
        foreach (var incumbent in incumbents)
        {
          var p = new HtmlP().AddTo(div);
          new HtmlInputCheckBox {Checked = false, Value = incumbent.PoliticianKey()}
            .AddTo(p, "incumbent");
          new LiteralControl(Politicians.FormatName(incumbent)).AddTo(p);
        }
      }
    }

    #endregion Private

    #region Event handlers and overrides

    protected void ButtonAdjustIncumbents_OnClick(object sender, EventArgs e)
    {
      switch (AdjustIncumbentsReloading.Value)
      {
        case "reloading":
        {
          if (Elections.IsPrimaryElection(GetElectionKey()))
            throw new VoteException("This tab is only for general elections");
          AdjustIncumbentsReloading.Value = Empty;
          AdjustIncumbentsDataArea.RemoveCssClass("hidden");
          _AdjustIncumbentsTabInfo.LoadControls();
          SetElectionHeading(HeadingAdjustIncumbents);
          FeedbackAdjustIncumbents.AddInfo(
            $"{AdjustIncumbentsTabItem.OfficeCount} offices need incumbent adjustments.");
        }
          break;

        case "":
        {
          // normal update
          _AdjustIncumbentsTabInfo.Update(FeedbackAdjustIncumbents);
          _AdjustIncumbentsTabInfo.LoadControls();
        }
          break;

        default:
          throw new VoteException(
            $"Unknown reloading option: '{AdjustIncumbentsReloading.Value}'");
      }
    }

    #endregion Event handlers and overrides
  }
}