using System;
using System.Web.UI;

namespace Vote.Admin
{
  public partial class UpdatePoliticiansPage
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
        => InitializeGroup(page, "AddCandidates");

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
          _ManagePoliticiansPanel.LoadControls();
          _ManagePoliticiansPanel.ClearAddNewCandidate(true);
        }
          break;

        case "":
        {
          //// normal update
          //_ManagePoliticiansPanel.Update();
          //_ManagePoliticiansPanel.ClearAddNewCandidate();
        }
          break;

        default:
          throw new VoteException("Unknown reloading option");
      }
    }

    #endregion Event handlers and overrides
  }
}