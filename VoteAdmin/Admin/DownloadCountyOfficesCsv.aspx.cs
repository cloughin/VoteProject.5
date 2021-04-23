using System;
using System.Data;
using System.IO;
using System.Linq;
using DB;
using DB.Vote;
using Vote;
using static System.String;

namespace VoteAdmin.Admin
{
  public partial class DownloadCountyOfficesCsvPage : SecureAdminPage
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      var stateCode = Request.QueryString["state"];
      if (IsNullOrWhiteSpace(stateCode))
        throw new VoteException("State code is missing.");
      if (!StateCache.IsValidStateCode(stateCode))
        throw new VoteException("State code is invalid.");
      var includeMissing = GetQueryString("m") == "1";
      var includeWith = GetQueryString("w") == "1";
      var fileDesc = includeMissing
        ? includeWith ? ".all" : ".missing-incumbents"
        : ".with-incumbents";

      // get the data
      var table = Counties.GetOfficesForCsv(stateCode);

      // create the csv
      string csv;
      using (var ms = new MemoryStream())
      {
        var streamWriter = new StreamWriter(ms);
        var csvWriter = new SimpleCsvWriter();

        // write headers
        csvWriter.AddField("State Code");
        csvWriter.AddField("County Name");
        csvWriter.AddField("County Code");
        csvWriter.AddField("Office Name");
        csvWriter.AddField("Office Key");
        csvWriter.AddField("Incumbent Name");
        csvWriter.AddField("Politician Key");
        csvWriter.AddField("Election Key");
        csvWriter.Write(streamWriter);

        foreach (var row in table.Rows.OfType<DataRow>()
          .Where(r => !IsNullOrWhiteSpace(r.ElectionKey()) &&
            (includeMissing && IsNullOrWhiteSpace(r.PoliticianKey()) ||
             includeWith && !IsNullOrWhiteSpace(r.PoliticianKey()))))
        {
          csvWriter.AddField(row.StateCode());
          csvWriter.AddField(row.County());
          csvWriter.AddField(row.CountyCode());
          csvWriter.AddField(Offices.FormatOfficeName(row));
          csvWriter.AddField(row.OfficeKey() ?? Empty);
          csvWriter.AddField(Politicians.FormatName(row));
          csvWriter.AddField(row.PoliticianKey() ?? Empty);
          csvWriter.AddField(row.ElectionKey() ?? Empty);
          csvWriter.AddField(
            row.ElectionKey() == null ?
              Empty
              : $"=HYPERLINK(\"{UrlManager.GetAdminUri(GetAdminFolderPageUrl("election", "election", row.ElectionKey()))}\",\"Election Report\")");
          csvWriter.AddField(
            row.ElectionKey() == null || row.OfficeKey() == null ?
              Empty
              : $"=HYPERLINK(\"{UrlManager.GetAdminUri(GetOfficeWinnerPageUrl(row.ElectionKey(), row.OfficeKey()))}\",\"Identify Winners\")");

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
        $"attachment;filename=\"county-offices-{stateCode}{fileDesc}.csv\"");
      Response.Write("\xfeff"); // BOM
      Response.Write(csv);
      Response.End();
    }
  }
}