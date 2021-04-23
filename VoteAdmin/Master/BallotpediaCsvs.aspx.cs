using System;

namespace Vote.Master
{
  [PageInitializers]
  public partial class BallotPediaCsvsPage : SecurePage, ISuperUser
  {
    [PageInitializer]
// ReSharper disable once UnusedMember.Local
    private class UploadDataItem : DataItemBase
    {
      private const string GroupName = "Upload";

      private UploadDataItem() : base(GroupName)
      {
      }

      protected override string GetCurrentValue()
      {
        return null; // not used for UploadDataItem
      }

// ReSharper disable once UnusedMethodReturnValue.Local
      private static UploadDataItem[] GetTabInfo(SecurePage page)
      {
        var uploadDataInfo = new[]
        {
          new UploadDataItem {Column = "Overwrite", Description = "Overwrite Existing CSV"},
          new UploadDataItem {Column = "SaveAs", Description = "Save CSV As"}
        };

        uploadDataInfo.Initialize(page, GroupName);

        return uploadDataInfo;
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(BallotPediaCsvsPage page)
        // ReSharper restore UnusedMember.Local
      {
        GetTabInfo(page);
        page.UploadUrl.Value = "/master/ajaxcsvuploader.aspx";
        if (!page.IsPostBack)
        {
          InitializeGroup(page, GroupName);
        }
      }

      protected override void Log(string oldValue, string newValue)
      {
      }

      protected override bool Update(object newValue)
      {
        return false;
      }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        Page.Title = "BallotPedia CSVs";
        H1.InnerHtml = "BallotPedia CSVs";
      }
    }
  }
}