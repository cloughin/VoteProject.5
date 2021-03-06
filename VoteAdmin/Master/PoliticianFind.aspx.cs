using System;
using System.Data;
using System.Text.RegularExpressions;
using DB.Vote;

namespace Vote.Master
{
  public partial class FindPolitician : SecurePage, ISuperUser
  {
    #region from db

    public static string Ok(string msg) => $"<span class=\"MsgOk\">SUCCESS!!! {msg}</span>";

    public static string Fail(string msg) => $"<span class=\"MsgFail\">****FAILURE**** {msg}</span>";

    public static string Message(string msg) => $"<span class=\"Msg\">{msg}</span>";

    public static string SqlLit(string str)
    {
      //Enclose string in single quotes and double up any embededded single quotes
      str = "'" + str.Replace("'", "''") + "'";
      return str;
    }

    public static string Str_Remove_Single_And_Double_Quotes(string str2Modify)
    {
      var str = str2Modify;
      str = str.Replace("\'", string.Empty);
      str = str.Replace("\"", string.Empty);

      return str;
    }

    public static string DbErrorMsg(string sql, string err)
    {
      //Write code to log database errors to a DBFailues Table
      return "Database Failure for SQL Statement::" + sql + " :: Error Msg:: " + err;
    }

    public static string Anchor_Mailto_Email(string emailAddress)
    {
      if (emailAddress != string.Empty)
      {
        return "<a href=mailto:" + emailAddress + ">" + emailAddress + "</a>";
      }
      return string.Empty;
    }

    public static string Anchor(string url, string anchorText, string toolTip, string target)
    {
      //<a href="Url" title="ToolTip" Class= "className" target=Target>AnchorText</a>
      var anchor = string.Empty;
      anchor += "<a href=";
      anchor += "\"";
      anchor += url;
      anchor += "\"";

      if (toolTip != string.Empty)
      {
        anchor += " title=";
        anchor += "\"";
        anchor += Str_Remove_Single_And_Double_Quotes(toolTip);
        anchor += "\"";
        anchor += " ";
      }

      if (target != string.Empty)
      {
        anchor += " target=";
        anchor += "\"";
        anchor += target;
        anchor += "\"";
        anchor += " ";
      }

      anchor += ">";

      if (anchorText != string.Empty)
        anchor += anchorText;
      else
        anchor += url;

      anchor += "</a>";
      return anchor;
    }

    public static string Fix_Url_Parms(string url)
    {
      //sets first parm in a query string to ? if all seperators are &'s
      if ((url.IndexOf('?', 0, url.Length) == -1) //no ?
        && (url.IndexOf('&', 0, url.Length) > -1)) //but one or more &
      {
        var loc = url.IndexOf('&', 0, url.Length);
        var lenAfter = url.Length - loc - 1;
        var urlBefore = url.Substring(0, loc);
        var urlAfter = url.Substring(loc + 1, lenAfter);
        return urlBefore + "?" + urlAfter;
      }
      return url;
    }

    public static string Url_Politician_Intro()
    {
      return @"/Politician/IntroPage.aspx";
    }

    public static string Url_Politician_Intro(string politicianKey)
    {
      var url = Url_Politician_Intro();

      if (!string.IsNullOrEmpty(politicianKey))
        url += "&Id=" + politicianKey;
      else if (!string.IsNullOrEmpty(QueryId))
        url += "&Id=" + QueryId;

      return Fix_Url_Parms(url);
    }
    public static void Single_Key_Update_Str(string table, string column, string columnValue,
      string keyName, string keyValue)
    {
      Db.UpdateColumnByKey(table, column, columnValue, keyName, keyValue);
    }

    public static void Single_Key_Update_Int(string table, string column, int columnValue,
      string keyName, string keyValue)
    {
      var updateSql =
        $"UPDATE {table} SET {column} = {columnValue} WHERE {keyName} = {SqlLit(keyValue)}";
      G.ExecuteSql(updateSql);
    }

    public static void Single_Key_Update_Date(string table, string column, DateTime columnValue,
      string keyName, string keyValue)
    {
      var updateSql =
        $"UPDATE {table} SET {column} = {SqlLit(Db.DbDateTime(columnValue))} WHERE {keyName} = {SqlLit(keyValue)}";
      G.ExecuteSql(updateSql);
    }

    public static string Anchor(string url)
    {
      return Anchor(url, string.Empty, string.Empty, string.Empty);
    }

    public static string Url_Master_FindPolitician(string politicianKey)
    {
      return "/Master/PoliticianFind.aspx?Id=" + politicianKey;
    }

    public static string Politicians_Str(string politicianKey, string columnName)
    {
      return Politicians_Str(politicianKey, Politicians.GetColumn(columnName));
    }

    public static string Politicians_Str(string politicianKey, Politicians.Column column)
    {
      var value = Politicians.GetColumn(column, politicianKey);
      if (value == null) return string.Empty;
      return value as string;
    }

    public static string Politician_Column_Description(PoliticianColumn politicianColumn)
    {
      switch (politicianColumn)
      {
        case PoliticianColumn.Password:
          return "Password";
        case PoliticianColumn.PasswordHint:
          return "Password Hint";
        case PoliticianColumn.TemporaryOfficeKey:
          return "Office Key";
        case PoliticianColumn.StateCode:
          return "State Code";
        case PoliticianColumn.FName:
          return "First Name";
        case PoliticianColumn.MName:
          return "Middle Name";
        case PoliticianColumn.Nickname:
          return "Nick Name";
        case PoliticianColumn.LName:
          return "Last Name";
        case PoliticianColumn.Suffix:
          return "Name Suffix";
        case PoliticianColumn.AddOn:
          return "Additional Name Info";
        case PoliticianColumn.EmailAddrVoteUSA:
          return "VoteUSA Email Address";
        case PoliticianColumn.EmailAddr:
          return "Candidate Email Address";
        case PoliticianColumn.StateEmailAddr:
          return "State Email Address";
        case PoliticianColumn.WebAddr:
          return "Candidate Website Address";
        case PoliticianColumn.StateWebAddr:
          return "State Website Address";
        case PoliticianColumn.Phone:
          return "Candidate Phone";
        case PoliticianColumn.StatePhone:
          return "State Phone";
        case PoliticianColumn.Gender:
          return "Gender";
        case PoliticianColumn.PartyKey:
          return "Party Key";
        case PoliticianColumn.Address:
          return "Street Address";
        case PoliticianColumn.CityStateZip:
          return "City, State Zip";
        case PoliticianColumn.StateAddress:
          return "State Street Address";
        case PoliticianColumn.StateCityStateZip:
          return "State City, State Zip";
        case PoliticianColumn.CampaignName:
          return "Campaign Name";
        case PoliticianColumn.CampaignAddr:
          return "Campaign Address";
        case PoliticianColumn.CampaignCityStateZip:
          return "Campaign City State Zip";
        case PoliticianColumn.CampaignPhone:
          return "Campaign Phone";
        case PoliticianColumn.CampaignEmail:
          return "Campaign Email";
        case PoliticianColumn.FacebookWebAddress:
          return "Facebook Address";
        case PoliticianColumn.WikipediaWebAddress:
          return "Wikipedia Address";
        case PoliticianColumn.YouTubeWebAddress:
          return "YouTubeWeb Address";
        case PoliticianColumn.FlickrWebAddress:
          return "Flickr WebAddress";
        case PoliticianColumn.TwitterWebAddress:
          return "Twitter WebAddress";
        case PoliticianColumn.RSSFeedWebAddress:
          return "RSSFeed WebAddress";
        default:
          return string.Empty;
      }
    }

    //public static string Politician_Column_Name(
    //  PoliticianColumn politicianColumn
    //  )
    //{
    //  switch (politicianColumn)
    //  {
    //    case PoliticianColumn.Password:
    //      return "Password";
    //    case PoliticianColumn.PasswordHint:
    //      return "PasswordHint";
    //    case PoliticianColumn.TemporaryOfficeKey:
    //      return "TemporaryOfficeKey";
    //    case PoliticianColumn.StateCode:
    //      return "StateCode";
    //    case PoliticianColumn.FName:
    //      return "FName";
    //    case PoliticianColumn.MName:
    //      return "MName";
    //    case PoliticianColumn.Nickname:
    //      return "Nickname";
    //    case PoliticianColumn.LName:
    //      return "LName";
    //    case PoliticianColumn.Suffix:
    //      return "Suffix";
    //    case PoliticianColumn.AddOn:
    //      return "AddOn";
    //    case PoliticianColumn.EmailAddrVoteUSA:
    //      return "EmailAddrVoteUSA";
    //    case PoliticianColumn.EmailAddr:
    //      return "EmailAddr";
    //    case PoliticianColumn.StateEmailAddr:
    //      return "StateEmailAddr";
    //    case PoliticianColumn.WebAddr:
    //      return "WebAddr";
    //    case PoliticianColumn.StateWebAddr:
    //      return "StateWebAddr";
    //    case PoliticianColumn.Phone:
    //      return "Phone";
    //    case PoliticianColumn.StatePhone:
    //      return "StatePhone";
    //    case PoliticianColumn.Gender:
    //      return "Gender";
    //    case PoliticianColumn.PartyKey:
    //      return "PartyKey";
    //    case PoliticianColumn.Address:
    //      return "Address";
    //    case PoliticianColumn.CityStateZip:
    //      return "CityStateZip";
    //    case PoliticianColumn.StateAddress:
    //      return "StateAddress";
    //    case PoliticianColumn.StateCityStateZip:
    //      return "StateCityStateZip";
    //    case PoliticianColumn.CampaignName:
    //      return "CampaignName";
    //    case PoliticianColumn.CampaignAddr:
    //      return "CampaignAddr";
    //    case PoliticianColumn.CampaignCityStateZip:
    //      return "CampaignCityStateZip";
    //    case PoliticianColumn.CampaignPhone:
    //      return "CampaignPhone";
    //    case PoliticianColumn.CampaignEmail:
    //      return "CampaignEmail";
    //    case PoliticianColumn.FacebookWebAddress:
    //      return "FacebookWebAddress";
    //    case PoliticianColumn.WikipediaWebAddress:
    //      return "YouTubeWebAddress";
    //    case PoliticianColumn.YouTubeWebAddress:
    //      return "WikipediaWebAddress";
    //    case PoliticianColumn.FlickrWebAddress:
    //      return "FlickrWebAddress";
    //    case PoliticianColumn.TwitterWebAddress:
    //      return "TwitterWebAddress";
    //    case PoliticianColumn.RSSFeedWebAddress:
    //      return "RSSFeedWebAddress";
    //    default:
    //      return string.Empty;
    //  }
    //}

    public enum PoliticianColumn
    {
      Password,
      PasswordHint,
      TemporaryOfficeKey,
      StateCode,
      FName,
      MName,
      Nickname,
      LName,
      Suffix,
      AddOn,
      // ReSharper disable once InconsistentNaming
      EmailAddrVoteUSA,
      EmailAddr,
      StateEmailAddr,
      WebAddr,
      StateWebAddr,
      Phone,
      StatePhone,
      Gender,
      PartyKey,
      Address,
      CityStateZip,
      StateAddress,
      StateCityStateZip,
      CampaignName,
      CampaignAddr,
      CampaignCityStateZip,
      CampaignPhone,
      CampaignEmail,
      FacebookWebAddress,
      WikipediaWebAddress,
      YouTubeWebAddress,
      FlickrWebAddress,
      TwitterWebAddress,
      // ReSharper disable once InconsistentNaming
      RSSFeedWebAddress,
      VimeoWebAddress
    }

    //public static string Politician(string politicianKey, PoliticianColumn column)
    //{
    //  return Politician(GetPageCache(), politicianKey, column);
    //}

    //public static string Politician(PageCache cache,
    //  string politicianKey, PoliticianColumn column)
    //{
    //  switch (column)
    //  {
    //    case PoliticianColumn.Password:
    //      return Politicians.GetPassword(politicianKey, string.Empty);

    //    case PoliticianColumn.PasswordHint:
    //      return Politicians.GetPasswordHint(politicianKey, string.Empty);

    //    case PoliticianColumn.TemporaryOfficeKey:
    //      return Politicians.GetTemporaryOfficeKey(politicianKey, string.Empty);

    //    case PoliticianColumn.StateCode:
    //      return Politicians.GetStateCode(politicianKey, string.Empty);

    //    case PoliticianColumn.FName:
    //      return cache.Politicians.GetFirstName(politicianKey);

    //    case PoliticianColumn.MName:
    //      return Politicians.GetMiddleName(politicianKey, string.Empty);

    //    case PoliticianColumn.Nickname:
    //      return Politicians.GetNickname(politicianKey, string.Empty);

    //    case PoliticianColumn.LName:
    //      return cache.Politicians.GetLastName(politicianKey);

    //    case PoliticianColumn.Suffix:
    //      return Politicians.GetSuffix(politicianKey, string.Empty);

    //    case PoliticianColumn.AddOn:
    //      return Politicians.GetAddOn(politicianKey, string.Empty);

    //    case PoliticianColumn.EmailAddrVoteUSA:
    //      return Politicians.GetEmailVoteUSA(politicianKey, string.Empty);

    //    case PoliticianColumn.EmailAddr:
    //      return Politicians.GetEmail(politicianKey, string.Empty);

    //    case PoliticianColumn.StateEmailAddr:
    //      return Politicians.GetStateEmail(politicianKey, string.Empty);

    //    case PoliticianColumn.WebAddr:
    //      return Politicians.GetWebAddress(politicianKey, string.Empty);

    //    case PoliticianColumn.StateWebAddr:
    //      return Politicians.GetStateWebAddress(politicianKey, string.Empty);

    //    case PoliticianColumn.Phone:
    //      return Politicians.GetPhone(politicianKey, string.Empty);

    //    case PoliticianColumn.StatePhone:
    //      return Politicians.GetStatePhone(politicianKey, string.Empty);

    //    case PoliticianColumn.Gender:
    //      return Politicians.GetGender(politicianKey, string.Empty);

    //    case PoliticianColumn.PartyKey:
    //      return cache.Politicians.GetPartyKey(politicianKey);

    //    case PoliticianColumn.Address:
    //      return Politicians.GetAddress(politicianKey, string.Empty);

    //    case PoliticianColumn.CityStateZip:
    //      return Politicians.GetCityStateZip(politicianKey, string.Empty);

    //    case PoliticianColumn.StateAddress:
    //      return Politicians.GetStateAddress(politicianKey, string.Empty);

    //    case PoliticianColumn.StateCityStateZip:
    //      return Politicians.GetStateCityStateZip(politicianKey, string.Empty);

    //    case PoliticianColumn.CampaignName:
    //      return Politicians.GetCampaignName(politicianKey, string.Empty);

    //    case PoliticianColumn.CampaignAddr:
    //      return Politicians.GetCampaignAddress(politicianKey, string.Empty);

    //    case PoliticianColumn.CampaignCityStateZip:
    //      return Politicians.GetCampaignCityStateZip(politicianKey, string.Empty);

    //    case PoliticianColumn.CampaignPhone:
    //      return Politicians.GetCampaignPhone(politicianKey, string.Empty);

    //    case PoliticianColumn.CampaignEmail:
    //      return Politicians.GetCampaignEmail(politicianKey, string.Empty);

    //    default:
    //      return string.Empty;
    //  }
    //}

    public static void Politicians_Update_Str(string politicianKey, string column,
      string columnValue)
    {
      Single_Key_Update_Str("Politicians", column, columnValue, "PoliticianKey", politicianKey);
    }

    public static void Politicians_Update_Password(string politicianKey, string columnValue)
    {
      Politicians_Update_Str(politicianKey, "Password", columnValue);
    }

    public static void Politicians_Update_PasswordHint(string politicianKey, string columnValue)
    {
      Politicians_Update_Str(politicianKey, "PasswordHint", columnValue);
    }

    public static void Politicians_Update_TemporaryOfficeKey(string politicianKey,
      string columnValue)
    {
      Politicians_Update_Str(politicianKey, "TemporaryOfficeKey", columnValue);
    }

    public static void Politicians_Update_StateCode(string politicianKey, string columnValue)
    {
      Politicians_Update_Str(politicianKey, "StateCode", columnValue);
    }

    public static void Politicians_Update_FName(string politicianKey, string columnValue)
    {
      Politicians_Update_Str(politicianKey, "FName", columnValue);
    }

    public static void Politicians_Update_MName(string politicianKey, string columnValue)
    {
      Politicians_Update_Str(politicianKey, "MName", columnValue);
    }

    public static void Politicians_Update_Nickname(string politicianKey, string columnValue)
    {
      Politicians_Update_Str(politicianKey, "Nickname", columnValue);
    }

    public static void Politicians_Update_LName(string politicianKey, string columnValue)
    {
      Politicians_Update_Str(politicianKey, "LName", columnValue);
    }

    public static void Politicians_Update_Suffix(string politicianKey, string columnValue)
    {
      Politicians_Update_Str(politicianKey, "Suffix", columnValue);
    }

    public static void Politicians_Update_AddOn(string politicianKey, string columnValue)
    {
      Politicians_Update_Str(politicianKey, "AddOn", columnValue);
    }

    public static void Politicians_Update_EmailAddrVoteUSA(string politicianKey, string columnValue)
    {
      Politicians_Update_Str(politicianKey, "EmailAddrVoteUSA", columnValue);
    }

    public static void Politicians_Update_EmailAddr(string politicianKey, string columnValue)
    {
      Politicians_Update_Str(politicianKey, "EmailAddr", columnValue);
    }

    public static void Politicians_Update_StateEmailAddr(string politicianKey, string columnValue)
    {
      Politicians_Update_Str(politicianKey, "StateEmailAddr", columnValue);
    }

    public static void Politicians_Update_WebAddr(string politicianKey, string columnValue)
    {
      Politicians_Update_Str(politicianKey, "WebAddr", columnValue);
    }

    public static void Politicians_Update_StateWebAddr(string politicianKey, string columnValue)
    {
      Politicians_Update_Str(politicianKey, "StateWebAddr", columnValue);
    }

    public static void Politicians_Update_Phone(string politicianKey, string columnValue)
    {
      Politicians_Update_Str(politicianKey, "Phone", columnValue);
    }

    public static void Politicians_Update_StatePhone(string politicianKey, string columnValue)
    {
      Politicians_Update_Str(politicianKey, "StatePhone", columnValue);
    }

    public static void Politicians_Update_Gender(string politicianKey, string columnValue)
    {
      Politicians_Update_Str(politicianKey, "Gender", columnValue);
    }

    public static void Politicians_Update_PartyKey(string politicianKey, string columnValue)
    {
      Politicians_Update_Str(politicianKey, "PartyKey", columnValue);
    }

    public static void Politicians_Update_Address(string politicianKey, string columnValue)
    {
      Politicians_Update_Str(politicianKey, "Address", columnValue);
    }

    public static void Politicians_Update_CityStateZip(string politicianKey, string columnValue)
    {
      Politicians_Update_Str(politicianKey, "CityStateZip", columnValue);
    }

    public static void Politicians_Update_StateAddress(string politicianKey, string columnValue)
    {
      Politicians_Update_Str(politicianKey, "StateAddress", columnValue);
    }

    public static void Politicians_Update_StateCityStateZip(string politicianKey, string columnValue)
    {
      Politicians_Update_Str(politicianKey, "StateCityStateZip", columnValue);
    }

    public static void Politicians_Update_CampaignName(string politicianKey, string columnValue)
    {
      Politicians_Update_Str(politicianKey, "CampaignName", columnValue);
    }

    public static void Politicians_Update_CampaignAddr(string politicianKey, string columnValue)
    {
      Politicians_Update_Str(politicianKey, "CampaignAddr", columnValue);
    }

    public static void Politicians_Update_CampaignCityStateZip(string politicianKey,
      string columnValue)
    {
      Politicians_Update_Str(politicianKey, "CampaignCityStateZip", columnValue);
    }

    public static void Politicians_Update_CampaignPhone(string politicianKey, string columnValue)
    {
      Politicians_Update_Str(politicianKey, "CampaignPhone", columnValue);
    }

    public static void Politicians_Update_CampaignEmail(string politicianKey, string columnValue)
    {
      Politicians_Update_Str(politicianKey, "CampaignEmail", columnValue);
    }

    public static void Politicians_Update_FacebookWebAddress(string politicianKey,
      string columnValue)
    {
      Politicians_Update_Str(politicianKey, "FacebookWebAddress", columnValue);
    }

    public static void Politicians_Update_WikipediaWebAddress(string politicianKey,
      string columnValue)
    {
      Politicians_Update_Str(politicianKey, "WikipediaWebAddress", columnValue);
    }

    public static void Politicians_Update_YouTubeWebAddress(string politicianKey, string columnValue)
    {
      Politicians_Update_Str(politicianKey, "YouTubeWebAddress", columnValue);
    }

    public static void Politicians_Update_FlickrWebAddress(string politicianKey, string columnValue)
    {
      Politicians_Update_Str(politicianKey, "FlickrWebAddress", columnValue);
    }

    public static void Politicians_Update_TwitterWebAddress(string politicianKey, string columnValue)
    {
      Politicians_Update_Str(politicianKey, "TwitterWebAddress", columnValue);
    }

    public static void Politicians_Update_RSSFeedWebAddress(string politicianKey, string columnValue)
    {
      Politicians_Update_Str(politicianKey, "RSSFeedWebAddress", columnValue);
    }

    public static void Politicians_Update_VimeoWebAddress(string politicianKey, string columnValue)
    {
      Politicians_Update_Str(politicianKey, "VimeoWebAddress", columnValue);
    }

    public static void Politicians_Update_Int(string politicianKey, string column, int columnValue)
    {
      Single_Key_Update_Int("Politicians", column, columnValue, "PoliticianKey", politicianKey);
    }

    public static int Politicians_Update_DataUpdatedCount(string politicianKey)
    {
      return Politicians.GetDataUpdatedCount(politicianKey, 0);
    }

    public static void Politician_Update_Increment_Count_Data_Updated(string politicianKey)
    {
      var dataUpdatedCount = Politicians_Update_DataUpdatedCount(politicianKey);
      dataUpdatedCount++;
      Politicians_Update_Int(politicianKey, "DataUpdatedCount", dataUpdatedCount);
    }

    public static void Politicians_Update_Date(string politicianKey, string column,
      DateTime columnValue)
    {
      Single_Key_Update_Date("Politicians", column, columnValue, "PoliticianKey", politicianKey);
    }

    public static void Politician_Update_DataLastUpdated(string politicianKey)
    {
      Politicians_Update_Date(politicianKey, "DataLastUpdated", DateTime.Now);
    }

    public static void Politician_Update_Str(string politicianKey, PoliticianColumn column,
      string strNewValue)
    {
      switch (column)
      {
        case PoliticianColumn.Password:
          Politicians_Update_Password(politicianKey, strNewValue);
          break;
        case PoliticianColumn.PasswordHint:
          Politicians_Update_PasswordHint(politicianKey, strNewValue);
          break;
        case PoliticianColumn.TemporaryOfficeKey:
          Politicians_Update_TemporaryOfficeKey(politicianKey, strNewValue);
          break;
        case PoliticianColumn.StateCode:
          Politicians_Update_StateCode(politicianKey, strNewValue);
          break;
        case PoliticianColumn.FName:
          Politicians_Update_FName(politicianKey, strNewValue);
          break;
        case PoliticianColumn.MName:
          Politicians_Update_MName(politicianKey, strNewValue);
          break;
        case PoliticianColumn.Nickname:
          Politicians_Update_Nickname(politicianKey, strNewValue);
          break;
        case PoliticianColumn.LName:
          Politicians_Update_LName(politicianKey, strNewValue);
          break;
        case PoliticianColumn.Suffix:
          Politicians_Update_Suffix(politicianKey, strNewValue);
          break;
        case PoliticianColumn.AddOn:
          Politicians_Update_AddOn(politicianKey, strNewValue);
          break;
        case PoliticianColumn.EmailAddrVoteUSA:
          Politicians_Update_EmailAddrVoteUSA(politicianKey, strNewValue);
          break;
        case PoliticianColumn.EmailAddr:
          Politicians_Update_EmailAddr(politicianKey, strNewValue);
          break;
        case PoliticianColumn.StateEmailAddr:
          Politicians_Update_StateEmailAddr(politicianKey, strNewValue);
          break;
        case PoliticianColumn.WebAddr:
          Politicians_Update_WebAddr(politicianKey, strNewValue);
          break;
        case PoliticianColumn.StateWebAddr:
          Politicians_Update_StateWebAddr(politicianKey, strNewValue);
          break;
        case PoliticianColumn.Phone:
          Politicians_Update_Phone(politicianKey, strNewValue);
          break;
        case PoliticianColumn.StatePhone:
          Politicians_Update_StatePhone(politicianKey, strNewValue);
          break;
        case PoliticianColumn.Gender:
          Politicians_Update_Gender(politicianKey, strNewValue);
          break;
        case PoliticianColumn.PartyKey:
          Politicians_Update_PartyKey(politicianKey, strNewValue);
          break;
        case PoliticianColumn.Address:
          Politicians_Update_Address(politicianKey, strNewValue);
          break;
        case PoliticianColumn.CityStateZip:
          Politicians_Update_CityStateZip(politicianKey, strNewValue);
          break;
        case PoliticianColumn.StateAddress:
          Politicians_Update_StateAddress(politicianKey, strNewValue);
          break;
        case PoliticianColumn.StateCityStateZip:
          Politicians_Update_StateCityStateZip(politicianKey, strNewValue);
          break;
        case PoliticianColumn.CampaignName:
          Politicians_Update_CampaignName(politicianKey, strNewValue);
          break;
        case PoliticianColumn.CampaignAddr:
          Politicians_Update_CampaignAddr(politicianKey, strNewValue);
          break;
        case PoliticianColumn.CampaignCityStateZip:
          Politicians_Update_CampaignCityStateZip(politicianKey, strNewValue);
          break;
        case PoliticianColumn.CampaignPhone:
          Politicians_Update_CampaignPhone(politicianKey, strNewValue);
          break;
        case PoliticianColumn.CampaignEmail:
          Politicians_Update_CampaignEmail(politicianKey, strNewValue);
          break;
        case PoliticianColumn.FacebookWebAddress:
          Politicians_Update_FacebookWebAddress(politicianKey, strNewValue);
          break;
        case PoliticianColumn.WikipediaWebAddress:
          Politicians_Update_WikipediaWebAddress(politicianKey, strNewValue);
          break;
        case PoliticianColumn.YouTubeWebAddress:
          Politicians_Update_YouTubeWebAddress(politicianKey, strNewValue);
          break;
        case PoliticianColumn.FlickrWebAddress:
          Politicians_Update_FlickrWebAddress(politicianKey, strNewValue);
          break;
        case PoliticianColumn.TwitterWebAddress:
          Politicians_Update_TwitterWebAddress(politicianKey, strNewValue);
          break;
        case PoliticianColumn.RSSFeedWebAddress:
          Politicians_Update_RSSFeedWebAddress(politicianKey, strNewValue);
          break;
        case PoliticianColumn.VimeoWebAddress:
          Politicians_Update_VimeoWebAddress(politicianKey, strNewValue);
          break;
      }
    }

    public static void Politician_Update_Post(string politicianKey)
    {
      Politician_Update_Increment_Count_Data_Updated(politicianKey);
      Politician_Update_DataLastUpdated(politicianKey);
    }

    public static string Politician_Update_Transaction_Str(string politicianKey,
      PoliticianColumn column, string newValue)
    {
      //LogPoliticianChanges.Insert(
      //  DateTime.Now,
      //  UserSecurityClass,
      //  UserName,
      //  politicianKey,
      //  G.Politician_Column_Name(column),
      //  Politician(politicianKey, column).ToStringOrNull(),
      //  newValue.ToStringOrNull().Trim());

      Politician_Update_Str(politicianKey, column, newValue);

      Politician_Update_Post(politicianKey);

      return Politician_Column_Description(column);
    }

    public static string UrlAddressEmail(string emailAddress)
    {
      return emailAddress != string.Empty ? "mailto:" + emailAddress : string.Empty;
    }

    public static string Anchor_Politician_Intro_HappyFace(string politicianKey)
    {
      var anchor = string.Empty;
      anchor += "<a href=";
      anchor += "\"";
      anchor += Url_Politician_Intro(politicianKey);
      anchor += "\"";

      anchor += " target=";
      anchor += "\"";
      anchor += "politician";
      anchor += "\"";

      anchor += " title=";
      anchor += "\"";
      anchor += "Update Politicians Intro Page";
      anchor += "\"";

      anchor += ">";

      anchor += " <img src=";
      anchor += "\"";
      //Anchor += "/images/PoliticianHappyFace.jpg";
      anchor += Url_Politician_HappyFace_Image();
      anchor += "\"";

      anchor += " border=0";

      anchor += "</a>";
      return anchor;
    }

    public static string Url_Politician_HappyFace_Image()
    {
      return "/images/PoliticianHappyFace.jpg";
    }

    public static string Anchor(Uri uri)
    {
      return Anchor(uri.ToString(), string.Empty, string.Empty, string.Empty);
    }

    public static string Anchor_Master_FindPolitician(string politicianKey)
    {
      var anchor = string.Empty;
      anchor += "<a href=";
      anchor += "\"";
      anchor += Url_Master_FindPolitician(politicianKey);
      anchor += "\"";

      anchor += ">";
      anchor += "<nobr>" + Politicians.GetFormattedName(politicianKey) + "</nobr>";
      anchor += "</a>";
      return anchor;
    }

    #endregion from db

    private string _PoliticianKey;

    protected string PoliticiansReport(string stateCode, string lastName)
    {
      var sqlText = string.Empty;
      sqlText += " SELECT ";
      sqlText += " Politicians.PoliticianKey ";
      sqlText += " ,Politicians.PartyKey ";
      sqlText += " ,Politicians.TemporaryOfficeKey ";
      sqlText += " ,Politicians.EmailAddrVoteUSA ";
      sqlText += " ,Politicians.EmailAddr ";
      sqlText += " ,Politicians.StateEmailAddr ";
      sqlText += " ,Politicians.WebAddr ";
      sqlText += " ,Politicians.StateWebAddr ";
      sqlText += " ,Politicians.Password ";
      sqlText += " FROM Politicians ";
      sqlText += " WHERE Politicians.LName = " + SqlLit(lastName);
      var reportPoliticians1 = string.Empty;
      if (stateCode != string.Empty)
      {
        sqlText += " AND Politicians.StateCode = " + SqlLit(stateCode);
      }
      sqlText += " ORDER BY Politicians.FName,Politicians.MName";
      var politiciansTable = G.Table(sqlText);

      if (politiciansTable.Rows.Count > 0)
      {
        foreach (DataRow politiciansRow in politiciansTable.Rows)
        {
          reportPoliticians1 += "<br>" +
            Anchor_Master_FindPolitician(politiciansRow["PoliticianKey"].ToString());
          //if (db.Name_Electoral_And_Contest(PoliticiansRow["OfficeKey"].ToString()) != string.Empty)
          //  ReportPoliticians1 += " <strong>Office:</strong> " + db.Name_Electoral_And_Contest(PoliticiansRow["OfficeKey"].ToString());
          //string officeKey = Politicians.GetOfficeKey(politiciansRow["PoliticianKey"].ToString(), string.Empty);
          var politicianKey = politiciansRow["PoliticianKey"].ToString();
          var officeKey = GetPageCache().Politicians.GetOfficeKey(politicianKey);
          if (Offices.GetLocalizedOfficeName(officeKey) != string.Empty)
            reportPoliticians1 += " <strong>Office:</strong> " + StateCache.GetStateName(stateCode) +
              " " + Offices.GetLocalizedOfficeName(officeKey);
          if (politiciansRow["EmailAddr"].ToString() != string.Empty)
            reportPoliticians1 += " <strong>Email Address:</strong> " + politiciansRow["EmailAddr"];
          if (politiciansRow["StateEmailAddr"].ToString() != string.Empty)
            reportPoliticians1 += " <strong>State Email Address:</strong> " +
              politiciansRow["StateEmailAddr"];
          if (politiciansRow["EmailAddrVoteUSA"].ToString() != string.Empty)
            reportPoliticians1 += " <strong>Vote-USA Email Address:</strong> " +
              politiciansRow["EmailAddrVoteUSA"];
          if (politiciansRow["WebAddr"].ToString() != string.Empty)
            reportPoliticians1 += " <strong>Web Address:</strong> " + politiciansRow["WebAddr"];
          if (politiciansRow["StateWebAddr"].ToString() != string.Empty)
            reportPoliticians1 += " <strong>State Web Address:</strong> " +
              politiciansRow["StateWebAddr"];
        }
        Msg.Text = Message("<br><br>Click on a politician link to get the email text.");
      }
      else
      {
        reportPoliticians1 = "<br><br>No politicians found.";
        Msg.Text = Message("<br><br>No politicians were found in this State with this last name.");
      }
      return reportPoliticians1;
    }

    protected static string Subsitutions_Politician_Find(string politicianKey,
      string strToApplySubsitutions)
    {
      var newStr = strToApplySubsitutions;
      newStr = Regex.Replace(newStr, @"\[\[USERNAME\]\]",
        Politicians_Str(politicianKey, "PoliticianKey"), RegexOptions.IgnoreCase);

      newStr = Regex.Replace(newStr, @"\[\[PASSWORD\]\]", Politicians_Str(politicianKey, "Password"),
        RegexOptions.IgnoreCase);

      var stateCode = Politicians.GetStateCodeFromKey(politicianKey).ToUpper();
      newStr = Regex.Replace(newStr, @"\[\[STATE\]\]", StateCache.GetStateName(stateCode),
        RegexOptions.IgnoreCase);

      newStr = Regex.Replace(newStr, @"\[\[VOTEXXANCHOR\]\]"
        //, db.Anchor(@"http://Vote-" + StateCode + ".org/")
        , Anchor(UrlManager.GetDefaultPageUri(stateCode)), RegexOptions.IgnoreCase);

      newStr = Regex.Replace(newStr, @"\[\[MGREMAIL\]\]", Anchor_Mailto_Email("mgr@Vote-USA.org"),
        RegexOptions.IgnoreCase);

      newStr = Regex.Replace(newStr, @"\[\[INTROANCHOR\]\]"
        //, db.Anchor(@"http://Vote-" + StateCode + ".org/Intro.aspx?Id=" + db.Politicians_Str(PoliticianKey, "PoliticianKey"))
        , Anchor(UrlManager.GetIntroPageUri(politicianKey)), RegexOptions.IgnoreCase);

      newStr = Regex.Replace(newStr, @"\[\[POLITICIANENTRY\]\]"
        //, db.Anchor(@"http://Vote-" + StateCode + ".org/Politician")
        , Anchor(UrlManager.GetStateUri(stateCode) + "Politician"), RegexOptions.IgnoreCase);
      return newStr;
    }

    protected void ButtonFindPolitician_Click(object sender, EventArgs e)
    {
      try
      {
        CheckForDangerousInput(TextBoxStateCode, TextBoxLastName);

        ReportPoliticians1.Text = PoliticiansReport(TextBoxStateCode.Text.Trim(),
          TextBoxLastName.Text.Trim());
        Report1.Text = string.Empty;
        Report2.Text = string.Empty;
        LabelPoliticianPage.Text = string.Empty;
        LabelSendEmail.Text = string.Empty;
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    protected void TextBox_Email_TextChanged(object sender, EventArgs e)
    {
      try
      {
        CheckForDangerousInput(TextBox_Email);

        var email = Validation.StripWebProtocol(TextBox_Email.Text.Trim());
        TextBox_Email.Text = email;

        var columnName = Politician_Update_Transaction_Str(_PoliticianKey,
          PoliticianColumn.CampaignEmail, email);

        Msg.Text = Ok(email + " has been recorded as the politician's " + columnName);
      }
      catch (Exception ex)
      {
        #region

        Msg.Text = Fail(ex.Message);
        LogAdminError(ex);

        #endregion
      }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      _PoliticianKey = QueryId;

      if (!IsPostBack)
      {
        Page.Title = "Find Politician";
        if (!IsMasterUser)
          HandleSecurityException();

        try
        {
          if (_PoliticianKey != string.Empty)
          {
            #region politician selected

            TextBoxStateCode.Text = Politicians.GetStateCodeFromKey(_PoliticianKey);

            TextBoxLastName.Text = Politicians_Str(_PoliticianKey, "LName");

            //Report1.Text = Subsitutions_Politician_Find(_PoliticianKey,
            //  DB.Vote.Master.GetReport1()).ReplaceNewLinesWithBreakTags();

            //Report2.Text = Subsitutions_Politician_Find(_PoliticianKey,
            // DB.Vote.Master.GetReport2()).ReplaceNewLinesWithBreakTags();

            // get the templates
            var template1 =
              EmailTemplates.GetDataByNameOwnerTypeOwner("Candidate Credentials 1", "U", "SpecialTemplates")[
                0];

            var template2 =
              EmailTemplates.GetDataByNameOwnerTypeOwner("Candidate Credentials 2", "U", "SpecialTemplates")[
                0];

            var substitutions = new Substitutions()
            {
              PoliticianKey = _PoliticianKey
            };

            var subject1 = substitutions.Substitute(template1.Subject);
            var body1 = substitutions.Substitute(template1.Body);
            Report1.Text = body1;

            var subject2 = substitutions.Substitute(template2.Subject);
            var body2 = substitutions.Substitute(template2.Body);
            Report2.Text = body2;

            LabelPoliticianPage.Text = "Politician's Intro.aspx Page: ";
            LabelPoliticianPage.Text +=
              Anchor_Politician_Intro_HappyFace(_PoliticianKey);
            LabelSendEmail.Text = "Send Email to: ";
            LabelSendEmail.Text +=
              UrlAddressEmail(
                Anchor_Mailto_Email(Politicians_Str(_PoliticianKey,
                  "EmailAddr")));

            TableEmail.Visible = true;

            #endregion politician selected
          }
          else
          {
            #region First time form is presented

            TextBoxStateCode.Text = string.Empty;
            TextBoxLastName.Text = string.Empty;
            ReportPoliticians1.Text = string.Empty;
            Report1.Text = string.Empty;
            Report2.Text = string.Empty;
            LabelPoliticianPage.Text = string.Empty;
            LabelSendEmail.Text = string.Empty;

            TableEmail.Visible = false;

            #endregion First time form is presented
          }
        }
        catch (Exception ex)
        {
          #region

          Msg.Text = Fail(ex.Message);
          LogAdminError(ex);

          #endregion
        }
      }
    }
  }
}