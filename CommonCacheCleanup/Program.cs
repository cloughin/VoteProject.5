using System;

namespace CommonCacheCleanup
{
  internal static class Program
  {
    // ReSharper disable once UnusedParameter.Local
    private static void Main(string[] args)
    {
      Console.WriteLine("Begin CommonCacheCleanup");
      Vote.CommonCacheInvalidation.CleanUpCacheInvalidation();
      DB.Vote.TempEmailBatches.CleanUpTempEmailBatches();
      Console.WriteLine("End CommonCacheCleanup");
    }
  }
}
