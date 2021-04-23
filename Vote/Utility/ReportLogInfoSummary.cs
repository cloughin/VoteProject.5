using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB.VoteLog;

namespace Vote
{
  public static class ReportLogInfoSummary
  {
    public static void Report()
    {
      // Note: this method is so long because it uses an anonymous type 
      // that is needed throughout...
      // I would normally break it into smaller methods.

      // These limits are to protect against code gone wild, so we limit
      // the length of the email.
      const int maxSources = 30;
      const int maxLinesPerSource = 30;

      // Get yesterday's activity and group it by source
      var date = DateTime.UtcNow.Date.AddDays(-1);
      var sources = LogInfo.GetDataByDate(date)
        .GroupBy(row => row.Source)
        .ToList();
      var extraSources = Math.Max(0, sources.Count - maxSources);

      // Create the summary line. We turn this into an array so we can "iterate" 
      // it to create the outputLines list with an anonymous type that we can 
      // later add to.
      var sourceSummary = new[]
        {
          $"Source count: {sources.Count}" +
          $" {(extraSources > 0 ? $"- {extraSources} were skipped" : string.Empty)}"
        };

      var outputLines =
        sourceSummary.Select(
          l =>
            new
              {
                Source = null as string,
                Time = null as string,
                Machine = null as string,
                Message = l,
                IsError = extraSources > 0
              })
          .ToList();

      foreach (var source in sources.Take(maxSources))
      {
        var lines = source.ToList();
        var sourceKey = source.Key;
        var errorLines = new List<LogInfoRow>();
        switch (source.Key)
        {
          case "CleanUpTempEmailBatches":
            {
              var matches = 0;
              var batches = 0;
              var rows = 0;
              errorLines.AddRange(lines.Where(l =>
              {
                if (l.Message == "Started") return false;
                var match = Regex.Match(l.Message,
                  @"(?<batches>\d+) TempEmailBatches deleted," +
                    @" (?<rows>\d+) TempEmail rows deleted");
                if (!match.Success) return true;
                matches++;
                batches += int.Parse(match.FirstCapture("batches"));
                rows += int.Parse(match.FirstCapture("rows"));
                return false;
              })
                .Take(maxLinesPerSource));
              if (matches == 0)
                outputLines.Add(
                  new
                  {
                    Source = sourceKey,
                    Time = string.Empty,
                    Machine = string.Empty,
                    Message = "No matching log lines found",
                    IsError = true
                  });
              else
                outputLines.Add(
                  new
                  {
                    Source = sourceKey,
                    Time = "SUMMARY",
                    Machine = string.Empty,
                    Message =
                    $"{batches} TempEmailBatches deleted," + $" {rows} TempEmail rows deleted",
                    IsError = false
                  });
            }
            break;

          case "CommonCacheInvalidation":
            {
              var matches = 0;
              var processed = 0;
              var deleted = 0;
              errorLines.AddRange(lines.Where(l =>
                {
                  if (l.Message == "Started") return false;
                  var match = Regex.Match(l.Message,
                    @"(?<processed>\d+) CacheInvalidation rows processed," +
                      @" (?<deleted>\d+) Page rows deleted");
                  if (!match.Success) return true;
                  matches++;
                  processed += int.Parse(match.FirstCapture("processed"));
                  deleted += int.Parse(match.FirstCapture("deleted"));
                  return false;
                })
                .Take(maxLinesPerSource));
              if (matches == 0)
                outputLines.Add(
                  new
                    {
                      Source = sourceKey,
                      Time = string.Empty,
                      Machine = string.Empty,
                      Message = "No matching log lines found",
                      IsError = true
                    });
              else
                outputLines.Add(
                  new
                    {
                      Source = sourceKey,
                      Time = "SUMMARY",
                      Machine = string.Empty,
                      Message =
                      $"{processed} CacheInvalidation rows processed," +
                      $" {deleted} Page rows deleted",
                      IsError = false
                    });
            }
            break;

          case "ImageCaching":
            {
              var matches = 0;
              var browser = 0;
              var memory = 0;
              var disc = 0;
              errorLines.AddRange(lines.Where(l =>
                {
                  var match = Regex.Match(l.Message,
                    @"(?<browser>\d+) from browser cache," +
                      @" (?<memory>\d+) from memory cache," +
                      @" (?<disc>\d+) from disc");
                  if (!match.Success) return true;
                  matches++;
                  browser += int.Parse(match.FirstCapture("browser"));
                  memory += int.Parse(match.FirstCapture("memory"));
                  disc += int.Parse(match.FirstCapture("disc"));
                  return false;
                })
                .Take(maxLinesPerSource));
              if (matches == 0)
                outputLines.Add(
                  new
                    {
                      Source = sourceKey,
                      Time = string.Empty,
                      Machine = string.Empty,
                      Message = "No matching log lines found",
                      IsError = true
                    });
              else
                outputLines.Add(
                  new
                    {
                      Source = sourceKey,
                      Time = "SUMMARY",
                      Machine = string.Empty,
                      Message =
                        $"{browser} from browser cache, {memory} from memory cache," +
                        $" {disc} from disc, {browser + memory + disc} total politician images served",
                      IsError = false
                    });
            }
            break;

          case "PageCaching":
            {
              var matches = 0;
              var memory = 0;
              var local = 0;
              var common = 0;
              var created = 0;
              errorLines.AddRange(lines.Where(l =>
                {
                  var match = Regex.Match(l.Message,
                    @"(?<memory>\d+) from memory cache," +
                      @" (?<local>\d+) from local cache," +
                      @" (?<common>\d+) from common cache," +
                      @" (?<created>\d+) created");
                  if (!match.Success) return true;
                  matches++;
                  memory += int.Parse(match.FirstCapture("memory"));
                  local += int.Parse(match.FirstCapture("local"));
                  common += int.Parse(match.FirstCapture("common"));
                  created += int.Parse(match.FirstCapture("created"));
                  return false;
                })
                .Take(maxLinesPerSource));
              if (matches == 0)
                outputLines.Add(
                  new
                    {
                      Source = sourceKey,
                      Time = string.Empty,
                      Machine = string.Empty,
                      Message = "No matching log lines found",
                      IsError = true
                    });
              else
                outputLines.Add(
                  new
                    {
                      Source = sourceKey,
                      Time = "SUMMARY",
                      Machine = string.Empty,
                      Message =
                        $"{memory} from memory cache, {local} from local cache," +
                        $" {common} from common cache, {created} created," +
                        $" {memory + local + common + created} total pages served",
                      IsError = false
                    });
            }
            break;

          default:
            {
              var extraLines = Math.Max(0, lines.Count - maxLinesPerSource);
              outputLines.AddRange(lines.Take(maxLinesPerSource)
                .Select(
                  l =>
                    new
                      {
                        Source = sourceKey,
                        Time = l.DateStamp.ToShortTimeString(),
                        l.Machine,
                        l.Message,
                        IsError = false
                      }));
              if (extraLines > 0)
                outputLines.Add(
                  new
                    {
                      Source = sourceKey,
                      Time = string.Empty,
                      Machine = string.Empty,
                      Message = $"{extraLines} more",
                      IsError = true
                    });
            }
            break;
        }

        // Add any error lines to the output. There normally won't be any.
        outputLines.AddRange(
          errorLines.Select(
            l =>
              new
                {
                  Source = sourceKey,
                  Time = l.DateStamp.ToShortTimeString(),
                  l.Machine,
                  l.Message,
                  IsError = true
                }));
      }

      // Group the output lines and turn them into an HtmlTable for emailing.
      // Normally we would put the attributes in a css style sheet, but this
      // is for email.
      var htmlTable = new HtmlTable
        {
          CellSpacing = 0,
          CellPadding = 3,
          Border = 1,
          BorderColor = "#ccc"
        };
      htmlTable.Style.Add(HtmlTextWriterStyle.BorderCollapse, "collapse");
      htmlTable.Style.Add(HtmlTextWriterStyle.FontFamily, "arial");
      htmlTable.Style.Add(HtmlTextWriterStyle.FontSize, "8pt");
      var tr = new HtmlTableRow().AddTo(htmlTable);

      // Add the heading row
      var td = new HtmlTableCell {InnerHtml = "Source"}.AddTo(tr);
      td.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
      td = new HtmlTableCell {InnerHtml = "Time"}.AddTo(tr);
      td.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
      td = new HtmlTableCell {InnerHtml = "Machine"}.AddTo(tr);
      td.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
      td = new HtmlTableCell {InnerHtml = "Message"}.AddTo(tr);
      td.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");

      // Add each source to the output table
      foreach (var source in outputLines.GroupBy(l => l.Source))
      {
        var sourceText = source.Key;
        foreach (var line in source)
        {
          tr = new HtmlTableRow().AddTo(htmlTable);
          if (line.IsError)
            tr.Style.Add(HtmlTextWriterStyle.Color, "red");
          if (sourceText == null)
            new HtmlTableCell {InnerHtml = line.Message, ColSpan = 4}.AddTo(tr);
          else
          {
            new HtmlTableCell {InnerHtml = sourceText}.AddTo(tr);
            new HtmlTableCell {InnerHtml = line.Time}.AddTo(tr);
            new HtmlTableCell {InnerHtml = line.Machine}.AddTo(tr);
            new HtmlTableCell {InnerHtml = line.Message}.AddTo(tr);
          }
          sourceText = "&nbsp;";
        }
      }

      SendEmails(htmlTable, date);
    }

    private static void SendEmails(Control report, DateTime date)
    {
      var emailAddresses = LogControl.GetReportEmailAddresses();
      if (string.IsNullOrWhiteSpace(emailAddresses)) return;

      var mailMessage = new MailMessage
        {
          IsBodyHtml = true,
          From = new MailAddress("mgr@vote-usa.org", "VoteUSA servers"),
          Subject = $"Info Log Summary for {date.ToShortDateString()}",
          Body = report.RenderToString()
        };
      mailMessage.To.Add(emailAddresses);
      //new SmtpClient("localhost").Send(mailMessage);
      EmailUtility.GetConfiguredSmtpClient().Send(mailMessage);
    }
  }
}