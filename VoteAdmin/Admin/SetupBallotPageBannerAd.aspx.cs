using System;
using System.Diagnostics;
using System.Globalization;
using System.Web.UI.HtmlControls;
using DB.Vote;
using Vote;
using static System.String;

namespace VoteAdmin.Admin
{
  [PageInitializers]
  public partial class SetupBallotPageBannerAdPage : SecureAdminPage, ISuperUser, IAllowEmptyStateCode
  {
    #region DataItem objects

    [PageInitializer]
    private class SetupAdItem : DataItemBase
    {
      private const string GroupName = "SetupAd";
      private readonly SetupBallotPageBannerAdPage ThisPage;

      private SetupAdItem(SetupBallotPageBannerAdPage page) :
        base(GroupName)
      {
        ThisPage = page;
      }

      protected override string GetCurrentValue()
      {
        if (Column == "AdImageChanged" || Column == "AdImageUpdated")
          return "False"; // always unchanged from db
        return ToDisplay(BannerAds.GetColumn(BannerAds.GetColumn(Column),
          "B", ThisPage.StateCode, Empty, Empty));
      }

      private static SetupAdItem[] GetInfo(SetupBallotPageBannerAdPage page)
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
      internal static void Initialize(SetupBallotPageBannerAdPage page)
      // ReSharper restore UnusedMember.Local
      {
        page._SetupAdDialogInfo = GetInfo(page);
      }

      protected override bool Update(object newValue)
      {
        if (Column != "AdImageChanged" && Column != "AdImageUpdated")
          BannerAds.UpdateColumn(BannerAds.GetColumn(Column), newValue,
            "B", ThisPage.StateCode, Empty, Empty);
        return true;
      }
    }

    private SetupAdItem[] _SetupAdDialogInfo;
    #endregion DataItem objects

    private void SetupSampleAd()
    {
      SampleAd.Visible = false;
      ButtonDeleteAd.Visible = false;
      if (BannerAds.AdTypeStateCodeElectionKeyOfficeKeyExists("B", StateCode, Empty, Empty))
      {
        SampleAd.Visible = true;
        ButtonDeleteAd.Visible = true;
        ImageLink.HRef = NormalizeUrl(BannerAds.GetAdUrl("B", StateCode, Empty, Empty));
        AdImage.Src = Utility.GetAdjustedSiteUri("banneradimage", $"B.{StateCode}...{DateTime.UtcNow.Ticks}");
      }
    }

    #region Event handlers and overrides

    protected void ButtonDeleteAd_OnClick(object sender, EventArgs e)
    {
      BannerAds.DeleteByAdTypeStateCodeElectionKeyOfficeKey("B", StateCode, Empty, Empty);
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
        Page.Title = "Setup Ballot Page Banner Ad";
        H1.InnerHtml = "Setup Ballot Page Banner Ad";
        _SetupAdDialogInfo.LoadControls();
        FeedbackSetupAd.AddInfo("Ad information loaded.");
        SetupSampleAd();
      }

      var body = Master.FindControl("body") as HtmlGenericControl;
      Debug.Assert(body != null, "body != null");
      body.Attributes.Add("data-state", StateCode);

      AdRate.InnerText =
        DB.Vote.Master.GetBallotAdRate(0)
          .ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
      State.InnerText = StateCache.GetStateName(StateCode.SafeString());

      if (AdminPageLevel == AdminPageLevel.Unknown)
      {
        UpdateControls.Visible = false;
        NoJurisdiction.CreateStateLinks("/admin/SetupBallotPageBannerAd.aspx?state={StateCode}");
        NoJurisdiction.Visible = true;
      }
    }
    #endregion Event handlers and overrides
  }
}