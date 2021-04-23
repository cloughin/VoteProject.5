using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB;
using DB.Vote;
using JetBrains.Annotations;
using static System.String;

namespace Vote.Admin
{
  [PageInitializers]
  public partial class BulkEmailPage : SecureAdminPage, ISuperUser
  {
    #region Public

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnassignedField.Global

    #endregion ReSharper disable

    // These are public so they can be used by the WebService

    public static List<SimpleListItem> GetEmailTypes()
    {
      return new List<SimpleListItem> { new SimpleListItem { Value = Empty, Text = "<no type>" } }
        .Union(EmailTypes.GetAllData().OrderBy(r => r.Description).Select(r =>
          new SimpleListItem { Value = r.EmailTypeCode, Text = r.Description })).ToList();
    }

    [NotNull]
    public static List<SimpleListItem> GetPreviewElectionItems(string stateCode,
      string countyCode, string localKey, string defaultText = "<none>")
    {
      return new List<SimpleListItem> {new SimpleListItem(Empty, defaultText)}.Union(
        Enumerable.Select(
          Elections
            .GetControlDataByStateCodeCountyCodeLocalKey(stateCode, countyCode, localKey)
            .OrderByDescending(row => row.ElectionDate).ThenBy(row => row.ElectionOrder)
            .ThenBy(row => row.ElectionDesc),
          row => new SimpleListItem(row.ElectionKey, row.ElectionDesc))).ToList();
    }

    [NotNull]
    public static List<SimpleListItem> GetPreviewOfficeItems(string electionKey)
    {
      return new List<SimpleListItem> {new SimpleListItem(Empty, "<none>")}.Union(
        ElectionsOffices.GetElectionOffices(electionKey).Rows.Cast<DataRow>().Select(row =>
          new SimpleListItem(row.OfficeKey(), Offices.FormatOfficeName(row)))).ToList();
    }

    public static List<SimpleListItem> GetPreviewCandidateItems(string electionKey,
      string officeKey)
    {
      return new List<SimpleListItem> {new SimpleListItem(Empty, "<none>")}.Union(
        ElectionsPoliticians.GetPoliticiansForOfficeInElection(electionKey, officeKey).Rows
          .Cast<DataRow>().Select(row =>
            new SimpleListItem(row.PoliticianKey(), Politicians.FormatName(row)))).ToList();
    }

    public static List<SimpleListItem> GetPreviewPartyItems(string stateCode)
    {
      return new List<SimpleListItem> {new SimpleListItem(Empty, "<none>")}.Union(Parties
        .GetDataByStateCode(stateCode).Rows.Cast<DataRow>().OrderBy(row => row.PartyOrder())
        .ThenBy(row => row.PartyName())
        .Select(row => new SimpleListItem(row.PartyKey(), row.PartyName()))).ToList();
    }

    public static List<SimpleListItem> GetPreviewPartyEmailItems(string partyKey)
    {
      return new List<SimpleListItem> {new SimpleListItem(Empty, "<none>")}.Union(
          PartiesEmails.GetDataByPartyKey(partyKey).Rows.Cast<DataRow>()
            .OrderBy(row => row.PartyContactLName()).Select(row =>
              new SimpleListItem(row.PartyEmail(),
                $"{row.PartyContactFName()} {row.PartyContactLName()} <{row.PartyEmail()}>")))
        .ToList();
    }

    #region ReSharper restore

    // ReSharper restore UnassignedField.Global
    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Public

    #region Private

    private void SetCredentialMessage()
    {
      switch (UserSecurityClass)
      {
        case MasterSecurityClass:
          CredentialMessage.InnerHtml =
            "Your sign-in credentials allow access to all bulk mail capabilities.";
          break;

        case StateAdminSecurityClass:
          CredentialMessage.InnerHtml =
            $"Your sign-in credentials permit bulk mailing to any {States.GetName(StateCode)} recipients.";
          break;

        case CountyAdminSecurityClass:
          CredentialMessage.InnerHtml =
            $"Your sign-in credentials permit bulk mailing only to {Counties.GetFullName(StateCode, CountyCode)} recipients.";
          break;

        case LocalAdminSecurityClass:
          CredentialMessage.InnerHtml =
            $"Your sign-in credentials permit bulk mailing only to {LocalDistricts.GetFullName(StateCode, CountyCode, LocalKey)} recipients.";
          break;

        default:
          throw new VoteException($"Unexpected UserSecurityClass: {UserSecurityClass}");
      }
    }

    private void SetSubHeading()
    {
      switch (AdminPageLevel)
      {
        case AdminPageLevel.AllStates:
          H2.InnerHtml = "Bulk Email for All States";
          break;

        case AdminPageLevel.State:
          H2.InnerHtml = $"Bulk Email for {States.GetName(StateCode)}";
          break;

        case AdminPageLevel.County:
          H2.InnerHtml = $"Bulk Email for {Counties.GetFullName(StateCode, CountyCode)}";
          break;

        case AdminPageLevel.Local:
          H2.InnerHtml =
            $"Bulk Email for {LocalDistricts.GetFullName(StateCode, CountyCode, LocalKey)}";
          break;

        case AdminPageLevel.Unknown:
          H2.InnerHtml = "No Jurisdiction Selected";
          break;
      }
    }

    #region DataItem objects

    [PageInitializer]
    // ReSharper disable once UnusedMember.Local
    private class AvailableSubstitutionsTabItem : DataItemBase
    {
      private const string GroupName = "AvailableSubstitutions";

      protected AvailableSubstitutionsTabItem() : base(GroupName)
      {
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(BulkEmailPage page)
        // ReSharper restore UnusedMember.Local
      {
        if (!page.IsPostBack)
        {
          BuildSubstitutionTable(page);
          BuildSubstitutionsOptionsDisplay(page);
        }
      }

      private static void BuildSubstitutionsOptionsDisplay(BulkEmailPage page)
      {
        new HtmlH3 {InnerText = "Substitution Options"}.AddTo(
          page.SubstitutionOptionsPlaceHolder, "options-display");
        foreach (var optionType in Substitutions.OptionTypeInfos)
        {
          var cssClass = "type-" + Regex
            .Replace(optionType.ShortName, "[^a-z0-9]+", "-", RegexOptions.IgnoreCase)
            .ToLowerInvariant();
          var div = new HtmlDiv().AddTo(page.SubstitutionOptionsPlaceHolder,
            cssClass + " options-display-type rounded-border");
          new HtmlH4 {InnerText = optionType.Name}.AddTo(div);
          new LiteralControl(optionType.HtmlDescription).AddTo(div);
        }

        var linkTextDiv = new HtmlDiv().AddTo(page.SubstitutionOptionsPlaceHolder,
          "type-linktext options-display-type rounded-border");
        new HtmlH4 { InnerText = "Literal Link Text" }.AddTo(linkTextDiv);
        new LiteralControl("Literal link text can be added to any hyperlink (##)" +
          " substitution. Enclose the text in curly braces ({}) and insert it just before" +
          " the closing ##. To use an uploaded image, use {Image:&lt;name&gt;} for the link text" +
          " where &lt;name&gt; is the external name of an uploaded image (without file extension).").AddTo(linkTextDiv);

        var embeddedKeysDiv = new HtmlDiv().AddTo(page.SubstitutionOptionsPlaceHolder,
          "type-embeddedkeys options-display-type rounded-border");
        new HtmlH4 { InnerText = "Embedded Keys" }.AddTo(embeddedKeysDiv);
        new LiteralControl("Certain key values (StateCode, CountyCode, LocalKey," +
          " ElectionKey, OfficeKey and PoliticianKey) that are not directly related to" +
          " the recipient selection can be embedded in the template. For example" +
          " <span class=\"escape\">[[</span>OfficeKey=USPresident<span class=\"escape\">]]</span>").AddTo(embeddedKeysDiv);
      }

      private static void BuildSubstitutionTable(BulkEmailPage page)
      {
        var allSubstitutions = Substitutions.GetAllInfo();

        // add specials for email
        AddEmailSubstitution(allSubstitutions, "[[Contact]]",
          "The name of the email recipient", 1);
        AddEmailSubstitution(allSubstitutions, "[[ContactTitle]]",
          "The title of the email recipient", 2);
        AddEmailSubstitution(allSubstitutions, "[[ContactEmail]]",
          "The email address of the email recipient", 3);
        AddEmailSubstitution(allSubstitutions, "[[ContactPhone]]",
          "The phone number of the email recipient", 4);
        AddEmailSubstitution(allSubstitutions, "@@ContactEmail@@",
          "Mailto link for the email recipient", 5);

        // add the generics
        allSubstitutions.Add("@@<i>any email address</i>@@",
          new SubstitutionInfo
          {
            HtmlDescription = "Mailto link for the <i>email address</i>.",
            Type = Substitutions.Type.Generic,
            DisplayOrder = 1
          });
        allSubstitutions.Add("##<i>any web address</i>##",
          new SubstitutionInfo
          {
            HtmlDescription = "Hyperlink to the <i>web address</i>.",
            Type = Substitutions.Type.Generic,
            DisplayOrder = 2
          });

        // sort and group
        var allSubstitutionGroups =
          allSubstitutions.GroupBy(kvp => kvp.Value.Type).OrderBy(g => g.Key);

        var table = new HtmlTable().AddTo(page.AvailableSubstitutionsPlaceHolder,
          "substitution-table");

        foreach (var group in allSubstitutionGroups)
        {
          var g = group.ToList(); // so we can extend it
          HtmlTableRow row;
          if (group.Key != Substitutions.Type.Undocumented &&
            group.Key != Substitutions.Type.Deprecated)
          {
            row = new HtmlTableRow().AddTo(table);
            new HtmlTableCell("th")
            {
              InnerText = Substitutions.TypeNameDictionary[group.Key],
              ColSpan = 2
            }.AddTo(row, "substitution-type");
            row = new HtmlTableRow().AddTo(table);

            new HtmlTableCell("th") {InnerHtml = "Name"}.AddTo(row);
            new HtmlTableCell("th") {InnerHtml = "Description"}.AddTo(row);
          }

          var odd = true;
          for (var n = 0; n < g.Count; n++)
          {
            var key = g[n].Key;
            var value = g[n].Value;
            row = new HtmlTableRow().AddTo(table,
              (odd ? "odd " : "even ") +
              (value.Type == Substitutions.Type.Undocumented ||
                value.Type == Substitutions.Type.Deprecated
                  ? "hidden "
                  : Empty) + Substitutions.GetClass(key).ToString().ToLowerInvariant());
            odd = !odd;

            var nameClasses = n < g.Count ? GetSubstititionNameClassNames(value) : null;

            var name = n < g.Count
              ? $"<span class=\"escape\">{key.Substring(0, 2)}</span>" +
              $"<span class=\"id\">{key.Substring(2, key.Length - 4)}</span>" +
              $"<span class=\"escape\">{key.Substring(key.Length - 2, 2)}</span>"
              : "&nbsp;";

            var desc = value.HtmlDescription;
            if (value.OptionTypes != Substitutions.OptionTypes.None)
              desc += "<br /><b>Options:</b> " + Join(", ",
                Substitutions.OptionTypeInfos
                  .Where(i => (i.OptionType & value.OptionTypes) != 0)
                  .Select(i => i.ShortName));

            new HtmlTableCell {InnerHtml = name}.AddTo(row, nameClasses);
            new HtmlTableCell {InnerHtml = desc}.AddTo(row, "desc");
          }
        }
      }

      private static void AddEmailSubstitution(
        IDictionary<string, SubstitutionInfo> allSubstitutions, string substitution,
        string description, int order)
      {
        allSubstitutions.Add(substitution,
          new SubstitutionInfo
          {
            HtmlDescription = description,
            Type = Substitutions.Type.EmailList,
            DisplayOrder = order
          });
      }

      private static string GetSubstititionNameClassNames(SubstitutionInfo info)
      {
        var classes = info.Requirements.ToString().Split(',').Where(c => c != "None")
          .Select(c => "req-" + c.Trim().ToLowerInvariant()).ToList();
        if (info.Type == Substitutions.Type.Generic) classes.Add("generic");
        classes.Add(info.Type == Substitutions.Type.Deprecated ? "deprecated" : "active");
        classes.Add("name");
        return Join(" ", classes);
      }
    }

    [PageInitializer]
    // ReSharper disable once UnusedMember.Local
    private class SelectRecipientsTabItem : DataItemBase
    {
      private const string GroupName = "SelectRecipients";

      protected SelectRecipientsTabItem() : base(GroupName)
      {
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(BulkEmailPage page)
        // ReSharper restore UnusedMember.Local
      {
        if (!page.IsPostBack)
        {
          page.SingleElectionFilteringMsg.AddCssClasses("hidden");
          switch (UserSecurityClass)
          {
            case MasterSecurityClass:
              page.RecipientsSingleElectionFiltering.Enabled = false;
              page.SingleElectionFilteringMsg.RemoveCssClass("hidden");
              page.RecipientsSelectJurisdictions.Initialize();
              break;

            case StateAdminSecurityClass:
              page.RecipientsSelectJurisdictions.Initialize(page.StateCode);
              break;

            case CountyAdminSecurityClass:
              page.RecipientsStateContact.Visible = false;
              page.RecipientsStateCandidate.Visible = false;
              page.RecipientsPartyOfficial.Visible = false;
              page.RecipientsCountyContact.Checked = true;
              page.RecipientsSelectJurisdictions.Initialize(page.StateCode,
                page.CountyCode);
              break;

            case LocalAdminSecurityClass:
              page.RecipientsStateContact.Visible = false;
              page.RecipientsStateCandidate.Visible = false;
              page.RecipientsCountyContact.Visible = false;
              page.RecipientsCountyCandidate.Visible = false;
              page.RecipientsPartyOfficial.Visible = false;
              page.RecipientsLocalContact.Checked = true;
              page.RecipientsSelectJurisdictions.Initialize(page.StateCode, page.CountyCode,
                page.LocalKey);
              break;
          }
        }
      }
    }

    [PageInitializer]
    // ReSharper disable once UnusedMember.Local
    private class EmailOptionsTabItem : DataItemBase
    {
      private const string GroupName = "EmailOptions";

      protected EmailOptionsTabItem() : base(GroupName)
      {
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(BulkEmailPage page)
        // ReSharper restore UnusedMember.Local
      {
        if (!page.IsPostBack)
        {
          var emailData = Security.GetEmailDataByUserName(UserName);
          if (emailData.Count == 1)
          {
            var emails = new List<string> {emailData[0].UserEmail.SafeString()};
            emails.AddRange(emailData[0].UserEmails.SafeString().Split(',')
              .Select(a => a.Trim()).Where(a =>
                a.EndsWith("@vote-usa.org", StringComparison.OrdinalIgnoreCase)));
            var emailList = emails.Select(a => a.Trim())
              .Where(Validation.IsValidEmailAddress)
              .Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            AddEmailsTo(page.OptionsFrom, emailList, "OptionsFrom", true, true);
            AddEmailsTo(page.OptionsCC, emailList, "OptionsCC", false, true);
            AddEmailsTo(page.OptionsBCC, emailList, "OptionsBCC", false, true);
            // On the Send Emails tab
            AddEmailsTo(page.TestAddress, emailList, "TestAddress", false);
          }
        }
      }

      private static void AddEmailsTo(Control parent, IEnumerable<string> emailList,
        string name, bool isRadio, bool isOption = false)
      {
        parent.Controls.Clear();
        var index = 1;
        foreach (var email in emailList)
        {
          var isChecked = index == 1;
          var id = name + index++;
          var div = new HtmlDiv().AddTo(parent, "tiptip");
          div.Attributes["title"] = email;
          HtmlInputControl inputControl;
          if (isRadio)
            inputControl = new HtmlInputRadioButton
            {
              ID = id,
              Value = email,
              Name = name,
              Checked = isChecked
            };
          else inputControl = new HtmlInputCheckBox {ID = id, Value = email};
          if (isOption) inputControl.AddCssClasses("is-option-click");
          inputControl.AddTo(div);
          new HtmlLabel {InnerText = email}.AddTo(div).Attributes["for"] = id;
        }
      }
    }

    [PageInitializer]
    // ReSharper disable once UnusedMember.Local
    private class PreviewSampleTabItem : DataItemBase
    {
      private const string GroupName = "PreviewSample";

      protected PreviewSampleTabItem() : base(GroupName)
      {
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(BulkEmailPage page)
        // ReSharper restore UnusedMember.Local
      {
        if (!page.IsPostBack)
        {
          if (IsNullOrWhiteSpace(page.StateCode))
          {
            StateCache.Populate(page.PreviewSampleStateDropDownList, "<none>", Empty);
            Utility.PopulateEmpty(page.PreviewSampleElectionDropDownList);
            Utility.PopulateEmpty(page.PreviewSamplePartyDropDownList);
          }
          else
          {
            StateCache.Populate(page.PreviewSampleStateDropDownList, page.StateCode);
            Utility.PopulateFromList(page.PreviewSampleElectionDropDownList,
              GetPreviewElectionItems(page.StateCode, page.CountyCode, page.LocalKey));
            Utility.PopulateFromList(page.PreviewSamplePartyDropDownList,
              GetPreviewPartyItems(page.StateCode));
          }

          CountyCache.Populate(page.PreviewSampleCountyDropDownList, page.StateCode,
            "<none>", Empty, page.CountyCode);
          LocalDistricts.Populate(page.PreviewSampleLocalDropDownList, page.StateCode,
            page.CountyCode, "<none>", Empty, page.LocalKey);
          Utility.PopulateEmpty(page.PreviewSampleOfficeDropDownList);
          Utility.PopulateEmpty(page.PreviewSampleCandidateDropDownList);
          Utility.PopulateEmpty(page.PreviewSamplePartyEmailDropDownList);

          switch (UserSecurityClass)
          {
            case MasterSecurityClass:
              break;

            case StateAdminSecurityClass:
              page.PreviewSampleStateDropDownList.Enabled = false;
              break;

            case CountyAdminSecurityClass:
              page.PreviewSampleStateDropDownList.Enabled = false;
              page.PreviewSampleCountyDropDownList.Enabled = false;
              page.PreviewSamplePartyDropDownList.Enabled = false;
              page.PreviewSamplePartyEmailDropDownList.Enabled = false;
              break;

            case LocalAdminSecurityClass:
              page.PreviewSampleStateDropDownList.Enabled = false;
              page.PreviewSampleCountyDropDownList.Enabled = false;
              page.PreviewSampleLocalDropDownList.Enabled = false;
              page.PreviewSamplePartyDropDownList.Enabled = false;
              page.PreviewSamplePartyEmailDropDownList.Enabled = false;
              break;
          }
        }
      }
    }

    [PageInitializer]
    // ReSharper disable once UnusedMember.Local
    private class EmailLoggingTabItem : DataItemBase
    {
      private const string GroupName = "EmailLogging";

      protected EmailLoggingTabItem() : base(GroupName)
      {
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(BulkEmailPage page)
        // ReSharper restore UnusedMember.Local
      {
        if (!page.IsPostBack)
        {
          page.LogSelectJurisdictions.Initialize();
        }
      }
    }

    [PageInitializer]
    // ReSharper disable once UnusedMember.Local
    private class MasterOnlyTabItem : DataItemBase
    {
      private const string GroupName = "MasterOnly";

      protected MasterOnlyTabItem() : base(GroupName)
      {
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(BulkEmailPage page)
        // ReSharper restore UnusedMember.Local
      {
        if (!IsMasterUser)
        {
          page.TabEmailTypes.Visible = false;
          page.TabMasterItem.Visible = false;
        }
      }
    }

    #endregion DataItem objects

    #endregion Private

    #region Protected

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable VirtualMemberNeverOverriden.Global
    // ReSharper disable UnusedMember.Global

    #endregion ReSharper disable

    public override IEnumerable<string> NonStateCodesAllowed
    {
      get { return new[] {Empty}; }
    }

    #region ReSharper restore

    // ReSharper restore UnusedMember.Global
    // ReSharper restore VirtualMemberNeverOverriden.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Protected

    #region Event handlers and overrides

    protected void Page_Load(object sender, EventArgs e)
    {
      const string title = "Bulk Email";
      Page.Title = title;
      H1.InnerHtml = title;

      SetSubHeading();
      SetCredentialMessage();

      // load email types
      var emailTypes = GetEmailTypes();
      Utility.PopulateFromList(EditTemplateEmailType, emailTypes);
      Utility.PopulateFromList(EmailTypeDialogEmailType, emailTypes);
      emailTypes[0].Text = "<add new type>";
      Utility.PopulateFromList(EditEmailTypeEmailType, emailTypes);

      // load organizations structural data
      var json = new JavaScriptSerializer();
      var body = Master.FindControl("body") as HtmlGenericControl;
      var orgData = Organizations.GetOrganizationsStructuralData();
      // ReSharper disable once PossibleNullReferenceException
      body.Attributes.Add("data-org-data", json.Serialize(orgData));

      // populate org type
      var orgTypes = orgData.OrgTypes.Select(t => new SimpleListItem
      {
        Text = t.OrgType,
        Value = t.OrgTypeId.ToString()
      });
      Utility.PopulateFromList(OrganizationTypes, orgTypes);

      // populate ideologies
      var orgIdeologies =
        new List<SimpleListItem>
        {
          new SimpleListItem {Text = "All ideologies", Value = "0"}
        }.Union(orgData.Ideologies.Select(i =>
          new SimpleListItem {Text = i.Ideology, Value = i.IdeologyId.ToString()}));
      Utility.PopulateFromList(OrganizationIdeologies, orgIdeologies);

      // populate embedded key states
      StateCache.Populate(EmbeddedKeyStates, "<select a state>", Empty);

      if (AdminPageLevel == AdminPageLevel.Unknown) UpdateControls.Visible = false;
    }

    #endregion Event handlers and overrides
  }
}