using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;
using static System.String;

namespace DB.Vote
{
  public partial class BannerAds
  {
    public static DataRow GetBannerAdInfo(string adType, string stateCode,
      string electionKey, string officeKey)
    {
      const string cmdText = "SELECT NOT AdImage IS NULL AS HasAdImage,AdImageName,AdUrl,AdEnabled," +
        "AdMediaType,AdYouTubeUrl,AdDescription1,AdDescription2,AdDescriptionUrl,AdIsPaid" +
        " FROM BannerAds" +
        " WHERE AdType = @adType AND StateCode = @stateCode AND ElectionKey = @electionKey" +
        " AND OfficeKey = @officeKey";

      var cmd = VoteDb.GetCommand(cmdText);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "adType", adType);
        VoteDb.AddCommandParameter(cmd, "stateCode", stateCode);
        VoteDb.AddCommandParameter(cmd, "electionKey", electionKey);
        VoteDb.AddCommandParameter(cmd, "officeKey", officeKey);
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.Count == 1 ? table.Rows[0] : null;
      }
    }

    public static void Upsert(string adType, string stateCode, string electionKey,
      string officeKey, string adImageName, string adUrl, bool adEnabled, string adMediaType,
      string adYouTubeUrl, string adDescription1, string adDescription2,
      string adDescriptionUrl, bool adIsPaid, byte[] file)
    {
      // with file
      if (IsNullOrWhiteSpace(adYouTubeUrl)) adYouTubeUrl = null;
      if (IsNullOrWhiteSpace(adDescription1)) adDescription1 = null;
      if (IsNullOrWhiteSpace(adDescription2)) adDescription2 = null;
      if (IsNullOrWhiteSpace(adDescriptionUrl)) adDescriptionUrl = null;
      //if (file == null)
      //  Upsert(adType, stateCode, electionKey, officeKey, adImageName,adUrl, adEnabled,
      //    adMediaType, adYouTubeUrl, adDescription1, adDescription2, adDescriptionUrl,
      //    adIsPaid);

      const string cmdText = 
        "INSERT INTO BannerAds (AdType,StateCode,ElectionKey,OfficeKey,AdImage,AdImageName," +
        "AdUrl,AdEnabled,AdMediaType,AdYouTubeUrl,AdDescription1,AdDescription2," +
        "AdDescriptionUrl,AdIsPaid)" + 
        " VALUES(@AdType,@StateCode,@ElectionKey,@OfficeKey,@AdImage,@AdImageName,@AdUrl," +
        "@AdEnabled,@AdMediaType,@AdYouTubeUrl,@AdDescription1,@AdDescription2," +
        "@AdDescriptionUrl,@AdIsPaid)" +
        " ON DUPLICATE KEY UPDATE AdImage=VALUES(AdImage),AdImageName=VALUES(AdImageName)," +
        "AdUrl=VALUES(AdUrl),AdEnabled=VALUES(AdEnabled),AdMediaType=VALUES(AdMediaType)," +
        "AdYouTubeUrl=VALUES(AdYouTubeUrl),AdDescription1=VALUES(AdDescription1)," +
        "AdDescription2=VALUES(AdDescription2),AdDescriptionUrl=VALUES(AdDescriptionUrl)," +
        "AdIsPaid=VALUES(AdIsPaid)";

      var cmd = VoteDb.GetCommand(cmdText);
      VoteDb.AddCommandParameter(cmd, "AdType", adType);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
      VoteDb.AddCommandParameter(cmd, "OfficeKey", officeKey);
      VoteDb.AddCommandParameter(cmd, "AdImage", file);
      VoteDb.AddCommandParameter(cmd, "AdImageName", adImageName);
      VoteDb.AddCommandParameter(cmd, "AdUrl", adUrl);
      VoteDb.AddCommandParameter(cmd, "AdEnabled", adEnabled);
      VoteDb.AddCommandParameter(cmd, "AdMediaType", adMediaType);
      VoteDb.AddCommandParameter(cmd, "AdYouTubeUrl", adYouTubeUrl);
      VoteDb.AddCommandParameter(cmd, "AdDescription1", adDescription1);
      VoteDb.AddCommandParameter(cmd, "AdDescription2", adDescription2);
      VoteDb.AddCommandParameter(cmd, "AdDescriptionUrl", adDescriptionUrl);
      VoteDb.AddCommandParameter(cmd, "AdIsPaid", adIsPaid);
      VoteDb.ExecuteScalar(cmd);
    }

    public static void Upsert(string adType, string stateCode, string electionKey,
      string officeKey, string adImageName, string adUrl, bool adEnabled, string adMediaType,
      string adYouTubeUrl, string adDescription1, string adDescription2,
      string adDescriptionUrl, bool adIsPaid)
    {
      // no file

      const string cmdText =
        "INSERT INTO BannerAds (AdType,StateCode,ElectionKey,OfficeKey,AdImageName," +
        "AdUrl,AdEnabled,AdMediaType,AdYouTubeUrl,AdDescription1,AdDescription2," +
        "AdDescriptionUrl,AdIsPaid)" +
        " VALUES(@AdType,@StateCode,@ElectionKey,@OfficeKey,@AdImageName,@AdUrl," +
        "@AdEnabled,@AdMediaType,@AdYouTubeUrl,@AdDescription1,@AdDescription2," +
        "@AdDescriptionUrl,@AdIsPaid)" +
        " ON DUPLICATE KEY UPDATE AdImageName=VALUES(AdImageName)," +
        "AdUrl=VALUES(AdUrl),AdEnabled=VALUES(AdEnabled),AdMediaType=VALUES(AdMediaType)," +
        "AdYouTubeUrl=VALUES(AdYouTubeUrl),AdDescription1=VALUES(AdDescription1)," +
        "AdDescription2=VALUES(AdDescription2),AdDescriptionUrl=VALUES(AdDescriptionUrl)," +
        "AdIsPaid=VALUES(AdIsPaid)";

      var cmd = VoteDb.GetCommand(cmdText);
      VoteDb.AddCommandParameter(cmd, "AdType", adType);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
      VoteDb.AddCommandParameter(cmd, "OfficeKey", officeKey);
      VoteDb.AddCommandParameter(cmd, "AdImageName", adImageName);
      VoteDb.AddCommandParameter(cmd, "AdUrl", adUrl);
      VoteDb.AddCommandParameter(cmd, "AdEnabled", adEnabled);
      VoteDb.AddCommandParameter(cmd, "AdMediaType", adMediaType);
      VoteDb.AddCommandParameter(cmd, "AdYouTubeUrl", adYouTubeUrl);
      VoteDb.AddCommandParameter(cmd, "AdDescription1", adDescription1);
      VoteDb.AddCommandParameter(cmd, "AdDescription2", adDescription2);
      VoteDb.AddCommandParameter(cmd, "AdDescriptionUrl", adDescriptionUrl);
      VoteDb.AddCommandParameter(cmd, "AdIsPaid", adIsPaid);
      VoteDb.ExecuteScalar(cmd);
    }
  }
}