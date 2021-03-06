using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB.Vote;
using static System.String;

namespace Vote.Controls
{
  public partial class CountiesMenuControl : UserControl
  {
    #region Private

    private string _HRefTemplate;
    private string _HRefTemplateForFormat;

    private void AddCounty(Control block, string countyCode, string countyName)
    {
      var a = new HtmlAnchor();
      block.Controls.Add(a);
      a.HRef = Format(_HRefTemplateForFormat, StateCode, countyCode);
      a.Title = countyName;
      a.InnerText = countyName;
    }

    #endregion Private

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global

    public string HRefTemplate
    {
      get { return _HRefTemplate; }
      set
      {
        _HRefTemplate = value;
        _HRefTemplateForFormat = value.Replace("{StateCode}", "{0}")
          .Replace("{CountyCode}", "{1}");
      }
    }

    public string StateCode { private get; set; }


    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public

    #region Event handlers and overrides

    protected void Page_Load(object sender, EventArgs e)
    {
      // ReSharper disable InvertIf
      if (!IsPostBack)
      {
        var table = Counties.GetCacheDataByStateCode(StateCode);

        int columns;
        if (table.Count == 0) columns = 0;
        else if (table.Count <= 17) columns = 1;
        else if (table.Count <= 34) columns = 2;
        else if (table.Count <= 90) columns = 3;
        else if (table.Count <= 120) columns = 4;
        else if (table.Count <= 160) columns = 5;
        else columns = 6;

        CountiesMenu.AddCssClasses("has-" + columns.ToString(CultureInfo.InvariantCulture) +
          "-cols");

        if (columns < 6) CountiesBlock6.Visible = false;
        if (columns < 5) CountiesBlock5.Visible = false;
        if (columns < 4) CountiesBlock4.Visible = false;
        if (columns < 3) CountiesBlock3.Visible = false;
        if (columns < 2) CountiesBlock2.Visible = false;

        if (columns == 0)
        {
          var span = new HtmlSpan();
          CountiesBlock1.Controls.Add(span);
          span.InnerText = "No counties found for " + StateCode;
        }
        else
        {
          var blockControls = new[]
          {
            CountiesBlock1, CountiesBlock2, CountiesBlock3, CountiesBlock4, CountiesBlock5,
            CountiesBlock6
          };
          var blockIndex = -1;
          var countiesPerBlock = (table.Count - 1) / columns + 1;
          for (var inx = 0; inx < table.Count; inx++)
          {
            var row = table[inx];
            if (inx % countiesPerBlock == 0) blockIndex++;
            AddCounty(blockControls[blockIndex], row.CountyCode, row.County);
          }
        }
      }
      // ReSharper restore InvertIf
    }

    #endregion Event handlers and overrides
  }
}