using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SendEmails
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Begin SendEmails");
      Vote.SendEmailFromQueueViaAmazonSes.ProcessPendingEmails();
      Console.WriteLine("End SendEmails");
    }
  }
}
