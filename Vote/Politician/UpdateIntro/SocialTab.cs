using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB.Vote;
using Vote.Controls;

namespace Vote.Politician
{
  public partial class UpdateIntroPage
  {
    #region Private

    #region DataItem object

    [PageInitializer]
    private class SocialMediaTabItem : PoliticianDataItem
    {
      private const string GroupName = "Social";
      public HtmlAnchor IconBox;
      private HtmlGenericControl _Heading;
      private readonly SocialMedium _SocialMedium;

      private SocialMediaTabItem(UpdateIntroPage page, SocialMedium socialMedium)
        : this(page, socialMedium, socialMedium.DatabaseColumn)
      {
        _SocialMedium = socialMedium;
      }

      private SocialMediaTabItem(UpdateIntroPage page, SocialMedium socialMedium,
        string column) : base(page, GroupName)
      {
        _SocialMedium = socialMedium;
        Column = column;
        Description = socialMedium.DisplayName;
        if (socialMedium.Name == "YouTube")
          Validator = ValidateYouTubeAddress;
        else
          Validator = ValidateWebAddress;
      }

      protected override string AsteriskToolTip
      {
        get { return base.AsteriskToolTip + " address"; }
      }

      private static SocialMediaTabItem[] GetSocialMediaTabInfo(
        UpdateIntroPage page)
      {
        var list = new List<SocialMediaTabItem>
        {
          // Build dummy items for web site and email
          new SocialMediaTabItem(page,
            new SocialMedium("WebSite", "Web Site", string.Empty, string.Empty, null, true),
            "PublicWebAddress"),
          new SocialMediaTabItem(page,
            new SocialMedium("Email", "Email", string.Empty, string.Empty, null, true),
            "PublicEmail") {Validator = ValidateEmail}
        };

        // Add the existing Social Media table
        list.AddRange(
          SocialMedia.SocialMediaList.Select(
            medium => new SocialMediaTabItem(page, medium)));

        var socialMediaTabInfo = list.ToArray();
        foreach (var item in socialMediaTabInfo)
          item.InitializeItem(page);

        InitializeGroup(page, GroupName);

        return socialMediaTabInfo;
      }

      private string IconToolTip
      {
        get
        {
          var tooltip = "Click to view the " + Description;
          if (!string.IsNullOrWhiteSpace(_SocialMedium.MediumType))
            tooltip += " " + _SocialMedium.MediumType;
          return tooltip;
        }
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      private static void Initialize(UpdateIntroPage page)
        // ReSharper restore UnusedMember.Local
      {
        page._SocialMediaTabInfo = GetSocialMediaTabInfo(page);
        page.RegisterUpdateAll(page.UpdateAllSocialMedia);
        new MainTabItem {TabName = GroupName}.Initialize(page);
        if (!page.IsPostBack)
        {
          page.LoadSocialMediaTabData();
          page.RefreshSocialMediaTab();
        }
      }

      protected override void InitializeItem(TemplateControl owner,
        bool addMonitorClasses = true, FeedbackContainerControl feedback = null)
      {
        var page = owner as SecurePage;

        // ReSharper disable once BaseMethodCallWithDefaultParameter
        base.InitializeItem(page, addMonitorClasses);

        Debug.Assert(page != null, "page != null");
        _Heading =
          page.Master.FindMainContentControl("Heading" + Column) as
            HtmlGenericControl;
        IconBox =
          page.Master.FindMainContentControl("IconBox" + Column) as HtmlAnchor;

        if (_Heading != null)
          _Heading.InnerHtml = Description;

        if (IconBox != null)
        {
          IconBox.Attributes.Add("title", IconToolTip);
          IconBox.AddCssClasses("tiptip");
        }
      }

      protected override string DataControlToolTip
      {
        get { return "Enter the " + Description + " address (http:// optional)"; }
      }

      protected override bool Update(object newValue)
      {
        if (_SocialMedium.Name == "YouTube")
        {
          // ReSharper disable once PossibleNullReferenceException
          var url = (newValue as string).Trim();
          var description = string.Empty;
          var runningTime = default(TimeSpan);
          var date = DefaultDbDate;
          if (!string.IsNullOrWhiteSpace(url))
          {
            YouTubeInfo info = null;
            if (url.IsValidYouTubeVideoUrl())
            {
              var id = url.GetYouTubeVideoId();
              info = YouTubeUtility.GetVideoInfo(id, true, 1);
            }
            else if (url.IsValidYouTubePlaylistUrl())
            {
              var id = url.GetYouTubePlaylistId();
              info = YouTubeUtility.GetPlaylistInfo(id, true, 1);
            }
            else if (url.IsValidYouTubeChannelUrl() || url.IsValidYouTubeCustomChannelUrl() ||
              url.IsValidYouTubeUserChannelUrl())
            {
              var id = YouTubeUtility.LookupChannelId(url, 1);
              if (!string.IsNullOrWhiteSpace(id))
                info = YouTubeUtility.GetChannelInfo(id, true, 1);
            }
            if (info?.IsValid == true && info.IsPublic)
            {
              description = info.ShortDescription;
              runningTime = info.Duration;
              date = info.PublishedAt;
            }
          }
          Politicians.UpdateYouTubeDescription(description, Page.PoliticianKey);
          Politicians.UpdateYouTubeRunningTime(runningTime, Page.PoliticianKey);
          Politicians.UpdateYouTubeDate(date, Page.PoliticianKey);
          Politicians.UpdateYouTubeAutoDisable(string.Empty, Page.PoliticianKey);
          Politicians.UpdateYouTubeVideoVerified(false, Page.PoliticianKey);
        }
        return base.Update(newValue);
      }

      private static bool ValidateYouTubeAddress(DataItemBase item)
      {
        string message = null;
        var value = item.DataControl.GetValue();

        if (!string.IsNullOrWhiteSpace(value))
          if (value.IsValidYouTubeVideoUrl())
          {
            var id = value.GetYouTubeVideoId();
            var info = YouTubeUtility.GetVideoInfo(id, true, 1);
            if (!info.IsValid)
              message = YouTubeInfo.VideoIdNotFoundMessage;
            else if (!info.IsPublic)
              message = YouTubeInfo.VideoNotPublicMessage;
          }
          else if (value.IsValidYouTubePlaylistUrl())
          {
            var id = value.GetYouTubePlaylistId();
            var info = YouTubeUtility.GetPlaylistInfo(id, true, 1);
            if (!info.IsValid)
              message = YouTubeInfo.PlaylistIdNotFoundMessage;
            else if (!info.IsPublic)
              message = YouTubeInfo.PlaylistNotPublicMessage;
          }
          else if (value.IsValidYouTubeChannelUrl() || value.IsValidYouTubeCustomChannelUrl() ||
            value.IsValidYouTubeUserChannelUrl())
          {
            var id = YouTubeUtility.LookupChannelId(value, 1);
            if (string.IsNullOrWhiteSpace(id))
              message = YouTubeInfo.ChannelIdNotFoundMessage;
            else
            {
              var info = YouTubeUtility.GetChannelInfo(id, true, 1);
              if (!info.IsValid)
                message = YouTubeInfo.ChannelIdNotFoundMessage;
              else if (!info.IsPublic)
                message = YouTubeInfo.ChannelNotPublicMessage;
            }
          }
          else
          {
            message = "The URL is not a valid YouTube channel, playlist or video URL";
          }

        if (string.IsNullOrWhiteSpace(message))
          item.DataControl.SetValue(
            Validation.StripWebProtocol(value));
        else item.Feedback.PostValidationError(item.DataControl, message);

        return string.IsNullOrWhiteSpace(message);
      }
    }

    private SocialMediaTabItem[] _SocialMediaTabInfo;

    #endregion DataItem object

    private void LoadSocialMediaTabData()
    {
      foreach (var item in _SocialMediaTabInfo)
        item.LoadControl();
    }

    private void RefreshSocialMediaTab()
    {
      foreach (var item in _SocialMediaTabInfo)
      {
        var value = item.DataControl.GetValue()
          .Trim();
        if (string.IsNullOrWhiteSpace(value))
        {
          item.IconBox.HRef = string.Empty;
          item.IconBox.AddCssClasses("disabled");
        }
        else
        {
          item.IconBox.HRef = NormalizeUrl(value);
          item.IconBox.RemoveCssClass("disabled");
        }
      }
    }

    private int UpdateAllSocialMedia(bool showSummary)
    {
      _SocialMediaTabInfo.ClearValidationErrors();
      var errorCount = _SocialMediaTabInfo.Update(FeedbackSocial, showSummary,
        UpdatePanelSocial);
      InvalidatePageCache();
      RefreshSocialMediaTab();
      // because email is on both
      //LoadContactTabData();
      //UpdatePanelContact.Update();
      return errorCount;
    }

    #endregion Private

    #region Event handlers and overrides

    protected void ButtonSocial_OnClick(object sender, EventArgs e)
    {
      try
      {
        // major kludge to support eamil appearing twice
        var oldEmail = Politicians.GetPublicEmail(PoliticianKey);
        UpdateAllSocialMedia(true);
        var newEmail = Politicians.GetPublicEmail(PoliticianKey);
        if (oldEmail != newEmail)
        {
          LoadContactTabData();
          UpdatePanelContact.Update();
        }
      }
      catch (Exception ex)
      {
        FeedbackSocial.HandleException(ex);
      }
    }

    #endregion Event handlers and overrides
  }
}