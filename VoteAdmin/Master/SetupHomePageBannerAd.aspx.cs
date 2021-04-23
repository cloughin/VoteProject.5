using System;
using System.Globalization;
using DB.Vote;
using static System.String;

namespace Vote.Master
{
  [PageInitializers]
  public partial class SetupHomePageBannerAdPage : SecurePage, ISuperUser
  {
    #region DataItem objects

    [PageInitializer]
    private class SetupAdItem : DataItemBase
    {
      private const string GroupName = "SetupAd";
      private readonly SetupHomePageBannerAdPage ThisPage;

      private SetupAdItem(SetupHomePageBannerAdPage page) :
        base(GroupName)
      {
        ThisPage = page;
      }

      protected override string GetCurrentValue()
      {
        if (Column == "AdImageChanged" || Column == "AdImageUpdated")
          return "False"; // always unchanged from db
        return ToDisplay(BannerAds.GetColumn(BannerAds.GetColumn(Column),
          "H", Empty, Empty, Empty));
      }

      private static SetupAdItem[] GetInfo(SetupHomePageBannerAdPage page)
      {
        var setupAdInfo = new[]
        {
            new SetupAdItem(page)
            {
              Column = "AdUrl",
              Description = "Ad URL",
              Validator = ValidateWebAddressRequired
            },
            new SetupAdItem(page)
            {
              Column = "AdImageName",
              Description = "Name of Ad Image File"
            },
            new SetupAdItem(page)
            {
              Column = "AdImageChanged",
              ConvertFn = ToBool
            },
            new SetupAdItem(page)
            {
              Column = "AdEnabled",
              Description = "Ad Enabled",
              ConvertFn = ToBool
            }
          };

        foreach (var item in setupAdInfo) item.InitializeItem(page);

        InitializeGroup(page, GroupName);

        return setupAdInfo;
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(SetupHomePageBannerAdPage page)
        // ReSharper restore UnusedMember.Local
      {
        page._SetupAdDialogInfo = GetInfo(page);
      }

      protected override bool Update(object newValue)
      {
        if (Column != "AdImageChanged" && Column != "AdImageUpdated")
          BannerAds.UpdateColumn(BannerAds.GetColumn(Column), newValue,
            "H", Empty, Empty, Empty);
        return true;
      }
    }

    private SetupAdItem[] _SetupAdDialogInfo;
    #endregion DataItem objects

    private void SetupSampleAd()
    {
      SampleAd.Visible = false;
      HomePageLink.Visible = false;
      ButtonDeleteAd.Visible = false;
      if (BannerAds.AdTypeStateCodeElectionKeyOfficeKeyExists("H", Empty, Empty, Empty))
      {
        SampleAd.Visible = true;
        HomePageLink.Visible = true;
        ButtonDeleteAd.Visible = true;
        ImageLink.HRef = NormalizeUrl(BannerAds.GetAdUrl("H", Empty, Empty, Empty));
        AdImage.Src = Utility.GetAdjustedSiteUri("banneradimage", $"H....{DateTime.UtcNow.Ticks}");
        HomePageLink.HRef = Utility.GetAdjustedSiteUri(null, "ad=show");
      }
    }

    #region Event handlers and overrides

    protected void ButtonDeleteAd_OnClick(object sender, EventArgs e)
    {
      BannerAds.DeleteByAdTypeStateCodeElectionKeyOfficeKey("H", Empty, Empty, Empty);
      _SetupAdDialogInfo.LoadControls();
      SetupSampleAd();
    }

    protected void ButtonSetupAd_OnClick(object sender, EventArgs e)
    {
      _SetupAdDialogInfo.ClearValidationErrors();

      _SetupAdDialogInfo.Update(FeedbackSetupAd);

      ControlSetupAdAdImageUpdated.Text = "False";

      if (FeedbackSetupAd.ValidationErrorCount == 0)
        SetupSampleAd();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        Page.Title = "Setup Home Page Banner Ad";
        H1.InnerHtml = "Setup Home Page Banner Ad";
        _SetupAdDialogInfo.LoadControls();
        FeedbackSetupAd.AddInfo("Ad information loaded.");
        SetupSampleAd();
      }
      AdRate.InnerText =
        DB.Vote.Master.GetHomeAdRate(0)
          .ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
    }
    #endregion Event handlers and overrides
  }
}