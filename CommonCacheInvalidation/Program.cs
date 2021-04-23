using System;

namespace CommonCacheInvalidation
{
  static class Program
  {
    // ReSharper disable once UnusedParameter.Local
    private static void Main(string[] args)
    {
      Console.WriteLine("Begin CommonCacheInvalidation");
      Vote.CommonCacheInvalidation.ProcessPendingTransactions();
      Console.WriteLine("End CommonCacheInvalidation");
    }
  }
}
