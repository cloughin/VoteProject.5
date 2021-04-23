using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DB.Vote;
using Vote;

namespace TestNameSearch
{
  public partial class MainForm : Form
  {
    public MainForm()
    {
      InitializeComponent();
      StatesComboBox.Items.Add("All");
      foreach (var stateCode in StateCache.All51StateCodes)
        StatesComboBox.Items.Add(stateCode);
      StatesComboBox.SelectedItem = "VA";
      LoadPoliticians();
    }

    private class StateInfo
    {
      public string StateCode;
      public Dictionary<string, List<PoliticianInfo>> Unaccented;
      public Dictionary<string, List<PoliticianInfo>> Metaphone;
      public Dictionary<string, List<PoliticianInfo>> Stripped;
    }

    private class PoliticianInfo
    {
      public string PoliticianKey;
      public string LowerLastName;
      public string UnaccentedLastName;
      public string MetaphoneLastName;
      public string StrippedLastName;
      public string OfficeKey;
      public PoliticianStatus OfficeStatus;
      public string DisplayName;

      public int ComputeDistance(string lname, string uname, string mname,
        string sname)
      {
        if (LowerLastName.StartsWith(lname, StringComparison.Ordinal)) return 0;
        if (UnaccentedLastName.StartsWith(uname, StringComparison.Ordinal)) return 0;
        var result = lname.LevenshteinDistance(LowerLastName);
        if (lname.Length < LowerLastName.Length)
          result = Math.Min(result,
            lname.LevenshteinDistance(LowerLastName.Substring(0, lname.Length)));
        result = Math.Min(result, sname.LevenshteinDistance(StrippedLastName));
        if (MetaphoneLastName.StartsWith(mname, StringComparison.Ordinal) ||
          StrippedLastName.StartsWith(sname, StringComparison.Ordinal))
          result = Math.Min(result, 3);
        return result;
      }

      public override string ToString()
      {
        return DisplayName;
      }
    }

    private class OfficeInfo
    {
      public string OfficeKey;
      public string OfficeLine1;
      public string OfficeLine2;
      public OfficeClass OfficeClass;
      public OfficeClass AlternateOfficeClass;
      public string CountyCode;
      public string LocalCode;

      private string StateCode { get { return Offices.GetStateCodeFromKey(OfficeKey); } }

      public string OfficeDescription
      {
        get
        {
          var result = OfficeLine1;
          if (!string.IsNullOrWhiteSpace(OfficeLine2) &&
            OfficeClass != OfficeClass.USPresident)
            result += " " + OfficeLine2;
          if (OfficeClass == OfficeClass.USPresident) return result;
          var stateCode = StateCode;
          if (String.IsNullOrWhiteSpace(CountyCode))
            return StateCache.GetStateName(stateCode) + " " + result;
          if (String.IsNullOrWhiteSpace(LocalCode))
            return CountyCache.GetCountyName(stateCode, CountyCode) + ", " +
              StateCache.GetStateName(StateCode) + " " + result;
          return CountyCache.GetCountyName(stateCode, CountyCode) + ", " +
            StateCache.GetStateName(StateCode) + ", " +
            LocalDistricts.GetLocalDistrict(stateCode, CountyCode, LocalCode) + " " +
            result;
        }
      }
    }

    private Dictionary<string, StateInfo> _StateDictionary;
    private Dictionary<string, OfficeInfo> _OfficeDictionary;

    private static string GetUnaccentedName(string name)
    {
      var normalizedString = name.Normalize(NormalizationForm.FormD);
      var sb = new StringBuilder(normalizedString.Length);
      foreach (var c in normalizedString)
        switch (CharUnicodeInfo.GetUnicodeCategory(c))
        {
          case UnicodeCategory.LowercaseLetter:
          case UnicodeCategory.UppercaseLetter:
            sb.Append(c);
            break;
        }
      return sb.ToString()
        .ToLowerInvariant();
    }

    private string GetOfficeLine(PoliticianInfo pi)
    {
      OfficeInfo officeInfo;
      _OfficeDictionary.TryGetValue(pi.OfficeKey, out officeInfo);
      if (officeInfo == null) return string.Empty;
      return pi.OfficeStatus.GetOfficeStatusDescription() +
        officeInfo.OfficeDescription;
    }

    private static string GetTwoCharacterKey(string name)
    {
      return name.Substring(0, Math.Min(2, name.Length));
    }

    private static bool IsVowel(char c)
    {
      switch (char.ToLowerInvariant(c))
      {
        case 'a':
        case 'e':
        case 'i':
        case 'o':
        case 'u':
        case 'y':
          return true;

        default:
          return false;
      }
    }

    private static string GetStrippedName(string unaccentedName)
    {
      var sb = new StringBuilder(unaccentedName.Length);
      var lastCh = (char) 0;
      foreach (var c in unaccentedName)
        if (IsVowel(c))
        {
          if (sb.Length == 0)
            sb.Append(c);
          lastCh = (char) 0;
        }
        else
        {
          if (c != lastCh)
            sb.Append(c);
          lastCh = c;
        }
      return sb.ToString();
    }

    private void LoadPoliticians()
    {
      _StateDictionary = StateCache.All51StateCodes.ToDictionary(state => state,
        state =>
          new StateInfo
            {
              StateCode = state,
              Unaccented = new Dictionary<string, List<PoliticianInfo>>(),
              Metaphone = new Dictionary<string, List<PoliticianInfo>>(),
              Stripped = new Dictionary<string, List<PoliticianInfo>>()
            },
        StringComparer.OrdinalIgnoreCase);

      var politicians = Politicians.GetAllNameSearchData();
      foreach (var row in politicians)
      {
        var lname = row.LastName.ToLowerInvariant();
        var uname = GetUnaccentedName(row.LastName);
        var mname = row.LastName.DoubleMetaphone()
          .ToLowerInvariant();
        var sname = GetStrippedName(uname);

        var pi = new PoliticianInfo
          {
            PoliticianKey = row.PoliticianKey,
            LowerLastName = lname,
            UnaccentedLastName = uname,
            MetaphoneLastName = mname,
            StrippedLastName = sname,
            OfficeKey = row.LiveOfficeKey,
            OfficeStatus = row.LiveOfficeStatus.ToPoliticianStatus(),
            DisplayName = Politicians.FormatName(row)
          };

        var stateCode = Politicians.GetStateCodeFromKey(pi.PoliticianKey);
        var si = _StateDictionary[stateCode];
        List<PoliticianInfo> list;

        var twoCharacterKey = GetTwoCharacterKey(uname);
        if (!si.Unaccented.TryGetValue(twoCharacterKey, out list))
        {
          list = new List<PoliticianInfo>();
          si.Unaccented.Add(twoCharacterKey, list);
        }
        list.Add(pi);

        twoCharacterKey = GetTwoCharacterKey(mname);
        if (!si.Metaphone.TryGetValue(twoCharacterKey, out list))
        {
          list = new List<PoliticianInfo>();
          si.Metaphone.Add(twoCharacterKey, list);
        }
        list.Add(pi);

        twoCharacterKey = GetTwoCharacterKey(uname);
        if (!si.Stripped.TryGetValue(twoCharacterKey, out list))
        {
          list = new List<PoliticianInfo>();
          si.Stripped.Add(twoCharacterKey, list);
        }
        list.Add(pi);
      }

      _OfficeDictionary = Offices.GetAllNameSearchData()
        .ToDictionary(row => row.OfficeKey,
          row =>
            new OfficeInfo
              {
                OfficeKey = row.OfficeKey,
                OfficeLine1 = row.OfficeLine1,
                OfficeLine2 = row.OfficeLine2,
                OfficeClass = row.OfficeLevel.ToOfficeClass(),
                AlternateOfficeClass = row.AlternateOfficeLevel.ToOfficeClass(),
                CountyCode = row.CountyCode,
                LocalCode = row.LocalCode
              });
    }

    private void NameTextBox_TextChanged(object sender, EventArgs e)
    {
      NamesWebBrowser.DocumentText = string.Empty;
      var stateCode = StatesComboBox.SelectedItem as string;
      if (stateCode == null) return;
      var name = NameTextBox.Text.Trim();
      var lname = name.ToLowerInvariant();
      if (lname.Length <= 1) return;

      var uname = GetUnaccentedName(name);
      var mname = name.DoubleMetaphone()
        .ToLowerInvariant();
      var sname = GetStrippedName(uname);
      var candidates = new List<PoliticianInfo>();

      GetCandidates(candidates, stateCode, uname, mname, sname);

      var temp = candidates.Distinct()
        .Select(
          pi =>
            new
              {
                Distance = pi.ComputeDistance(lname, uname, mname, sname),
                PoliticianInfo = pi
              })
        .OrderBy(o => o.Distance)
        .ToList();

      var toTake = Math.Max(10, temp.Count(o => o.Distance == 0));
      var toShow = temp.Take(toTake).Select(o => o.PoliticianInfo).ToList();

      NamesWebBrowser.DocumentText = "<style type=\"text/css\">" +
        "p {font-family: arial}" +
        "p span {font-style: italic;font-size:80%;color:#888}" + "</style>" + "<p>" +
        string.Join("</p><p>",
          toShow.Select(
            pi => pi.DisplayName + "<br/><span>" + GetOfficeLine(pi) + "</span>")) +
        "</p>";
    }

    private void GetCandidates(List<PoliticianInfo> candidates, string stateCode,
      string uname, string mname, string sname)
    {
      if (stateCode == "All")
        foreach (var s in StateCache.All51StateCodes)
          GetCandidates(candidates, _StateDictionary[s], uname, mname, sname);
      else
        GetCandidates(candidates, _StateDictionary[stateCode], uname, mname, sname);
    }

    private static void GetCandidates(List<PoliticianInfo> candidates, StateInfo si,
      string uname, string mname, string sname)
    {
      List<PoliticianInfo> list;

      var twoCharacterKey = GetTwoCharacterKey(uname);
      si.Unaccented.TryGetValue(twoCharacterKey, out list);
      if (list != null)
        candidates.AddRange(
          list.Where(
            pi => pi.UnaccentedLastName.StartsWith(uname, StringComparison.Ordinal)));

      twoCharacterKey = GetTwoCharacterKey(mname);
      si.Metaphone.TryGetValue(twoCharacterKey, out list);
      if (list != null)
        candidates.AddRange(
          list.Where(
            pi => pi.MetaphoneLastName.StartsWith(mname, StringComparison.Ordinal)));

      twoCharacterKey = GetTwoCharacterKey(sname);
      si.Stripped.TryGetValue(twoCharacterKey, out list);
      if (list != null)
        candidates.AddRange(
          list.Where(
            pi => pi.StrippedLastName.StartsWith(sname, StringComparison.Ordinal)));
    }
  }
}