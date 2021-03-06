using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using DB.VoteZipNewLocal;

namespace Vote
{
  [SuppressMessage("ReSharper", "NotAccessedField.Global")]
  public partial class StreetAnalyzer
  {
    public class StreetAnalyzerStatistics
    {
      public int InvalidZip4;
      public int MissingZip4;
      public int MissingZip4Range;
      public int MultipleZip4Range;
      public int Expanded;
      public int Summarized;
      public int Merged;
    }

    private class PoBoxInfo
    {
      public string Address;
      public LdsInfo LdsInfo;
    }

    private readonly StreetAnalyzerStatistics _Statistics;
    private Action<string> _FeedbackDelegate;
    private readonly bool _SuppressUpdate;
    private readonly TextWriter _TextWriter;

    public const string PoBox = "PO BOX";

    public StreetAnalyzer(bool suppressUpdate)
    {
      _Statistics = new StreetAnalyzerStatistics();
      _SuppressUpdate = suppressUpdate;
    }

    public StreetAnalyzer(TextWriter textWriter, bool suppressUpdate)
      : this(suppressUpdate)
    {
      _TextWriter = textWriter;
    }

    public int Analyze(List<StreetAnalysisData> dataList)
    {
      var rows = 0;
      PreprocessHouseNumber(dataList);
      ApplyLdsInfo(dataList);
      ApplyLdsInfoToPoBoxes(dataList);
      if (dataList.Count > 0) rows = SummarizeData(dataList);
      return rows;
    }

    private void AnalyzeWildcard(IList<StreetAnalysisData> list)
    {
      if (list.Count == 0) return;

      // Begin by checking for wild card entries (high and low both
      // empty). Bcause of the sorting, they will be at the beginning.

      var wildcards = new Dictionary<LdsInfo, object>();
      if (list[0].IsWildcard)
      {
        // We have a wildcard
        wildcards[list[0].LdsInfo] = null;
        var n = 1;
        // look for more
        while (n < list.Count)
          if (list[n].IsWildcard) // another wildcard
            if (wildcards.ContainsKey(list[n].LdsInfo)) // redundant, remove it
              list.RemoveAt(n);
            else
            {
              wildcards[list[n].LdsInfo] = null;
              ReportError("{0} {1} Conflicting wildcards", list[n - 1].UpdateKey,
                list[n].UpdateKey);
              n++;
            }
          else break;

        // If there was a single wildcard, we can remove any other
        // list entries with the same LdsInfo
        // Commented because not safe due to same street in multiple zips
        //if (wildcards.Count == 1)
        //{
        //  LdsInfo wildcardLdsInfo = wildcards.Keys.Single();
        //  n = 1;
        //  while (n < list.Count)
        //    if (list[n].LdsInfo == wildcardLdsInfo)
        //      list.RemoveAt(n);
        //    else
        //      n++;
        //}
      }
    }

    private void ApplyLdsInfo(IList<StreetAnalysisData> dataList)
    {
      if ((dataList == null) || (dataList.Count == 0)) return;

      // This dictionary caches the Zip+4 values
      var ldsDictionary = new Dictionary<string, LdsInfo>();

      // In the first pass we resolve all range values and collect the
      // single values;
      foreach (var data in dataList) // We skip all PO BOXes here. They are handled separately
        if (data.StreetName != PoBox)
          try
          {
            ValidatePlus4s(data);

            if (data.Plus4Low == data.Plus4High) ldsDictionary[data.Plus4Low] = null;
            else
            {
              var group = new Dictionary<LdsInfo, object>();

              var uszdTable = Uszd.GetDataByZipPlus4Range(data.ZipCode,
                data.Plus4Low, data.Plus4High, 0);
              foreach (var uszdRow in uszdTable)
              {
                var stateCode =
                  StateCache.StateCodeFromLdsStateCode(uszdRow.LdsStateCode);
                group[
                  new LdsInfo(stateCode, uszdRow.Congress, uszdRow.StateSenate,
                    uszdRow.StateHouse, uszdRow.County)] = null;
              }

              if (group.Count == 0)
              {
                _Statistics.MissingZip4Range++;
                throw new VoteException(
                  "*{0} missing Lds info range {1}-{2} {1}-{3}", data.UpdateKey,
                  data.ZipCode, data.Plus4Low, data.Plus4High);
              }

              if (group.Count > 1)
              {
                _Statistics.MultipleZip4Range++;
                if (data.StreetName == "PO BOX")
                  throw new VoteException(
                    "{0} multiple PO BOX range {1}-{2} {1}-{3}", data.UpdateKey,
                    data.ZipCode, data.Plus4Low, data.Plus4High);
                throw new VoteException(
                  "{0} multiple Lds info range {1}-{2} {1}-{3}", data.UpdateKey,
                  data.ZipCode, data.Plus4Low, data.Plus4High);
              }

              data.LdsInfo = group.Keys.Single();
            }
          }
          catch (VoteException ex)
          {
            if (!ex.Message.StartsWith("*")) ReportError(ex.Message);
            data.SetMissingLdsInfo();
          }

      // Now we get all the ldsInfos
      var table = Uszd.GetDataByZip4List(dataList[0].ZipCode, ldsDictionary.Keys, 0);
      foreach (var row in table)
      {
        var stateCode = StateCache.StateCodeFromLdsStateCode(row.LdsStateCode);
        ldsDictionary[row.Zip4] = new LdsInfo(stateCode, row.Congress,
          row.StateSenate, row.StateHouse, row.County);
      }

      // Finally, we apply the ldsInfos to the non-ranged data items
      foreach (var data in dataList)
        if ((data.StreetName != PoBox) && (data.Plus4Low == data.Plus4High))
        {
          LdsInfo ldsInfo;
          if (ldsDictionary.TryGetValue(data.Plus4Low, out ldsInfo) &&
            (ldsInfo != null)) data.LdsInfo = ldsInfo;
          else
          {
            _Statistics.MissingZip4++;
            //LogMissingZipPlus4(data.ZipCode, data.Plus4Low);
            //AppendErrorsText("{0} missing Lds info {1}-{2}",
            //  data.UpdateKey, data.ZipCode, data.Plus4Low);
            data.SetMissingLdsInfo();
          }
        }
    }

    private void ApplyLdsInfoToPoBoxes(ICollection<StreetAnalysisData> dataList)
    {
      if ((dataList == null) || (dataList.Count == 0)) return;

      // These are for list adjustments to be done after examining
      // all data items.
      var deletions = new List<StreetAnalysisData>();
      var additions = new List<StreetAnalysisData>();

      foreach (var data in dataList)
        if (data.StreetName == PoBox)
          try
          {
            // All these fields must be empty
            data.DirectionPrefix = string.Empty;
            data.StreetSuffix = string.Empty;
            data.DirectionSuffix = string.Empty;
            data.BuildingName = string.Empty;
            data.SecondaryType = string.Empty;
            data.SecondaryLowNumber = string.Empty;
            data.SecondaryHighNumber = string.Empty;
            data.SecondaryOddEven = string.Empty;
            // Always "B"
            data.PrimaryOddEven = "B";

            // Validate the zip4List -> must be 4 digit numeric,
            // properly collated
            if ((data.Plus4Low.Length != 4) || !data.Plus4Low.IsDigits() ||
              (data.Plus4High.Length != 4) || !data.Plus4High.IsDigits() ||
              data.Plus4Low.IsGt(data.Plus4High))
            {
              data.SetMissingLdsInfo();
              throw new VoteException("{0} Invalid PO BOX zip4List: {1} {2}",
                data.UpdateKey, data.Plus4Low, data.Plus4High);
            }

            // Case 1: PrimaryLowNumber == PrimaryHighNumber
            if (data.PrimaryLowNumber == data.PrimaryHighNumber)
            {
              // If they are both blank, it is a wildcard
              //if (data.PrimaryHighNumber == string.Empty)
              //  data.PrimaryHighNumber = HighPrimaryKey;
              var zip4List = CreateZip4List(data.Plus4Low, data.Plus4High);
              var ldsDictionary = GetLdsInfoDictionary(data.ZipCode, zip4List);
              // Summarize
              var ldsUniqueDictionary = new Dictionary<LdsInfo, object>();
              foreach (var info in ldsDictionary.Values) ldsUniqueDictionary[info] = null;
              if (ldsUniqueDictionary.Count != 1)
              {
                data.SetMissingLdsInfo();
                if (ldsUniqueDictionary.Count == 0)
                {
                  //LogMissingZipPlus4(data.ZipCode, data.Plus4Low);
                  //throw new VoteException("{0} missing PO BOX Lds info {1}-{2}",
                  //  data.UpdateKey, data.ZipCode, data.Plus4Low);
                }
              }
              else data.LdsInfo = ldsUniqueDictionary.Keys.Single();
              continue;
            }

            // Case 2: Plus4Low == Plus4High
            if (data.Plus4Low == data.Plus4High)
            {
              data.LdsInfo = AddressFinder.GetLdsInfoOrMissing(data.ZipCode,
                data.Plus4Low);
              if (data.LdsInfo.IsMissing)
              {
                //LogMissingZipPlus4(data.ZipCode, data.Plus4Low);
                //throw new VoteException("{0} missing PO BOX Lds info {1}-{2}",
                //  data.UpdateKey, data.ZipCode, data.Plus4Low);
              }
              continue;
            }

            // Validate the addresses -> at this point it must be MaxHouseNumberLength digit numeric,
            // properly collated
            if (!AddressFinder.IsNumericHouseNumber(data.PrimaryLowNumber) ||
              !AddressFinder.IsNumericHouseNumber(data.PrimaryHighNumber) ||
              data.PrimaryLowNumber.IsGt(data.PrimaryHighNumber))
            {
              data.SetMissingLdsInfo();
              throw new VoteException("{0} Invalid PO BOX Primaries: {1} {2}",
                data.UpdateKey, data.PrimaryLowNumber, data.PrimaryHighNumber);
            }

            var success = CheckPoBoxRange(data, deletions, additions);

            if (!success)
              throw new VoteException("{0} Invalid PO BOX Primaries: {1} {2}",
                data.UpdateKey, data.PrimaryLowNumber, data.PrimaryHighNumber);
          }
          catch (VoteException ex)
          {
            if (!ex.Message.StartsWith("*")) ReportError(ex.Message);
            data.SetMissingLdsInfo();
          }

      // Handle additions and deletions
      if ((deletions.Count > 0) || (additions.Count > 0))
      {
        _Statistics.Expanded += additions.Count - deletions.Count;
        foreach (var toDelete in deletions) dataList.Remove(toDelete);
        foreach (var toAdd in additions) dataList.Add(toAdd);
      }
    }

    private void CheckForMergeableItems(IList<StreetAnalysisData> list,
      string oddEven)
    {
      var hasWildcard = (list.Count > 0) && list[0].IsWildcard;
      var n = hasWildcard ? 2 : 1;
      if (list.Count < n + 1) return; // nothing to check
      while (n < list.Count)
      {
        // Look for merge opportuinities
        // We can be more aggressive on numeric "B" merges if there is no wildcard. 
        // "O" and "E" can't merge in gaps because they might be overriding a "B" value.
        var canMerge = list[n - 1].LdsInfo == list[n].LdsInfo;
        // lds's must be the same
        if (!hasWildcard && (oddEven == "B") &&
          AddressFinder.IsNumericHouseNumber(list[n].PrimaryLowNumber))
        {
          // ok as is
        }
        else
          //if (oddEven != "B" || !AddressFinder.IsNumericHouseNumber(list[n].PrimaryLowNumber))
          // make sure there are no gaps
          canMerge &= CanMergeAddresses(list[n - 1].PrimaryHighNumber,
            list[n].PrimaryLowNumber, oddEven);
        if (canMerge && list[n - 1].MatchesSecondary(list[n]))
        {
          list[n - 1].PrimaryHighNumber = list[n].PrimaryHighNumber;
          list.RemoveAt(n);
          _Statistics.Merged++;
        }
        n++;
      }
    }

    private void AnalyzeContainment(IList<StreetAnalysisData> list)
    {
      var hasWildcard = (list.Count > 0) && list[0].IsWildcard;
      var n = hasWildcard ? 2 : 1;
      if (list.Count < n + 1) return; // nothing to check

      while (n < list.Count)
      {
        if (AddressFinder.IsNumericHouseNumber(list[n].PrimaryLowNumber) &&
          AddressFinder.IsNumericHouseNumber(list[n - 1].PrimaryLowNumber))
          if (
            string.CompareOrdinal(list[n - 1].PrimaryHighNumber, list[n].PrimaryLowNumber) >
            0) // First check for containment
            if (ContainsPrimary(list[n - 1], list[n]))
            {
              if ((list[n - 1].LdsInfo == list[n].LdsInfo) &&
                list[n - 1].MatchesSecondary(list[n]))
              {
                // First contains second, remove second
                list.RemoveAt(n);
                _Statistics.Summarized++;
                continue;
              }
            }
            else if (ContainsPrimary(list[n], list[n - 1]))
            {
              if ((list[n - 1].LdsInfo == list[n].LdsInfo) &&
                list[n - 1].MatchesSecondary(list[n]))
              {
                // Second contains first, remove first
                list.RemoveAt(n - 1);
                _Statistics.Summarized++;
                continue;
              }
            }
            else if (list[n - 1].LdsInfo != list[n].LdsInfo)
            {
              // We only report overlap errors if there is no
              // secondary data to go on
              if (list[n - 1].MatchesSecondary(list[n]))
                ReportError("{0} {1} Overlap", list[n - 1].UpdateKey,
                  list[n].UpdateKey);
            }
            else if (list[n - 1].MatchesSecondary(list[n]))
            {
              // We know they are numeric addresses. Merge the two into one
              list[n - 1].PrimaryHighNumber = list[n].PrimaryHighNumber;
              list.RemoveAt(n);
              continue;
            }
        n++;
      }
    }

    private void CheckForSingleLdsInList(IList<StreetAnalysisData> list)
    {
      // We only do this if there is a wildcard entry
      if ((list.Count > 1) && list[0].IsWildcard)
      {
        var first = list[0];
        var single = true;
        for (var n = 1; n < list.Count; n++)
          if (list[n].LdsInfo != first.LdsInfo)
          {
            single = false;
            break;
          }
        if (single)
        {
          for (var n = list.Count - 1; n > 0; n--)
          {
            list.RemoveAt(n);
            _Statistics.Summarized++;
          }
          first.PrimaryLowNumber = string.Empty;
          first.PrimaryHighNumber = string.Empty;
          first.BuildingName = string.Empty;
          first.SecondaryType = string.Empty;
          first.SecondaryLowNumber = string.Empty;
          first.SecondaryHighNumber = string.Empty;
          first.SecondaryOddEven = string.Empty;
        }
      }
    }

    //private void CheckForSingleLdsOverall(Dictionary<string, List<StreetAnalysisData>> splits)
    //{
    //  // If all entries in all three lists have the same ldsInfo, we can 
    //  // substitute a single "B" entry
    //  StreetAnalysisData firstFound = null;
    //  bool canSubstitute = true;
    //  foreach (var split in splits.Values)
    //  {
    //    foreach (var data in split)
    //      if (firstFound == null)
    //        firstFound = data;
    //      else if (firstFound.LdsInfo != data.LdsInfo) // no good
    //      {
    //        canSubstitute = false;
    //        break;
    //      }
    //    if (!canSubstitute) break;
    //  }
    //  if (canSubstitute)
    //  {
    //    // use firstFound as the "B" entry
    //    firstFound.PrimaryOddEven = "B";
    //    firstFound.PrimaryLowNumber = string.Empty;
    //    firstFound.PrimaryHighNumber = string.Empty;
    //    firstFound.BuildingName = string.Empty;
    //    firstFound.SecondaryType = string.Empty;
    //    firstFound.SecondaryLowNumber = string.Empty;
    //    firstFound.SecondaryHighNumber = string.Empty;

    //    // adjust count and clear out old
    //    foreach (var split in splits.Values)
    //    {
    //      _Statistics.Merged += split.Count;
    //      split.Clear();
    //    }

    //    // add in new
    //    splits["B"].Add(firstFound);
    //    _Statistics.Merged--;
    //  }
    //}

    private static bool CheckPoBoxRange(StreetAnalysisData data,
      ICollection<StreetAnalysisData> deletions,
      ICollection<StreetAnalysisData> additions)
    {
      var addressLow = long.Parse(data.PrimaryLowNumber);
      var addressHigh = long.Parse(data.PrimaryHighNumber);
      var zip4Low = int.Parse(data.Plus4Low);
      var zip4High = int.Parse(data.Plus4High);

      // Ranges must correspond
      if (addressHigh - addressLow != zip4High - zip4Low) return false;

      // This dictionary caches the Zip+4 values
      var zip4List = CreateZip4List(zip4Low, zip4High);
      var ldsDictionary = GetLdsInfoDictionary(data.ZipCode, zip4List);

      // Build the list of results
      var boxList = new List<PoBoxInfo>();
      for (var n = 0; n <= addressHigh - addressLow; n++)
      {
        var address = (addressLow + n).ToString(CultureInfo.InvariantCulture)
          .ZeroPad(AddressFinder.MaxHouseNumberLength);
        var zip4 = (zip4Low + n).ToString(CultureInfo.InvariantCulture)
          .ZeroPad(4);
        LdsInfo ldsInfo;
        if (!ldsDictionary.TryGetValue(zip4, out ldsInfo)) ldsInfo = LdsInfo.Missing;
        boxList.Add(new PoBoxInfo {Address = address, LdsInfo = ldsInfo});
      }

      // Summarize to additions
      LdsInfo currentLdsInfo = null;
      string lowAddress = null;
      var sequence = 0;
      for (var n = 0; n <= boxList.Count; n++)
      {
        var endOfData = n == boxList.Count;
        if (endOfData || (boxList[n].LdsInfo != currentLdsInfo))
        {
          if (currentLdsInfo != null) // create new
          {
            sequence++;
            var toAdd = data.Clone();
            toAdd.UpdateKey += "-" + sequence.ToString(CultureInfo.InvariantCulture)
              .ZeroPad(2);
            toAdd.PrimaryLowNumber = lowAddress;
            toAdd.PrimaryHighNumber = boxList[n - 1].Address;
            toAdd.LdsInfo = currentLdsInfo;
            additions.Add(toAdd);
          }
          if (!endOfData)
          {
            currentLdsInfo = boxList[n].LdsInfo;
            lowAddress = boxList[n].Address;
          }
        }
      }

      deletions.Add(data);

      return true;
    }

    private static bool ContainsPrimary(StreetAnalysisData data1,
      StreetAnalysisData data2)
    {
      if (data1.PrimaryLowNumber.Length != data2.PrimaryLowNumber.Length) return false;
      if (data1.PrimaryHighNumber.Length != data2.PrimaryHighNumber.Length) return false;
      return (string.CompareOrdinal(data1.PrimaryLowNumber, data2.PrimaryLowNumber) <= 0) &&
        (string.CompareOrdinal(data1.PrimaryHighNumber, data2.PrimaryHighNumber) >= 0);
    }

    private static List<string> CreateZip4List(string zip4Low, string zip4High) => 
      CreateZip4List(int.Parse(zip4Low), int.Parse(zip4High));

    private static List<string> CreateZip4List(int zip4Low, int zip4High)
    {
      var zip4List = new List<string>();
      for (var zip4 = zip4Low; zip4 <= zip4High; zip4++)
        zip4List.Add(zip4.ToString(CultureInfo.InvariantCulture)
          .ZeroPad(4));
      return zip4List;
    }

    //private static string GetNextPrimary(string primary, string type)
    //{
    //  int incr = type == "B" ? 1 : 2;
    //  if (primary.Length == 10 && primary.IsDigits())
    //    return (int.Parse(primary) + incr).ToString().ZeroPad(10);
    //  else
    //  {
    //    Match match = RegexSplitAddress.Match(primary);
    //    if (match.Success)
    //    {
    //      string prefix = match.FirstCaptureOrEmpty("prefix");
    //      string suffix = match.FirstCaptureOrEmpty("suffix");
    //      string hyphen = match.FirstCaptureOrEmpty("hyphen");
    //      string numbers = match.FirstCapture("numbers");
    //      int value = int.Parse(numbers);
    //      value += incr;
    //      if (prefix != string.Empty) prefix += hyphen;
    //      if (suffix != string.Empty) suffix += hyphen;
    //      string newPrimary = prefix + value.ToString() + suffix;
    //      if (primary.Length == newPrimary.Length)
    //        return newPrimary;
    //      return null;
    //    }
    //    else
    //      return null;
    //  }
    //}

    public Action<string> Feedback
    {
      set { _FeedbackDelegate = value; }
    }

    private static Dictionary<string, LdsInfo> GetLdsInfoDictionary(string zip5,
      IEnumerable<string> zip4List)
    {
      var ldsDictionary = new Dictionary<string, LdsInfo>();
      var table = Uszd.GetDataByZip4List(zip5, zip4List, 0);
      foreach (var row in table)
      {
        var stateCode = StateCache.StateCodeFromLdsStateCode(row.LdsStateCode);
        ldsDictionary[row.Zip4] = new LdsInfo(stateCode, row.Congress,
          row.StateSenate, row.StateHouse, row.County);
      }
      return ldsDictionary;
    }

    //private static void LogMissingZipPlus4(string zip5, string zip4)
    //{
    //  try
    //  {
    //    ZipMissingZipPlus4.Insert(zip5, zip4);
    //  }
    //  catch { }
    //}

    //private void RemoveRedundantOddEvenLists(Dictionary<string, List<StreetAnalysisData>> splits)
    //{
    //  var bothList = splits["B"];
    //  var evenList = splits["E"];
    //  var oddList = splits["O"];

    //  if (bothList.Count == 1)
    //  {
    //    bothList[0].PrimaryLowNumber = string.Empty;
    //    bothList[0].PrimaryHighNumber = string.Empty;
    //    LdsInfo ldsInfo = bothList[0].LdsInfo;
    //    if (evenList.Count == 1 && evenList[0].LdsInfo == ldsInfo)
    //    {
    //      _Statistics.Summarized++;
    //      evenList.RemoveAt(0);
    //    }
    //    if (oddList.Count == 1 && oddList[0].LdsInfo == ldsInfo)
    //    {
    //      _Statistics.Summarized++;
    //      oddList.RemoveAt(0);
    //    }
    //  }
    //}

    //private void ReportError(string text)
    //{
    //  if (_FeedbackDelegate != null)
    //    _FeedbackDelegate(text);
    //}

    private void ReportError(string text, params object[] arguments)
    {
      if (_FeedbackDelegate == null) return;
      if (arguments.Length > 0) ReportError(string.Format(text, arguments));
      else _FeedbackDelegate(text);
    }

    //public StreetAnalyzerStatistics Statistics
    //{
    //  get { return _Statistics; }
    //}

    private int SummarizeData(IEnumerable<StreetAnalysisData> dataList)
    {
      // Start by sorting the data as follows on AddressPrimaryLowNumber, 
      // AddressPrimaryHighNumber and splitting it into 3 lists by odd, even, both;
      var splits = dataList.OrderBy(
          item =>
            item.PrimaryLowNumber.PadRight(AddressFinder.MaxHouseNumberLength) +
            item.PrimaryHighNumber)
        .GroupBy(item => item.PrimaryOddEvenNormalized)
        .ToDictionary(group => group.Key, group => group.ToList());

      List<StreetAnalysisData> evenList;
      List<StreetAnalysisData> oddList;
      List<StreetAnalysisData> bothList;

      // Make sure all 3 lists exist
      if (!splits.TryGetValue("E", out evenList))
      {
        evenList = new List<StreetAnalysisData>();
        splits["E"] = evenList;
      }
      if (!splits.TryGetValue("O", out oddList))
      {
        oddList = new List<StreetAnalysisData>();
        splits["O"] = oddList;
      }
      if (!splits.TryGetValue("B", out bothList))
      {
        bothList = new List<StreetAnalysisData>();
        splits["B"] = bothList;
      }

      // Fixup the "B" list
      foreach (var data in bothList) data.PrimaryOddEven = "B";

      // Process each list separately
      foreach (var split in splits)
      {
        CheckForSingleLdsInList(split.Value);
        AnalyzeWildcard(split.Value);
        AnalyzeContainment(split.Value);
        CheckForMergeableItems(split.Value, split.Key);
      }

      // Commented because not safe due to same street in multiple zips
      //CheckForSingleLdsOverall(splits);
      //RemoveRedundantOddEvenLists(splits);

      var rows = 0;
      foreach (var split in splits.Values)
        foreach (var data in split)
        {
          if (!_SuppressUpdate) data.Write(_TextWriter);
          rows++;
          rows += WritePastedVariants(data);
        }

      return rows;
    }

    private int WritePastedVariants(StreetAnalysisData data)
    {
      // We only paste adjacent pairs for now.
      if (data.StreetName == PoBox) return 0;
      var suffix = 'A';
      var rowsAdded = 0;
      var splitName = data.StreetName.Split(' ');
      if (splitName.Length > 1)
        for (var pasteIndex = 1; pasteIndex < splitName.Length; pasteIndex++)
        {
          var pastedParts = new List<string>();
          var pastedPart = splitName[pasteIndex - 1] + splitName[pasteIndex];
          pastedParts.AddRange(splitName.Take(pasteIndex - 1));
          pastedParts.Add(pastedPart);
          pastedParts.AddRange(splitName.Skip(pasteIndex + 1));
          var newData = data.Clone();
          newData.StreetName = string.Join(" ", pastedParts);
          if (!newData.UpdateKey.Contains('-')) newData.UpdateKey += '-';
          newData.UpdateKey += suffix;
          suffix = (char) (suffix + 1);
          if (!_SuppressUpdate) newData.Write(_TextWriter);
          rowsAdded++;
        }
      return rowsAdded;
    }

    // ReSharper disable once InconsistentNaming
    private void ValidatePlus4s(StreetAnalysisData data)
    {
      if ((data.Plus4Low == null) || (data.Plus4Low.Length != 4) ||
        !data.Plus4Low.IsDigits())
      {
        _Statistics.InvalidZip4++;
        throw new VoteException("{0} invalid Plus4Low: {1}-{2}", data.UpdateKey,
          data.ZipCode, data.Plus4Low.ToStringOrNull());
      }

      if ((data.Plus4High == null) || (data.Plus4High.Length != 4) ||
        !data.Plus4High.IsDigits())
      {
        _Statistics.InvalidZip4++;
        throw new VoteException("{0} invalid Plus4High: {1}-{2}", data.UpdateKey,
          data.ZipCode, data.Plus4High.ToStringOrNull());
      }

      if (string.CompareOrdinal(data.Plus4Low, data.Plus4High) > 0)
        throw new VoteException("{0} Plus4Low > Plus4High {1}-{2} {1}-{3}",
          data.UpdateKey, data.ZipCode, data.Plus4Low, data.Plus4High);
    }
  }
}