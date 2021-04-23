using System;
using System.Text.RegularExpressions;
using System.Web.UI;
using DB.Vote;

namespace Vote
{
  public static partial class G
  {
    //public static string GetStateCode()
    //{
    //  // reworked to eliminate ViewState references
    //  var stateCode = VotePage.QueryState;
    //  if (string.IsNullOrWhiteSpace(stateCode))
    //    if (VotePage.IsPublicPage)
    //      stateCode = UrlManager.CurrentDomainDataCode;
    //    else if (SecurePage.IsAdminUser)
    //      stateCode = SecurePage.UserStateCode;
    //    else if (SecurePage.IsPoliticianUser)
    //      stateCode = Politicians.GetStateCodeFromKey(SecurePage.UserPoliticianKey);
    //    else if (SecurePage.IsPartyUser)
    //      stateCode = Parties.GetStateCodeFromKey(SecurePage.UserPartyKey);

    //  return !string.IsNullOrWhiteSpace(stateCode) &&
    //    StateCache.IsValidStateOrFederalCode(stateCode)
    //    ? stateCode
    //    : string.Empty;

    //  //var stateCode = SecurePage.GetViewStateStateCode();
    //  //if (stateCode != null) return stateCode;

    //  //if (VotePage.IsPublicPage)
    //  //  return GetStateDomainCode();

    //  //if (SecurePage.IsMasterUser)
    //  //{
    //  //  if (!string.IsNullOrEmpty(VotePage.QueryState))
    //  //  {
    //  //    VotePage.PutSessionString("UserCountyCode", string.Empty);
    //  //    VotePage.PutSessionString("UserLocalCode", string.Empty);
    //  //    VotePage.PutSessionString("UserStateCode", VotePage.QueryState);

    //  //    return VotePage.QueryState;
    //  //  }
    //  //  return !string.IsNullOrEmpty(GetUserStateCode())
    //  //    ? GetUserStateCode()
    //  //    : string.Empty;
    //  //}
    //  //if (!SecurePage.IsStateAdminUser) return string.Empty;
    //  //if (StateCache.IsValidStateOrFederalCode(GetUserStateCode(), false))
    //  //  return GetUserStateCode();
    //  //var stateDomainCode = GetStateDomainCode();
    //  //return StateCache.IsValidStateOrFederalCode(stateDomainCode, false)
    //  //  ? stateDomainCode
    //  //  : string.Empty;
    //}

    //public static string GetCountyCode()
    //{
    //  // reworked to eliminate ViewState references
    //  var stateCode = GetStateCode();
    //  if (string.IsNullOrWhiteSpace(stateCode)) return string.Empty;

    //  var countyCode = VotePage.QueryCounty;
    //  if (string.IsNullOrWhiteSpace(countyCode))
    //    if (SecurePage.IsAdminUser)
    //      countyCode = SecurePage.UserCountyCode;

    //  return !string.IsNullOrWhiteSpace(countyCode) && 
    //    CountyCache.CountyExists(stateCode, countyCode) 
    //    ? countyCode 
    //    : string.Empty;

    //  //var viewStateCountyCode = SecurePage.GetViewStateCountyCode();
    //  //if (viewStateCountyCode != null) return viewStateCountyCode;

    //  //if ((SecurePage.IsMasterUser || SecurePage.IsStateAdminUser) &&
    //  //  !string.IsNullOrEmpty(VotePage.QueryCounty))
    //  //{
    //  //  VotePage.PutSessionString("UserCountyCode", VotePage.QueryCounty);
    //  //  VotePage.PutSessionString("UserLocalCode", string.Empty);
    //  //}
    //  //else
    //  //{
    //  //  if (!string.IsNullOrEmpty(GetStateCode()))
    //  //  {
    //  //    VotePage.PutSessionString("UserCountyCode", string.Empty);
    //  //    VotePage.PutSessionString("UserLocalCode", string.Empty);
    //  //  }
    //  //}

    //  //return CountyCache.CountyExists(GetStateCode(), GetUserCountyCode()) 
    //  //  ? GetUserCountyCode() 
    //  //  : string.Empty;
    //}

    //public static string GetLocalCode()
    //{
    //  // reworked to eliminate ViewState references
    //  var stateCode = GetStateCode();
    //  var countyCode = GetCountyCode();
    //  if (string.IsNullOrWhiteSpace(stateCode) || string.IsNullOrWhiteSpace(countyCode))
    //    return string.Empty;

    //  var localCode = VotePage.QueryLocal;
    //  if (string.IsNullOrWhiteSpace(localCode))
    //    if (SecurePage.IsAdminUser)
    //      localCode = SecurePage.UserLocalCode;

    //  return !string.IsNullOrWhiteSpace(localCode) && 
    //    LocalDistricts.IsValid(stateCode, countyCode, localCode) 
    //    ? localCode
    //    : string.Empty;

    //  //var viewStateLocalCode = SecurePage.GetViewStateLocalCode();
    //  //if (viewStateLocalCode != null) return viewStateLocalCode;

    //  //if (!string.IsNullOrEmpty(VotePage.QueryLocal))
    //  //  VotePage.PutSessionString("UserLocalCode", VotePage.QueryLocal);

    //  //return LocalDistricts.IsValid(GetStateCode(), GetUserCountyCode(), GetUserLocalCode()) 
    //  //  ? GetUserLocalCode() 
    //  //  : string.Empty;
    //}
  }
}