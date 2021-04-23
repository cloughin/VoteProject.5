using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CopyIISLogFiles
{
  class Copier
  {
    static Regex FileRegex = new Regex(@"u_ex(?<yy>\d\d)(?<mm>\d\d)(?<dd>\d\d)\.log");

    string SourceFolder;
    string DestinationFolder;
    string CopiedFolder;
    DirectoryInfo SourceDi;
    DirectoryInfo DestinationDi;
    string ComputerName;
    DateTime Today;

    public Copier()
    {
      SourceFolder = ConfigurationManager.AppSettings["VoteLogSource"];
      DestinationFolder = ConfigurationManager.AppSettings["VoteLogDestination"];
      ComputerName = Environment.MachineName;
      DestinationFolder = Path.Combine(DestinationFolder, ComputerName);
      CopiedFolder = Path.Combine(SourceFolder, "Copied");
      SourceDi = new DirectoryInfo(SourceFolder);
      DestinationDi = new DirectoryInfo(DestinationFolder);
      DestinationDi.Create();
      Directory.CreateDirectory(CopiedFolder);
      Today = DateTime.Today;
    }

    private void CopyFile(FileInfo fi)
    {
      StreamWriter streamWriter = 
        new StreamWriter(Path.Combine(DestinationDi.FullName, fi.Name));
      using (IISLogFileReader reader = new IISLogFileReader(fi.FullName))
      {
        int count = 0;
        int computerNameOrdinal = -1;
        while (reader.Read())
        {
          foreach (string comment in reader.Comments)
            streamWriter.WriteLine(comment);
          // Because we clone machines all over the place, there is a lot
          // of potential for duplicated log file entries. We prevent this by 
          // only copying entries that belong to the current machine.
          if (reader.HasNewFieldList)
            computerNameOrdinal = reader.GetOrdinal("s-computername");
          if (computerNameOrdinal >= 0 && 
            reader.GetString(computerNameOrdinal)
            .Equals(ComputerName, StringComparison.OrdinalIgnoreCase))
          {
            streamWriter.WriteLine(string.Join(" ", reader.Values));
          }
          count++;
        }
      }
      streamWriter.Close();
    }

    public void CopyLogFiles()
    {
      FileInfo[] sourceFiles =
        SourceDi.GetFiles("u_ex??????.log", SearchOption.TopDirectoryOnly);

      foreach (FileInfo fi in sourceFiles)
      {
        try
        {
          Match match = FileRegex.Match(fi.Name);
          if (match.Success)
          {
            // These are guaranteed not to fail because of the successful match
            int yy = int.Parse(match.Groups["yy"].Captures[0].Value);
            int mm = int.Parse(match.Groups["mm"].Captures[0].Value);
            int dd = int.Parse(match.Groups["dd"].Captures[0].Value);
            DateTime fileDate = new DateTime(2000 + yy, mm, dd);
            if (fileDate < Today) // do not try to do today's
            {
              CopyFile(fi);
              MoveFile(fi);
            }
          }
        }
        catch { } // skip any files with errors
      }
    }

    private void MoveFile(FileInfo fi)
    {
      fi.MoveTo(Path.Combine(CopiedFolder, fi.Name));
    }
  }
}
