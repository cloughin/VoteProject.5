using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using DB;
using DB.Vote;

namespace Vote.Reports
{
  //internal sealed class ElectedReport : OfficialsReport
  //{
  //  #region Private

  //  private sealed class AllLocalsCategory : LocalCategory
  //  {
  //    private IList<IGrouping<string, DataRow>> _CurrentData;

  //    protected override IList<IGrouping<string, DataRow>> GetOffices()
  //    {
  //      return _CurrentData;
  //    }

  //    public void GenerateAll(ElectedReport electedReport, string stateCode,
  //      string countyCode)
  //    {
  //      var localGroups =
  //        electedReport.DataManager.GetDataSubset(new LocalFilter(),
  //          new OfficialsSort())
  //          .GroupBy(row => row.LocalCode())
  //          .ToList();
  //      if (localGroups.Count == 0) return;

  //      var localNamesDictionary = LocalDistricts.GetNamesDictionary(stateCode,
  //        countyCode);
  //      localGroups = localGroups.OrderBy(g => localNamesDictionary[g.Key])
  //        .ToList();
  //      foreach (var local in localGroups)
  //      {
  //        LocalName = localNamesDictionary[local.Key];
  //        _CurrentData = local.GroupBy(row => row.OfficeKey())
  //          .ToList();
  //        Generate(electedReport, stateCode, countyCode);
  //      }
  //    }
  //  }

  //  private Control GenerateReport(string stateCode, string countyCode,
  //    string congressionalDistrict, string stateSenateDistrict,
  //    string stateHouseDistrict, bool includeLocals)
  //  {
  //    ReportUser = ReportUser.Public;

  //    DataManager.SetData(Offices.GetElectedReportData(stateCode, countyCode,
  //      congressionalDistrict, stateSenateDistrict, stateHouseDistrict, includeLocals));

  //    (new USPresidentCategory()).Generate(this);
  //    (new USSenateCategory()).Generate(this, stateCode);
  //    (new USHouseCategory()).Generate(this, stateCode);
  //    (new USGovernorsCategory()).Generate(this, stateCode);
  //    (new StatewideCategory()).Generate(this, stateCode);
  //    (new StateSenateCategory()).Generate(this, stateCode);
  //    (new StateHouseCategory()).Generate(this, stateCode);
  //    (new CountyCategory()).Generate(this, stateCode, countyCode);
  //    if (includeLocals)
  //      (new AllLocalsCategory()).GenerateAll(this, stateCode, countyCode);
  //    return ReportContainer;
  //  }

  //  #endregion Private

  //  #region Public

  //  #region ReSharper disable

  //  // ReSharper disable MemberCanBePrivate.Global
  //  // ReSharper disable MemberCanBeProtected.Global
  //  // ReSharper disable UnusedMember.Global
  //  // ReSharper disable UnusedMethodReturnValue.Global
  //  // ReSharper disable UnusedAutoPropertyAccessor.Global
  //  // ReSharper disable UnassignedField.Global

  //  #endregion ReSharper disable

  //  //public static Control GetReport(string stateCode, string countyCode,
  //  //  string congressionalDistrict, string stateSenateDistrict,
  //  //  string stateHouseDistrict, bool includeLocals)
  //  //{
  //  //  var reportObject = new ElectedReport();
  //  //  return reportObject.GenerateReport(stateCode, countyCode, congressionalDistrict,
  //  //    stateSenateDistrict, stateHouseDistrict, includeLocals);
  //  //}

  //  #region ReSharper restore

  //  // ReSharper restore UnassignedField.Global
  //  // ReSharper restore UnusedAutoPropertyAccessor.Global
  //  // ReSharper restore UnusedMethodReturnValue.Global
  //  // ReSharper restore UnusedMember.Global
  //  // ReSharper restore MemberCanBeProtected.Global
  //  // ReSharper restore MemberCanBePrivate.Global

  //  #endregion ReSharper restore

  //  #endregion Public
  //}
}