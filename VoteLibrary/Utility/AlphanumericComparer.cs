using System.Collections.Generic;
using Vote;

namespace VoteLibrary.Utility
{
  public class AlphanumericComparer : IComparer<string>
  {
    public int Compare(string x, string y)
    {
      return x.CompareAlphanumeric(y);
    }
  }
}
