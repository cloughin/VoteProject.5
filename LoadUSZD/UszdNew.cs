using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using DB.VoteZipNewLocal;

namespace LoadUSZD
{
  public static class UszdNew
  {
 
    public static void TruncateTable()
    {
      TruncateTable(-1);
    }

    public static void TruncateTable(int commandTimeout)
    {
      string cmdText = "TRUNCATE TABLE USZDNew";
      DbCommand cmd = VoteZipNewLocalDb.GetCommand(cmdText, commandTimeout);
      VoteZipNewLocalDb.ExecuteNonQuery(cmd);
    }
  }
}
