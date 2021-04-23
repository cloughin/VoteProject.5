using static System.String;

namespace DB.Vote
{
  public partial class Referendums
  {
    public static string GetStateCodeFromKey(string referendumKey)
    {
      if (referendumKey == null || referendumKey.Length < 2) return Empty;
      return referendumKey.Substring(0, 2)
        .ToUpperInvariant();
    }

    public static bool IsValidReferendumKey(string electionKey, string referendumKey)
    {
      return ElectionKeyReferendumKeyExists(electionKey,
        referendumKey);
    }
  }
}