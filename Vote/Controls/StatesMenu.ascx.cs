using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Vote.Controls
{
  public partial class StatesMenuControl : UserControl
  {
    #region Private

    private string _HRefTemplate;
    private string _HRefTemplateForFormat;

    private void AddState(Control block, string stateCode)
    {
      var a = new HtmlAnchor();
      block.Controls.Add(a);
      a.HRef = string.Format(_HRefTemplateForFormat, stateCode);
      a.InnerText = StateCache.GetStateName(stateCode);
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
        _HRefTemplateForFormat = value.Replace("{StateCode}", "{0}");
      }
    }


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
        var statesBlock = StatesBlock1;
        foreach (var stateCode in StateCache.All51StateCodes)
        {
          AddState(statesBlock, stateCode);
          switch (stateCode)
          {
            case "KS":
              statesBlock = StatesBlock2;
              break;
            case "NC":
              statesBlock = StatesBlock3;
              break;
          }
        }
      }
    }

    #endregion Event handlers and overrides
  }
}