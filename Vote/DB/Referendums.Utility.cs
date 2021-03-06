using Vote;

namespace DB.Vote
{
  public partial class Referendums
  {
    public static string GetStateCodeFromKey(string referendumKey)
    {
      if ((referendumKey == null) || (referendumKey.Length < 2)) return string.Empty;
      return referendumKey.Substring(0, 2)
        .ToUpperInvariant();
    }

    //public static bool IsValid(string electionKey, string referendumKey)
    //{
    //  if (string.IsNullOrWhiteSpace(electionKey) ||
    //    string.IsNullOrWhiteSpace(referendumKey)) return false;
    //  return VotePage.GetPageCache().Referendums.Exists(electionKey, referendumKey);
    //}
  }
}