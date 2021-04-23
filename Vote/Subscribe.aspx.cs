using System;
using DB.Vote;
using static System.String;

namespace Vote
{
  public partial class SubscribePage : PublicPage
  {
    protected SubscribePage()
    {
      NoUrlEdit = true;
      NoIndex = true;
    }

    private const string TitleTag = "{0} | Subscribe and Unsubscribe";

    private void UnsubscribeAddresses(int id, string email, string op)
    {
      if (id == 0 || IsNullOrWhiteSpace(email) || op != "unsubscribe" && op != "ballots")
      {
        Message.InnerText = "The requested email address was not found";
        return;
      }

      var handled = true;
      if (Addresses.GetEmailById(id).SafeString().IsEqIgnoreCase(email))
      {
        if (op == "ballots")
        {
          Response.Redirect(UrlManager.GetSampleBallotEnrollmentPageUri(email).ToString());
          Response.End();
        }
        EmailUtility.UpdateSubscription(email, op);
      }
      else handled = false;
      if (!handled) return;
      switch (op)
      {
        case "unsubscribe":
          Message.InnerText =
            $"Email {email} has been unsubscribed from all future email.";
          break;

        case "ballots":
          Message.InnerText =
            $"We will send future ballot choices to {email} as soon as they become available.";
          break;
      }
    }

    private void UnsubscribeDonor(string email)
    {
      var table = Donations.GetDataByEmail(email);
      if (table.Count == 0)
      {
        Message.InnerText = "The requested email address was not found";
        return;
      }

      foreach (var row in table)
        row.OptOut = true;
      Donations.UpdateTable(table);
      Message.InnerText =
        $"Email {email} has been unsubscribed from all future emails.";
    }

    private void UnsubscribeParty(string email)
    {
      var table = PartiesEmails.GetDataByPartyEmail(email);
      if (table.Count == 0)
      {
        Message.InnerText = "The requested email address was not found";
        return;
      }

      foreach (var row in table)
        row.OptOut = true;
      PartiesEmails.UpdateTable(table);
      Message.InnerText =
        $"Email {email} has been unsubscribed from all future party-related emails.";
    }

    private void UnsubscribePolitician()
    {
      var politicianKey = GetQueryString("key");
      var table = Politicians.GetDataByPoliticianKey(politicianKey);
      if (table.Count == 0)
      {
        Message.InnerText = "The requested email address was not found";
        return;
      }

      foreach (var row in table)
        row.OptOut = true;
      Politicians.UpdateTable(table);
      Message.InnerText =
        $"{Politicians.FormatName(table[0])} has been unsubscribed from all future candidate emails.";
    }

    private void UnsubscribeOrganizationContact(string email)
    {
      var table = OrganizationContacts.GetDataByEmail(email);
      if (table.Count == 0)
      {
        Message.InnerText = "The requested email address was not found";
        return;
      }

      foreach (var row in table)
        row.Email = Empty;
      OrganizationContacts.UpdateTable(table);
      Message.InnerText =
        $"Email {email} has been unsubscribed from all future organization emails.";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack) // ReSharper restore InvertIf
      {
        Title = Format(TitleTag, PublicMasterPage.SiteName);

        var email = GetQueryString("email");
        var code = GetQueryString("code");
        var op = GetQueryString("op");
        int.TryParse(code, out var id);

        switch (op)
        {
          case "unsubscribe":
          case "ballots":
            UnsubscribeAddresses(id, email, op);
            break;

          case "undnr":
            UnsubscribeDonor(email);
            break;

          case "unpty":
            UnsubscribeParty(email);
            break;

          case "unpol":
            UnsubscribePolitician();
            break;

          case "unorg":
            UnsubscribeOrganizationContact(email);
            break;
        }
      }
    }
  }
}