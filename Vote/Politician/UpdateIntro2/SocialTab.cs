using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Vote.Controls;

namespace Vote.Politician
{
  public partial class UpdateIntro2Page
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

      private SocialMediaTabItem(UpdateIntro2Page page, SocialMedium socialMedium)
        : this(page, socialMedium, socialMedium.DatabaseColumn)
      {
        _SocialMedium = socialMedium;
      }

      private SocialMediaTabItem(UpdateIntro2Page page, SocialMedium socialMedium,
        string column) : base(page, GroupName)
      {
        _SocialMedium = socialMedium;
        Column = column;
        Description = socialMedium.DisplayName;
        Validator = ValidateWebAddress;
      }

      protected override string AsteriskToolTip { get { return base.AsteriskToolTip + " address"; } }

      private static SocialMediaTabItem[] GetSocialMediaTabInfo(
        UpdateIntro2Page page)
      {
        var list = new List<SocialMediaTabItem>
          {
            // Build a dummy item for web site
            new SocialMediaTabItem(page,
              new SocialMedium("WebSite", "Web Site", string.Empty, string.Empty, null, true),
              "PublicWebAddress")
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
      private static void Initialize(UpdateIntro2Page page)
        // ReSharper restore UnusedMember.Local
      {
        page._SocialMediaTabInfo = GetSocialMediaTabInfo(page);
        page.RegisterUpdateAll(page.UpdateAllSocialMedia);
        new MainTabItem { TabName = GroupName }.Initialize(page);
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

      protected override string DataControlToolTip { get { return "Enter the " + Description + " address (http:// optional)"; } }
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
      return errorCount;
    }

    #endregion Private

    #region Event handlers and overrides

    protected void ButtonSocial_OnClick(object sender, EventArgs e)
    {
      try
      {
        UpdateAllSocialMedia(true);
      }
      catch (Exception ex)
      {
        FeedbackSocial.HandleException(ex);
      }
    }

    #endregion Event handlers and overrides
  }
}