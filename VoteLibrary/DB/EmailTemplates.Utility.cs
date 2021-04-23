using System;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using DB.VoteLog;
using Vote;
using static System.String;

namespace DB.Vote
{
  public partial class EmailTemplates
  {
    public static string GetOwnerType(string securityClass = null)
    {
      if (securityClass == null) securityClass = SecurePage.UserSecurityClass;
      switch (securityClass)
      {
        case SecurePage.PoliticianSecurityClass:
          return "P";

        case SecurePage.PartySecurityClass:
          return "E";

        default:
          return "U";
      }
    }

    public static Substitutions.Requirements GetTemplateRequirements(string subject,
      string body)
    {
      return Substitutions.GetRequirements(subject) | Substitutions.GetRequirements(body);
    }

    public static string GetTemplateRequirementsString(string subject, string body)
    {
      return SerializeRequirements(GetTemplateRequirements(subject, body));
    }

    public static void SendEmail(string subject, string body, string from, string[] to,
      string[] cc, string[] bcc, int retryMax = 3)
    {
      // sanitize the subject (pretties, but mainly gets rid of cr & lf)
      subject = subject.StripRedundantWhiteSpace();
      retryMax = Math.Max(0, retryMax);
      var mailMessage = new MailMessage(from, Join(",", to), subject, body);
      if (cc != null && cc.Length > 0) mailMessage.CC.Add(Join(",", cc));
      if (bcc != null && bcc.Length > 0) mailMessage.Bcc.Add(Join(",", bcc));
      mailMessage.IsBodyHtml = true;

#if !NoEmail
      while (retryMax >= 0)
        try
        {
          EmailUtility.GetConfiguredSmtpClient().Send(mailMessage);
          retryMax = -1; // kill the loop
        }
        catch (Exception)
        {
          if (retryMax-- <= 0) throw;
          Thread.Sleep(1000); // let things settle
        }
#endif
    }

    public static string SerializeRequirements(Substitutions.Requirements requirements)
    {
      // we don't use ToString because of a little custom formatting
      return Join(",",
        requirements.ToString().Split(',').Where(c => c != "None")
          .Select(c => c.Trim().ToLowerInvariant()));
    }

    // ReSharper disable once UnusedMember.Global
    public static Substitutions.Requirements DeserializeRequirements(string requirements)
    {
      return (Substitutions.Requirements) Enum.Parse(typeof (Substitutions.Requirements),
        requirements);
    }

    public static Substitutions GetSubstititionsForEmail(string stateCode,
      string countyCode, string localKey, string contact, string email, string title,
      string phone, string electionKey, string officeKey, string politicianKey,
      string partyKey, string partyEmail, int visitorId, int donorId, int orgContactId, 
      string sourceCode)
    {
      return new
        Substitutions("[[contact]]", contact, "[[contactemail]]", email, "@@contactemail@@",
          email, "[[contacttitle]]", title, "[[contactphone]]", phone)
        {
          StateCode = stateCode,
          CountyCode = countyCode,
          LocalKey = localKey,
          PoliticianKey = politicianKey,
          ElectionKey = electionKey,
          OfficeKey = officeKey,
          PartyKey = partyKey,
          PartyEmail = partyEmail,
          VisitorId = visitorId,
          DonorId = donorId,
          OrgContactId = orgContactId,
          IssueKey = "ALLBio",
          SourceCode = sourceCode
        };
    }

    public static void SendEmail(string subjectTemplate, string bodyTemplate,
      string emailType, string from, string[] to, string[] cc, string[] bcc,
      string stateCode, string countyCode, string localKey, string contact, string email,
      string title, string phone, string electionKey, string officeKey,
      string politicianKey, string partyKey, string partyEmail, int visitorId, int donorId,
      int orgContactId, string sourceCode, int logBatchId = -1, int retryMax = 3)
    {
      var wasSent = false;
      var subject = Empty;
      var body = Empty;
      string errorMessage = null;
      try
      {
        var substitution = GetSubstititionsForEmail(stateCode, countyCode, localKey,
          contact, email, title, phone, electionKey, officeKey, politicianKey, partyKey,
          partyEmail, visitorId, donorId, orgContactId, sourceCode);
        subject = substitution.Substitute(subjectTemplate);
        body = substitution.Substitute(bodyTemplate);
        if (to.Length <= 0) return;
        SendEmail(subject, body, from, to, cc, bcc, retryMax);
        wasSent = true;
      }
      catch (VoteSubstitutionException e)
      {
        errorMessage = e.Message;
        var inner = e.InnerException;
        while (inner != null)
        {
          errorMessage += "::" + inner.Message;
          inner = inner.InnerException;
        }

        throw;
      }
      catch (Exception e)
      {
        var sb = new StringBuilder();
        while (e != null)
        {
          if (sb.Length > 0)
          {
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
          }
          sb.Append(e.Message);
          sb.Append(Environment.NewLine);
          sb.Append(e.StackTrace);
          e = e.InnerException;
        }

        errorMessage = sb.ToString();

        throw;
      }
      finally
      {
        try
        {
          if (logBatchId >= 0)
            foreach (var toAddr in to)
              LogEmail.Insert(logBatchId, DateTime.UtcNow, wasSent, stateCode, countyCode,
                localKey, electionKey, officeKey, politicianKey, visitorId, contact, toAddr,
                subject, body, 0, false, errorMessage, emailType);
        }
        // ReSharper disable once EmptyGeneralCatchClause
        catch
        {
        }
      }
    }
  }
}