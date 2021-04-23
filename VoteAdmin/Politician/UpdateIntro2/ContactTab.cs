using System;
using System.Web.UI.HtmlControls;
using DB.Vote;

namespace Vote.Politician
{
  public partial class UpdateIntro2Page
  {
    #region Private

    #region DataItem object

    [PageInitializer]
    private class ContactTabItem : PoliticianDataItem
    {
      private const string GroupName = "Contact";

      private ContactTabItem(UpdateIntro2Page page) : base(page, GroupName) {}

      private static ContactTabItem[] GetContactTabInfo(UpdateIntro2Page page)
      {
        var contactTabInfo = new[]
          {
            new ContactTabItem(page)
              {
                Column = "FName",
                Description = "First Name",
                Validator = ValidateFirstName
              },
            new ContactTabItem(page)
              {
                Column = "MName",
                Description = "Middle Name",
                Validator = ValidateMiddleName
              },
            new ContactTabItem(page)
              {
                Column = "Nickname",
                Description = "Nickname",
                Validator = ValidateNickname
              },
            new ContactTabItem(page)
              {
                Column = "LName",
                Description = "Last Name",
                Validator = ValidateLastName
              },
            new ContactTabItem(page)
              {
                Column = "Suffix",
                Description = "Suffix",
                Validator = ValidateSuffix
              },
            new ContactTabItem(page)
              {
                Column = "PublicAddress",
                Description = "Street Address",
                Validator = ValidateStreet
              },
            new ContactTabItem(page)
              {
                Column = "PublicCityStateZip",
                Description = "City, State Zip",
                Validator = ValidateCityStateZip
              },
            new ContactTabItem(page)
              {
                Column = "PublicPhone",
                Description = "Phone",
                Validator = ValidateWhiteSpace
              },
            new ContactTabItem(page)
              {
                Column = "PublicEmail",
                Description = "Email",
                Validator = ValidateEmail
              },
            new ContactTabItem(page)
              {
                Column = "DateOfBirth",
                Description = "Date of Birth",
                Validator = ValidateDateOfBirth
              },
            new ContactTabItem(page)
              {
                Column = "PartyKey",
                Description = "Political Party"
              }
          };

        foreach (var item in contactTabInfo)
          item.InitializeItem(page);

        InitializeGroup(page, GroupName);

        return contactTabInfo;
      }

      // ReSharper disable UnusedMember.Global
      // Invoked via Reflection
      internal static void Initialize(UpdateIntro2Page page)
        // ReSharper restore UnusedMember.Global
      {
        page._ContactTabInfo = GetContactTabInfo(page);
        page.RegisterUpdateAll(page.UpdateAllContacts);
        new MainTabItem { TabName = GroupName }.Initialize(page);
        if (!page.IsPostBack)
        {
          page.LoadContactTabData();
          page.RefreshContactTab();
        }
      }
    }

    private ContactTabItem[] _ContactTabInfo;

    #endregion DataItem object

    private void LoadContactTabData()
    {
      LoadPartiesDropdown(Politicians.GetStateCodeFromKey(PoliticianKey),
        ControlContactPartyKey, string.Empty, PartyCategories.None,
        PartyCategories.StateParties, PartyCategories.NationalParties,
        PartyCategories.NonParties);

      foreach (var item in _ContactTabInfo)
        item.LoadControl();

      if (IsSuperUser)
        BallotNameInfo.Visible = false;
      else
      {
        ControlContactFName.Enabled = false;
        ControlContactMName.Enabled = false;
        ControlContactNickname.Enabled = false;
        ControlContactLName.Enabled = false;
        ControlContactSuffix.Enabled = false;
      }
    }

    private void RefreshContactTab()
    {
      NameOnBallots.InnerText =
        PageCache.Politicians.GetPoliticianName(PoliticianKey);
      ControlContactAge.Text = PageCache.Politicians.GetAge(PoliticianKey);
      SetPartyNameAndLink(ControlContactPartyKey.GetValue(), PartyName);
    }

    internal static void SetPartyNameAndLink(string partyKey,
      HtmlContainerControl partyNameControl)
    {
      var partyName = Parties.GetPartyName(partyKey);
      var partyCode = Parties.GetPartyCode(partyKey);
      if (!string.IsNullOrWhiteSpace(partyName))
      {
        var displayPartyName = partyName;
        if (!string.IsNullOrWhiteSpace(partyCode))
          displayPartyName += " (" + partyCode + ")";
        var webSite = Parties.GetPartyUrl(partyKey);
        partyNameControl.Controls.Clear();
        partyNameControl.InnerHtml = string.Empty;
        if (string.IsNullOrWhiteSpace(webSite))
          partyNameControl.InnerText = displayPartyName;
        else
          new HtmlAnchor
            {
              InnerText = displayPartyName,
              HRef = NormalizeUrl(webSite),
              Title = "Visit the " + partyName + " web site",
              Target = "_View"
            }.AddTo(partyNameControl, "tiptip");
      }
      else
        partyNameControl.InnerText = "none";
    }

    private int UpdateAllContacts(bool showSummary)
    {
      _ContactTabInfo.ClearValidationErrors();
      var errorCount = _ContactTabInfo.Update(FeedbackContact, showSummary,
        UpdatePanelContact);
      InvalidatePageCache();
      RefreshContactTab();
      return errorCount;
    }

    #endregion Private

    #region Event handlers and overrides

    protected void ButtonContact_OnClick(object sender, EventArgs e)
    {
      try
      {
        UpdateAllContacts(true);
      }
      catch (Exception ex)
      {
        FeedbackContact.HandleException(ex);
      }
    }

    #endregion Event handlers and overrides
  }
}