using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Windows.Forms;
using DB.Vote;
using ExcelDataReader;
using Vote;
using static System.String;

namespace LoadOrgs
{
  static class Program
  {
    private static void Main()
    {
      try
      {
        var fileToRead =
          new FileInfo(
            @"D:\Users\Curt\Dropbox\Documents\Vote\Mantis\772\PACs Master List revised.v2.xlsx");
        using (var stream = fileToRead.OpenRead())
        {
          var reader = ExcelReaderFactory.CreateReader(stream);
          using (reader)
          {
            var config = new ExcelDataSetConfiguration
            {
              ConfigureDataTable = tableReader =>
                new ExcelDataTableConfiguration {UseHeaderRow = true}
            };
            var result = reader.AsDataSet(config);
            var dataTable = result.Tables[0];
            MessageBox.Show(Join("\n",
              dataTable.Columns.OfType<DataColumn>().Select(c => c.ColumnName)));
            MessageBox.Show(dataTable.Rows.Count.ToString());
            MessageBox.Show(Join("\n",
              dataTable.Rows.OfType<DataRow>().Select(r => r["Type"] as string)
                .Where(t => !IsNullOrWhiteSpace(t)).Distinct()), "Type");
            MessageBox.Show(Join("\n",
              dataTable.Rows.OfType<DataRow>().Select(r => r["Ideology"] as string)
                .Where(t => !IsNullOrWhiteSpace(t)).Distinct()), "Ideology");
            MessageBox.Show(Join("\n",
              dataTable.Rows.OfType<DataRow>().Select(r => r["Status"] as string)
                .Where(t => !IsNullOrWhiteSpace(t)).Distinct()), "Status");
            MessageBox.Show(Join("\n",
              dataTable.Rows.OfType<DataRow>().Select(r => r["State"] as string)
                .Where(s => !IsNullOrWhiteSpace(s) && !StateCache.IsValidStateCode(s)).Distinct()), "Invalid States");
            MessageBox.Show(Join("\n",
              dataTable.Rows.OfType<DataRow>().Select(r => r["Notes"] as string)
                .Where(n => !IsNullOrWhiteSpace(n) && n.Contains("\n"))), "Multi-line Notes");
            MessageBox.Show(Join("\n",
              dataTable.Rows.OfType<DataRow>().Select(r => r["Mission URL"] as string)
                .Where(n => !IsNullOrWhiteSpace(n) && n.Contains("\n"))), "Multi-line Mission URL");
            MessageBox.Show(Join("\n",
              dataTable.Rows.OfType<DataRow>().Select(r => r["Email1"] as string)
                .Where(n => !IsNullOrWhiteSpace(n) && n.Contains("\n"))), "Multi-line Email1");
            MessageBox.Show(Join("\n",
              dataTable.Rows.OfType<DataRow>().Select(r => r["Email2"] as string)
                .Where(n => !IsNullOrWhiteSpace(n) && n.Contains("\n"))), "Multi-line Email2");
            MessageBox.Show(Join("\n",
              dataTable.Rows.OfType<DataRow>().Select(r => r["Email1"] as string)
                .Where(m => !IsNullOrWhiteSpace(m) && m != "#N/A" && !Validation.IsValidEmailAddress(m)).Distinct()), "Invalid Email1s");
            MessageBox.Show(Join("\n",
              dataTable.Rows.OfType<DataRow>().Select(r => r["Email2"] as string)
                .Where(m => !IsNullOrWhiteSpace(m) && m != "#N/A" && !Validation.IsValidEmailAddress(m)).Distinct()), "Invalid Email2s");
            MessageBox.Show(Join("\n",
              dataTable.Rows.OfType<DataRow>().Select(r => r["PAC URL"] as string)
                .Where(m => !IsNullOrWhiteSpace(m) && !VotePage.IsValidUrl(m)).Distinct()), "Invalid URLs");
            MessageBox.Show(Join("\n",
              dataTable.Rows.OfType<DataRow>().Select(r => r["Mission URL"] as string)
                .SelectMany(s => s.SafeString().Split('\n'))
                .Where(m => !IsNullOrWhiteSpace(m) && !VotePage.IsValidUrl(m)).Distinct()), "Invalid Mission URLs");

            var orgTypeDictionary = OrganizationTypes.GetAllData()
              .ToDictionary(r => r.OrgType, r => r.OrgTypeId);

            var orgSubTypeDictionary = OrganizationSubTypes.GetAllData()
              .ToDictionary(r => new { r.OrgTypeId, r.OrgSubType }, r => r.OrgSubTypeId);

            var ideologyDictionary = OrganizationIdeologies.GetAllData()
              .ToDictionary(r => r.Ideology, r => r.IdeologyId);
            ideologyDictionary.Add(Empty, 0);

            string Field(DataRow row, string fieldname)
            {
              return (row[fieldname] as string).SafeString().Trim();
            }

            foreach (var row in dataTable.Rows.OfType<DataRow>())
            {
              var orgTypeId = orgTypeDictionary["PAC"];
              string orgSubType;
              switch (Field(row, "Type"))
              {
                case "Independent Expenditures PAC":
                  orgSubType = "Independent Expenditures";
                  break;

                default:
                  orgSubType = "Standard";
                  break;
              }

              var orgSubTypeId =
                orgSubTypeDictionary[new {OrgTypeId = orgTypeId, OrgSubType = orgSubType}];

              var orgId = Organizations.Insert(orgTypeId, orgSubTypeId, ideologyDictionary[Field(row, "Ideology")], 
                DateTime.UtcNow, Field(row, "Name"), Field(row, "Abbreviated Name"), Empty, Empty, 
                Field(row, "City"), Field(row, "State"), Empty, Field(row, "PAC URL"), Empty, Empty,
                Empty, null, Empty, Empty, null, Empty);

              if (Field(row, "Contact 1") != Empty || Field(row, "Title 1") != Empty ||
                Field(row, "Email1") != Empty || Field(row, "Phone1") != Empty)
                OrganizationContacts.Insert(orgId, Field(row, "Contact 1"),
                  Field(row, "Email1"), Field(row, "Phone1"), Field(row, "Title 1"), 10);

              if (Field(row, "Contact 2") != Empty || Field(row, "Title 2") != Empty ||
                Field(row, "Email2") != Empty || Field(row, "Phone2") != Empty)
                OrganizationContacts.Insert(orgId, Field(row, "Contact 2"),
                  Field(row, "Email2"), Field(row, "Phone2"), Field(row, "Title 2"), 10);

              var mOrder = 0;
              foreach (var missionUrl in Field(row, "Mission URL").Split('\n')
                .Select(s => s.Trim()).Where(s => s != Empty))
              {
                mOrder += 10;
                OrganizationMissionUrls.Insert(orgId, missionUrl, mOrder);
              }

              var notes = Field(row, "Notes");
              if (notes != Empty)
                OrganizationNotes.Insert(orgId, DateTime.UtcNow, notes);
            }
          }
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }
  }
}
