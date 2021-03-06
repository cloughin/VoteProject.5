using System;
using System.IO;
using System.Web;
using DB.Vote;

namespace Vote
{
  // These classes might be reorganized and perhaps renamed later.
  //
  public static class DomainDesign
  {
    private const string ImagePathStringTemplate = "/images/designs/{0}/";

    // We cache these values so we aren't hitting the db all the time
    private static bool? _IncludeMetaDescriptionForAllStates;
    private static bool? _IncludeMetaDescriptionForSingleState;
    //private static bool? _IncludeMetaKeywordsForAllStates;
    //private static bool? _IncludeMetaKeywordsForSingleState;
    private static bool? _IncludeTitleForAllStates;
    private static bool? _IncludeTitleForSingleState;

    private static string FromQueryStringOrDomain
    {
      get
      {
        var designCode = HttpContext.Current.Request.QueryString["Design"];
        return string.IsNullOrWhiteSpace(designCode)
          ? UrlManager.CurrentDomainDesignCode
          : designCode.ToUpperInvariant();
      }
    }

    public static Uri GetImageUri(string imageName)
    {
      return GetImageUri(FromQueryStringOrDomain, imageName);
    }

    private static Uri GetImageUri(string designCode, string imageName)
    {
      return UrlManager.GetDomainDesignCodeUri(
        designCode, ImagePath(designCode, imageName));
    }

    //public static string GetSubstitutionText(string textKey)
    //{
    //  // There may be more caching opportunities here
    //  return db.Subsitutions_All(db.Design_This(textKey));
    //}

    public static string GetSubstitutionText(string textKey)
    {
      // There may be more caching opportunities here
      //return db.Subsitutions_All(cache, db.Design_This(textKey));
      return DomainDesigns.GetDesignStringWithSubstitutions(textKey);
    }

    private static string ImagePath(string designCode, string imageName)
    {
      var folder = string.Format(ImagePathStringTemplate, designCode);
      return Path.Combine(folder, imageName);
    }

    public static bool IncludeMetaDescriptionForAllStates
    {
      get
      {
        if (_IncludeMetaDescriptionForAllStates == null)
          _IncludeMetaDescriptionForAllStates =
            DomainDesigns
              .GetIsIncludedMetaDescriptionTagDefaultPageAllStatesDomainByDomainDesignCode
              (FromQueryStringOrDomain, false);
        return _IncludeMetaDescriptionForAllStates.Value;
      }
    }

    public static bool IncludeMetaDescriptionForSingleState
    {
      get
      {
        if (_IncludeMetaDescriptionForSingleState == null)
          _IncludeMetaDescriptionForSingleState =
            DomainDesigns
              .GetIsIncludedMetaDescriptionTagDefaultPageSingleStateDomainByDomainDesignCode
              (FromQueryStringOrDomain, false);
        return _IncludeMetaDescriptionForSingleState.Value;
      }
    }

    //public static bool IncludeMetaKeywordsForAllStates
    //{
    //  get
    //  {
    //    if (_IncludeMetaKeywordsForAllStates == null)
    //      _IncludeMetaKeywordsForAllStates =
    //        DomainDesigns
    //          .GetIsIncludedMetaKeywordsTagDefaultPageAllStatesDomainByDomainDesignCode
    //          (FromQueryStringOrDomain, false);
    //    return _IncludeMetaKeywordsForAllStates.Value;
    //  }
    //}

    //public static bool IncludeMetaKeywordsForSingleState
    //{
    //  get
    //  {
    //    if (_IncludeMetaKeywordsForSingleState == null)
    //      _IncludeMetaKeywordsForSingleState =
    //        DomainDesigns
    //          .GetIsIncludedMetaKeywordsTagDefaultPageSingleStateDomainByDomainDesignCode
    //          (FromQueryStringOrDomain, false);
    //    return _IncludeMetaKeywordsForSingleState.Value;
    //  }
    //}

    public static bool IncludeTitleForAllStates
    {
      get
      {
        if (_IncludeTitleForAllStates == null)
          _IncludeTitleForAllStates =
            DomainDesigns
              .GetIsIncludedTitleTagDefaultPageAllStatesDomainByDomainDesignCode(
                FromQueryStringOrDomain, false);
        return _IncludeTitleForAllStates.Value;
      }
    }

    public static bool IncludeTitleForSingleState
    {
      get
      {
        if (_IncludeTitleForSingleState == null)
          _IncludeTitleForSingleState =
            DomainDesigns
              .GetIsIncludedTitleTagDefaultPageSingleStateDomainByDomainDesignCode(
                FromQueryStringOrDomain, false);
        return _IncludeTitleForSingleState.Value;
      }
    }
  }
}