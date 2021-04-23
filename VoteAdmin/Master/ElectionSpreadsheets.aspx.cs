using System;
using System.Collections.Generic;
using System.Linq;
using Vote;
using DB.Vote;
using static System.String;
using System.IO;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using ExcelDataReader;
using Vote.Controls;

namespace VoteAdmin
{
  [PageInitializers]
  public partial class ElectionSpreadsheetsPage : SecureAdminPage, ISuperUser, IAllowEmptyStateCode
  {
    private ManagePoliticiansPanel _ManagePoliticiansPanel;

    public class MappingOption
    {
      public string Text;
      public string Value;
      public bool Multiple;
    }

    public static readonly MappingOption[] MappingOptions =
    {
      new MappingOption {Text = "Ignored", Value = Empty},
      new MappingOption {Text = "First Middle (Nickname) Last", Value = "NAMEFULL"},
      new MappingOption {Text = "Last, First Middle (Nickname)", Value = "LASTFIRST"},
      new MappingOption {Text = "First Name", Value = "FIRSTNAME"},
      new MappingOption {Text = "Middle Name", Value = "MIDDLENAME"},
      new MappingOption {Text = "Nickname", Value = "NICKNAME"},
      new MappingOption {Text = "Last Name", Value = "LASTNAME"},
      new MappingOption {Text = "Name Suffix", Value = "NAMESUFFIX"},
      new MappingOption {Text = "Date of Birth", Value = "DOB"},
      new MappingOption {Text = "Email", Value = "EMAIL"},
      new MappingOption {Text = "Web Address", Value = "WEB"},
      new MappingOption {Text = "Mailing Address Line 1", Value = "ADDRESS1"},
      new MappingOption {Text = "Mailing Address Line 2", Value = "ADDRESS2"},
      new MappingOption {Text = "Mailing City", Value = "CITY"},
      new MappingOption {Text = "Mailing State", Value = "STATE"},
      new MappingOption {Text = "Mailing Zip", Value = "ZIP"},
      new MappingOption {Text = "Office", Value = "OFFICE"},
      new MappingOption {Text = "Party", Value = "PARTY"},
      new MappingOption {Text = "Primary Party", Value = "PPARTY"},
      new MappingOption {Text = "General Philosophy", Value = "PHILOSOPHY", Multiple = true},
      new MappingOption {Text = "Personal and Family", Value = "PERSONAL", Multiple = true},
      new MappingOption {Text = "Professional Experience", Value = "PROFESSIONAL", Multiple = true},
      new MappingOption {Text = "Civic Involvement", Value = "CIVIC", Multiple = true},
      new MappingOption {Text = "Political Experience", Value = "POLITICAL", Multiple = true},
      new MappingOption {Text = "Religious Affiliation", Value = "RELIGIOUS", Multiple = true},
      new MappingOption {Text = "Accomplishments and Awards", Value = "ACCOMPLISHMENTS", Multiple = true},
      new MappingOption {Text = "Educational Background", Value = "EDUCATIONAL", Multiple = true},
      new MappingOption {Text = "Military Service", Value = "MILITARY", Multiple = true},
      new MappingOption {Text = "BallotPedia", Value = "BALLOTPEDIA"},
      new MappingOption {Text = "Blogger", Value = "BLOGGER"},
      new MappingOption {Text = "Podcast", Value = "PODCAST"},
      new MappingOption {Text = "Crowdpac", Value = "CROWDPAC"},
      new MappingOption {Text = "Facebook", Value = "FACEBOOK"},
      new MappingOption {Text = "Flickr", Value = "FLICKR"},
      new MappingOption {Text = "GoFundMe", Value = "GOFUNDME"},
      new MappingOption {Text = "Google+", Value = "GOOGLEPLUS"},
      new MappingOption {Text = "Instagram", Value = "INSTAGRAM"},
      new MappingOption {Text = "LinkedIn", Value = "LINKEDIN"},
      new MappingOption {Text = "PInterest", Value = "PINTEREST"},
      new MappingOption {Text = "RSS Feed", Value = "RSSFEED"},
      new MappingOption {Text = "Twitter", Value = "TWITTER"},
      new MappingOption {Text = "Vimeo", Value = "VIMEO"},
      new MappingOption {Text = "Wikipedia", Value = "WIKIPEDIA"},
      new MappingOption {Text = "YouTube", Value = "YOUTUBE"}
    };

    public static string GetSpreadsheetListHtml(bool all = false, int? id = null)
    {
      var table = all
        ? ElectionSpreadsheets.GetAllListData()
        : ElectionSpreadsheets.GetListDataByCompleted(false);
      if (table.Count == 0)
        return "<div>No spreadsheets found</div>";

      // get a dictionary of election descriptions
      var dictionary =
        Elections.GetElectionDescriptions(table.Select(recordStates =>
          recordStates.ElectionKey));

      // append jurisdictions
      var mods = new List<KeyValuePair<string, string>>();
      foreach (var kvp in dictionary)
        if (!Elections.IsStateElection(kvp.Key))
        {
          var stateCode = Elections.GetStateCodeFromKey(kvp.Key);
          var jurisdiction = Elections.IsCountyElection(kvp.Key)
            ? Counties.GetCounty(stateCode, Elections.GetCountyCodeFromKey(kvp.Key))
            : LocalDistricts.GetLocalDistrict(stateCode, Elections.GetLocalKeyFromKey(kvp.Key));
          mods.Add(new KeyValuePair<string, string>(kvp.Key, jurisdiction));
        }

      foreach (var kvp in mods)
        dictionary[kvp.Key] += ", " + kvp.Value;

      return Join(Empty,
        table.OrderByDescending(r => r.UploadTime).Select(r =>
          $"<div data-completed=\"{r.Completed.ToString().ToLower()}\"" +
          $" data-id=\"{r.Id}\" data-rows=\"{r.Rows}\"" +
          $"{(r.Id == id ? " class=\"selected\"" : Empty)}>" +
          $"{r.Filename} ({r.UploadTime:d}: {dictionary[r.ElectionKey]}" + 
          $"{(r.ElectionScope == "A" ? " et. al." : Empty)}" + 
          $"{(r.JurisdictionScope == "S" ? ", state level only" : Empty)})</div>"));
    }

    public static string GetSpreadsheetMappingHtml(int id)
    {
      string FormatMappingOptions(string selected)
      {
        return Join(Empty,
          MappingOptions.Select(o =>
            $"<option value=\"{o.Value}\"" +
            $"{(o.Value == selected ? " selected=\"selected\"" : Empty)}" + 
            $"{(o.Multiple ? " data-multiple=\"true\"" : Empty)}>" +
            $"{o.Text}</option>"));
      }

      var table = ElectionSpreadsheetsColumns.GetDataById(id);
      return Join(Empty,
        table.OrderBy(r => r.Sequence).Select(r =>
          $"<div data-sequence=\"{r.Sequence}\" class=\"one-mapping\">" +
          $"<span class=\"column-name\">{r.ColumnName}</span>" +
          $"<select class=\"select-mapping styled-select\">{FormatMappingOptions(r.Mapping)}</select></div>"));
    }

    public static SpreadsheetRow GetSpreadsheetRow(int id, int sequence)
    {
      var spreadsheet = ReadSpreadsheet(id);
      if (spreadsheet == null) return null;
      var table = ElectionSpreadsheetsRows.GetDataByIdSequence(id, sequence);
      if (table.Count != 1) return null;
      var row = table[0];
      return new SpreadsheetRow
      {
        Id = row.Id,
        Sequence = row.Sequence,
        ElectionKey = row.ElectionKey,
        OfficeKey = row.OfficeKey,
        PoliticianKey = row.PoliticianKey,
        Status = row.Status,
        Columns = spreadsheet.Rows[sequence].ItemArray.Select(i => i as string).ToArray()
      };
    }

    public static bool IsExcel(string filename)
    {
      return new[] {".xls", ".xlsx"}.Contains(Path.GetExtension(filename),
        StringComparer.OrdinalIgnoreCase);
    }

    public static DataTable ParseSpreadsheet(Stream stream, bool isExcel)
    {
      stream.Position = 0;
      var reader = isExcel
        ? ExcelReaderFactory.CreateReader(stream)
        : ExcelReaderFactory.CreateCsvReader(stream);
      using (reader)
      {
        var config = new ExcelDataSetConfiguration
        {
          ConfigureDataTable = tableReader =>
            new ExcelDataTableConfiguration {UseHeaderRow = true}
        };
        var result = reader.AsDataSet(config);
        if (result.Tables.Count != 1)
          throw new Exception(
            $"Cannot handle an Excel File with {result.Tables.Count} sheets");
        return result.Tables[0];
      }
    }

    public static DataTable ReadSpreadsheet(int id)
    {
      var table = ElectionSpreadsheets.GetDataById(id);
      if (table.Count != 1) return null;
      using (var stream = new MemoryStream(table[0].Content))
        return ParseSpreadsheet(stream, IsExcel(table[0].Filename));
    }

    //protected override void OnLoadComplete(EventArgs e)
    //{
    //  base.OnLoadComplete(e);
    //  _ManagePoliticiansPanel.LoadControls();
    //  _ManagePoliticiansPanel.ClearAddNewCandidate(true);
    //}

    #region DataItem object

    [PageInitializer]
    // ReSharper disable once UnusedMember.Local
    // Invoked via Reflection
    private class AddCandidatesTabItem : DataItemBase
    {
      // The rest of this is in the ManagePoliticiansPanel control
      // ReSharper disable once UnusedMember.Local
      internal static void Initialize(TemplateControl page)
      {
        InitializeGroup(page, "AddCandidates");
      }

      protected override bool Update(object newValue) => false;
    }

    #endregion DataItem object

    protected void ButtonAddCandidates_OnClick(object sender, EventArgs e)
    {
      switch (AddCandidatesReloading.Value)
      {
        case "reloading":
          {
            _ManagePoliticiansPanel.LoadControls();
            _ManagePoliticiansPanel.ClearAddNewCandidate(true);
            break;
          }

        default:
          throw new VoteException($"Unknown reloading option: '{AddCandidatesReloading.Value}'");
      }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      _ManagePoliticiansPanel =
        Master.FindMainContentControl("ManagePoliticiansPanel") as ManagePoliticiansPanel;
      if (_ManagePoliticiansPanel != null)
      {
        _ManagePoliticiansPanel.Mode = ManagePoliticiansPanel.DataMode.AddPoliticians;
        _ManagePoliticiansPanel.PageFeedback = FeedbackAddCandidates;
      }

      if (!IsPostBack)
      {
        Page.Title = "Process Election Spreadsheets";
        H1.InnerHtml = "Process Election Spreadsheets";
        StateCache.Populate(SelectState, "<select>", Empty);
        SpreadsheetListLiteral.Text = GetSpreadsheetListHtml();
        var d =
          $"{{{Join(",", Validation.GetNameSuffixDictionary().Select(kvp => $"{HttpUtility.JavaScriptStringEncode(kvp.Key, true)}:{HttpUtility.JavaScriptStringEncode(kvp.Value, true)}"))}}}";
        (Master.FindControl("Body") as HtmlControl)?.Attributes.Add("data-suffixes", d);
      }
    }
  }

  public class SpreadsheetRow
  {
    public int Id;
    public int Sequence;
    public string ElectionKey;
    public string OfficeKey;
    public string PoliticianKey;
    public string Status;
    public string[] Columns;
  }
}