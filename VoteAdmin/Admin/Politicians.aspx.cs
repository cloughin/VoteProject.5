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
  public partial class PoliticiansPage : SecureAdminPage
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
            GetOfficeClassesOptions.IncludeState;
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

        var text = GetOfficeClassDescription(officeClass.ToOfficeClass(), shortDesc: true) +
          " ({0})";

        var politicianCount =
          PoliticiansAdminReportView.CountData(GetPoliticiansAdminReportOptions(officeClass));

        listItem.Text = Format(text, politicianCount);
      }
    }

    private void GenerateReport()
    {
      LabelPoliticiansReportTitle.Text = GetOfficeClassDescription(GetSelectedOfficeClass());

      PoliticiansAdminReport.GetReport(GetPoliticiansAdminReportOptions()).AddTo(ReportPlaceHolder);

      Msg.Text =
        Message(
          $"The report below is for {GetOfficeClassDescription(GetSelectedOfficeClass())}" +
          $" sorted by {(SortByOffice ? "office" : "name")}");
    }

    private string GetOfficeClassDescription(OfficeClass officeClass, bool shortDesc = false)
      =>
      (shortDesc
        ? Offices.GetShortLocalizedOfficeClassDescription(officeClass, StateCode, CountyCode,
          LocalKey)
        : Offices.GetLocalizedOfficeClassDescription(officeClass, StateCode, CountyCode, LocalKey)) +
      " Politicians";

    private PoliticiansAdminReportOptions GetPoliticiansAdminReportOptions(
      OfficeClass? officeClass = null)
    {
      var option = PoliticiansAdminReportViewOption.None;
      switch (AdminPageLevel)
      {
        case AdminPageLevel.State:
          option = PoliticiansAdminReportViewOption.ByState;
          break;

        case AdminPageLevel.County:
          option = PoliticiansAdminReportViewOption.ByCounty;
          break;

        case AdminPageLevel.Local:
          option = PoliticiansAdminReportViewOption.ByLocal;
          break;
      }

      return new PoliticiansAdminReportOptions
      {
        Option = option,
        OfficeClass = officeClass ?? GetSelectedOfficeClass(),
        StateCode = StateCode,
        CountyCode = CountyCode,
        LocalKey = LocalKey,
        SortByOffice = SortByOffice
      };
    }

    private OfficeClass GetSelectedOfficeClass()
      => Offices.GetValidatedOfficeClass(RadioButtonListOfficeClass.SelectedValue);

    private void SetPageTitleAndHeading()
    {
      var title = AdminPageLevel == AdminPageLevel.Federal
        ? "U.S. President, Senate and House"
        : Offices.GetElectoralClassDescription(StateCode, CountyCode, LocalKey);

      title += " Politicians";
      Page.Title = title;
      H1.InnerHtml = title;
    }

    private bool SortByOffice => RadioButtonListSort.SelectedValue == "Office";

    private void UpdateReport()
    {
      if (GetSelectedOfficeClass() == OfficeClass.Undefined)
      {
        SortOrderTable.Visible = false;
        ReportTable.Visible = false;
        PoliticiansMaintTable.Visible = false;
      }
      else
      {
        SortOrderTable.Visible = true;
        ReportTable.Visible = true;
        PoliticiansMaintTable.Visible = true;
        GenerateReport();
      }
    }

    #endregion Private

    #region Event handlers and overrides

    private void Page_Load(object sender, EventArgs e)
    {
      UpdatePoliticiansLink.HRef = GetUpdatePoliticiansPageUrl(StateCode);
      if (!IsPostBack)
        try
        {
          var scriptManager = ScriptManager.GetCurrent(this);
          Debug.Assert(scriptManager != null, "scriptManager != null");
          scriptManager.Scripts.Add(new ScriptReference("/js/legacy.js"));

          SetPageTitleAndHeading();

          if (GetQueryString("sort") == "Office")
            RadioButtonListSort.SelectedValue = "Office";

          CreateOfficeClassRadioButtons();
          //UpdateMasterOnlyControls();
          UpdateReport();

          if (GetSelectedOfficeClass() == OfficeClass.Undefined)
            Msg.Text =
              Message("Select a radio button for a report of a specific group of politicians.");
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
        //UpdateMasterOnlyControls();
        UpdateReport();
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        LogException("RadioButtonListOffice_SelectedIndexChanged", ex);
      }
    }

    protected void RadioButtonListSort_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        UpdateReport();
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        LogException("RadioButtonListSort_SelectedIndexChanged", ex);
      }
    }

    #endregion Event handlers and overrides
  }
}