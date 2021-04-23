using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using DB;
using DB.Vote;
using Vote;
using static System.String;

namespace VoteAdmin.Master
{
  public partial class DownloadOrganizationsCsv : SecurePage, ISuperUser
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      // get the data
      var table = Organizations.GetOrganizationsForCsv();
      var groups = table.Rows.OfType<DataRow>()
        .GroupBy(r => r.OrgId());

      // create the csv
      string csv;
      using (var ms = new MemoryStream())
      {
        var streamWriter = new StreamWriter(ms);
        var csvWriter = new SimpleCsvWriter();

        // write headers
        csvWriter.AddField("Organization Name");
        csvWriter.AddField("Abbreviation");
        csvWriter.AddField("Type");
        csvWriter.AddField("Sub Type");
        csvWriter.AddField("Ideology");
        csvWriter.AddField("State");
        csvWriter.AddField("URL");
        csvWriter.AddField("Address");
        csvWriter.AddField("Contacts");
        csvWriter.Write(streamWriter);

        foreach (var group in groups)
        {
          var row = group.First();

          var mailingAddress = Empty;
          if (!IsNullOrWhiteSpace(row.City()) && !IsNullOrWhiteSpace(row.StateCode()))
          {
            var street = new List<string>();
            if (!IsNullOrWhiteSpace(row.Address1()))
              street.Add(row.Address1());
            if (!IsNullOrWhiteSpace(row.Address2()))
              street.Add(row.Address2());
            mailingAddress = Join(", ", street);
            if (!IsNullOrWhiteSpace(mailingAddress))
              mailingAddress += ", ";
            mailingAddress += row.City() + ", " + row.StateCode() + " " + row.Zip();
          }

          var contactList = new List<string>();
          foreach (var contact in group)
          {
            if (!IsNullOrWhiteSpace(contact.Contact()) ||
              !IsNullOrWhiteSpace(contact.Email()) ||
              !IsNullOrWhiteSpace(contact.Phone()) ||
              !IsNullOrWhiteSpace(contact.Title()))
            {
              string c;
              if (!IsNullOrWhiteSpace(contact.Email()))
                c = !IsNullOrWhiteSpace(contact.Contact()) 
                  ? $"{contact.Contact()}<{contact.Email()}>" 
                  : contact.Email();
              else c = contact.Contact();
              if (!IsNullOrWhiteSpace(contact.Title()))
              {
                if (!IsNullOrWhiteSpace(c))
                  c += ", ";
                c += contact.Title();
              }
              if (!IsNullOrWhiteSpace(contact.Phone()))
              {
                if (!IsNullOrWhiteSpace(c))
                  c += ", ";
                c += contact.Phone();
              }

              contactList.Add(c);
            }
          }

          var contacts = Join("; ", contactList);

          csvWriter.AddField(row.Name());
          csvWriter.AddField(row.OrgAbbreviation());
          csvWriter.AddField(row.OrgType());
          csvWriter.AddField(row.OrgSubType());
          csvWriter.AddField(row.Ideology());
          csvWriter.AddField(row.StateCode());
          csvWriter.AddField(row.Url());
          csvWriter.AddField(mailingAddress);
          csvWriter.AddField(contacts);

          csvWriter.Write(streamWriter);
        }
        streamWriter.Flush();
        ms.Position = 0;
        csv = new StreamReader(ms).ReadToEnd();
      }

      // download
      Response.Clear();
      Response.ContentType = "application/vnd.ms-excel";
      Response.AddHeader("Content-Disposition",
        "attachment;filename=\"organizations.csv\"");
      Response.Write("\xfeff"); // BOM
      Response.Write(csv);
      Response.End();
    }
  }
}