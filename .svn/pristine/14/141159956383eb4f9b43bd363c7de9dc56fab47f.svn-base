using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LoadZipCitiesDatabase
{
  static class Program
  {
    enum OutputType
    {
      DbOutput,
      CsvOutput
    }

    // Change to appropriate value
    static OutputType Type = OutputType.DbOutput;

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      switch (Type)
      {
        case OutputType.DbOutput:
          Application.Run(new DbMainForm());
          break;

        case OutputType.CsvOutput:
          Application.Run(new CsvMainForm());
          break;
      }
    }
  }
}
