using System;

namespace DB.Vote
{
  public partial class DebugLog
  {
    public static int Insert(string messageType, string message)
    {
      return Insert(DateTime.UtcNow, messageType, message);
    }
  }
}