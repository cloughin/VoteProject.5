namespace Vote.Politician
{
  public partial class UpdateIntro2Page
  {
    #region Private

    #region DataItem object

    // ReSharper disable UnusedMember.Local
    // No updating here, only to provide initialization services
    [PageInitializer]
    private class UploadTabItem : PoliticianDataItem
      // ReSharper restore UnusedMember.Local
    {
      private const string GroupName = "Upload";

      public UploadTabItem(UpdateIntro2Page page, string groupName)
        : base(page, groupName) {}

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(UpdateIntro2Page page)
        // ReSharper restore UnusedMember.Local
      {
        new MainTabItem
        {
          TabName = GroupName,
          TabAsteriskToolTip =
            "There is a picture selected but it has not yet been uploaded",
          ButtonToolTip = "Upload the picture"
        }.Initialize(page);
        page.UploadUrl.Value = AjaxImageUploaderUrl;
        if (!page.IsPostBack)
        {
          page.LoadPictureTabData();
          InitializeGroup(page, GroupName);
        }
      }
    }

    #endregion DataItem object

    private void LoadPictureTabData()
    {
      ImagePicture.ImageUrl = NoCacheImageProfile300Url;
    }

    #endregion Private
  }
}