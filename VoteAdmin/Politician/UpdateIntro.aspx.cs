using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB.Vote;
using DB.VoteCache;
using Vote.Controls;

namespace Vote.Politician
{
  [PageInitializers]
  public partial class UpdateIntroPage : SecurePoliticianPage
  {
    #region class MainTabItem

    private class MainTabItem
    {
      internal string TabName;
      internal string TabAsteriskToolTip;
      internal string ButtonToolTip; // if null, will build one from the tab label
      private HtmlAnchor _TabLabel;
      private Control _Button;
      private HtmlInputHidden _DescriptionTag;

      internal void Initialize(SecurePage page)
      {
        _TabLabel =
          page.Master.FindMainContentControl("TabMain" + TabName) as HtmlAnchor;
        _Button = page.Master.FindMainContentControl("Button" + TabName);
        _DescriptionTag =
          page.Master.FindMainContentControl("Description" + TabName) as
            HtmlInputHidden;

        Debug.Assert(_TabLabel != null, "_TabLabel != null");
        var li = _TabLabel.Parent as HtmlGenericControl;
        var astDiv = new HtmlDiv();
        astDiv.Attributes.Add("class", "tab-ast tiptip");
        if (TabAsteriskToolTip != null) astDiv.Attributes.Add("title", TabAsteriskToolTip);
        else
          astDiv.Attributes.Add("title",
            "There are unsaved changes on the " + _TabLabel.InnerText + " tab");
        astDiv.AddTo(li);
        if (_Button != null)
          if (ButtonToolTip != null) _Button.SetToolTip(ButtonToolTip);
          else _Button.SetToolTip("Update " + _TabLabel.InnerText);
        if (_DescriptionTag != null) _DescriptionTag.Value = _TabLabel.InnerText;
      }
    }

    #endregion class MainTabItem

    #region class PoliticianDataItem

    private abstract class PoliticianDataItem : DataItemBase
    {
      protected readonly UpdateIntroPage Page;

      protected PoliticianDataItem(UpdateIntroPage page, string groupName)
        : base(groupName)
      {
        Page = page;
      }

      protected override string GetCurrentValue()
      {
        return Politicians.GetColumnExtended(Column, Page.PoliticianKey);
      }

      protected override void IncrementUpdateCount()
      {
        Page._UpdateCount++;
      }

      protected override void Log(string oldValue, string newValue)
      {
        Page.LogPoliticiansDataChange(Column, oldValue, newValue);
      }

      protected override bool Update(object newValue)
      {
        Politicians.UpdateColumnExtended(Column, newValue, Page.PoliticianKey);
        return true;
      }
    }

    #endregion class PoliticianDataItem

    private readonly List<Func<bool, int>> _UpdateAllList =
      new List<Func<bool, int>>();

    #region Private Members

    private int _UpdateCount;

    private void RegisterUpdateAll(Func<bool, int> updateAll)
    {
      _UpdateAllList.Add(updateAll);
    }

    private void UpdateAll(FeedbackContainerControl feedback)
    {
      try
      {
        var errorCount = 0;

        var oldEmail = Politicians.GetPublicEmail(PoliticianKey);
        foreach (var updateAll in _UpdateAllList) errorCount += updateAll(false);
        var newEmail = Politicians.GetPublicEmail(PoliticianKey);
        if (oldEmail != newEmail)
        {
          LoadContactTabData();
          UpdatePanelContact.Update();
          LoadSocialMediaTabData();
          UpdatePanelSocial.Update();
        }

        feedback.AddInfo(_UpdateCount.ToString(CultureInfo.InvariantCulture) +
          _UpdateCount.Plural(" item was", " items were") + " updated.");
        if (errorCount > 0)
          feedback.AddError(errorCount.ToString(CultureInfo.InvariantCulture) +
            errorCount.Plural(" item was", " items were") +
            " not updated due to errors.");
      }
      catch (Exception ex)
      {
        feedback.HandleException(ex);
      }
    }

    #endregion Private Members

    #region Event Handlers

    protected void Page_Load(object sender, EventArgs e)
    {
      _UpdateCount = 0;

      if (UserSecurityClass == PoliticianSecurityClass)
        PodcastEntry.AddCssClasses("hidden");

      if (!IsPostBack)
      {
        var title = PoliticianName + " - Update Intro";
        Page.Title = title;
        H2.InnerHtml = OfficeAndStatus;
        ShowIntroLink.Attributes["href"] = IntroPageUrl;
        //UpdateIssuesLink.Attributes["href"] = UpdateIssuesPageUrl;
        //ShowPoliticianIssueLink.Attributes["href"] = PoliticianIssuePageUrl;

        // Set temp nocache
        SetNoCacheForState();

        // mark if birthday is needed (politician access only)
        if (UserSecurityClass == PoliticianSecurityClass &&
          string.IsNullOrWhiteSpace(Politicians.GetDateOfBirthAsString(PoliticianKey)))
          Master.FindControl("Body").AddCssClasses("need-dob");
      }
    }

    protected override void OnPreRender(EventArgs e)
    {
      base.OnPreRender(e);

      // Make sure this reflects changes
      InvalidatePageCache();
      var heading = PageCache.Politicians.GetPoliticianName(PoliticianKey);
      if (heading != H1.InnerHtml)
      {
        H1.InnerHtml = heading;
        UpdatePanelHeading.Update();
      }
      // This isn't available at Page_Load time
      if (PageCachingSubHeadingWithHelp.FindTemplateControl("ViewIntroLink") is HtmlAnchor
        viewIntroLink) viewIntroLink.HRef = IntroPageUrl;

      if (PageCachingSubHeadingWithHelp.FindTemplateControl("CacheExpirationMsg") is
        HtmlGenericControl cacheExpirationMsg)
        cacheExpirationMsg.InnerText =
          CachePages.DisplayExpiration(MemCache.CacheExpiration);

      // We handle post-update processing here so we only have to do it once per postback,
      // not on each field.
      if (_UpdateCount <= 0) return;
      Politicians.IncrementDataUpdatedCount(PoliticianKey);
    }

    protected void ButtonUpdateAll_OnClick(object sender, EventArgs e)
    {
      try
      {
        UpdateAll(FeedbackUpdateAll);
      }
      catch (Exception ex)
      {
        FeedbackUpdateAll.HandleException(ex);
      }
    }

    #endregion Event Handlers
  }
}