using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using static System.String;

namespace Vote.Controls
{
  public partial class NoJurisdiction : UserControl
  {
    public bool IncludeAll { get; set; }

    public void SetHead(string head)
    {
      HeadMain.Controls.Clear();
      HeadMain.InnerHtml = head;
    }

    public void ShowHeadOptional(bool show) => HeadOptional.Visible = show;

    public void CreateLink(string href, string text) =>
      new HtmlAnchor
      {
        HRef = href,
        InnerText = text
      }.AddTo(StateLinks);

    public void CreateStateLinks(string template)
    {
      template = template.Replace("{StateCode}", "{0}");
      if (IncludeAll)
        CreateLink(Format(template, "US"), "All States");
      foreach (var stateCode in StateCache.All51StateCodes)
        CreateLink(Format(template, stateCode), StateCache.GetStateName(stateCode));
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }
  }
}