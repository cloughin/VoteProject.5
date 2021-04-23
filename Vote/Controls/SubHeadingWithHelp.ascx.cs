using System;
using System.Web.UI;

namespace Vote.Controls
{
  public partial class SubHeadingWithHelpControl : UserControl
  {
    #region Private

    private SubHeadingWithHelpContainer _Container;

    #endregion Private

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global

    public class SubHeadingWithHelpContainer : Control, INamingContainer
    {
    }

    public string CssClass { get; set; }

    public Control FindTemplateControl(string id) => 
      _Container?.FindControl(id);

    public string Title { get; set; }

    public string Tooltip { get; set; }

    public string Transition { get; set; }

    [PersistenceMode(PersistenceMode.InnerProperty)]
    [TemplateContainer(typeof (SubHeadingWithHelpContainer))]
    public ITemplate ContentTemplate { get; set; }


    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public

    #region Event handlers and overrides

    protected void Page_Load(object sender, EventArgs e)
    {
      PlaceHolder.Controls.Clear();
      if (!string.IsNullOrWhiteSpace(Title))
        SubHeading.InnerHtml = Title;
      if (string.IsNullOrWhiteSpace(Tooltip))
      {
        if (!string.IsNullOrWhiteSpace(Title))
          HelpButton.Title += " for " + Title;
      }
      else
        HelpButton.Title = Tooltip;
      if (ContentTemplate == null)
        HelpButton.Style.Add(HtmlTextWriterStyle.Display, "none");
      else
      {
        _Container = new SubHeadingWithHelpContainer();
        ContentTemplate.InstantiateIn(_Container);
        PlaceHolder.Controls.Add(_Container);
        if (string.IsNullOrWhiteSpace(Transition))
          Transition = "blind";
        var onClick =
          $"$('#{FindControl("SubHeadingOuter").ClientID} div.help').toggle('{Transition}',400);";
        HelpButton.Attributes["onclick"] = onClick;
        if (!string.IsNullOrWhiteSpace(CssClass))
          HelpButton.AddCssClasses(CssClass);
      }
    }

    #endregion Event handlers and overrides
  }
}