using System;
using System.Web.UI;
using DB.Vote;
using static System.String;

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
          AddCandidatesReloading.Value = Empty;
          OfficeControl.ShowSelectOfficePanel = false;
          _ManagePoliticiansPanel.LoadControls();
          var officeName = Offices.FormatOfficeName(OfficeControl.OfficeKey);
          if (IsNullOrWhiteSpace(officeName)) officeName = "Invalid office";
          HeadingAddCandidatesOffice.InnerText = officeName;
          _ManagePoliticiansPanel.ClearAddNewCandidate(true);
        }
          break;

        case "":
        {
          var isVirtualOffice = Offices.IsVirtualKey(OfficeControl.OfficeKey);
          if (isVirtualOffice)
            OfficeControl.OfficeKey = Offices.ActualizeOffice(OfficeControl.OfficeKey, CountyCode,
              LocalKey);
          // normal update
          _ManagePoliticiansPanel.Update();
          _ManagePoliticiansPanel.ClearAddNewCandidate();
          if (isVirtualOffice)
            LoadOfficeControl();
        }
          break;

        default:
          throw new VoteException($"Unknown reloading option: '{AddCandidatesReloading.Value}'");
      }
    }

    #endregion Event handlers and overrides
  }
}