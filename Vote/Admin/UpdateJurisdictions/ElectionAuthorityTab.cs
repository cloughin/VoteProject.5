using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Vote.Admin
{
  public partial class UpdateJurisdictionsPage
  {
    #region Private

    #region DataItem object

    [PageInitializer]
    private class ElectionAuthorityTabItem : AllJurisdictionsDataItem
    {
      private const string GroupName = "ElectionAuthority";

      protected ElectionAuthorityTabItem(UpdateJurisdictionsPage page)
        : base(page, GroupName)
      {
      }

      private static ElectionAuthorityTabItem[] GetTabInfo(
        UpdateJurisdictionsPage page)
      {
        var electionAuthorityTabInfo = new[]
        {
          new ElectionAuthorityTabItem(page)
          {
            Column = "JurisdictionName",
            Description = "Jurisdiction Name",
            Validator = ValidateRequired
          },
          new ElectionAuthorityTabItem(page)
          {
            Column = "Url",
            Description = "Web Address",
            Validator = ValidateWebAddress,
            LabelIsHyperLink = true
          },
          new ElectionAuthorityTabItem(page)
          {
            Column = "Email",
            Description = "Email Address for Voters",
            Validator = ValidateEmail,
            LabelIsMailTo = true
          },
          new ElectionAuthorityTabItem(page)
          {
            Column = "Contact",
            Description = "Main Contact Name"
          },
          new ElectionAuthorityTabItem(page)
          {
            Column = "ContactTitle",
            Description = "Main Contact Title"
          },
          new ElectionAuthorityTabItem(page)
          {
            Column = "Phone",
            Description = "Main Contact Phone"
          },
          new ElectionAuthorityContactEmailTabItem(page)
          {
            Column = "ContactEmail",
            Description = "Main Contact Email",
            Validator = ValidateEmail,
            LabelIsMailTo = true
          },
          new ElectionAuthorityTabItem(page)
          {
            Column = "AltContact",
            Description = "Alternate Contact Name"
          },
          new ElectionAuthorityTabItem(page)
          {
            Column = "AltContactTitle",
            Description = "Alternate Contact Title"
          },
          new ElectionAuthorityTabItem(page)
          {
            Column = "AltPhone",
            Description = "Alternate Contact Phone"
          },
          new ElectionAuthorityContactEmailTabItem(page)
          {
            Column = "AltEmail",
            Description = "Alternate Contact Email",
            Validator = ValidateEmail,
            LabelIsMailTo = true
          },
          new ElectionAuthorityTabItem(page)
          {
            Column = "AddressLine1",
            Description = "Mailing Address Line 1"
          },
          new ElectionAuthorityTabItem(page)
          {
            Column = "AddressLine2",
            Description = "Mailing Address Line 2"
          },
          new ElectionAuthorityTabItem(page)
          {
            Column = "CityStateZip",
            Description = "Mailing Address City State Zip"
          },
          new ElectionAuthorityTabItem(page)
          {
            Column = "Notes",
            Description = "Notes"
          }
        };

        foreach (var item in electionAuthorityTabInfo) item.InitializeItem(page);

        InitializeGroup(page, GroupName);

        return electionAuthorityTabInfo;
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(UpdateJurisdictionsPage page)
        // ReSharper restore UnusedMember.Local
        => page._ElectionAuthorityTabInfo = GetTabInfo(page);
    }

    private class ElectionAuthorityContactEmailTabItem : ElectionAuthorityTabItem
    {
      public ElectionAuthorityContactEmailTabItem(UpdateJurisdictionsPage page)
        : base(page)
      {
      }

      protected override void AfterUpdate(string newValue)
      {
        base.AfterUpdate(newValue);
        if (Label?.Controls.Count == 1)
        {
          var literal = Label.Controls[0] as LiteralControl;
          if (literal != null)
            literal.Text = literal.Text.Replace(" Email", string.Empty);
          else
          {
            var mailLink = Label.Controls[0] as HyperLink;
            if (mailLink != null)
              mailLink.Text = mailLink.Text.Replace(" Email", string.Empty);
          }
        }
      }
    }

    private ElectionAuthorityTabItem[] _ElectionAuthorityTabInfo;

    #endregion DataItem object

    #endregion Private

    #region Event handlers and overrides

    protected void ButtonElectionAuthority_OnClick(object sender, EventArgs e)
    {
      switch (ElectionAuthorityReloading.Value)
      {
        case "reloading":
        {
          ElectionAuthorityReloading.Value = string.Empty;
          _ElectionAuthorityTabInfo.LoadControls();
          FeedbackElectionAuthority.AddInfo("Election authority information loaded.");
        }
          break;

        case "":
        {
          // normal update
          _ElectionAuthorityTabInfo.ClearValidationErrors();
          _ElectionAuthorityTabInfo.Update(FeedbackElectionAuthority);
        }
          break;

        default:
          throw new VoteException("Unknown reloading option");
      }
    }

    #endregion Event handlers and overrides
  }
}