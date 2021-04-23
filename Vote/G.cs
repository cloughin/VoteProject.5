using System;
using System.Globalization;
using System.Text;
using System.Web;
using DB.Vote;

namespace Vote
{
  public static partial class G
  {
    private static string Session_Get(string str)
    {
      return HttpContext.Current.Session[str] as string;
    }

    private static void Session_Put(string sessionStr, string valueStr)
    {
      HttpContext.Current.Session[sessionStr] = valueStr;
    }

    private static string User_StateCode()
    {
      if (
        SecurePage.IsMasterUser
          && !string.IsNullOrEmpty(VotePage.QueryState)
        )
        return VotePage.QueryState;
      return Session_Get("UserStateCode").ToUpper();
    }

    public static string User_CountyCode()
    {
      if (
        (SecurePage.IsMasterUser || SecurePage.IsStateAdminUser)
          && !string.IsNullOrEmpty(VotePage.QueryCounty)
        )
        return VotePage.QueryCounty;
      return Session_Get("UserCountyCode");
    }

    public static string User_LocalCode()
    {
      if (
        (SecurePage.IsMasterUser || SecurePage.IsStateAdminUser || SecurePage.IsCountyAdminUser)
          && (!string.IsNullOrEmpty(VotePage.QueryLocal))
        )
        return VotePage.QueryLocal;
      return Session_Get("UserLocalCode");
    }

    public static string State_Code()
    {
      //**Test
      var stateCode = SecurePage.GetViewStateStateCode();
      if (stateCode != null) return stateCode;

      if (VotePage.IsPublicPage)
        return StateCode_Domain_This();

      if (SecurePage.IsMasterUser)
      {

        if (!string.IsNullOrEmpty(VotePage.QueryState))
        {
          Session_Put("UserCountyCode", string.Empty);
          Session_Put("UserLocalCode", string.Empty);
          Session_Put("UserStateCode", VotePage.QueryState);

          return VotePage.QueryState;
        }
        return !string.IsNullOrEmpty(User_StateCode())
          ? User_StateCode()
          : string.Empty;
      }
      if (!SecurePage.IsStateAdminUser) return string.Empty;
      if (StateCache.IsValidStateOrFederalCode(User_StateCode(), false))
        return User_StateCode();
      return StateCache.IsValidStateOrFederalCode(Domain_StateCode_This(), false)
        ? Domain_StateCode_This()
        : string.Empty;
    }

    public static string County_Code()
    {
      var viewStateCountyCode = SecurePage.GetViewStateCountyCode();
      if (viewStateCountyCode != null) return viewStateCountyCode;

      var countyCode = string.Empty;

      if (
        !VotePage.IsSessionStateEnabled
          || !SecurePage.IsSignedIn
        )
        //if (db.Is_User_Anonymous())
      {
        if (!string.IsNullOrEmpty(VotePage.QueryElection))
          //could be old ElectionKey format
          countyCode = Elections.GetCountyCodeFromKey(
            //db.ElectionKey_New_Format(db.QueryString("Election")));
            VotePage.QueryElection);
        else if (!string.IsNullOrEmpty(VotePage.QueryOffice))
          countyCode = Offices.GetCountyCodeFromKey(VotePage.QueryOffice);
        else if (!string.IsNullOrEmpty(VotePage.QueryCounty))
          countyCode = VotePage.QueryCounty;
        if (
          (countyCode == "000") //Directory of Counties
            || (CountyCache.CountyExists(State_Code()
              , countyCode))
          )
          return countyCode;
        return string.Empty;
      }

      //only MASTER or State ADMIN can change CountyCode of county
      //Setting UserLocalCode empty resets to County level security

      if ((SecurePage.IsMasterUser || (SecurePage.IsStateAdminUser))
        && !string.IsNullOrEmpty(VotePage.QueryCounty)
        )
      {
        Session_Put("UserCountyCode", VotePage.QueryCounty);
        Session_Put("UserLocalCode", string.Empty);
      }
      else
      {
        //Need to reset CountyCode to empty
        //When there is a StateCode query string 
        //but no CountyCode query string
        if (!string.IsNullOrEmpty(State_Code()))
        {
          Session_Put("UserCountyCode", string.Empty);
          Session_Put("UserLocalCode", string.Empty);
        }
      }

      if (CountyCache.CountyExists(
        State_Code()
        , User_CountyCode()))
        return User_CountyCode();
      return string.Empty;
    }

    public static string Local_Code()
    {
      var viewStateLocalCode = SecurePage.GetViewStateLocalCode();
      if (viewStateLocalCode != null) return viewStateLocalCode;

      var localCode = string.Empty;

      if (
        !VotePage.IsSessionStateEnabled
          || !SecurePage.IsSignedIn
        )
      {
        if (!string.IsNullOrEmpty(VotePage.QueryElection))
          localCode = Elections.GetLocalCodeFromKey(
            VotePage.QueryElection);
        else if (!string.IsNullOrEmpty(VotePage.QueryOffice))
          localCode = Offices.GetLocalCodeFromKey(VotePage.QueryOffice);
        else if (!string.IsNullOrEmpty(VotePage.QueryLocal))
          localCode = VotePage.QueryLocal;
        if (
          (localCode == "00") //Directory of Local Districts
            || (LocalDistricts.IsValid(State_Code()
              , County_Code()
              , localCode))
          )
          return localCode;
        return string.Empty;
      }

      if (!string.IsNullOrEmpty(VotePage.QueryLocal))
        //Session["UserLocalCode"] = db.QueryString("Local");
        Session_Put("UserLocalCode", VotePage.QueryLocal);

      //Local_Code = db.User_LocalCode();
      if (LocalDistricts.IsValid(
        State_Code()
        , User_CountyCode()
        , User_LocalCode()))
        return User_LocalCode();
      return string.Empty;
    }

    private static string Domain_DataCode_This()
    {
      var queryData = VotePage.GetQueryString("Data").ToUpperInvariant();
      if (!string.IsNullOrEmpty(queryData))
        return queryData;
      var queryState = VotePage.QueryState;
      return !string.IsNullOrEmpty(queryState)
        ? queryState
        : UrlManager.CurrentDomainDataCode;
    }

    private static string StateCode_Domain_This()
    {
      return StateCache.IsValidStateCode(Domain_DataCode_This())
        ? Domain_DataCode_This()
        : string.Empty;
    }

    private static string Domain_StateCode_This()
    {
      return StateCode_Domain_This();
    }

    public static void Unreferenced()
    {
      
    }

#region CreateOfficeKey: rework and put in Offices class

    public static string Str_ReCase(string str2Fix)
    {
      var sb = new StringBuilder(str2Fix.Length);
      var wordBegin = true;
      foreach (var c in str2Fix)
      {
        sb.Append(wordBegin
          ? char.ToUpper(c)
          : char.ToLower(c));
        //wordBegin = char.IsWhiteSpace(c);
        if (
          (char.IsWhiteSpace(c))
            || (char.IsPunctuation(c))
          )
          wordBegin = true;
        else
          wordBegin = false;
      }
      return sb.ToString();
    }

    public static string Str_Replace_Puctuation_With(string str2Modify, string strReplaceWith)
    {
      var str = str2Modify;
      //characters to strip off
      str = str.Trim();
      str = str.Replace("\"", strReplaceWith);
      str = str.Replace("\'", strReplaceWith);
      str = str.Replace(".", strReplaceWith);
      str = str.Replace(",", strReplaceWith);
      str = str.Replace("(", strReplaceWith);
      str = str.Replace(")", strReplaceWith);
      str = str.Replace("[", strReplaceWith);
      str = str.Replace("[", strReplaceWith);
      str = str.Replace("_", strReplaceWith);
      str = str.Replace("-", strReplaceWith);
      return str;
    }

    public static string Str_Remove_Puctuation(string str2Modify)
    {
      var str = str2Modify;
      //characters to strip off
      str = str.Trim();
      str = str.Replace("\"", string.Empty);
      str = str.Replace("\'", string.Empty);
      str = str.Replace(".", string.Empty);
      str = str.Replace(",", string.Empty);
      str = str.Replace("(", string.Empty);
      str = str.Replace(")", string.Empty);
      str = str.Replace("[", string.Empty);
      str = str.Replace("[", string.Empty);
      str = str.Replace("_", string.Empty);
      str = str.Replace("-", string.Empty);
      return str;
    }

    public static bool Is_Valid_Integer(string number2Check)
    {
      int value;
      return int.TryParse(number2Check, out value);
    }

    public static string Str_Remove_SpecialChars_All(string str2Modify)
    {
      var str = str2Modify;
      str = str.Trim();
      str = str.Replace("-", string.Empty);
      str = str.Replace("+", string.Empty);
      str = str.Replace("=", string.Empty);
      str = str.Replace("\"", string.Empty);
      str = str.Replace("\'", string.Empty);
      str = str.Replace(".", string.Empty);
      str = str.Replace(",", string.Empty);
      str = str.Replace("(", string.Empty);
      str = str.Replace(")", string.Empty);
      str = str.Replace("!", string.Empty);
      str = str.Replace("@", string.Empty);
      str = str.Replace("#", string.Empty);
      str = str.Replace("%", string.Empty);
      str = str.Replace("&", string.Empty);
      str = str.Replace("*", string.Empty);
      str = str.Replace(":", string.Empty);
      str = str.Replace(";", string.Empty);
      str = str.Replace("$", string.Empty);
      str = str.Replace("^", string.Empty);
      str = str.Replace("?", string.Empty);
      str = str.Replace("<", string.Empty);
      str = str.Replace(">", string.Empty);
      str = str.Replace("[", string.Empty);
      str = str.Replace("]", string.Empty);
      str = str.Replace("{", string.Empty);
      str = str.Replace("}", string.Empty);
      str = str.Replace("|", string.Empty);
      str = str.Replace("~", string.Empty);
      str = str.Replace("`", string.Empty);
      str = str.Replace("_", string.Empty);
      str = str.Replace("/", string.Empty);
      return str;
    }

    public static string Str_Remove_Non_Key_Chars(string str2Modify)
    {
      var str = str2Modify;
      str = Str_Remove_SpecialChars_All(str);
      str = str.Replace(" ", string.Empty);
      return str;
    }

    public static string CreateOfficeKey(int officeClass, string stateCode, string countyCode, string localCode,
      string districtCode, string districtCodeAlpha, string officeLine1, string officeLine2)
    {
      //CountyCode - 3 digits
      //LocalCode - 2 digits
      //DistrictCode - 3 digits
      //DistrictCodeAlpha - 1-4 alpha chars

      //string theOffice = (
      //  OfficeLine1 + OfficeLine2).Trim();
      //theOffice = db.Str_Remove_Puctuation(theOffice);

      officeLine1 = Str_ReCase(Str_Replace_Puctuation_With(officeLine1, " ")).Trim();
      officeLine2 = Str_ReCase(Str_Replace_Puctuation_With(officeLine2, " ")).Trim();
      var theOffice = officeLine1 + officeLine2;
      theOffice = Str_Remove_Puctuation(theOffice);

      if (theOffice != string.Empty)
      {
        var stateNameUpper = StateCache.GetStateName(stateCode).ToUpper();
        var stateNamePos = theOffice.ToUpper().IndexOf(stateNameUpper, 0, theOffice.Length, StringComparison.Ordinal);
        if (stateNamePos >= 0) //State is in the Office Name - remove it
        {
          var stateNameLen = stateNameUpper.Length;
          theOffice = theOffice.Remove(stateNamePos, stateNameLen);
        }
      }

      string districtCodeWithout0S;
      if (Is_Valid_Integer(districtCode))
      {
        var districtInt = Convert.ToInt32(districtCode);
        districtCodeWithout0S = districtInt.ToString(CultureInfo.InvariantCulture);
      }
      else
      {
        districtCodeWithout0S = districtCode.Trim();
      }

      var theOfficeKey = string.Empty;
      theOffice = Str_Remove_Non_Key_Chars(theOffice);
      switch (officeClass.ToOfficeClass())
      {
        case OfficeClass.USPresident:
          theOfficeKey = stateCode.ToUpper();
          theOfficeKey += "President";
          break;
        case OfficeClass.USSenate:
          theOfficeKey = stateCode.ToUpper()
            + "USSenate" + officeLine2;
          break;
        case OfficeClass.USHouse:
          theOfficeKey = stateCode.ToUpper()
            + "USHouse"
            + districtCodeWithout0S;
          break;

        case OfficeClass.StateWide:
        case OfficeClass.StateJudicial:
        case OfficeClass.StateParty:
          theOfficeKey = stateCode.ToUpper()
            + theOffice;
          break;
        case OfficeClass.StateSenate:
          theOfficeKey = stateCode.ToUpper()
            + "StateSenate"
            + districtCodeWithout0S
            + districtCodeAlpha.Trim();
          break;
        case OfficeClass.StateHouse:
          theOfficeKey = stateCode.ToUpper()
            + "StateHouse"
            + districtCodeWithout0S
            + districtCodeAlpha.Trim();
          break;
        case OfficeClass.StateDistrictMultiCounties:
        case OfficeClass.StateDistrictMultiCountiesJudicial:
        case OfficeClass.StateDistrictMultiCountiesParty:
          theOfficeKey = stateCode.ToUpper()
            + districtCode
            + districtCodeAlpha.Trim()
            + theOffice;
          break;

        case OfficeClass.CountyExecutive:
        case OfficeClass.CountyLegislative:
        case OfficeClass.CountySchoolBoard:
        case OfficeClass.CountyCommission:
        case OfficeClass.CountyJudicial:
        case OfficeClass.CountyParty:
          theOfficeKey = stateCode.ToUpper()
            + countyCode
            + theOffice;
          break;

        case OfficeClass.LocalExecutive:
        case OfficeClass.LocalLegislative:
        case OfficeClass.LocalSchoolBoard:
        case OfficeClass.LocalCommission:
        case OfficeClass.LocalParty:
        case OfficeClass.LocalJudicial:
          theOfficeKey = stateCode.ToUpper()
            + countyCode
            + localCode
            + theOffice;
          break;
      }

      if (theOfficeKey.Length > 150)
        theOfficeKey = theOfficeKey.Substring(0, 150);

      return theOfficeKey;
    }

#endregion
  }
}