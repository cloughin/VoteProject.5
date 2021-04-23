using System;
using System.Globalization;
using DB.Vote;
using Vote;
using Vote.Master;
using static System.String;

namespace VoteAdmin.Admin
{
  [PageInitializers]
  public partial class SetUpComparePageBannerAdPage : SecureAdminPage, ISuperUser
  {
    #region DataItem objects

    [PageInitializer]
    private class SetupAdItem : DataItemBase
    {
      private const string GroupName = "SetupAd";
      private readonly SetUpComparePageBannerAdPage ThisPage;

      private SetupAdItem(SetUpComparePageBannerAdPage page) :
        base(GroupName)
      {
        ThisPage = page;
      }

      protected override string GetCurrentValue()
      {
        if (Column == "AdImageChanged" || Column == "AdImageUpdated")
          return "False"; // always unchanged from db
        return ToDisplay(BannerAds.GetColumn(BannerAds.GetColumn(Column),
          "C", Empty, QueryElection, QueryOffice));
      }

      private static SetupAdItem[] GetInfo(SetUpComparePageBannerAdPage page)
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
      internal static void Initialize(SetUpComparePageBannerAdPage page)
      // ReSharper restore UnusedMember.Local
      {
        page._SetupAdDialogInfo = GetInfo(page);
      }

      protected override bool Update(object newValue)
      {
        if (Column != "AdImageChanged" && Column != "AdImageUpdated")
          BannerAds.UpdateColumn(BannerAds.GetColumn(Column), newValue,
            "C", Empty, QueryElection, QueryOffice);
        return true;
      }
    }

    private SetupAdItem[] _SetupAdDialogInfo;
    #endregion DataItem objects

    private void SetupSampleAd()
    {
      SampleAd.Visible = false;
      ComparePageLink.Visible = false;
      ButtonDeleteAd.Visible = false;
      if (BannerAds.AdTypeStateCodeElectionKeyOfficeKeyExists("C", Empty, QueryElection, QueryOffice))
      {
        SampleAd.Visible = true;
        ComparePageLink.Visible = true;
        ButtonDeleteAd.Visible = true;
        ImageLink.HRef = NormalizeUrl(BannerAds.GetAdUrl("C", Empty, QueryElection, QueryOffice));
        AdImage.Src = Utility.GetAdjustedSiteUri("banneradimage", $"C..{QueryElection}.{QueryOffice}.{DateTime.UtcNow.Ticks}");
        ComparePageLink.HRef =
          Utility.GetAdjustedSiteUri("CompareCandidates.aspx",
            $"election={QueryElection}&office={QueryOffice}");
      }
    }

    #region Event handlers and overrides

    protected void ButtonDeleteAd_OnClick(object sender, EventArgs e)
    {
      BannerAds.DeleteByAdTypeStateCodeElectionKeyOfficeKey("C", Empty, QueryElection, QueryOffice);
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
        Page.Title = "Setup Compare Page Banner Ad";
        H1.InnerHtml = "Setup Compare Page Banner Ad";
        _SetupAdDialogInfo.LoadControls();
        FeedbackSetupAd.AddInfo("Ad information loaded.");
        SetupSampleAd();
      }

      var election = Elections.GetElectionDesc(QueryElection);
      var officeTable = Offices.GetCacheData(QueryOffice);
      var office = officeTable.Count > 0 ? Offices.GetLocalizedOfficeName(officeTable[0]) : null;
      if (IsNullOrWhiteSpace(election) || IsNullOrWhiteSpace(office))
      {
        Response.StatusCode = 404;
        return;
      }

      Election.InnerText = election;
      Office.InnerText = office;
      AdRate.InnerText =
        DB.Vote.Master.GetContestAdRate(0)
          .ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
    }
    #endregion Event handlers and overrides
  }
}