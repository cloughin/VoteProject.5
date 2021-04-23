using System;
using System.Globalization;
using DB.VoteCache;

namespace Vote.Master
{
  public partial class CacheControlPage : SecurePage, ISuperUser
  {
    #region Private

    private void SetAllCacheLabels()
    {
      var cacheRemovedAll = CacheControl.GetWhenCleared();
      if (cacheRemovedAll != null)
        LabelAllPagesLastRemoved.Text = cacheRemovedAll.Value.DbDateToShortDateTime(true);
      LabelAllPagesCurrent.Text = CachePages.CountTable()
        .ToString(CultureInfo.InvariantCulture);
    }

    #endregion Private

    #region Event handlers and overrides

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        Page.Title = "Page Cache Control";
        H1.InnerHtml = "Page Cache Control";
        SetAllCacheLabels();
        RadioCacheExpiration.SelectedValue = CacheControl.GetExpiration(0)
          .ToString(CultureInfo.InvariantCulture);
      }
    }

    protected void ButtonRemoveAllPages_Click(object sender, EventArgs e)
    {
      try
      {
        CommonCacheInvalidation.ScheduleInvalidateAll();
        SetAllCacheLabels();
        PageCachingFeedback.AddInfo("All Cached Pages will be cleared in 5 to 10 minutes.");
      }
      catch (Exception ex)
      {
        PageCachingFeedback.HandleException(ex);
      }
    }

    protected void RadioCacheExpiration_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        var expiration = int.Parse(RadioCacheExpiration.SelectedValue);
        CacheControl.UpdateExpiration(expiration);
        CacheExpirationFeedback.AddInfo(
          $"Cache expiration has been set to {expiration} minutes.");
      }
      catch (Exception ex)
      {
        CacheExpirationFeedback.HandleException(ex);
      }
    }

    #endregion Event handlers and overrides
  }
}