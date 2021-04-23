using System;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB.Vote;
using DB.VoteLog;
using static System.String;

namespace Vote.Controls
{
  public partial class ManagePoliticiansPanel
  {
    #region DataItem objects

    private class SetupAdDialogItem : DataItemBase
    {
      private const string GroupName = "SetupAd";
      private readonly ManagePoliticiansPanel ThisControl;

      private SetupAdDialogItem(ManagePoliticiansPanel thisControl) :
        base(GroupName)
      {
        ThisControl = thisControl;
      }

      protected override string GetCurrentValue()
      {
        if (Column == "AdImageChanged" || Column == "AdImageUpdated")
          return "False"; // always unchanged from db
        return ToDisplay(ElectionsPoliticians.GetColumn(ElectionsPoliticians.GetColumn(Column),
          ThisControl.SafeGetElectionKey(), ThisControl.SafeGetOfficeKey(), 
          ThisControl.AdSetupCandidate.Value));
      }

      public static SetupAdDialogItem[] GetDialogInfo(ManagePoliticiansPanel control)
      {
        var setupAdInfo = new [] 
        {
          new SetupAdDialogItem(control)
          {
            Column = "AdType",
            Description = "Ad Type (YouTube or Image)",
            Validator = @base =>
            {
              if (!ValidateRequired(@base)) return false;
              var politicianKey = control.AdSetupCandidate.Value;
              if (IsNullOrWhiteSpace(Politicians.GetPublicWebAddress(politicianKey)))
              {
                var link = SecurePage.IsPoliticianUser
                  ? SecurePoliticianPage.GetPoliticianFolderPageUrl("updateintro")
                  : SecurePoliticianPage.GetUpdateIntroPageUrl(politicianKey);
                {
                  control.FeedbackSetupAd.PostValidationError(@base.DataControl,
                    "An Image ad needs a link to navigate to when clicked." +
                    $" <a href=\"{link}\" target=\"intro\">Please enter a link for" +
                    " your web site or an equivalent page</b>, like your" +
                    " Facebook page.</a>");
                  return false;
                }
              }

              return true;
            }
            //Validator = ValidateRequired
          },
          new SetupAdDialogItem(control)
          {
            Column = "AdUrl",
            Description = "Video or Channel URL",
            Validator = b =>
            {
              // only required for type "Y"
              var adTypeItem = control._SetupAdDialogInfo.First(i => i.Column == "AdType");
              return adTypeItem.DataControl.GetValue() != "Y" || 
                ValidateYouTubeAddressRequired(b);
            }
          },
          new SetupAdDialogItem(control)
          {
            Column = "AdThumbnailUrl",
            Description = "Video for Channel Thumbnail",
            Validator = ValidateYouTubeVideoAddressOptional
          },
          new SetupAdDialogItem(control)
          {
            Column = "AdImageName",
            Description = "Name of Ad Image File"
          },
          new SetupAdDialogItem(control)
          {
            Column = "AdImageChanged",
            ConvertFn = ToBool
          },
          new SetupAdDialogItem(control)
          {
            Column = "AdImageUpdated",
            Description = "Ad Image",
            ConvertFn = ToBool
          },
          new SetupAdDialogItem(control)
          {
            Column = "AdTimeStamp",
            Description = "Ad Date for Sorting",
            ConvertFn = ToDateTime,
            Validator = ValidateDateOptional
          },
          new SetupAdDialogItem(control)
          {
            Column = "AdEnabled",
            Description = "Ad Enabled",
            ConvertFn = ToBool
          },
          new SetupAdDialogItem(control)
          {
            Column = "AdSponsor",
            Description = "Ad Sponsor",
            Validator = @base =>
            {
              // if there is a sponsor URL, there must be either an ad sponsor or the checkbox must be checked
              var sponsorUrlItem = control._SetupAdDialogInfo.First(i => i.Column == "AdSponsorUrl");
              if (!IsNullOrWhiteSpace(sponsorUrlItem.DataControl.GetValue()))
              {
                var checkboxItem = control._SetupAdDialogInfo.First(i => i.Column == "AdIsCandidateSponsored");
                if (IsNullOrWhiteSpace(@base.DataControl.GetValue()) &&
                  checkboxItem.DataControl.GetValue().Equals("false", StringComparison.OrdinalIgnoreCase))
                {
                  control.FeedbackSetupAd.PostValidationError(@base.DataControl, 
                    "Since you provided a Sponsor URL you need to either enter a" +
                    " Ad Sponsor or check the 'Use candidate's campaigm as Ad Sponsor'" +
                    " checkbox.");
                  return false;
                }
              }
              return true;
            }
          },
          new SetupAdDialogItem(control)
          {
            Column = "AdSponsorUrl",
            Description = "Ad Sponsor URL"
          },
          new SetupAdDialogItem(control)
          {
            Column = "AdIsCandidateSponsored",
            Description = "Ad Is Candidate Sponsored",
            ConvertFn = ToBool
          }
        };

        foreach (var item in setupAdInfo) item.InitializeItem(control);

        InitializeGroup(control, GroupName);

        return setupAdInfo;
      }

      protected override void Log(string oldValue, string newValue)
      {
        if (Column != "AdImageChanged" && Column != "AdImageUpdated")
          LogDataChange.LogUpdate(Politicians.TableName, Column, oldValue, newValue,
            VotePage.UserName, SecurePage.UserSecurityClass, DateTime.UtcNow,
            ThisControl.GetPoliticianKeyToEdit().Key);
      }

      protected override bool Update(object newValue)
      {
        if (Column != "AdImageChanged" && Column != "AdImageUpdated")
          ElectionsPoliticians.UpdateColumn(ElectionsPoliticians.GetColumn(Column), newValue,
            ThisControl.SafeGetElectionKey(), ThisControl.SafeGetOfficeKey(),
            ThisControl.AdSetupCandidate.Value);
        return true;
      }
    }

    private SetupAdDialogItem[] _SetupAdDialogInfo;

    private void SetupSampleAd(string electionKey, string officeKey, string politicianKey,
      string videoUrl = null)
    {
      SampleAd.Visible = false;
      VideoWrapper.Visible = false;
      ImageWrapper.Visible = false;

      var adTypeItem = _SetupAdDialogInfo.First(i => i.Column == "AdType");

      switch (adTypeItem.DataControl.GetValue())
      {
        case "Y": // YouTube
          VideoWrapper.Visible = true;
          if (IsNullOrWhiteSpace(videoUrl))
          {
            var urlItem = _SetupAdDialogInfo.First(i => i.Column == "AdUrl");
            videoUrl = urlItem.DataControl.GetValue();
          }
          if (IsNullOrWhiteSpace(videoUrl)) return;
          string thumbId;
          if (videoUrl.IsValidYouTubeVideoUrl())
            thumbId = videoUrl.GetYouTubeVideoId();
          else
          {
            var thumbItem = _SetupAdDialogInfo.First(i => i.Column == "AdThumbnailUrl");
            var thumbUrl = thumbItem.DataControl.GetValue();
            if (IsNullOrWhiteSpace(thumbUrl)) return;
            thumbId = thumbUrl.GetYouTubeVideoId();
          }

          if (IsNullOrWhiteSpace(thumbId)) return;
          SampleThumb.Src = $"http://i.ytimg.com/vi/{thumbId}/hqdefault.jpg";
          break;

        case "I": // Image
          ImageWrapper.Visible = true;
          var website = Politicians.GetPublicWebAddress(politicianKey);
          //if (IsNullOrWhiteSpace(website)) return;
          //ImageLink.HRef = VotePage.NormalizeUrl(website);
          //AdImage.Src = UrlManager
          //  .GetSiteUri("adimage", $"{electionKey}.{officeKey}.{politicianKey}.{DateTime.UtcNow.Ticks}").ToString();
          if (IsNullOrWhiteSpace(website))
            ImageLink.Attributes.Remove("href");
          else
            ImageLink.HRef = VotePage.NormalizeUrl(website);
          AdImage.Src = UrlManager
            .GetSiteUri("adimage", $"{electionKey}.{officeKey}.{politicianKey}.{DateTime.UtcNow.Ticks}").ToString();
          break;
      }

      var sponsor = _SetupAdDialogInfo.First(i => i.Column == "AdSponsor").DataControl.GetValue();
      var sponsorUrl = _SetupAdDialogInfo.First(i => i.Column == "AdSponsorUrl").DataControl.GetValue();
      var isCandidateSponsored = _SetupAdDialogInfo.First(i => i.Column == "AdIsCandidateSponsored").DataControl.GetValue() == "True";

      var paidMessage = new PlaceHolder();
      if (isCandidateSponsored)
      {
        var url = IsNullOrWhiteSpace(sponsorUrl)
          ? SecurePoliticianPage.GetPoliticianPublicPageUrl("intro", politicianKey)
          : VotePage.NormalizeUrl(sponsorUrl);
        new LiteralControl("Paid advertisement by ").AddTo(paidMessage);
        new HtmlBreak().AddTo(paidMessage);
        new HtmlAnchor
        {
          HRef = url,
          Target = "_blank",
          InnerHtml = "candidate&rsquo;s campaign"
        }.AddTo(paidMessage);
      }
      else if (!IsNullOrWhiteSpace(sponsor))
      {
        if (IsNullOrWhiteSpace(sponsorUrl))
        {
          new LiteralControl("Paid advertisement by ").AddTo(paidMessage);
          new HtmlBreak().AddTo(paidMessage);
          new LiteralControl($"{sponsor}").AddTo(paidMessage);
        }
        else
        {
          new LiteralControl("Paid advertisement by ").AddTo(paidMessage);
          new HtmlBreak().AddTo(paidMessage);
          new HtmlAnchor
          {
            HRef = VotePage.NormalizeUrl(sponsorUrl),
            Target = "_blank",
            InnerText = sponsor
          }.AddTo(paidMessage);
        }
      }
      else
        new LiteralControl("Paid advertisement").AddTo(paidMessage);

      SampleAd.Visible = true;
      SampleName.InnerText = PageCache.GetTemporary().Politicians
        .GetPoliticianName(politicianKey);
      SampleProfile.Src =
        VotePage.GetPoliticianImageUrl(politicianKey, "Headshot100");
      PaidAdvertisement.Controls.Clear();
      PaidAdvertisement.Controls.Add(paidMessage);
      ComparePageLink.HRef = $"{UrlManager.GetCompareCandidatesPageUri(electionKey, officeKey)}" +
        $"&ad={politicianKey}";

    }

    #endregion DataItem objects

    #region Event handlers and overrides

    protected void ButtonSetupAd_OnClick(object sender, EventArgs e)
    {
      var politicianKey = AdSetupCandidate.Value;
      var electionKey = SafeGetElectionKey();
      var officeKey = SafeGetOfficeKey();
      CandidateAdName.InnerText = PageCache.GetTemporary().Politicians
        .GetPoliticianName(politicianKey);
      CandidateAdOffice.InnerText = Offices.FormatOfficeName(officeKey);
      CandidateAdElection.InnerText = PageCache.GetTemporary().Elections
        .GetElectionDesc(electionKey);
      CandidateAdRate.InnerText =
        Offices.GetAdRate(electionKey, officeKey)
          .ToString("C", CultureInfo.CreateSpecificCulture("en-US"));

      switch (SetupAdReloading.Value)
      {
        case "reloading":
          {
            SetupAdReloading.Value = Empty; 
            _SetupAdDialogInfo.LoadControls();
            FeedbackSetupAd.AddInfo("Ad information loaded.");
            var urlItem = _SetupAdDialogInfo.First(i => i.Column == "AdUrl");
            var videoUrl = urlItem.DataControl.GetValue();
            if (IsNullOrWhiteSpace(videoUrl))
            {
              videoUrl = DefaultAdVideo.Value = PageCache.GetTemporary().Politicians
                .GetYouTubeWebAddress(politicianKey);
            }
            else
              DefaultAdVideo.Value = Empty;
            SetupSampleAd(electionKey, officeKey, politicianKey, videoUrl);
          }
          break;

        case "":
          {
            // normal update
            _SetupAdDialogInfo.ClearValidationErrors();
            DefaultAdVideo.Value = Empty;

            // handle default date
            var dateItem = _SetupAdDialogInfo.First(i => i.Column == "AdTimeStamp");
            if (IsNullOrWhiteSpace(dateItem.DataControl.GetValue()))
              dateItem.DataControl.SetValue(DateTime.UtcNow.ToShortDateString());

            var adTypeItem = _SetupAdDialogInfo.First(i => i.Column == "AdType");
            var urlItem = _SetupAdDialogInfo.First(i => i.Column == "AdUrl");
            var thumbItem = _SetupAdDialogInfo.First(i => i.Column == "AdThumbnailUrl");
            switch (adTypeItem.DataControl.GetValue())
            {
              case "Y": // YouTube
                // make sure we have a thumbnail video if main URL is not a video
                var videoUrl = urlItem.DataControl.GetValue();
                if (!videoUrl.IsValidYouTubeVideoUrl())
                {
                  var thumbUrl = thumbItem.DataControl.GetValue();
                  if (IsNullOrWhiteSpace(thumbUrl))
                    FeedbackSetupAd.PostValidationError(thumbItem.DataControl,
                      "Thumbnail URL is required for channels or playlists");
                }
                // clear the image name
                var adImageNameItem =
                  _SetupAdDialogInfo.First(i => i.Column == "AdImageName");
                adImageNameItem.DataControl.SetValue(Empty);
                break;

              case "I": // image
                // clear the video and thumbnail
                urlItem.DataControl.SetValue(Empty);
                thumbItem.DataControl.SetValue(Empty);
                break;
            }

            _SetupAdDialogInfo.Update(FeedbackSetupAd);

            ControlSetupAdAdImageUpdated.Text = "False";

            if (FeedbackSetupAd.ValidationErrorCount == 0)
            {
              SetupSampleAd(electionKey, officeKey, politicianKey);
            }
          }
          break;

        default:
          throw new VoteException(
            $"Unknown reloading option: '{EditPoliticianReloading.Value}'");
      }
    }

    #endregion Event handlers and overrides
  }
}