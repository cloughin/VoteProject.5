using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace Vote
{
  //public class OldCacheablePage : PublicPage
  //{
  //  #region Private

  //  private static string InsertNoCacheIntoRenderedPage(string renderedPage)
  //  {
  //    // to add an additional nocache page, add it to the regex in this
  //    // method and override its SuppressCaching property
  //    renderedPage = Regex.Replace(
  //      renderedPage, @"/(?:image|intro|politicianissue).aspx\?",
  //      match => InsertNoCacheIntoUrl(match.Value), RegexOptions.IgnoreCase);
  //    return renderedPage;
  //  }

  //  #endregion Private

  //  #region Protected

  //  // ReSharper disable MemberCanBePrivate.Global
  //  // ReSharper disable VirtualMemberNeverOverriden.Global

  //  protected virtual string GetCacheImage()
  //  {
  //    return null;
  //  }

  //  protected virtual void SaveCacheImage(string cacheImage)
  //  {
  //    // Empty stub
  //  }

  //  protected virtual bool SuppressCaching { get { return false; } }

  //  protected static bool UrlContainsNoCache { get { return GetQueryString(NoCacheParameter) == "1"; } }

  //  // ReSharper restore VirtualMemberNeverOverriden.Global
  //  // ReSharper restore MemberCanBePrivate.Global

  //  #endregion Protected

  //  #region Event handlers and overrides

  //  protected override void OnPreInit(EventArgs e)
  //  {
  //    base.OnPreInit(e);
  //    if (!MemCache.IsCachingPages || SuppressCaching) return;
  //    var cacheImage = GetCacheImage();
  //    if (string.IsNullOrEmpty(cacheImage)) return;
  //    Response.Write(cacheImage);
  //    Response.End();
  //  }

  //  protected override void Render(HtmlTextWriter writer)
  //  {
  //    if (MemCache.IsCachingPages || SuppressCaching)
  //    {
  //      // Render the page to a string
  //      var stringWriter = new StringWriter();
  //      var textWriter = new HtmlTextWriter(stringWriter);
  //      base.Render(textWriter);
  //      var renderedPage = stringWriter.ToString();
  //      if (SuppressCaching)
  //        // insert the no-cache code into all cached images
  //        renderedPage = InsertNoCacheIntoRenderedPage(renderedPage);
  //      else
  //        SaveCacheImage(renderedPage);
  //      writer.Write(renderedPage);
  //    }
  //    else
  //      base.Render(writer);
  //  }

  //  #endregion Event handlers and overrides
  //}
}