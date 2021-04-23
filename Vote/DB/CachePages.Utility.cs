namespace DB.VoteCache
{
  public partial class CachePages
  {
    public static string DisplayExpiration(int expirationMinutes)
    {
      if (expirationMinutes <= 5)
        return "five minutes";
      if (expirationMinutes <= 15)
        return "fifteen minutes";
      if (expirationMinutes <= 60)
        return "one hour";
      if (expirationMinutes <= 360)
        return "six hours";
      if (expirationMinutes <= 720)
        return "twelve hours";
      return expirationMinutes <= 1440 ? "twenty-four hours" : "several days";
    }
  }
}