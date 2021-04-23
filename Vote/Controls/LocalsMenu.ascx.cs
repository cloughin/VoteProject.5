using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB.Vote;

namespace Vote.Controls
{
  public partial class LocalsMenuControl : UserControl
  {
    #region Private

    private string _HRefTemplate;
    private string _HRefTemplateForFormat;
    private string _CountyCode;

    private void AddLocal(Control block, string localCode, string localName)
    {
      var a = new HtmlAnchor();
      block.Controls.Add(a);
      a.HRef = string.Format(
        _HRefTemplateForFormat, StateCode, CountyCode, localCode);
      a.Title = localName;
      a.InnerText = localName;
    }

    #endregion Private

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global

    public string CountyCode
    {
      get { return _CountyCode; }
      set { _CountyCode = value.ZeroPad(3); }
    }

    public string HRefTemplate
    {
      get { return _HRefTemplate; }
      set
      {
        _HRefTemplate = value;
        _HRefTemplateForFormat = value.Replace("{StateCode}", "{0}")
          .Replace("{CountyCode}", "{1}")
          .Replace("{LocalCode}", "{2}");
      }
    }

    public string StateCode { get; set; }


    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public

    #region Event handlers and overrides

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        var table = LocalDistricts.GetDataByStateCodeCountyCode(
          StateCode, CountyCode);

        int columns;
        if (table.Count == 0) columns = 0;
        else if (table.Count <= 17) columns = 1;
        else if (table.Count <= 34) columns = 2;
        else if (table.Count <= 90) columns = 3;
        else if (table.Count <= 120) columns = 4;
        else if (table.Count <= 160) columns = 5;
        else columns = 6;

        LocalsMenu.AddCssClasses(
          "has-" + columns.ToString(CultureInfo.InvariantCulture) + "-cols");

        if (columns < 6) LocalsBlock6.Visible = false;
        if (columns < 5) LocalsBlock5.Visible = false;
        if (columns < 4) LocalsBlock4.Visible = false;
        if (columns < 3) LocalsBlock3.Visible = false;
        if (columns < 2) LocalsBlock2.Visible = false;

        if (columns == 0)
        {
          var span = new HtmlSpan();
          LocalsBlock1.Controls.Add(span);
          span.InnerText = "No districts found for County" + StateCode + "-" +
            CountyCode;
        }
        else
        {
          var blockControls = new[]
          {
            LocalsBlock1, LocalsBlock2, LocalsBlock3, LocalsBlock4, LocalsBlock5,
            LocalsBlock6
          };
          var blockIndex = -1;
          var countiesPerBlock = (table.Count - 1) / columns + 1;
          for (var inx = 0; inx < table.Count; inx++)
          {
            var row = table[inx];
            if (inx % countiesPerBlock == 0)
              blockIndex++;
            AddLocal(
              blockControls[blockIndex], row.LocalCode.ZeroPad(3), row.LocalDistrict);
          }
        }
      }
    }

    #endregion Event handlers and overrides
  }
}