using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using DB;
using DB.Vote;

namespace Vote.Reports
{
  internal sealed class ElectedReportResponsive : OfficialsReportResponsive
  {
    #region Private

    private sealed class AllLocalsCategory : LocalCategory
    {
      private IList<IGrouping<string, DataRow>> _CurrentData;

      protected override IList<IGrouping<string, DataRow>> GetOffices()
      {
        return _CurrentData;
      }

      public void GenerateAll(ElectedReportResponsive electedReport, string stateCode,
        string countyCode, Control reportContainer = null)
      {
        var localGroups =
          electedReport.DataManager.GetDataSubset(new LocalFilter(),
            new OfficialsSort())
            .GroupBy(row => row.LocalCode())
            .ToList();
        if (localGroups.Count == 0) return;

        var localNamesDictionary = LocalDistricts.GetNamesDictionary(stateCode,
          countyCode);
        localGroups = localGroups.OrderBy(g => localNamesDictionary[g.Key])
          .ToList();
        foreach (var local in localGroups)
        {
          LocalName = localNamesDictionary[local.Key];
          _CurrentData = local.GroupBy(row => row.OfficeKey())
            .ToList();
          Generate(electedReport, false, stateCode, countyCode, string.Empty, reportContainer);
        }
      }
    }

    private Control GenerateReport(string stateCode, string countyCode,
      string congressionalDistrict, string stateSenateDistrict,
      string stateHouseDistrict, bool includeLocals, bool localsSeparated = false)
    {
      ReportUser = ReportUser.Public;

      DataManager.SetData(Offices.GetElectedReportData(stateCode, countyCode,
        congressionalDistrict, stateSenateDistrict, stateHouseDistrict, includeLocals));

      (new USPresidentCategory()).Generate(this);
      (new USSenateCategory()).Generate(this, false, stateCode);
      (new USHouseCategory()).Generate(this, false, stateCode);
      (new USGovernorsCategory()).Generate(this, false, stateCode);
      (new StatewideCategory()).Generate(this, false, stateCode);
      (new StateSenateCategory()).Generate(this, false, stateCode);
      (new StateHouseCategory()).Generate(this, false, stateCode);
      (new CountyCategory()).Generate(this, false, stateCode, countyCode);
      if (includeLocals)
      {
        var reportContainer = ReportContainer;
        if (localsSeparated)
        {
          new HtmlDiv { InnerText = "Local Elected Officials" }.AddTo(ReportContainer, "category-title accordion-header elected-report-locals");
          reportContainer = new HtmlDiv().AddTo(ReportContainer,
            "category-content accordion-content elected-report-locals elected-report-locals-content");
        }
        new AllLocalsCategory().GenerateAll(this, stateCode, countyCode, reportContainer);
      }

      return ReportContainer.AddCssClasses("elected-report");
    }

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

    public static Control GetReport(string stateCode, string countyCode,
      string congressionalDistrict, string stateSenateDistrict,
      string stateHouseDistrict, bool includeLocals, bool localsSeparated = false)
    {
      var reportObject = new ElectedReportResponsive();
      return reportObject.GenerateReport(stateCode, countyCode, congressionalDistrict,
        stateSenateDistrict, stateHouseDistrict, includeLocals, localsSeparated);
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