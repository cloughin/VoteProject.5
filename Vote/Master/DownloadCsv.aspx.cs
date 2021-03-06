using System;
using System.IO;
using System.Web.UI;
using DB.Vote;
using LumenWorks.Framework.IO.Csv;

namespace Vote.Master
{
  public partial class DownloadCsvPage : Page
  {
    private static string GetLiveIntroPageUrl(string id)
    {
      var uri = UrlManager.GetIntroPageUri(id);
      return new UriBuilder(uri)
      {
        Host = UrlManager.GetCanonicalLiveHostName(uri.Host),
        Port = 80
      }.Uri.ToString();
    }

    private static string AddUrlToCsv(string csv)
    {
      using (var csvReader = new CsvReader(new StringReader(csv), true))
      {
        var headers = csvReader.GetFieldHeaders();

        using (var ms = new MemoryStream())
        {
          var streamWriter = new StreamWriter(ms);
          var csvWriter = new SimpleCsvWriter();

          // write headers
          foreach (var col in headers)
            csvWriter.AddField(col);
          csvWriter.Write(streamWriter);
          while (csvReader.ReadNextRecord())
          {
            foreach (var col in headers)
              if (col == "VoteUSA Url")
              {
                var url = string.Empty;
                var id = csvReader["VoteUSA Id"];
                if (!string.IsNullOrWhiteSpace(id))
                  url = GetLiveIntroPageUrl(id);
                csvWriter.AddField(url);
              }
              else csvWriter.AddField(csvReader[col]);
            csvWriter.Write(streamWriter);
          }
          streamWriter.Flush();
          ms.Position = 0;
          csv = new StreamReader(ms).ReadToEnd();
        }
      }
      return csv;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      int id;
      if (int.TryParse(Request.QueryString["id"], out id))
      {
        var table = BallotPediaCsvs.GetDataById(id);
        if ((table.Count == 1) && !string.IsNullOrWhiteSpace(table[0].Content))
        {
          Response.ContentType = "application/vnd.ms-excel";
          Response.AddHeader("Content-Disposition", "attachment;filename=" + table[0].Filename);
          Response.Write("\xfeff"); // BOM
          Response.Write(AddUrlToCsv(table[0].Content));
        }
      }
      Response.End();
    }
  }
}