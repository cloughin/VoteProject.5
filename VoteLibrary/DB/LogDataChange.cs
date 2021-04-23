using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DB.Vote;
using Vote;
using static System.String;

namespace DB.VoteLog
{
  public partial class LogDataChange
  {
    #region Private

    private static void Log(string tableName, string columnName, string oldValue,
      string newValue, string additionalInfo, string userName, string userSecurity,
      DateTime dateStamp, bool isBase64, params object[] keys)
    {
      Log(tableName, columnName, oldValue, newValue, null, additionalInfo, userName,
        userSecurity, dateStamp, isBase64, keys);
    }

    private static void Log(string tableName, string columnName, string oldValue,
      string newValue, string program, string additionalInfo, string userName,
      string userSecurity, DateTime dateStamp, bool isBase64, params object[] keys)
    {
      var keyString = JoinKeys(keys);
      Insert(dateStamp, userName, userSecurity, tableName, columnName, keyString, oldValue,
        newValue, program, isBase64, additionalInfo);
    }

    #endregion Private

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnassignedField.Global

    public class LoggedImageInfo
    {
      public DateTime DateStamp;
      public string UserName;
    }

    public static LogDataChangeTable GetBillingDataByUserNameTableNameDateStampRange(
      string userName, string tableName, DateTime lowDate, DateTime highDate,
      int commandTimeout = -1)
    {
      var cmdText = SelectBillingCommandText +
        " WHERE UserName=@UserName AND TableName=@TableName AND" +
        " DateStamp>=@LowDate AND DateStamp<=@HighDate AND Program IS NULL" +
        " ORDER BY DateStamp ASC";
      var cmd = VoteLogDb.GetCommand(cmdText, commandTimeout);
      VoteLogDb.AddCommandParameter(cmd, "UserName", userName);
      VoteLogDb.AddCommandParameter(cmd, "TableName", tableName);
      VoteLogDb.AddCommandParameter(cmd, "LowDate", lowDate);
      VoteLogDb.AddCommandParameter(cmd, "HighDate", highDate);
      return FillTable(cmd, LogDataChangeTable.ColumnSet.Billing);
    }

    public static DateTime GetSecondLatestProfileImageDate(string politicianKey,
      out string userName)
    {
      var cmdText = "SELECT DateStamp,UserName FROM LogDataChange" +
        " WHERE TableName='PoliticiansImagesBlobs' AND ColumnName='ProfileOriginal'" +
        " AND KeyValues=@PoliticianKey ORDER BY DateStamp DESC";
      cmdText = VoteDb.InjectSqlLimit(cmdText, 2);
      var cmd = VoteLogDb.GetCommand(cmdText, -1);
      VoteLogDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
      var list = FillTable(cmd, LogDataChangeTable.ColumnSet.DateAndUser)
        .Select(row => new {row.DateStamp, row.UserName}).ToList();
      if (list.Count < 2)
      {
        userName = Empty;
        return VoteDb.DateTimeMin;
      }
      var item = list.OrderByDescending(o => o.DateStamp).Skip(1).First();
      userName = item.UserName;
      return item.DateStamp;
    }

    public static List<LoggedImageInfo> GetTwoLatestProfileImageInfos(string politicianKey)
    {
      var cmdText = "SELECT DateStamp,UserName FROM LogDataChange" +
        " WHERE TableName='PoliticiansImagesBlobs' AND ColumnName='ProfileOriginal'" +
        " AND KeyValues=@PoliticianKey ORDER BY DateStamp DESC";
      cmdText = VoteDb.InjectSqlLimit(cmdText, 2);
      var cmd = VoteLogDb.GetCommand(cmdText, -1);
      VoteLogDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
      var list = FillTable(cmd, LogDataChangeTable.ColumnSet.DateAndUser).Select(row =>
        new LoggedImageInfo {DateStamp = row.DateStamp, UserName = row.UserName}).ToList();
      return list.OrderByDescending(o => o.DateStamp).Take(2).ToList();
    }

    public static string JoinKeys(params object[] keys)
    {
      // concatenate keys with pipe
      if (keys.Length == 0) throw new ArgumentException("missing keys");
      return Join("|", keys.Select(key => key.ToString()));
    }

    public static void LogDelete(string tableName, string userName, string userSecurity,
      DateTime dateStamp, params object[] keys)
    {
      Log(tableName, "*DELETE", null, null, null, userName, userSecurity, dateStamp, false,
        keys);
    }

    public static void LogDelete(string tableName, string additionalInfo, string userName,
      string userSecurity, DateTime dateStamp, params object[] keys)
    {
      Log(tableName, "*DELETE", null, null, additionalInfo, userName, userSecurity,
        dateStamp, false, keys);
    }

    public static void LogDelete(string tableName, DateTime dateStamp, params object[] keys)
    {
      Log(tableName, "*DELETE", null, null, null, VotePage.UserName,
        SecurePage.UserSecurityClass, dateStamp, false, keys);
    }

    public static void LogDelete(string tableName, string additionalInfo,
      DateTime dateStamp, params object[] keys)
    {
      Log(tableName, "*DELETE", null, null, additionalInfo, VotePage.UserName,
        SecurePage.UserSecurityClass, dateStamp, false, keys);
    }

    public static void LogInsert(string tableName, string userName, string userSecurity,
      DateTime dateStamp, params object[] keys)
    {
      Log(tableName, "*INSERT", null, null, null, userName, userSecurity, dateStamp, false,
        keys);
    }

    public static void LogInsert(string tableName, string additionalInfo, string userName,
      string userSecurity, DateTime dateStamp, params object[] keys)
    {
      Log(tableName, "*INSERT", null, null, additionalInfo, userName, userSecurity,
        dateStamp, false, keys);
    }

    public static void LogInsert(string tableName, DateTime dateStamp, params object[] keys)
    {
      Log(tableName, "*INSERT", null, null, null, VotePage.UserName,
        SecurePage.UserSecurityClass, dateStamp, false, keys);
    }

    public static void LogInsert(string tableName, string additionalInfo,
      DateTime dateStamp, params object[] keys)
    {
      Log(tableName, "*INSERT", null, null, additionalInfo, VotePage.UserName,
        SecurePage.UserSecurityClass, dateStamp, false, keys);
    }

    public static void LogUpdate(string tableName, string columnName, string oldValue,
      string newValue, DateTime dateStamp, params object[] keys)
    {
      Log(tableName, columnName, oldValue, newValue, null, VotePage.UserName,
        SecurePage.UserSecurityClass, dateStamp, false, keys);
    }

    public static void LogUpdate(string tableName, string columnName, string oldValue,
      string newValue, string additionalInfo, DateTime dateStamp, params object[] keys)
    {
      Log(tableName, columnName, oldValue, newValue, additionalInfo, VotePage.UserName,
        SecurePage.UserSecurityClass, dateStamp, false, keys);
    }

    public static void LogUpdate(string tableName, string columnName, string oldValue,
      string newValue, string userName, string userSecurity, DateTime dateStamp,
      params object[] keys)
    {
      Log(tableName, columnName, oldValue, newValue, null, userName, userSecurity,
        dateStamp, false, keys);
    }

    public static void LogUpdate(string tableName, string columnName, string oldValue,
      string newValue, string additionalInfo, string userName, string userSecurity,
      DateTime dateStamp, params object[] keys)
    {
      Log(tableName, columnName, oldValue, newValue, additionalInfo, userName, userSecurity,
        dateStamp, false, keys);
    }

    public static void LogUpdate(Enum column, object oldValue, object newValue,
      DateTime dateStamp, params object[] keys)
    {
      LogUpdate(column, oldValue, newValue, null, VotePage.UserName,
        SecurePage.UserSecurityClass, dateStamp, keys);
    }

    public static void LogUpdate(Enum column, object oldValue, object newValue,
      string additionalInfo, DateTime dateStamp, params object[] keys)
    {
      LogUpdate(column, oldValue, newValue, additionalInfo, VotePage.UserName,
        SecurePage.UserSecurityClass, dateStamp, keys);
    }

    public static void LogUpdate(Enum column, object oldValue, object newValue,
      string userName, string userSecurity, DateTime dateStamp, params object[] keys)
    {
      LogUpdate(column, oldValue, newValue, null, userName, userSecurity, dateStamp, keys);
    }

    public static void LogUpdate(Enum column, object oldValue, object newValue,
      string additionalInfo, string userName, string userSecurity, DateTime dateStamp,
      params object[] keys)
    {
      // get column name and table name via reflection
      var tableType = column.GetType().DeclaringType;
      if (tableType == null)
        throw new ArgumentException("column is not an appropriate type");
      var tableName = tableType.GetField("TableName").GetValue(null) as string;
      var columnName =
        tableType.GetMethod("GetColumnName")?.Invoke(null, new object[] {column}) as string;

      // convert blobs to base64
      var oldBinary = oldValue != null && oldValue.GetType() == typeof (byte[]);
      var newBinary = newValue != null && newValue.GetType() == typeof (byte[]);
      if (oldBinary && !newBinary && newValue != null ||
        newBinary && !oldBinary && oldValue != null)
        throw new ArgumentException("mismatched binary values");
      if (oldBinary)
      {
        Debug.Assert(oldValue is byte[], "oldValue as byte[] != null");
        oldValue = Convert.ToBase64String((byte[]) oldValue);
      }
      if (newBinary)
      {
        Debug.Assert(newValue is byte[], "newValue as byte[] != null");
        newValue = Convert.ToBase64String((byte[]) newValue);
      }

      var oldString = oldValue?.ToString();
      var newString = newValue?.ToString();

      Log(tableName, columnName, oldString, newString, additionalInfo, userName,
        userSecurity, dateStamp, oldBinary | newBinary, keys);
    }

    public static string[] SplitKeys(string keys)
    {
      return keys.Split('|');
    }

    // ReSharper restore UnassignedField.Global
    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }
}