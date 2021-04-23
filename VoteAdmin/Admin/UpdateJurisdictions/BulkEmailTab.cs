using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB.Vote;

namespace Vote.Admin
{
  public partial class UpdateJurisdictionsPage
  {
    //#region Private

    //#region DataItem objects

    //[PageInitializer]
    //// ReSharper disable once UnusedMember.Local
    //private class BulkEmailAvailableSubstitutionsTabItem : JurisdictionsDataItem
    //{
    //  private const string GroupName = "BulkEmailAvailableSubstitutions";

    //  protected BulkEmailAvailableSubstitutionsTabItem(UpdateJurisdictionsPage page)
    //    : base(page, GroupName) { }

    //  // ReSharper disable UnusedMember.Local
    //  // Invoked via Reflection
    //  internal static void Initialize(UpdateJurisdictionsPage page)
    //  // ReSharper restore UnusedMember.Local
    //  {
    //    if (!page.IsPostBack)
    //    {
    //      InitializeAvailableSubstitutionsTab(page);
    //      InitializeSelectRecipientsTab(page);
    //      InitializeEmailOptionsTab(page);
    //      InitializePreviewSampleTab(page);
    //    }
    //  }

    //  private static void InitializeAvailableSubstitutionsTab(
    //    UpdateJurisdictionsPage page)
    //  {
    //    const int columns = 1;

    //    var availableSubstitutions = EmailTemplates
    //      .GetAvailableSubstitutionsForJurisdictions()
    //      .OrderBy(kvp => kvp.Key.Substring(2, kvp.Key.Length - 4))
    //      .GroupBy(kvp => Substitutions.GetClass(kvp.Key))
    //      .OrderBy(g => g.Key);

    //    var table = new HtmlTable().AddTo(page.AvailableSubstitutionsPlaceHolder,
    //      "substitution-table");

    //    var substitutions = new List<string>();

    //    foreach (var group in availableSubstitutions)
    //    {
    //      var g = @group.ToList(); // so we can extend it
    //      var names = string.Join("|",
    //        g.Select(kvp => kvp.Key.Substring(2, kvp.Key.Length - 4)));
    //      switch (@group.Key)
    //      {
    //        case Substitutions.Class.Text:
    //          substitutions.Add("substitutionList:\"" + names + "\"");
    //          break;

    //        case Substitutions.Class.Email:
    //          substitutions.Add("emailSubstitutionList:\"" + names + "\"");
    //          g.Add(new KeyValuePair<string, string>(
    //            "@@<i>any email address</i>@@",
    //            "Mailto link for the <i>email address</i>."));
    //          break;

    //        case Substitutions.Class.Web:
    //          substitutions.Add("webSubstitutionList:\"" + names + "\"");
    //          g.Add(new KeyValuePair<string, string>("##<i>any web address</i>##",
    //            "Hyperlink to the <i>web address</i>."));
    //          break;
    //      }

    //      page.AvailableSubstitutionsLiteral.Text =
    //        "var availableSubstitutions={" + string.Join(",", substitutions) + "};";

    //      var row = new HtmlTableRow().AddTo(table);
    //      new HtmlTableCell("th")
    //      {
    //        InnerText = Substitutions.GetClassDescription(@group.Key, true),
    //        ColSpan = 2 * columns
    //      }.AddTo(row, "substitution-class");
    //      row = new HtmlTableRow().AddTo(table);
    //      for (var n = 0; n < columns; n++)
    //      {
    //        new HtmlTableCell("th") { InnerHtml = "Name" }.AddTo(row);
    //        new HtmlTableCell("th") { InnerHtml = "Description" }.AddTo(row);
    //      }
    //      var reordered = g.ReorderVertically(columns)
    //        .ToList();
    //      var odd = true;
    //      for (var n = 0; n < reordered.Count; n += columns)
    //      {
    //        row = new HtmlTableRow().AddTo(table, odd ? "odd" : "even");
    //        odd = !odd;
    //        for (var m = n; m < n + columns; m++)
    //        {
    //          var name = m < reordered.Count
    //            ? String.Format(
    //              "<span class=\"escape\">{0}</span>{1}<span class=\"escape\">{2}</span>",
    //              reordered[m].Key.Substring(0, 2),
    //              reordered[m].Key.Substring(2, reordered[m].Key.Length - 4),
    //              reordered[m].Key.Substring(reordered[m].Key.Length - 2, 2))
    //            : "&nbsp;";
    //          var desc = m < reordered.Count ? reordered[m].Value : "&nbsp;";
    //          new HtmlTableCell { InnerHtml = name }.AddTo(row, "name");
    //          new HtmlTableCell { InnerHtml = desc }.AddTo(row, "desc");
    //        }
    //      }
    //    }
    //  }

    //  private static void InitializeSelectRecipientsTab(
    //    UpdateJurisdictionsPage page)
    //  {
    //    page.BulkEmailRecipientsSpecificState.InnerText =
    //      StateCache.GetStateName(page.StateCode);
    //    page.BulkEmailRecipientsAllCountiesCheckbox.Checked = true;
    //    page.BulkEmailRecipientsAllCountiesCheckbox.Disabled = true;
    //    page.BulkEmailRecipientsCountiesListButton.Disabled = true;
    //    page.BulkEmailRecipientsAllLocalsCheckbox.Checked = true;
    //    page.BulkEmailRecipientsAllLocalsCheckbox.Disabled = true;
    //    page.BulkEmailRecipientsLocalsListButton.Disabled = true;
    //    switch (UserSecurityClass)
    //    {
    //      case MasterSecurityClass:
    //        page.BulkEmailRecipientsAllStates.RemoveCssClass("hidden");
    //        page.BulkEmailRecipientsAllStatesCheckbox.Checked = true;
    //        page.BulkEmailRecipientsSpecificState.AddCssClasses("hidden");
    //        page.BulkEmailRecipientsStatesList.RemoveCssClass("hidden");
    //        page.BulkEmailRecipientsStatesList.Controls.Clear();
    //        foreach (var stateCode in StateCache.All51StateCodes)
    //        {
    //          var stateName = StateCache.GetStateName(stateCode);
    //          var p =
    //            new HtmlP().AddTo(
    //              page.BulkEmailRecipientsStatesList, "sublabel");
    //          p.Attributes["title"] = stateName;
    //          new HtmlInputCheckBox { Value = stateCode, Checked = true }.AddTo(p);
    //          new LiteralControl(stateName).AddTo(p);
    //        }
    //        page.BulkEmailRecipientCounties.AddCssClasses("disabled hidden");
    //        page.BulkEmailRecipientsCountiesListButtonContainer.RemoveCssClass(
    //          "hidden");
    //        page.BulkEmailRecipientLocals.AddCssClasses("hidden");
    //        break;

    //      case StateAdminSecurityClass:
    //        page.BulkEmailRecipientCounties.AddCssClasses("hidden");
    //        page.BulkEmailRecipientsAllCountiesCheckbox.Disabled = false;
    //        page.BulkEmailRecipientsCountiesList.RemoveCssClass("hidden");
    //        page.BulkEmailRecipientsCountiesList.Controls.Clear();
    //        foreach (
    //          var countyCode in CountyCache.GetCountiesByState(page.StateCode))
    //        {
    //          var countyName = CountyCache.GetCountyName(page.StateCode, countyCode);
    //          var p =
    //            new HtmlP().AddTo(
    //              page.BulkEmailRecipientsCountiesList, "sublabel");
    //          p.Attributes["title"] = countyName;
    //          new HtmlInputCheckBox { Value = countyCode, Checked = true }.AddTo(p);
    //          new LiteralControl(countyName).AddTo(p);
    //        }
    //        page.BulkEmailRecipientLocals.AddCssClasses("hidden");
    //        break;

    //      case CountyAdminSecurityClass:
    //        page.BulkEmailRecipientsStateContact.Visible = false;
    //        page.BulkEmailRecipientsCountyContact.Checked = true;
    //        page.BulkEmailRecipientsAllCounties.AddCssClasses("hidden");
    //        page.BulkEmailRecipientsSpecificCounty.RemoveCssClass("hidden");
    //        page.BulkEmailRecipientsSpecificCounty.InnerText =
    //          CountyCache.GetCountyName(page.StateCode, page.CountyCode);
    //        page.BulkEmailRecipientLocals.RemoveCssClass("disabled");
    //        page.BulkEmailRecipientsAllLocalsCheckbox.Disabled = false;
    //        page.BulkEmailRecipientsLocalsListButtonContainer.AddCssClasses(
    //          "hidden");
    //        page.BulkEmailRecipientsLocalsList.RemoveCssClass("hidden");
    //        var locals = LocalDistricts.GetNamesDictionary(page.StateCode,
    //          page.CountyCode);
    //        page.BulkEmailRecipientsLocalsList.Controls.Clear();
    //        foreach (var local in locals.OrderBy(kvp => kvp.Value))
    //        {
    //          var p =
    //            new HtmlP().AddTo(
    //              page.BulkEmailRecipientsLocalsList, "sublabel");
    //          p.Attributes["title"] = local.Value;
    //          new HtmlInputCheckBox { Value = local.Key, Checked = true }.AddTo(p);
    //          new LiteralControl(local.Value).AddTo(p);
    //        }
    //        page.BulkEmailRecipientLocals.AddCssClasses("hidden");
    //        break;

    //      case LocalAdminSecurityClass:
    //        page.BulkEmailRecipientsStateContact.Visible = false;
    //        page.BulkEmailRecipientsCountyContact.Visible = false;
    //        page.BulkEmailRecipientsLocalContact.Checked = true;
    //        page.BulkEmailRecipientsAllCounties.AddCssClasses("hidden");
    //        page.BulkEmailRecipientsSpecificCounty.RemoveCssClass("hidden");
    //        page.BulkEmailRecipientsSpecificCounty.InnerText =
    //          CountyCache.GetCountyName(page.StateCode, page.CountyCode);
    //        page.BulkEmailRecipientLocals.RemoveCssClass("disabled");
    //        page.BulkEmailRecipientsAllLocals.AddCssClasses("hidden");
    //        page.BulkEmailRecipientsSpecificLocal.RemoveCssClass("hidden");
    //        page.BulkEmailRecipientsSpecificLocal.InnerText =
    //          LocalDistricts.GetLocalDistrict(page.StateCode, page.CountyCode,
    //            page.LocalCode);
    //        page.BulkEmailRecipientsLocalsListButtonContainer.AddCssClasses(
    //          "hidden");
    //        break;
    //    }
    //  }

    //  private static void AddEmailsTo(Control parent, IEnumerable<string> emailList,
    //    string name, bool isRadio)
    //  {
    //    parent.Controls.Clear();
    //    var index = 1;
    //    foreach (var email in emailList)
    //    {
    //      var isChecked = index == 1;
    //      var id = name + index++;
    //      var div = new HtmlDiv().AddTo(parent, "tiptip");
    //      div.Attributes["title"] = email;
    //      HtmlInputControl inputControl;
    //      if (isRadio)
    //        inputControl = new HtmlInputRadioButton
    //        {
    //          ID = id,
    //          Value = email,
    //          Name = name,
    //          Checked = isChecked
    //        };
    //      else inputControl = new HtmlInputCheckBox { ID = id, Value = email };
    //      inputControl.AddTo(div);
    //      HtmlLabel { InnerText = email }.AddTo(div)
    //        .Attributes["for"] = id;
    //    }
    //  }

    //  private static void InitializeEmailOptionsTab(UpdateJurisdictionsPage page)
    //  {
    //    var emailData = Security.GetEmailDataByUserName(UserName);
    //    if (emailData.Count == 1)
    //    {
    //      var emails = new List<string> { emailData[0].UserEmail.SafeString() };
    //      emails.AddRange(emailData[0].UserEmails.SafeString()
    //        .Split(new[] { ',' })
    //        .Select(a => a.Trim()));
    //      var emailList = emails.Select(a => a.Trim())
    //        .Where(Validation.IsValidEmailAddress)
    //        .Distinct(StringComparer.OrdinalIgnoreCase)
    //        .ToList();
    //      AddEmailsTo(page.BulkEmailOptionsFrom, emailList, "BulkEmailOptionsFrom",
    //        true);
    //      AddEmailsTo(page.BulkEmailOptionsCC, emailList, "BulkEmailOptionsCC",
    //        false);
    //      AddEmailsTo(page.BulkEmailOptionsBCC, emailList, "BulkEmailOptionsBCC",
    //        false);
    //      // On the Send Emails tab
    //      AddEmailsTo(page.BulkEmailTestAddress, emailList, "BulkEmailTestAddress",
    //        false);
    //    }
    //  }

    //  private static void InitializePreviewSampleTab(UpdateJurisdictionsPage page)
    //  {
    //    StateCache.Populate(page.PreviewSampleStateDropDownList, page.StateCode);
    //    CountyCache.Populate(page.PreviewSampleCountyDropDownList,
    //      page.StateCode, "<none>", string.Empty, page.CountyCode);
    //    LocalDistricts.Populate(page.PreviewSampleLocalDropDownList,
    //     page.StateCode, page.CountyCode, "<none>", string.Empty,
    //     page.LocalCode);

    //    switch (UserSecurityClass)
    //    {
    //      case MasterSecurityClass:
    //        break;

    //      case StateAdminSecurityClass:
    //        page.PreviewSampleStateDropDownList.Enabled = false;
    //        break;

    //      case CountyAdminSecurityClass:
    //        page.PreviewSampleStateDropDownList.Enabled = false;
    //        page.PreviewSampleCountyDropDownList.Enabled = false;
    //        break;

    //      case LocalAdminSecurityClass:
    //        page.PreviewSampleStateDropDownList.Enabled = false;
    //        page.PreviewSampleCountyDropDownList.Enabled = false;
    //        page.PreviewSampleLocalDropDownList.Enabled = false;
    //        break;
    //    }
    //  }
    //}

    //#endregion DataItem objects

    //#endregion Private

    //#region Event handlers and overrides

    //#endregion Event handlers and overrides
  }
}