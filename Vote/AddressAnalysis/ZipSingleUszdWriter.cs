using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DB.VoteZipNewLocal;

namespace Vote
{
  public sealed class ZipSingleUszdWriter
  {
    #region Private

    private Action<string> _FeedbackDelegate;
    private readonly bool _SuppressUpdate;

    private static IEnumerable<string> CreateZipPlus4List(
      IEnumerable<ZipStreetsDownloadedRow> table)
    {
      var zip4Dictionary = new Dictionary<string, object>();
      foreach (var row in table)
        if (row.Plus4Low == row.Plus4High)
          zip4Dictionary[row.Plus4Low] = null;
        else
        {
          var low = int.Parse(row.Plus4Low);
          var high = int.Parse(row.Plus4High);
          // high = Math.Min(high, low + 199);
          for (var n = low; n <= high; n++)
            zip4Dictionary[n.ToString(CultureInfo.InvariantCulture)
              .ZeroPad(4)] = null;
        }
      return zip4Dictionary.Keys;
    }

    private void Report(string text, params object[] arguments) => 
      _FeedbackDelegate?.Invoke(string.Format(text, arguments));

    #endregion Private

    #region Public

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnassignedField.Global

    #endregion ReSharper disable

    public ZipSingleUszdWriter(bool suppressUpdate)
    {
      _SuppressUpdate = suppressUpdate;
    }

    public bool DoOneZipCode(string zipCode)
    {
      // Get all zip rows from ZipStreetsDownloaded
      var isSingle = false;
      var found = false;
      string singleLdsStateCode = null;
      var streetsTable = ZipStreetsDownloaded.GetLookupDataByZipCode(zipCode, 0);
      if (streetsTable.Count > 0)
      {
        found = true;

        // Fetch and summarize the USZD data
        var ldsDictionary = new Dictionary<LdsInfo, object>();
        var zip4S = CreateZipPlus4List(streetsTable);
        var uszdTable = Uszd.GetDataByZip4List(zipCode, zip4S, 0);
        foreach (var row in uszdTable)
        {
          var stateCode = StateCache.StateCodeFromLdsStateCode(row.LdsStateCode);
          singleLdsStateCode = row.LdsStateCode;
          var ldsInfo = new LdsInfo(stateCode, row.Congress, row.StateSenate,
            row.StateHouse, row.County);
          ldsDictionary[ldsInfo] = null;
        }
        if (ldsDictionary.Count == 1)
        {
          isSingle = true;
          var singleLdsInfo = ldsDictionary.Keys.Single();
          if (!_SuppressUpdate)
            ZipSingleUszd.Insert(zipCode: zipCode,
              congress: singleLdsInfo.Congress.Substring(1),
              // LdsInfo padds it to 3
              stateSenate: singleLdsInfo.StateSenate,
              stateHouse: singleLdsInfo.StateHouse,
              ldsStateCode: singleLdsStateCode, county: singleLdsInfo.County,
              stateCode: singleLdsInfo.StateCode);
        }
      }

      if (found)
        Report("{0}: {1}", zipCode, isSingle ? "Single" : "Multiple");

      return isSingle;
    }

    public Action<string> Feedback
    {
      set { _FeedbackDelegate = value; }
    }

    #region ReSharper restore

    // ReSharper restore UnassignedField.Global
    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Public
  }
}