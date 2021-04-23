using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Vote.Master
{
  public partial class SetNoCachePage : Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        Page.Title = "Set NoCache";
        H1.InnerHtml = "Set NoCache";
      }
      else
      {
        PlaceHolder.Controls.Clear();
        var thead = new HtmlTHead();
        thead.AddTo(PlaceHolder);
        var tr = new HtmlTableRow();
        tr.AddTo(thead);
        var th = new HtmlTableHeadingCell { InnerText = "Domain", Width = "20%"};
        th.AddTo(tr);
        th = new HtmlTableHeadingCell { InnerText = "Result", Width = "80%" };
        th.AddTo(tr);

        var tbody = new HtmlTBody();
        tbody.AddTo(PlaceHolder);
        tr = new HtmlTableRow();
        tr.AddTo(tbody);
        var uri = UrlManager.GetSiteUri("/nocache.aspx");
        var td = new HtmlTableCell {InnerText = uri.Host};
        td.AddTo(tr);
        td = new HtmlTableCell();
        td.AddTo(tr);
        new HtmlIframe { Src = uri + "?P" }.AddTo(td);

        foreach (var stateCode in StateCache.All51StateCodes)
        {
          tr = new HtmlTableRow();
          tr.AddTo(tbody);
          uri = UrlManager.GetSiteUri("/nocache.aspx");
          //uri = UrlManager.GetStateUri(stateCode, "/nocache.aspx");
          td = new HtmlTableCell { InnerText = uri.Host };
          td.AddTo(tr);
          td = new HtmlTableCell();
          td.AddTo(tr);
          new HtmlIframe { Src = uri + "?P" }.AddTo(td);
        }
      }
    }
  }
}