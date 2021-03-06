using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB.Vote;

namespace Vote.Controls
{
  public partial class SelectJurisdictions : UserControl
  {
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    public string CssClass { get; set; }
    public string InnerCssClass { get; set; }
    // ReSharper restore UnusedAutoPropertyAccessor.Global

    private void BuildCountiesList(string stateCode, string singleCounty = null)
    {
      CountiesPickerList.Controls.Clear();
      foreach (var countyCode in CountyCache.GetCountiesByState(stateCode))
        if ((singleCounty == null) || (singleCounty == countyCode))
        {
          var countyName = CountyCache.GetCountyName(stateCode, countyCode);
          var p = new HtmlP().AddTo(CountiesPickerList,
            "sub-label");
          p.Attributes["title"] = countyName;
          new HtmlInputCheckBox {Value = countyCode, Checked = true}.AddTo(p);
          new HtmlSpan {InnerHtml = countyName}.AddTo(p);
        }
    }

    private void BuildLocalsList(string stateCode, string countyCode,
      string singleLocal = null)
    {
      var locals = LocalDistricts.GetNamesDictionary(stateCode,
        countyCode);
      LocalsPickerList.Controls.Clear();
      foreach (var local in locals.OrderBy(kvp => kvp.Value))
        if ((singleLocal == null) || (singleLocal == local.Key))
        {
          var p = new HtmlP().AddTo(LocalsPickerList,
            "sub-label");
          p.Attributes["title"] = local.Value;
          new HtmlInputCheckBox {Value = local.Key, Checked = true}.AddTo(p);
          new HtmlSpan {InnerHtml = local.Value}.AddTo(p);
        }
    }

    private void BuildStatesList(string singleState = null)
    {
      StatesPickerList.Controls.Clear();
      foreach (var stateCode in StateCache.All51StateCodes)
        if ((singleState == null) || (singleState == stateCode))
        {
          var stateName = StateCache.GetStateName(stateCode);
          var p = new HtmlP().AddTo(StatesPickerList,
            "sub-label");
          p.Attributes["title"] = stateName;
          new HtmlInputCheckBox {Value = stateCode, Checked = true}.AddTo(p);
          new HtmlSpan {InnerHtml = stateName}.AddTo(p);
        }
    }

    public void Initialize(string stateCode = null, string countyCode = null,
      string localCode = null)
    {
      if (stateCode != null)
        StatesPickerSpecific.InnerText =
          StateCache.GetStateName(stateCode);
      CountiesPickerAllCheckbox.Checked = true;
      CountiesPickerAllCheckbox.Disabled = true;
      CountiesPickerListButton.Disabled = true;
      LocalsPickerAllCheckbox.Checked = true;
      LocalsPickerAllCheckbox.Disabled = true;
      LocalsPickerListButton.Disabled = true;
      if (stateCode == null)
      {
        StatesPickerAll.RemoveCssClass("hidden");
        StatesPickerAllCheckbox.Checked = true;
        StatesPickerSpecific.AddCssClasses("hidden");
        StatesPickerList.RemoveCssClass("hidden");
        BuildStatesList();
        CountiesPicker.AddCssClasses("disabled hidden");
        CountiesPickerListButtonContainer.RemoveCssClass("hidden");
        LocalsPicker.AddCssClasses("disabled hidden");
      }
      else if (countyCode == null)
      {
        CountiesPicker.AddCssClasses("hidden");
        CountiesPickerAllCheckbox.Disabled = false;
        BuildStatesList(stateCode);
        CountiesPickerList.RemoveCssClass("hidden");
        BuildCountiesList(stateCode);
        LocalsPicker.AddCssClasses("disabled hidden");
      }
      else if (localCode == null)
      {
        BuildStatesList(stateCode);
        CountiesPickerAll.AddCssClasses("hidden");
        CountiesPickerAllCheckbox.Checked = false;
        CountiesPickerSpecific.RemoveCssClass("hidden");
        CountiesPickerSpecific.InnerText =
          CountyCache.GetCountyName(stateCode, countyCode);
        BuildCountiesList(stateCode, countyCode);
        LocalsPicker.RemoveCssClass("disabled");
        LocalsPickerAllCheckbox.Disabled = false;
        LocalsPickerListButtonContainer.AddCssClasses("hidden");
        LocalsPickerList.RemoveCssClass("hidden");
        LocalsPicker.AddCssClasses("disabled hidden");
        BuildLocalsList(stateCode, countyCode);
      }
      else
      {
        BuildStatesList(stateCode);
        CountiesPickerAll.AddCssClasses("hidden");
        CountiesPickerAllCheckbox.Checked = false;
        CountiesPickerSpecific.RemoveCssClass("hidden");
        CountiesPickerSpecific.InnerText =
          CountyCache.GetCountyName(stateCode, countyCode);
        BuildCountiesList(stateCode, countyCode);
        LocalsPicker.RemoveCssClass("disabled");
        LocalsPickerAll.AddCssClasses("hidden");
        LocalsPickerAllCheckbox.Checked = false;
        LocalsPickerSpecific.RemoveCssClass("hidden");
        LocalsPickerSpecific.InnerText =
          LocalDistricts.GetLocalDistrict(stateCode, countyCode,
            localCode);
        LocalsPickerListButtonContainer.AddCssClasses("hidden");
        BuildLocalsList(stateCode, countyCode, localCode);
      }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      SelectJurisdictionsControl.AddCssClasses(CssClass);
      StatesPicker.AddCssClasses(InnerCssClass);
      CountiesPicker.AddCssClasses(InnerCssClass);
      LocalsPicker.AddCssClasses(InnerCssClass);
    }
  }
}