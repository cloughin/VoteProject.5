using System;

namespace Vote
{
  // Public methods and properties relation to politicians
  public partial class VotePage
  {
    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global

    public static string GetPoliticianImageUrl(
      string politicianKey, string size, bool noCache = false)
    {
      var url = string.Format(
        "/Image.aspx?Id={0}&Col={1}&Def={1}", politicianKey, size);
      if (noCache)
        url = InsertNoCacheIntoUrl(url);
      return url;
    }

    public static string GetPoliticianImageUrl(
      string politicianKey, int width, bool noCache = false)
    {
      var columnName = ImageManager.GetPoliticianImageColumnNameByWidth(width);
      if (string.IsNullOrWhiteSpace(columnName))
        throw new ArgumentException("Invalid width");
      return GetPoliticianImageUrl(politicianKey, columnName, noCache);
    }


    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }
}