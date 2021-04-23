using JetBrains.Annotations;
using static System.String;

namespace Vote.Admin
{
  public partial class UpdateElectionsPage
  {
    #region Private

    private readonly string _MostRecentElectionKey = Empty;

    #region DataItem object

    // ReSharper disable UnusedMember.Local
    // No updating here, only to provide initialization services
    [PageInitializer]
    private class SelectElectionControlItem : ElectionsDataItem
      // ReSharper restore UnusedMember.Local
    {
      private const string GroupName = "ElectionControl";

      public SelectElectionControlItem(UpdateElectionsPage page, string groupName) :
        base(page, groupName)
      {
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(UpdateElectionsPage page)
        // ReSharper restore UnusedMember.Local
      {
        InitializeGroup(page, GroupName);
        if (!page.IsPostBack) page.PopulateElectionControl();
      }
    }

    #endregion DataItem object

    private void PopulateElectionControl()
    {
      ElectionControl.Populate(StateCode, CountyCode, LocalKey);
    }

    private void ReloadElectionControl([CanBeNull] string selectedElectionKey = null)
    {
      PopulateElectionControl();
      if (selectedElectionKey != null) SelectedElectionKey.Value = selectedElectionKey;
      ElectionControlUpdatePanel.Update();
    }

    #endregion Private

    #region Event handlers and overrides

    #endregion Event handlers and overrides
  }
}