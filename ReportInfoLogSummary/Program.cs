using System;

namespace ReportInfoLogSummary
{
  static class Program
  {
    // ReSharper disable once UnusedParameter.Local
    private static void Main(string[] args)
    {
      Console.WriteLine("Begin ReportLogInfoSummary");
      Vote.ReportLogInfoSummary.Report();
      Console.WriteLine("End ReportLogInfoSummary");
      Console.WriteLine("Begin ReportPoliticianSignins");
      Vote.ReportPoliticianSignins.Report();
      Console.WriteLine("End ReportPoliticianSignins");
    }
  }
}
