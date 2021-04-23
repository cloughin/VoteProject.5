using System;
using System.Diagnostics;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using DB.Vote;
using Vote.Reports;
using static System.String;

namespace Vote.Admin
{
  public partial class OfficesPage : SecureAdminPage
  {
    #region legacy

    public static string Fail(string msg) => $"<span class=\"MsgFail\">****FAILURE**** {msg}</span>";

    public static string Message(string msg) => $"<span class=\"Msg\">{msg}</span>";

    #endregion legacy

    #region Private

    private void CreateOfficeClassRadioButtons()
    {
      // initial office class selection is from Query String as string ordinal
      var initialOfficeClass = Offices.GetValidatedOfficeClass(GetQueryString("class"));

      // iterator options
      var iteratorOptions = GetOfficeClassesOptions.IncludeAll;
      switch (AdminPageLevel)
      {
        case AdminPageLevel.State:
          iteratorOptions |= GetOfficeClassesOptions.IncludeCongress |
            GetOfficeClassesOptions.IncludeState |
            GetOfficeClassesOptions.IncludeByDistrict;
          break;

        case AdminPageLevel.County:
          iteratorOptions |= GetOfficeClassesOptions.IncludeCounty;
          break;

        case AdminPageLevel.Local:
          iteratorOptions |= GetOfficeClassesOptions.IncludeLocal;
          break;
      }

      // create a button for each OfficeClass returned by the iterator
      foreach (var officeClass in Offices.GetOfficeClasses(iteratorOptions))
      {
        var listItem = new ListItem();
        RadioButtonListOfficeClass.Items.Add(listItem);
        listItem.Value = officeClass.ToInt().ToString(CultureInfo.InvariantCulture);
        listItem.Selected = officeClass == initialOfficeClass;

        var text = GetOfficeClassDescription(officeClass.ToOfficeClass(), true) +
          " ({0})";

        var officeCount = OfficesAdminReportView.CountData(GetOfficesAdminReportOptions(officeClass));

        listItem.Text = Format(text, officeCount);
      }
    }

    private void GenerateReport()
    {
      var desc = GetOfficeClassDescription(GetSelectedOfficeClass());

      LabelOfficesReportTitle.Text = desc;

      OfficesAdminReport.GetReport(GetOfficesAdminReportOptions()).AddTo(ReportPlaceHolder);

      Msg.Text = Message($"The report below is for {desc}");
    }

    private string GetOfficeClassDescription(OfficeClass officeClass, bool shortDesc = false)
    {
      return (shortDesc
        ? Offices.GetShortLocalizedOfficeClassDescription(officeClass, StateCode,
          CountyCode, LocalKey)
        : Offices.GetLocalizedOfficeClassDescription(officeClass, StateCode,
          CountyCode, LocalKey)) + " Offices";
    }

    private OfficesAdminReportViewOptions GetOfficesAdminReportOptions(
      OfficeClass? officeClass = null)
    {
      var option = OfficesAdminReportViewOption.None;
      switch (AdminPageLevel)
      {
        case AdminPageLevel.State:
          option = OfficesAdminReportViewOption.ByState;
          break;

        case AdminPageLevel.County:
          option = OfficesAdminReportViewOption.ByCounty;
          break;

        case AdminPageLevel.Local:
          option = OfficesAdminReportViewOption.ByLocal;
          break;
      }

      return new OfficesAdminReportViewOptions
      {
        Option = option,
        OfficeClass = officeClass ?? GetSelectedOfficeClass(),
        StateCode = StateCode,
        CountyCode = CountyCode,
        LocalKey = LocalKey
      };
    }

    private OfficeClass GetSelectedOfficeClass()
      => Offices.GetValidatedOfficeClass(RadioButtonListOfficeClass.SelectedValue);

    private void SetPageTitleAndHeading()
    {
      var title =
        $"{Offices.GetElectoralClassDescription(StateCode, CountyCode, LocalKey)} Elected Offices";
      Page.Title = title;
      H1.InnerHtml = title;
    }

    private void UpdateReport()
    {
      if (GetSelectedOfficeClass() == OfficeClass.Undefined)
      {
        ReportTable.Visible = false;
        ReportPlaceHolder.Visible = false;
      }
      else
      {
        ReportTable.Visible = true;
        ReportPlaceHolder.Visible = true;
        GenerateReport();
      }
    }

    #endregion Private

    #region Event handlers and overrides

    private void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
        try
        {
          var scriptManager = ScriptManager.GetCurrent(this);
          Debug.Assert(scriptManager != null, "scriptManager != null");
          scriptManager.Scripts.Add(new ScriptReference("/js/legacy.js"));

          SetPageTitleAndHeading();

          CreateOfficeClassRadioButtons();
          //UpdateMasterOnlyControls();
          UpdateReport();

          if (GetSelectedOfficeClass() == OfficeClass.Undefined)
            Msg.Text =
              Message(
                "Select a radio button for a report of the elected offices in a specific category.");
        }
        catch (Exception ex)
        {
          Msg.Text = Fail(ex.Message);
          LogException("Page_Load", ex);
        }
    }

    protected void RadioButtonListOffice_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        UpdateReport();
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        LogException("RadioButtonListOffice_SelectedIndexChanged", ex);
      }
    }

    #endregion Event handlers and overrides
  }
}