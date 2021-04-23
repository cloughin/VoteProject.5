using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
//using Amazon.SimpleEmail;
//using Amazon.SimpleEmail.Model;
//using DB.Vote;

namespace Vote
{
    //public static class SendEmailFromQueueViaAmazonSes
    //{
    //    private class TransmissionStatus
    //    {
    //        public SendEmailRequest Request { get; set; }
    //        public SendEmailResponse Response { get; set; }
    //        public Exception SubmissionException { get; set; }
    //        public string Code { get; set; }
    //    }

    //    private const int ThreadFactor = 1; // assume it takes n secs per email

    //    private static readonly string ReturnAddress;
    //    private static int _QuotaRemainingToday;
    //    private static int _QuotaPerSecond;
    //    private static readonly int QuotaSafetyPercentage;
    //    private static TimeSpan _SendInterval;
    //    private static readonly string AwsAccessKeyId;
    //    private static readonly string AwsSecretKeyId;

    //    private static readonly object TimingLock = new object();
    //    private static DateTime _LastSendTime = DateTime.MinValue;

    //    private static readonly Regex SubstitutionsRegex = new Regex(
    //      @"\[\[[a-z0-9]+]]", RegexOptions.IgnoreCase);

    //    static SendEmailFromQueueViaAmazonSes()
    //    {
    //        var emailMaster = EmailMaster.GetAllData()[0];
    //        ReturnAddress = emailMaster.ReturnAddress;
    //        AwsAccessKeyId = emailMaster.AwsAccessKeyId;
    //        AwsSecretKeyId = emailMaster.AwsSecretKeyId;
    //        QuotaSafetyPercentage = emailMaster.QuotaSafetyPercentage;
    //    }

    //    private static string DoSubstitution(Capture match, EmailQueueViewRow row)
    //    {
    //        var substitution = string.Empty;
    //        switch (match.Value.ToLowerInvariant())
    //        {
    //            case "[[address]]":
    //                substitution = row.Address;
    //                break;

    //            case "[[city]]":
    //                substitution = row.City;
    //                break;

    //            case "[[congress]]":
    //                substitution = row.CongressionalDistrict;
    //                break;

    //            case "[[county]]":
    //                substitution = row.County;
    //                break;

    //            case "[[firstname]]":
    //                substitution = row.FirstName;
    //                break;

    //            case "[[fullname]]":
    //                substitution = row.FirstName;
    //                if (substitution.Length > 0 && row.LastName.Length > 0)
    //                    substitution += ' ' + row.LastName;
    //                if (string.IsNullOrWhiteSpace(substitution))
    //                    substitution = row.StateCode + ' ' + "Voter";
    //                break;

    //            case "[[htmladdress]]":
    //                substitution = row.Address;
    //                if (!string.IsNullOrWhiteSpace(substitution))
    //                    substitution += "<br />";
    //                if (!string.IsNullOrWhiteSpace(row.City))
    //                    substitution += row.City + ", " + row.StateCode + ' ';
    //                if (row.Zip5.Length > 0)
    //                {
    //                    substitution += row.Zip5;
    //                    if (row.Zip4.Length > 0)
    //                        substitution += '-' + row.Zip4;
    //                }
    //                break;

    //            case "[[id]]":
    //                substitution = row.Id.ToString(CultureInfo.InvariantCulture);
    //                break;

    //            case "[[lastname]]":
    //                substitution = row.LastName;
    //                break;

    //            case "[[statecode]]":
    //                substitution = row.StateCode;
    //                break;

    //            case "[[statehouse]]":
    //                substitution = row.StateHouseDistrict;
    //                break;

    //            case "[[statename]]":
    //                substitution = StateCache.GetStateName(row.StateCode);
    //                break;

    //            case "[[statesenate]]":
    //                substitution = row.StateSenateDistrict;
    //                break;

    //            case "[[toemail]]":
    //                substitution = row.ToAddress.ToLowerInvariant();
    //                break;

    //            case "[[zip]]":
    //                if (!string.IsNullOrWhiteSpace(row.Zip5))
    //                    if (!string.IsNullOrWhiteSpace(row.Zip4))
    //                        substitution = row.Zip5 + '-' + row.Zip4;
    //                    else
    //                        substitution = row.Zip5;
    //                break;

    //            case "[[zip4]]":
    //                substitution = row.Zip4;
    //                break;

    //            case "[[zip5]]":
    //                substitution = row.Zip5;
    //                break;
    //        }

    //        return substitution;
    //    }

    //    private static string DoSubstitutions(string input, EmailQueueViewRow row)
    //    {
    //        return SubstitutionsRegex.Replace(input, match => DoSubstitution(match, row));
    //    }

    //    private static void GetSendQuotas()
    //    {
    //        using (
    //          var client = new AmazonSimpleEmailServiceClient(
    //            AwsAccessKeyId, AwsSecretKeyId))
    //        {
    //            var sendQuota = client.GetSendQuota()
    //              .GetSendQuotaResult;
    //            _QuotaPerSecond = Convert.ToInt32(sendQuota.MaxSendRate);
    //            var max24HourSend = Convert.ToInt32(sendQuota.Max24HourSend);

    //            // Reduce the quotas by the safetyPercentage
    //            max24HourSend = (max24HourSend * (100 - QuotaSafetyPercentage)) / 100;
    //            _QuotaPerSecond = (_QuotaPerSecond * (100 - QuotaSafetyPercentage)) / 100;

    //            _QuotaRemainingToday = max24HourSend -
    //              Convert.ToInt32(sendQuota.SentLast24Hours);
    //            _SendInterval = new TimeSpan(TimeSpan.TicksPerSecond / _QuotaPerSecond);
    //        }
    //    }

    //    private static void Run(IEnumerable<EmailQueueViewRow> table)
    //    {
    //        var bag = new ConcurrentBag<TransmissionStatus>();

    //        var query = table.AsParallel()
    //          .WithDegreeOfParallelism(_QuotaPerSecond * ThreadFactor)
    //          .Select(Send);

    //        query.ForAll(bag.Add);
    //    }

    //    private static TransmissionStatus Send(EmailQueueViewRow row)
    //    {
    //        TransmissionStatus result;
    //        SendEmailRequest request = null;
    //        var rejected = false;
    //        var rejectedReason = string.Empty;

    //        try
    //        {
    //            // Setup the email recipients.
    //            var destination = new Destination();
    //            destination.WithToAddresses(
    //              new List<string> { row.ToAddress.ToLowerInvariant() });

    //            // Create the email subject.
    //            var subject = new Content();
    //            subject.WithData(DoSubstitutions(row.Subject, row));

    //            // Create the email content.
    //            var content = new Content();
    //            content.WithData(DoSubstitutions(row.Template, row));
    //            var body = new Body();
    //            body.WithHtml(content);

    //            // Create and transmit the email to the recipients via Amazon SES.
    //            var message = new Message();
    //            message.WithSubject(subject);
    //            message.WithBody(body);

    //            // Create the Amazon SES request.
    //            request = new SendEmailRequest();
    //            request.WithSource(row.FromAddress);
    //            request.WithDestination(destination);
    //            request.WithMessage(message);
    //            request.ReturnPath = ReturnAddress;

    //            WaitForTimeSlot();
    //            //Console.WriteLine("Send " + row.ToAddress);

    //            using (
    //              var client = new AmazonSimpleEmailServiceClient(
    //                AwsAccessKeyId, AwsSecretKeyId))
    //            {
    //                var response = client.SendEmail(request);
    //                result = new TransmissionStatus
    //                  {
    //                      Request = request,
    //                      Response = response,
    //                      Code = "OK"
    //                  };
    //                //result = new TransmissionStatus() { Request = request, Response = null, Code = "OK" };
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            rejected = true;
    //            rejectedReason = ex.Message;
    //            result = new TransmissionStatus
    //              {
    //                  Request = request,
    //                  SubmissionException = ex,
    //                  Code = ex.Message
    //              };
    //        }
    //        finally
    //        {
    //            var table = EmailQueue.GetDataById(row.Id);
    //            if (table.Count > 0)
    //            {
    //                table[0].SentTime = DateTime.Now;
    //                table[0].Rejected = rejected;
    //                table[0].RejectedReason = rejectedReason;
    //                EmailQueue.UpdateTable(table);
    //            }
    //        }

    //        return result;
    //    }

    //    // ReSharper disable FunctionNeverReturns
    //    public static void ProcessPendingEmails()
    //    {
    //        while (true)
    //        {
    //            var sleep = false;
    //            GetSendQuotas();
    //            if (_QuotaRemainingToday > 0)
    //            {
    //                var maxPerMinute = _QuotaPerSecond * 60; // get a minutes worth
    //                var table = EmailQueueView.GetPendingEmail(maxPerMinute);
    //                if (table.Count > 0)
    //                    //DateTime startTime = DateTime.Now;
    //                    Run(table);
    //                //Console.WriteLine(table.Count.ToString() + ':' + (DateTime.Now - startTime).TotalSeconds.ToString());
    //                else sleep = true;
    //            }
    //            else
    //                sleep = true;
    //            if (sleep)
    //                // sleep 10 minutes waiting for more emails or more quota
    //                Thread.Sleep(new TimeSpan(0, 10, 0));
    //        }
    //    }

    //    // ReSharper restore FunctionNeverReturns

    //    private static void WaitForTimeSlot()
    //    {
    //        TimeSpan sleepTime;
    //        lock (TimingLock)
    //        {
    //            var nextSendTime = _LastSendTime + _SendInterval;
    //            var now = DateTime.Now;
    //            if (now > nextSendTime)
    //            {
    //                _LastSendTime = now;
    //                return;
    //            }
    //            sleepTime = nextSendTime - now;
    //            _LastSendTime = nextSendTime;
    //        }
    //        Thread.Sleep(sleepTime);
    //    }
    //}
}