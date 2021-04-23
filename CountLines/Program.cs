using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AnalyzeLogFiles;
using System.Text.RegularExpressions;

namespace CountLines
{
  class Program
  {
    static string HostKey(string host)
    {
      Match match = Regex.Match(host, @"vote-(?<key>[a-z]{2,3})\.");
      if (match.Success)
        return match.Groups["key"].Captures[0].Value.ToUpperInvariant();
      else
        return "other";
    }

    static string HostSortCode(string hostKey)
    {
      if (hostKey == "USA")
        return "0";
      else if (hostKey == "other")
        return "2";
      else
        return "1";
    }

    static void WriteHosts(string path, Dictionary<string, int> hosts)
    {
      var data = hosts
        .GroupBy(kvp => HostKey(kvp.Key))
        .Select(g => new { HostKey = g.Key, Count = g.Sum(kvp => kvp.Value)})
        .OrderBy(o => HostSortCode(o.HostKey) + o.HostKey)
        .ToList();
      using (var output = new StreamWriter(path))
      {
        foreach (var o in data)
        {
          output.Write(o.HostKey);
          output.Write(',');
          output.WriteLine(o.Count);
        }
      }
    }

    static string PageKey(string page)
    {
      page = page.ToLowerInvariant();
      switch (page)
      {
        case "/webservice.asmx/":
          return "webservice.asmx";

        case "default.aspx":
          return "Home";

        case "/admin/":
        case "/articles/":
        case "/css/":
        case "/design/":
        case "/images/":
        case "/jq/":
        case "/js/":
        case "/master/":
        case "/organization/":
        case "/party/":
        case "/politician/":
        case "about.aspx":
        case "ballot.aspx":
        case "contact.aspx":
        case "donate.aspx":
        case "elected.aspx":
        case "election.aspx":
        case "forcandidates.aspx":
        case "forelectionauthorities.aspx":
        case "forpartners.aspx":
        case "forpoliticalparties.aspx":
        case "forresearch.aspx":
        case "forvolunteers.aspx":
        case "forvoters.aspx":
        case "image.aspx":
        case "intro.aspx":
        case "issue.aspx":
        case "issuelist.aspx":
        case "officials.aspx":
        case "ping.aspx":
        case "politicianissue.aspx":
        case "privacy.aspx":
        case "referendum.aspx":
        case "sampleballotbuttons.aspx":
        case "setcookies.aspx":
          return page;

        default:
          return "other";
      }
    }

    static string PageSortCode(string page)
    {
      if (page.StartsWith("/"))
        return "0";
      else if (page == "Home")
        return "1";
      else if (page == "other")
        return "3";
      else
        return "2";
    }

    static bool IsPublicPage(string page)
    {
      page = page.ToLowerInvariant();
      switch (page)
      {
        case "about.aspx":
        case "ballot.aspx":
        case "contact.aspx":
        case "default.aspx":
        case "donate.aspx":
        case "elected.aspx":
        case "election.aspx":
        case "forcandidates.aspx":
        case "forelectionauthorities.aspx":
        case "forpartners.aspx":
        case "forpoliticalparties.aspx":
        case "forresearch.aspx":
        case "forvolunteers.aspx":
        case "forvoters.aspx":
        case "intro.aspx":
        case "issue.aspx":
        case "issuelist.aspx":
        case "officials.aspx":
        case "politicianissue.aspx":
        case "privacy.aspx":
        case "referendum.aspx":
        case "sampleballotbuttons.aspx":
          return true;

        default:
          return false;
      }
    }

    static void WritePages(string path, Dictionary<string, int> pages)
    {
      var data = pages
        .GroupBy(kvp => PageKey(kvp.Key))
        .Select(g => new { PageKey = g.Key, Count = g.Sum(kvp => kvp.Value) })
        .OrderBy(o => PageSortCode(o.PageKey) + o.PageKey)
        .ToList();
      using (var output = new StreamWriter(path))
      {
        foreach (var o in data)
        {
          output.Write(o.PageKey);
          output.Write(',');
          output.WriteLine(o.Count);
        }
      }
    }

    static void WriteStatuses(string path, Dictionary<int, int> statuses)
    {
      var data = statuses
        .Select(kvp => new { Status = kvp.Key, Count = kvp.Value })
        .OrderBy(o => o.Status)
        .ToList();
      using (var output = new StreamWriter(path))
      {
        foreach (var o in data)
        {
          output.Write(o.Status);
          output.Write(',');
          output.WriteLine(o.Count);
        }
      }
    }

    static void Main(string[] args)
    {
      int total = 0;
      int totalFiltered = 0;
      int ivn = 0;
      int ivnFiltered = 0;
      Dictionary<string, int> pages = new Dictionary<string, int>();
      Dictionary<string, int> hosts = new Dictionary<string, int>();
      Dictionary<int, int> statuses = new Dictionary<int, int>();
      Dictionary<string, int> pagesFiltered = new Dictionary<string, int>();
      Dictionary<string, int> hostsFiltered = new Dictionary<string, int>();
      for (int n = 1; n <= 8; n++)
      {
        string path = string.Format(@"C:\Users\Curt\Documents\IIS Logs 2012-10-31\u_ex121031-0{0}.log",
          n);
        using (var reader = File.OpenText(path))
        {
          int lines = 0;
          int linesFiltered = 0;
          int linesIvn = 0;
          int linesIvnFiltered = 0;
          string[] fieldList = null;
          while (!reader.EndOfStream)
          {
            string line = reader.ReadLine().ToLowerInvariant();
            if (LogEntry.IsData(line, ref fieldList))
            {
              var logEntry = new LogEntry(fieldList, line);
              if (!logEntry.Path.StartsWith("/"))
                throw new Exception("uri exception");
              int value = 0;
              bool isIvn = logEntry.Query.IndexOf("site=ivn", StringComparison.OrdinalIgnoreCase) >= 0;
              if (!statuses.TryGetValue(logEntry.Status, out value))
                statuses.Add(logEntry.Status, 0);
              statuses[logEntry.Status] = value + 1;
              lines++;
              if (isIvn) linesIvn++;
              string page;
              string[] folders = logEntry.Path.Substring(1).Split('/');
              if (folders[0] == string.Empty)
                page = "Home";
              else if (folders.Length > 1)
                page = "/" + folders[0] + "/";
              else
                page = folders[0];
              int queryIndex = page.IndexOf('?');
              if (queryIndex >= 0)
                page = page.Substring(0, queryIndex);
              if (!pages.TryGetValue(page, out value))
                pages.Add(page, 0);
              pages[page] = value + 1;
              if (!hosts.TryGetValue(logEntry.HostName, out value))
                hosts.Add(logEntry.HostName, 0);
              hosts[logEntry.HostName] = value + 1;
              if (logEntry.Status != 200)
                continue;
              if (HostKey(logEntry.HostName) == "other")
                continue;
              if (!IsPublicPage(page))
                continue;
              linesFiltered++;
              if (isIvn) linesIvnFiltered++;
              if (!pagesFiltered.TryGetValue(page, out value))
                pagesFiltered.Add(page, 0);
              pagesFiltered[page] = value + 1;
              if (!hostsFiltered.TryGetValue(logEntry.HostName, out value))
                hostsFiltered.Add(logEntry.HostName, 0);
              hostsFiltered[logEntry.HostName] = value + 1;
            }
          }
          total += lines;
          totalFiltered += linesFiltered;
          ivn += linesIvn;
          ivnFiltered += linesIvnFiltered;
        }
      }
      WriteStatuses(@"c:\LogAnalysis\statuses-total.csv", statuses);
      WriteHosts(@"c:\LogAnalysis\hosts-total.csv", hosts);
      WritePages(@"c:\LogAnalysis\pages-total.csv", pages);
      WriteHosts(@"c:\LogAnalysis\hosts-filtered.csv", hostsFiltered);
      WritePages(@"c:\LogAnalysis\pages-filtered.csv", pagesFiltered);
    }
  }
}
