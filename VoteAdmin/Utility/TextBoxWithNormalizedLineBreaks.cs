using System.Web.UI;
using System.Web.UI.WebControls;

namespace Vote
{
  [ToolboxData(
    "<{0}:TextBoxWithNormalizedLineBreaks runat=\"server\"></{0}:TextBoxWithNormalizedLineBreaks>"
    )]
  public sealed class TextBoxWithNormalizedLineBreaks : TextBox
  {
    public override string Text { get { return base.Text.NormalizeNewLines(); } set { base.Text = value.NormalizeNewLines(); } }
  }

  [ToolboxData(
    "<{0}:StaticClientIdPlaceHolder runat=\"server\"></{0}:StaticClientIdPlaceHolder>"
    )]
  public sealed class StaticClientIdPlaceHolder : PlaceHolder, INamingContainer
  {
    public StaticClientIdPlaceHolder()
    {
      ClientIDMode = ClientIDMode.Static;
    }
  }
}