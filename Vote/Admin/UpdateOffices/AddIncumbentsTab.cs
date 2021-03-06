using System;
using System.Web.UI;
using DB.Vote;

namespace Vote.Admin
{
  public partial class UpdateOfficesPage
  {
    #region Private

    #region DataItem object

    [PageInitializer]
    // ReSharper disable once UnusedMember.Local
    // Invoked via Reflection
    private class AddCandidatesTabItem : DataItemBase
    {
      // The rest of this is in the ManagePoliticiansPanel control
      // ReSharper disable once UnusedMember.Local
      internal static void Initialize(TemplateControl page)
      {
        InitializeGroup(page, "SelectOffice");
        InitializeGroup(page, "AddCandidates");
      }

      protected override bool Update(object newValue) => false;
    }

    #endregion DataItem object

    #endregion Private

    #region Event handlers and overrides

    protected void ButtonAddCandidates_OnClick(object sender, EventArgs e)
    {
      switch (AddCandidatesReloading.Value)
      {
        case "reloading":
        {
          AddCandidatesReloading.Value = string.Empty;
          OfficeControl.ShowSelectOfficePanel = false;
          _ManagePoliticiansPanel.LoadControls();
          var officeName = Offices.FormatOfficeName(OfficeControl.OfficeKey);
          if (string.IsNullOrWhiteSpace(officeName)) officeName = "Invalid office";
          HeadingAddCandidatesOffice.InnerText = officeName;
          _ManagePoliticiansPanel.ClearAddNewCandidate(true);
        }
          break;

        case "":
        {
          var isVirtualOffice = Offices.IsVirtualKey(OfficeControl.OfficeKey);
          if (isVirtualOffice)
            OfficeControl.OfficeKey = Offices.ActualizeOffice(OfficeControl.OfficeKey, CountyCode,
              LocalCode);
          // normal update
          _ManagePoliticiansPanel.Update();
          _ManagePoliticiansPanel.ClearAddNewCandidate();
          if (isVirtualOffice)
            LoadOfficeControl();
        }
          break;

        default:
          throw new VoteException("Unknown reloading option");
      }
    }

    #endregion Event handlers and overrides
  }
}