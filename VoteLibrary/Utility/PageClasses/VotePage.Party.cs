using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using DB.Vote;
using static System.String;

namespace Vote
{
  // Public methods and properties relation to parties
  public partial class VotePage
  {
    #region Private

    private static void LoadPartiesDropdownGroup(
      ListControl dropdown, string group, IEnumerable<PartiesRow> table,
      string selected)
    {
      var item = new ListItem();
      item.Attributes.Add("optgroup", group);
      dropdown.Items.Add(item);
      foreach (var row in table)
      {
        item = new ListItem {Value = row.PartyKey, Text = row.PartyName};
        if (row.PartyKey == selected)
          item.Selected = true;
        dropdown.Items.Add(item);
      }
    }

    #endregion Private

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global

    [Flags]
    public enum PartyCategories
    {
      None = 0,
      MajorNationalParties = 0x01,
      MinorNationalParties = 0x02,
      NationalParties = 0x03,
      StateParties = 0x04,
      NonParties = 0x08
    }

    public static void LoadPartiesDropdown(
      string stateCode, DropDownListWithOptionGroup dropdown,
      string selectedPartyCode, params PartyCategories[] categories)
    {
      var first = true;
      foreach (var category in categories)
      {
        switch (category)
        {
          case PartyCategories.None:
            if (first)
            {
              //var item = new ListItem { Value = "X", Text = "None" };
              var item = new ListItem {Value = Empty, Text = "None"};
              if (IsNullOrWhiteSpace(selectedPartyCode))
                item.Selected = true;
              dropdown.Items.Add(item);
            }
            break;

          case PartyCategories.NationalParties:
          {
            var table = Parties.GetCacheDataByStateCode("US")
              .Where(row => !IsNullOrWhiteSpace(row.PartyCode));
            LoadPartiesDropdownGroup(
              dropdown, "National Parties", table, selectedPartyCode);
          }
            break;

          case PartyCategories.MajorNationalParties:
          {
            var table = Parties.GetCacheDataByStateCodeIsPartyMajor("US", true);
            LoadPartiesDropdownGroup(
              dropdown, "Major National Parties", table, selectedPartyCode);
          }
            break;

          case PartyCategories.MinorNationalParties:
          {
            var table = Parties.GetCacheDataByStateCodeIsPartyMajor("US", false)
              .Where(row => !IsNullOrWhiteSpace(row.PartyCode));
            LoadPartiesDropdownGroup(
              dropdown, "Minor National Parties", table, selectedPartyCode);
          }
            break;

          case PartyCategories.StateParties:
          {
            var table = Parties.GetCacheDataByStateCode(stateCode);
            LoadPartiesDropdownGroup(
              dropdown, "State Parties", table, selectedPartyCode);
          }
            break;

          case PartyCategories.NonParties:
          {
            var table = Parties.GetCacheDataByStateCodePartyCode(
              "US", Empty);
            LoadPartiesDropdownGroup(
              dropdown, "Non-Parties", table, selectedPartyCode);
          }
            break;
        }
        first = false;
      }
    }

    //public static void LoadPartiesDropdown(
    //  string stateCode, DropDownListWithOptionGroup dropdown,
    //  params PartyCategories[] categories)
    //{
    //  LoadPartiesDropdown(stateCode, dropdown, null, categories);
    //}

    //public static void LoadPartiesDropdown(
    //  DropDownListWithOptionGroup dropdown, string selectedPartyCode,
    //  params PartyCategories[] categories)
    //{
    //  LoadPartiesDropdown(null, dropdown, selectedPartyCode, categories);
    //}

    //public static void LoadPartiesDropdown(
    //  DropDownListWithOptionGroup dropdown, params PartyCategories[] categories)
    //{
    //  LoadPartiesDropdown(null, dropdown, null, categories);
    //}


    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }
}