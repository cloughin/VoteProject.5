using System;
using DB.Vote;

namespace DB.VoteZipNewLocal
{
  public partial class ZipCitiesDownloadedRow
  {
  }

  public partial class ZipCitiesDownloaded
  {
    public static ZipCitiesDownloadedTable GetCityAliasesDataLikeCityAliasName(
      string cityAliasName)
    {
      return GetCityAliasesDataLikeCityAliasName(cityAliasName, -1);
    }

    public static ZipCitiesDownloadedTable GetCityAliasesDataLikeCityAliasName(
      string cityAliasName, int commandTimeout)
    {
      var cmdText = SelectCityAliasesCommandText +
        " WHERE CityAliasName LIKE @CityAliasName";
      var cmd = VoteZipNewLocalDb.GetCommand(cmdText, commandTimeout);
      VoteZipNewLocalDb.AddCommandParameter(cmd, "CityAliasName", cityAliasName);
      return FillTable(cmd, ZipCitiesDownloadedTable.ColumnSet.CityAliases);
    }

    public static ZipCitiesDownloadedTable GetCityAliasesDataByStateLikeCityAliasName
      (string state, string cityAliasName)
    {
      return GetCityAliasesDataByStateLikeCityAliasName(state, cityAliasName, -1);
    }

    public static ZipCitiesDownloadedTable GetCityAliasesDataByStateLikeCityAliasName
      (string state, string cityAliasName, int commandTimeout)
    {
      const string cityCondition = " CityAliasName LIKE @CityAliasName ";

      var cmdText = SelectCityAliasesCommandText + " WHERE State=@State AND" +
        cityCondition;
      var cmd = VoteZipNewLocalDb.GetCommand(cmdText, commandTimeout);
      VoteZipNewLocalDb.AddCommandParameter(cmd, "State", state);
      VoteZipNewLocalDb.AddCommandParameter(cmd, "CityAliasName", cityAliasName);
      return FillTable(cmd, ZipCitiesDownloadedTable.ColumnSet.CityAliases);
    }

    public static string GetFirstStateByZipCode(string zipCode)
    {
      var cmdText = "SELECT State FROM ZipCitiesDownloaded WHERE ZipCode=@ZipCode";
      cmdText = VoteDb.InjectSqlLimit(cmdText, 1);
      var cmd = VoteZipNewLocalDb.GetCommand(cmdText, -1);
      VoteZipNewLocalDb.AddCommandParameter(cmd, "ZipCode", zipCode);
      var result = VoteZipNewLocalDb.ExecuteScalar(cmd);
      if ((result == null) || (result == DBNull.Value)) return null;
      return result as string;
    }

    public static string GetPrimaryCityByZipCode(string zipCode)
    {
      const string cmdText = "SELECT City FROM ZipCitiesDownloaded" +
        " WHERE ZipCode=@ZipCode" + "   AND PrimaryRecord='P'";
      var cmd = VoteZipNewLocalDb.GetCommand(cmdText, -1);
      VoteZipNewLocalDb.AddCommandParameter(cmd, "ZipCode", zipCode);
      var result = VoteZipNewLocalDb.ExecuteScalar(cmd);
      if ((result == null) || (result == DBNull.Value)) return null;
      return result as string;
    }
  }
}