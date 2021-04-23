using System;
using Vote;

namespace CreateSitemaps
{
  static class Program
  {
    // ReSharper disable once UnusedParameter.Local
    private static void Main(string[] args)
    {
      Console.WriteLine("Begin CreateSitemaps");
      //Console.WriteLine($"{SitemapManager.UpdateAllSitemapVirtualPages()} pages written");
      Console.WriteLine($"{SitemapManager.UpdateSitemapVirtualPage()} pages written");
      Console.WriteLine("End CreateSitemaps");
    }
  }
}
