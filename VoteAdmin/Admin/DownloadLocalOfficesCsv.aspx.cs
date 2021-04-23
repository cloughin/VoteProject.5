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
  public partial class DownloadLocalOfficesCsvPage : SecureAdminPage
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
        ? (includeWith ? ".all" : ".missing-incumbents")
        : ".with-incumbents";

      // get the data
      var rows = LocalDistricts.GetOfficesForCsv(stateCode).Rows.OfType<DataRow>()
        .Where(r => !IsNullOrWhiteSpace(r.ElectionKey()) &&
        (includeMissing && IsNullOrWhiteSpace(r.PoliticianKey()) ||
          includeWith && !IsNullOrWhiteSpace(r.PoliticianKey())))
        .ToList();

      // apply counties to the data
      var countiesForLocals =
        LocalIdsCodes.FindCountiesWithNames(stateCode, rows.Select(r => r.LocalKey()).Distinct());
      var data = rows.SelectMany(r => countiesForLocals[r.LocalKey()].Select(c =>
        new
        {
          Row = r,
          CountyCode = c.Value,
          County = c.Text,
          AlsoIn = countiesForLocals[r.LocalKey()]
            .Where(o => o.Text != c.Text).Select(o => o.Text).ToArray()
        }))
        .OrderBy(r => r.County)
        .ThenBy(r => r.Row.LocalDistrict())
        .ThenBy(r => r.Row.OfficeLine1())
        .ThenBy(r => r.Row.OfficeLine2())
        .ToArray();

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
        csvWriter.AddField("Also In");
        csvWriter.AddField("Local Name");
        csvWriter.AddField("Local Key");
        csvWriter.AddField("Office Name");
        csvWriter.AddField("Office Key");
        csvWriter.AddField("Incumbent Name");
        csvWriter.AddField("Politician Key");
        csvWriter.AddField("Election Key");
        csvWriter.Write(streamWriter);

        foreach (var row in data)
        {
          csvWriter.AddField(row.Row.StateCode());
          csvWriter.AddField(row.County);
          csvWriter.AddField(row.CountyCode);
          csvWriter.AddField(Join(", ", row.AlsoIn));
          csvWriter.AddField(row.Row.LocalDistrict());
          csvWriter.AddField(row.Row.LocalKey());
          csvWriter.AddField(Offices.FormatOfficeName(row.Row));
          csvWriter.AddField(row.Row.OfficeKey() ?? Empty);
          csvWriter.AddField(Politicians.FormatName(row.Row));
          csvWriter.AddField(row.Row.PoliticianKey() ?? Empty);
          csvWriter.AddField(row.Row.ElectionKey() ?? Empty);
          csvWriter.AddField(
            row.Row.ElectionKey() == null ?
              Empty
              : $"=HYPERLINK(\"{UrlManager.GetAdminUri(GetAdminFolderPageUrl("election", "election", row.Row.ElectionKey()))}\",\"Election Report\")");
          csvWriter.AddField(
            row.Row.ElectionKey() == null || row.Row.OfficeKey() == null ?
              Empty
              : $"=HYPERLINK(\"{UrlManager.GetAdminUri(GetOfficeWinnerPageUrl(row.Row.ElectionKey(), row.Row.OfficeKey()))}\",\"Identify Winners\")");

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
        $"attachment;filename=\"local-offices-{stateCode}{fileDesc}.csv\"");
      Response.Write("\xfeff"); // BOM
      Response.Write(csv);
      Response.End();
    }
  }
}