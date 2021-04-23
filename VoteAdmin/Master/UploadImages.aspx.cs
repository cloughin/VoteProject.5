using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DB.Vote;

namespace Vote.Master
{
  [PageInitializers]
  public partial class UploadImagesPage : SecurePage, ISuperUser
  {
    #region DataItem objects

    [PageInitializer]
    private class UploadImageItem : DataItemBase
    {
      private const string GroupName = "UploadImage";
      private readonly UploadImagesPage ThisPage;

      private UploadImageItem(UploadImagesPage page) :
        base(GroupName)
      {
        ThisPage = page;
      }

      protected override string GetCurrentValue()
      {
        switch (Column)
        {
          case "ImageChanged":
          case "ImageUpdated":
          case "DeleteImage":
            return "False";

          case "ImageId":
            return ThisPage.ControlUploadImageImageId.Text;

          default:
            return ToDisplay(UploadedImages.GetColumn(UploadedImages.GetColumn(Column),
              ThisPage.GetImageId()));
        }
      }

      private static UploadImageItem[] GetInfo(UploadImagesPage page)
      {
        var UploadImageInfo = new[]
        {
            new UploadImageItem(page)
            {
              Column = "ExternalName",
              Description = "External Name",
              Validator = ValidateRequired
            },
            new UploadImageItem(page)
            {
              Column = "FileName",
              Description = "Uploaded File Name"
            },
            new UploadImageItem(page)
            {
              Column = "ImageType",
              Description = "Uploaded Image Type"
            },
            new UploadImageItem(page)
            {
              Column = "Comments",
              Description = "Comments"
            },
            new UploadImageItem(page)
            {
              Column = "ImageChanged",
              ConvertFn = ToBool
            },
            new UploadImageItem(page)
            {
              Column = "ImageUpdated",
              ConvertFn = ToBool
            },
            new UploadImageItem(page)
            {
              Column = "ImageId",
              ConvertFn = ToInt
            },
            new UploadImageItem(page)
            {
              Column = "DeleteImage",
              ConvertFn = ToBool
            }
          };

        foreach (var item in UploadImageInfo) item.InitializeItem(page);

        InitializeGroup(page, GroupName);

        return UploadImageInfo;
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(UploadImagesPage page)
      // ReSharper restore UnusedMember.Local
      {
        page._UploadImageDialogInfo = GetInfo(page);
      }

      protected override bool Update(object newValue)
      {
        if (Column != "ImageChanged" && Column != "ImageUpdated" && Column != "ImageId" && Column != "DeleteImage")
          UploadedImages.UpdateColumn(UploadedImages.GetColumn(Column), newValue,
            ThisPage.GetImageId());
        return true;
      }
    }

    private UploadImageItem[] _UploadImageDialogInfo;
    #endregion DataItem objects

    private void DeleteImage()
    {
      UploadedImages.DeleteById(GetImageId());
      RadioModes.SelectedValue = "new";
      LoadExternalNameDropdown();
      UpdateUi();
    }

    private int GetImageId()
    {
      if (!int.TryParse(ControlUploadImageImageId.Text, out var id))
        return 0;
      return id;
    }

    private void LoadExternalNameDropdown()
    {
      var first = new List<SimpleListItem>{new SimpleListItem
      {
        Text = "<select the external name of an image to update>",
        Value = "0"
      }};
      var items = UploadedImages.GetAllKeysData()
        .Select(r => new SimpleListItem
        {
          Text = r.ExternalName,
          Value = r.Id.ToString()
        });
      Utility.PopulateFromList(SelectExternalName, first.Union(items));
    }

    private void SetupSampleImage()
    {
      var id = GetImageId();
      if (id == 0)
      {
        PreviewImage.Visible = false;
        return;
      }

      PreviewImage.Visible = true;
      var blob = UploadedImages.GetImage(id);
      using (var memoryStream = new MemoryStream(blob, 0, blob.Length))
      {
        var image = System.Drawing.Image.FromStream(memoryStream);
        PreviewSize.InnerText = $"{image.Width}x{image.Height}";
      }

      SampleImage.Src = UrlManager.GetSiteUri($"/image/{ControlUploadImageExternalName.Text}{ControlUploadImageImageType.Text}").ToString();
    }

    private void UpdateUi()
    {
      var updateMode = RadioModes.SelectedValue == "update";
      if (updateMode)
        SelectExternalName.RemoveCssClass("hidden");
      else
      {
        SelectExternalName.AddCssClasses("hidden");
        SelectExternalName.SelectedValue = "0";
        ControlUploadImageImageId.Text = "0";
      }
      ButtonDeleteImage.Visible = updateMode && GetImageId() != 0;
      switch (RadioModes.SelectedValue)
      {
        case "new":
          ButtonUploadImage.Text = "Upload Image";
          ButtonUploadImage.Attributes["title"] = "Upload the Image";
          break;

        case "update":
          ButtonUploadImage.Text = "Update Image";
          ButtonUploadImage.Attributes["title"] = "Update the Image";
          break;
      }

      _UploadImageDialogInfo.LoadControls();
      SetupSampleImage();
    }

    #region Event handlers and overrides

    //protected void ButtonDeleteImage_OnClick(object sender, EventArgs e)
    //{
    //}

    protected void ButtonUploadImage_OnClick(object sender, EventArgs e)
    {
      if (ControlUploadImageDeleteImage.Text == "True")
      {
        ControlUploadImageDeleteImage.Text = "False";
        DeleteImage();
        return;
      }
      if (RadioModes.SelectedValue == "new")
      {
        ControlUploadImageImageChanged.Text = "False";
        ControlUploadImageImageUpdated.Text = "False";
        //switch to update mode
        RadioModes.SelectedValue = "update";
      }
      LoadExternalNameDropdown();
      SelectExternalName.SelectedValue = GetImageId().ToString();
      UpdateUi();
      _UploadImageDialogInfo.ClearValidationErrors();
    }

    protected void RadioModes_OnSelectedIndexChanged(object sender, EventArgs e)
    {
      UpdateUi();
    }

    protected void SelectExternalName_SelectedIndexChanged(object sender, EventArgs e)
    {
      // load the existimg image
      ControlUploadImageImageId.Text = SelectExternalName.SelectedValue;
      UpdateUi();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        Page.Title = "Upload Images";
        H1.InnerHtml = "Upload Images";
        LoadExternalNameDropdown();
        _UploadImageDialogInfo.LoadControls();
        FeedbackUploadImage.AddInfo("Image information loaded.");
        UpdateUi();
      }
    }
    #endregion Event handlers and overrides
  }
}