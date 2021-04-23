using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using DB;
using DB.Vote;
using VoteLibrary.Utility;
using static System.String;

namespace Vote.Reports
{
  public sealed class ElectedReportResponsive : OfficialsReportResponsive
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
            new OfficialsSort()).GroupBy(row => row.LocalKey()).ToList();

        if (localGroups.Count == 0) return;

        var localNamesDictionary = LocalDistricts.GetNamesDictionary(stateCode,
          countyCode);
        localGroups = localGroups.OrderBy(g => localNamesDictionary[g.Key], new AlphanumericComparer())
          .ToList();
        foreach (var local in localGroups)
        {
          LocalName = localNamesDictionary[local.Key];
          _CurrentData = local.GroupBy(row => row.OfficeKey())
            .ToList();
          Generate(electedReport, false, stateCode, countyCode, Empty, reportContainer);
        }
      }
    }

    private Control GenerateReport(string stateCode, string countyCode, string congressionalDistrict, 
      string stateSenateDistrict, string stateHouseDistrict, string district, string place,
      string elementary, string secondary, string unified, string cityCouncil, 
      string countySupervisors, string schoolDistrictDistrict, bool includeLocals, bool localsSeparated = false)
    {
      ReportUser = ReportUser.Public;

      DataManager.SetData(
        Offices.GetElectedReportData(stateCode, countyCode, congressionalDistrict,
          stateSenateDistrict, stateHouseDistrict, district, place, elementary, secondary, 
          unified, cityCouncil, countySupervisors, schoolDistrictDistrict, includeLocals));

      new USPresidentCategory().Generate(this);
      new USSenateCategory().Generate(this, false, stateCode);
      new USHouseCategory().Generate(this, false, stateCode);
      new USGovernorsCategory().Generate(this, false, stateCode);
      new StatewideCategory().Generate(this, false, stateCode);
      new StateSenateCategory().Generate(this, false, stateCode);
      new StateHouseCategory().Generate(this, false, stateCode);
      new CountyCategory().Generate(this, false, stateCode, countyCode);
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

    public static Control GetReport(string stateCode, string countyCode, string congressionalDistrict, 
      string stateSenateDistrict, string stateHouseDistrict, string district, string place,
      string elementary, string secondary, string unified, string cityCouncil, 
      string countySupervisors, string schoolDistrictDistrict, bool includeLocals, bool localsSeparated = false)
    {
      var reportObject = new ElectedReportResponsive();
      return reportObject.GenerateReport(stateCode, countyCode, congressionalDistrict,
        stateSenateDistrict, stateHouseDistrict, district, place, elementary, secondary, unified, 
        cityCouncil, countySupervisors, schoolDistrictDistrict, includeLocals, localsSeparated);
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