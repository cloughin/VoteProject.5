using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using DoubleMetaphone;

namespace TestRedirect
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      //Form form = new TestRedirectForm(); // use appSettings "VoteUseLiveDomains" = true
      //Form form = new TestFindAddress(); // use appSettings "VoteUseLiveDomains" = false
      //Form form = new FixPoliticianKeys(); // use appSettings "VoteUseLiveDomains" = false
      //Form form = new TestFindAddressWithTestData(); //  use appSettings "VoteUseLiveDomains" = false
      //Form form = new BuildZipSingleUSZD();
      //Form form = new AnalyzeStreets();
      //Form form = new POBoxes();
      //Form form = new TestSmtp();
      //Form form = new XmlTest();
      //Form form = new Instructions();
      Form form = new Cache();

      Application.Run(form);
    }
  }
}
