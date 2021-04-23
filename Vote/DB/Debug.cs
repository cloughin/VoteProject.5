using System;

namespace DB.Vote
{
  public partial class DebugLogRow
  {
  }

  public partial class DebugLog
  {
    public static int Insert(string message)
    {
      return Insert(DateTime.UtcNow, "Default", message);
    }

    public static int Insert(string messageType, string message)
    {
      return Insert(DateTime.UtcNow, messageType, message);
    }
  }
}