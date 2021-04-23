using System;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;
using DB;
using DB.Vote;
using DB.VoteLog;
using static System.String;

namespace Vote
{
  public static class ReportPoliticianSignins
  {
    public static void Report()
    {
      // Get yesterday's activity 
      var placeholder = new PlaceHolder();
      var date = DateTime.UtcNow.Date.AddDays(-1);
      var logins = LogLogins.GetPoliticianLoginsByDateStampRange(date).Rows.OfType<DataRow>()
        .ToList();

      var message = new HtmlP();
      message.AddTo(placeholder);
      message.Style.Add(HtmlTextWriterStyle.FontFamily, "arial");
      message.Style.Add(HtmlTextWriterStyle.FontSize, "8pt");
      if (logins.Count == 0)
      {
        message.InnerText = $"There were no politician sign-ins on {date:d}";
      }
      else
      {
        message.InnerText = $"These politicians signed in on {date:d}";
        var lines = logins.Select(r =>
        {
          var tr = new TableRow {TableSection = TableRowSection.TableBody};
          new TableCell { Text = r.StateCode() }.AddTo(tr);
          new TableCell { Text = r.PoliticianKey() }.AddTo(tr);
          new TableCell { Text = Politicians.FormatName(r)}.AddTo(tr);
          new TableCell { Text = Offices.FormatOfficeName(r) }.AddTo(tr);
          new TableCell { Text = r.ElectionDescription() }.AddTo(tr);
          new TableCell { Text = r.LiveOfficeStatus() }.AddTo(tr);
          foreach (var td in tr.Controls.OfType<TableCell>())
          {
            td.Style.Add(HtmlTextWriterStyle.BorderWidth, "1px");
            td.Style.Add(HtmlTextWriterStyle.BorderStyle, "solid");
            td.Style.Add(HtmlTextWriterStyle.BorderColor, "#cccccc");
            td.Style.Add(HtmlTextWriterStyle.Padding, "3px");
          }
          return tr;
        });

        var table = new Table();
        table.AddTo(placeholder);
        table.Style.Add(HtmlTextWriterStyle.BorderCollapse, "collapse");
        table.Style.Add(HtmlTextWriterStyle.FontFamily, "arial");
        table.Style.Add(HtmlTextWriterStyle.FontSize, "8pt");
        table.Style.Add(HtmlTextWriterStyle.MarginTop, "20px");

        // Add the heading row
        var thr = new TableHeaderRow();
        thr.AddTo(table);
        thr.TableSection = TableRowSection.TableHeader;
        thr.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
        thr.Style.Add(HtmlTextWriterStyle.Color, "#ffffff");
        thr.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#666666");
        new TableHeaderCell { Text = "State" }.AddTo(thr);
        new TableHeaderCell { Text = "Politician Key" }.AddTo(thr);
        new TableHeaderCell { Text = "Name" }.AddTo(thr);
        new TableHeaderCell { Text = "Office" }.AddTo(thr);
        new TableHeaderCell { Text = "Election" }.AddTo(thr);
        new TableHeaderCell { Text = "Status" }.AddTo(thr);
        foreach (var th in thr.Controls.OfType<TableHeaderCell>())
        {
          th.Style.Add(HtmlTextWriterStyle.BorderWidth, "1px");
          th.Style.Add(HtmlTextWriterStyle.BorderStyle, "solid");
          th.Style.Add(HtmlTextWriterStyle.BorderColor, "#666666");
          th.Style.Add(HtmlTextWriterStyle.Padding, "3px");
          th.Style.Add(HtmlTextWriterStyle.TextAlign, "left");
        }

        foreach (var line in lines)
          line.AddTo(table);
      }

      SendEmails(placeholder, date);
    }

    private static void SendEmails(Control report, DateTime date)
    {
      var emailAddresses = LogControl.GetPoliticianSigninEmailAddresses();
      if (IsNullOrWhiteSpace(emailAddresses)) return;

      var mailMessage = new MailMessage
        {
          IsBodyHtml = true,
          From = new MailAddress("mgr@vote-usa.org", "VoteUSA servers"),
          Subject = $"Politician Sign-in Summary for {date.ToShortDateString()}",
          Body = report.RenderToString()
        };
      mailMessage.To.Add(emailAddresses);
      EmailUtility.GetConfiguredSmtpClient().Send(mailMessage);
    }
  }
}