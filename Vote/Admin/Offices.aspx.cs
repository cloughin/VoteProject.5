using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB.Vote;
using Vote.Reports;

namespace Vote.Admin
{
  public partial class OfficesPage : SecureAdminPage
  {
    #region from db

    public static string Ok(string msg) =>
      $"<span class=\"MsgOk\">SUCCESS!!! {msg}</span>";

    public static string Fail(string msg) =>
      $"<span class=\"MsgFail\">****FAILURE**** {msg}</span>";

    public static string Message(string msg) =>
      $"<span class=\"Msg\">{msg}</span>";

    #endregion from db

    #region Private

    private void CreateOfficeClassAllIdentifiedCheckBoxes()
    {
      CheckBoxListOfficeClassAllIdentified.Items.Clear();

      var options = GetOfficeClassesOptions.None;
      switch (AdminPageLevel)
      {
        case AdminPageLevel.State:
          options = GetOfficeClassesOptions.IncludeUSSenate |
            GetOfficeClassesOptions.IncludeUSHouse |
            GetOfficeClassesOptions.IncludeState;
          break;

        case AdminPageLevel.County:
          options = GetOfficeClassesOptions.IncludeCounty;
          break;

        case AdminPageLevel.Local:
          options = GetOfficeClassesOptions.IncludeLocal;
          break;
      }

      foreach (var officeClass in
        Offices.GetOfficeClasses(options))
      {
        var listItem = new ListItem();
        CheckBoxListOfficeClassAllIdentified.Items.Add(listItem);
        var allIdentified = OfficesAllIdentified.GetIsOfficesAllIdentified(
          StateCode, officeClass.ToInt(), CountyCode, LocalCode);
        listItem.Text = GetOfficeClassLinkOrText(StateCode, officeClass,
          allIdentified, CountyCode, LocalCode);
        listItem.Value = officeClass.ToInt()
          .ToString(CultureInfo.InvariantCulture);
        listItem.Selected = allIdentified;
      }
    }

    private void CreateOfficeClassRadioButtons()
    {
      // initial office class selection is from Query String as string ordinal
      var initialOfficeClass =
        Offices.GetValidatedOfficeClass(GetQueryString("class"));

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
        listItem.Value = officeClass.ToInt()
          .ToString(CultureInfo.InvariantCulture);
        listItem.Selected = officeClass == initialOfficeClass;

        var text =
          GetOfficeClassDescription(officeClass.ToOfficeClass(), shortDesc: true) +
          " ({0})";

        var officeCount =
          OfficesAdminReportView.CountData(GetOfficesAdminReportOptions(officeClass));

        listItem.Text = string.Format(text, officeCount);
      }
    }

    private void GenerateReport()
    {
      var desc = GetOfficeClassDescription(GetSelectedOfficeClass());

      LabelOfficesReportTitle.Text = desc;

      OfficesAdminReport.GetReport(GetOfficesAdminReportOptions())
        .AddTo(ReportPlaceHolder);

      Msg.Text = Message("The report below is for " + desc);
    }

    private string GetOfficeClassDescription(OfficeClass officeClass,
      bool shortDesc = false) => 
      (shortDesc
      ? Offices.GetShortLocalizedOfficeClassDescription(officeClass, StateCode,
        CountyCode, LocalCode)
      : Offices.GetLocalizedOfficeClassDescription(officeClass, StateCode,
        CountyCode, LocalCode)) + " Offices";

    private string GetOfficeClassLinkOrText(string stateCode,
      OfficeClass officeClass, bool allIdentified, string countyCode = "",
      string localCode = "")
    {
      var text = GetOfficeClassDescription(officeClass);

      if (allIdentified) return text;

      var a = new HtmlAnchor
      {
        HRef = GetOfficePageUrl(stateCode, officeClass, countyCode, localCode),
        Target = "office",
        InnerHtml = text
      };

      return a.RenderToString();
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
        OfficeClass =
          officeClass ?? GetSelectedOfficeClass(),
        StateCode = StateCode,
        CountyCode = CountyCode,
        LocalCode = LocalCode
      };
    }

    private OfficeClass GetSelectedOfficeClass() => 
      Offices.GetValidatedOfficeClass(RadioButtonListOfficeClass.SelectedValue);

    private void SetPageTitleAndHeading()
    {
      var title =
        Offices.GetElectoralClassDescription(StateCode, CountyCode, LocalCode) +
        " Elected Offices";
      Page.Title = title;
      H1.InnerHtml = title;
    }

    private void UpdateMasterOnlyControls()
    {
      if (!IsSuperUser /*|| AdminPageLevel != AdminPageLevel.State*/) return;

      MasterOnlyTable.Visible = true;

      CreateOfficeClassAllIdentifiedCheckBoxes();

      HyperLinkOffices.NavigateUrl = "/Master/MgtReports.aspx?Report=Offices" +
        "&State=" + StateCode + "&What=All";
      HyperLinkStateOffices.NavigateUrl =
        "/Master/MgtReports.aspx?Report=Offices" + "&State=" + StateCode +
        "&What=State";
      HyperLinkCountyOffices.NavigateUrl =
        "/Master/MgtReports.aspx?Report=Offices" + "&State=" + StateCode +
        "&What=Counties";
      HyperLinkLocalOffices.NavigateUrl =
        "/Master/MgtReports.aspx?Report=Offices" + "&State=" + StateCode +
        "&What=Locals";
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
          UpdateMasterOnlyControls();
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

    protected void RadioButtonListOffice_SelectedIndexChanged(object sender,
      EventArgs e)
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

    protected void CheckBoxListOfficeClassAllIdentified_SelectedIndexChanged(
      object sender, EventArgs e)
    {
      try
      {
        // Get all OfficesAllIdentified rows for the jurisdiction and make a 
        // dictionary of only the true values
        var dictionary = OfficesAllIdentified.GetDataByStateCode(StateCode,
            CountyCode, LocalCode)
          .Where(row => row.IsOfficesAllIdentified)
          .ToDictionary(row => row.OfficeLevel.ToOfficeClass(),
            row => null as object);

        // now get any items that don't match the dictionary
        var items = CheckBoxListOfficeClassAllIdentified.Items.OfType<ListItem>()
          .Where(
            item =>
              dictionary.ContainsKey(Offices.GetValidatedOfficeClass(item.Value)) !=
              item.Selected);

        // should only be one item, but we'll loop just in case...
        Msg.Text = string.Empty;
        var regenerateReport = GetSelectedOfficeClass() == OfficeClass.All;
        foreach (var item in items)
        {
          var officeClass = Offices.GetValidatedOfficeClass(item.Value);
          if (OfficesAllIdentified.StateCodeOfficeLevelCountyCodeLocalCodeExists(
            StateCode, officeClass.ToInt(), CountyCode, LocalCode))
            OfficesAllIdentified.UpdateIsOfficesAllIdentified(item.Selected,
              StateCode, officeClass.ToInt(), CountyCode, LocalCode);
          else if (item.Selected)
            OfficesAllIdentified.Insert(StateCode, CountyCode, LocalCode,
              officeClass.ToInt(), true);
          var officeDesc = GetOfficeClassDescription(officeClass, true);
          Msg.Text += item.Selected
            ? Ok(officeDesc +
              " was changed to PROHIBIT the addition of offices.")
            : Ok(officeDesc + " was changed to ALLOW the addition of offices.");
          if (officeClass == GetSelectedOfficeClass())
            regenerateReport = true;
        }
        if (regenerateReport)
        {
          GenerateReport();
          UpdatePanel.Update();
        }
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        LogException("CheckBoxListOfficeClassAllIdentified_SelectedIndexChanged",
          ex);
      }
    }

    #endregion Event handlers and overrides
  }
}