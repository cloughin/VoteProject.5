using System;
using Vote;

namespace DB.VoteLog
{
  public partial class Log404PageNotFound
  {
    public static void Log(string msg)
    {
      if (MemCache.IsLoggingErrors)
        Insert(DateTime.Now, VotePage.CurrentUrl, msg);
    }
  }
}