using System;

namespace LocalCacheInvalidation
{
  static class Program
  {
    private static void Main(string[] args)
    {
      Console.WriteLine("Begin LocalCacheInvalidation");
      Vote.LocalCacheInvalidation.ProcessPendingTransactions();
      Console.WriteLine("End LocalCacheInvalidation");
    }
  }
}
