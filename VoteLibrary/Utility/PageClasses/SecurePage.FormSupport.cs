using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using static System.String;

namespace Vote
{
  //  Contains methods for standard components of maintenance forms
  //
  public partial class SecurePage
  {
    private const string MonitorPrefix = "mc";

    protected MonitorFactory MonitorFactory { get; } = new MonitorFactory(MonitorPrefix);

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global

    public static HtmlContainerControl CreateIndicator(string id, string className, string title)
    {
      var divIndicator = new HtmlDiv {EnableViewState = false};
      divIndicator.AddCssClasses(className);
      if (id != null)
        divIndicator.ID = id;
      if (title != null)
      {
        divIndicator.AddCssClasses("tiptip");
        divIndicator.Attributes.Add("title", title);
      }
      return divIndicator;
    }

    public static HtmlContainerControl AddAccordionControl(
      Control parent, string id, string className, string accordionClassName = "accordion")
    {
      var tabControl = CreateAccordionControl(id, className, accordionClassName);
      parent.Controls.Add(tabControl);
      return tabControl;
    }

    public static HtmlContainerControl AddAccordionHeader(
      Control parent, string content)
    {
      var accordionElement = CreateAccordionHeader(content);
      parent.Controls.Add(accordionElement);
      return accordionElement;
    }

    public static UpdatePanel AddAjaxUpdatePanel(Control parent, string id)
    {
      var updatePanel = CreateAjaxUpdatePanel(id);
      parent.Controls.Add(updatePanel);
      return updatePanel;
    }

    public static HtmlContainerControl AddAsteriskIndicator(
      Control parent, string id, string className, string title)
    {
      var divAsterisk = CreateAsteriskIndicator(id, className, title);
      parent.Controls.Add(divAsterisk);
      return divAsterisk;
    }

    public static HtmlInputButton AddButton(
      Control parent, string id, string className, string value)
    {
      var button = CreateButton(id, className, value);
      parent.Controls.Add(button);
      return button;
    }

    public static HtmlContainerControl AddButtonInputElement(Control parent,
      string textboxId, string textboxClassName, string value, string outerClassName)
    {
      var element = CreateButtonInputElement(textboxId, textboxClassName, value,
        outerClassName);
      parent.Controls.Add(element);
      return element;
    }

    public static HtmlInputCheckBox AddCheckBox(Control parent, string id, string className,
      string value,
      bool isChecked = false)
    {
      var checkbox = CreateCheckBox(id, className, value, isChecked);
      parent.Controls.Add(checkbox);
      return checkbox;
    }

    public static HtmlContainerControl AddCheckboxInputElement(Control parent, string checkboxId,
      string checkboxClassName, string value, bool isChecked, string checkboxLabel,
      string outerClassName,
      string asteriskId, string asteriskClassName, string asteriskTitle,
      string kalyptoClass = "kalypto")
    {
      var element = CreateCheckboxInputElement(checkboxId, checkboxClassName, value, isChecked,
        checkboxLabel, outerClassName, asteriskId, asteriskClassName, asteriskTitle, kalyptoClass);
      parent.Controls.Add(element);
      return element;
    }

    public static HtmlContainerControl AddClearBoth(Control parent)
    {
      var clear = CreateClearBoth();
      parent.Controls.Add(clear);
      return clear;
    }

    public static HtmlContainerControl AddClearButton(
      Control parent, string id, string className, string title)
    {
      var clear = CreateClearButton(id, className, title);
      parent.Controls.Add(clear);
      return clear;
    }

    public static HtmlContainerControl AddContainer(
      Control parent, string id, string className)
    {
      var divContainer = CreateContainer(id, className);
      parent.Controls.Add(divContainer);
      return divContainer;
    }

    public static HtmlContainerControl AddHorzTabControl(
      Control parent, string id, string className)
    {
      var tabControl = CreateHorzTabControl(id, className);
      parent.Controls.Add(tabControl);
      return tabControl;
    }

    public static HtmlContainerControl AddHeading(
      Control parent, int level, string id, string className, object content)
    {
      var heading = CreateHeading(level, id, className, content);
      parent.Controls.Add(heading);
      return heading;
    }

    public static HtmlInputHidden AddHiddenField(
      Control parent, string id, string className, string value = null)
    {
      var hidden = CreateHiddenField(id, className, value);
      parent.Controls.Add(hidden);
      return hidden;
    }

    public static HtmlContainerControl AddHorzTab(
      Control parent, string hRef, object content, string className)
    {
      var tab = CreateHorzTab(hRef, content, className);
      parent.Controls.Add(tab);
      return tab;
    }

    public static LiteralControl AddLiteral(Control parent, string text)
    {
      var literal = new LiteralControl(text);
      parent.Controls.Add(literal);
      return literal;
    }

    public static HtmlContainerControl AddParagraph(
      Control parent, string id, string className)
    {
      var paragraph = CreateParagraph(id, className);
      parent.Controls.Add(paragraph);
      return paragraph;
    }

    public static HtmlContainerControl AddSpan(
      Control parent, string id, string className, string text)
    {
      var span = CreateSpan(id, className, text);
      parent.Controls.Add(span);
      return span;
    }

    public static HtmlContainerControl AddStarIndicator(
      Control parent, string id, string className, string title)
    {
      var divStar = CreateStarIndicator(id, className, title);
      parent.Controls.Add(divStar);
      return divStar;
    }

    public static HtmlContainerControl AddTab(
      Control parent, string hRef, object content)
    {
      var tab = CreateTab(hRef, content);
      parent.Controls.Add(tab);
      return tab;
    }

    public static HtmlContainerControl AddTabAnchor(
      Control parent, string hRef, object content)
    {
      var a = CreateTabAnchor(hRef, content);
      parent.Controls.Add(a);
      return a;
    }

    public static HtmlContainerControl AddTabContainer(
      Control parent, string id, string className)
    {
      var container = CreateTabContainer(id, className);
      parent.Controls.Add(container);
      return container;
    }

    public static HtmlContainerControl AddTabControl(
      Control parent, string id, string className)
    {
      var tabControl = CreateTabControl(id, className);
      parent.Controls.Add(tabControl);
      return tabControl;
    }

    public static TextBox AddTextArea(
      Control parent, string id, string className, string value, string placeholder,
      bool spellcheck, bool disabled = false)
    {
      var textarea = CreateTextArea(id, className, value, placeholder, spellcheck, disabled);
      parent.Controls.Add(textarea);
      return textarea;
    }

    public static HtmlContainerControl AddTextAreaInputElement(
      Control parent, string textboxId, string textboxClassName, string value, string placeholder,
      bool spellcheck, string textboxToolTip, bool required, string fieldName, string outerClassName,
      string asteriskId, string asteriskClassName, string asteriskTitle, bool disabled = false,
      string instructions = null)
    {
      var textarea = CreateTextAreaInputElement(textboxId, textboxClassName, value, placeholder,
        spellcheck, textboxToolTip, required, fieldName, outerClassName, asteriskId,
        asteriskClassName, asteriskTitle, disabled, instructions);
      parent.Controls.Add(textarea);
      return textarea;
    }

    public static TextBox AddTextBox(Control parent, string id, string className, string value,
      string placeholder, bool spellcheck, bool disabled = false)
    {
      var textarea = CreateTextBox(id, className, value, placeholder, spellcheck, disabled);
      parent.Controls.Add(textarea);
      return textarea;
    }

    public static HtmlContainerControl AddTextInputElement(
      Control parent, string textboxId, string textboxClassName, string value,
      string textboxToolTip, bool required, string fieldName, string outerClassName,
      string asteriskId, string asteriskClassName, string asteriskTitle, bool disabled = false)
    {
      var element = CreateTextInputElement(
        textboxId, textboxClassName, value, textboxToolTip, required, fieldName,
        outerClassName, asteriskId, asteriskClassName, asteriskTitle, disabled);
      parent.Controls.Add(element);
      return element;
    }

    //public static HtmlContainerControl AddTimeSpanInputElement(
    //  Control parent, string controlId, string textboxClassName, TimeSpan? value,
    //  string[] toolTips, bool required, string fieldName, string outerClassName,
    //  string asteriskId, string asteriskClassName, string asteriskTitle, bool includeHours = false)
    //{
    //  var element = CreateTimeSpanInputElement(
    //    controlId, textboxClassName, value, toolTips, required, fieldName,
    //    outerClassName, asteriskId, asteriskClassName, asteriskTitle, includeHours);
    //  parent.Controls.Add(element);
    //  return element;
    //}

    public static HtmlContainerControl AddUndoButton(
      Control parent, string id, string className, string title)
    {
      var undo = CreateUndoButton(id, className, title);
      parent.Controls.Add(undo);
      return undo;
    }

    public static HtmlContainerControl AddUpdateButtonInContainer(
      Control parent, string id, string containerClassName, string buttonClassName,
      string toolTip, EventHandler onClick)
    {
      var container = CreateUpdateButtonInContainer(
        id, containerClassName, buttonClassName, toolTip, onClick);
      parent.Controls.Add(container);
      return container;
    }

    public static HtmlContainerControl AddVertTab(
      Control parent, string hRef, object content, string className)
    {
      var tab = CreateVertTab(hRef, content, className);
      parent.Controls.Add(tab);
      return tab;
    }

    public static HtmlContainerControl AddVertTabControl(
      Control parent, string id, string className)
    {
      var tabControl = CreateVertTabControl(id, className);
      parent.Controls.Add(tabControl);
      return tabControl;
    }

    public static HtmlContainerControl Center(
      Control control, bool horizontal, bool vertical, string className = "")
    {
      var outer = new HtmlDiv {EnableViewState = false};
      //li.Controls.Add(outer);
      if (horizontal)
        outer.AddCssClasses("horz-center");
      if (vertical)
        outer.AddCssClasses("vert-center");
      if (!IsNullOrEmpty(className))
        outer.AddCssClasses(className);
      var inner = new HtmlDiv();
      outer.Controls.Add(inner);
      inner.AddCssClasses("center-inner");
      control.AddCssClasses("center-element");

      var index = control.Parent.Controls.IndexOf(control);
      control.Parent.Controls.AddAt(index, outer);
      control.Parent.Controls.Remove(control);
      inner.Controls.Add(control);

      return outer;
    }

    public static HtmlContainerControl CreateAccordionControl(
      string id, string className, string accordionClassName = "accordion")
    {
      var tabControl = CreateContainer(id, className);
      tabControl.AddCssClasses(accordionClassName);
      return tabControl;
    }

    public static HtmlContainerControl CreateAccordionHeader(object content)
    {
      var h3Accordion = new HtmlH3 {EnableViewState = false};
      h3Accordion.AddCssClasses("accordion-header");
      InsertContent(h3Accordion, content);
      return h3Accordion;
    }

    public static UpdatePanel CreateAjaxUpdatePanel(string id)
    {
      var updatePanel = new UpdatePanel {EnableViewState = false};
      if (id != null)
        updatePanel.ID = id;
      updatePanel.UpdateMode = UpdatePanelUpdateMode.Conditional;
      return updatePanel;
    }

    public static HtmlContainerControl CreateAsteriskIndicator(
      string id, string className, string title)
    {
      var asterisk = CreateIndicator(id, className, title);
      asterisk.AddCssClasses("tab-ast");
      return asterisk;
    }

    public static HtmlInputCheckBox CreateCheckBox(string id, string className, string value,
      bool isChecked = false)
    {
      var checkbox = new HtmlInputCheckBox();
      if (id != null)
        checkbox.ID = id;
      checkbox.AddCssClasses(className);
      if (value != null)
        checkbox.Value = value;
      checkbox.Checked = isChecked;
      return checkbox;
    }

    public static HtmlContainerControl CreateCheckboxInputElement(string checkboxId,
      string checkboxClassName, string value, bool isChecked, string checkboxLabel,
      string outerClassName,
      string asteriskId, string asteriskClassName, string asteriskTitle,
      string kalyptoClass = "kalypto")
    {
      var element = CreateContainer(null, outerClassName);
      element.AddCssClasses("input-element");

      var databox = AddContainer(element, null, "databox kalypto-checkbox");
      var container = new HtmlDiv().AddTo(databox, "kalypto-container");
      AddCheckBox(container, checkboxId, kalyptoClass + " " + checkboxClassName.SafeString(), value,
        isChecked);
      new HtmlDiv {InnerText = checkboxLabel}.AddTo(databox, "kalypto-checkbox-label");
      if (asteriskId != null || asteriskClassName != null || asteriskTitle != null)
      {
        var asterisk = AddContainer(databox, asteriskId, asteriskClassName);
        if (!IsNullOrWhiteSpace(asteriskTitle))
        {
          asterisk.Attributes.Add("title", asteriskTitle);
          asterisk.AddCssClasses("tiptip");
        }
      }

      return element;
    }

    public static HtmlContainerControl CreateClearBoth()
    {
      var clear = new HtmlDiv {EnableViewState = false};
      clear.Style.Add("clear", "both");
      return clear;
    }

    public static HtmlInputButton CreateButton(string id, string className, string value)
    {
      var button = new HtmlInputButton {EnableViewState = false, Value = value};
      if (id != null)
        button.ID = id;
      button.AddCssClasses(className);
      return button;
    }

    public static HtmlContainerControl CreateButtonInputElement(string buttonId,
      string buttonClassName, string value, string outerClassName)
    {
      var element = CreateContainer(null, outerClassName);
      element.AddCssClasses("input-element");
      AddButton(element, buttonId, buttonClassName, value);
      return element;
    }

    public static HtmlContainerControl CreateClearButton(
      string id, string className, string title)
    {
      var clearOuter = CreateContainer(null, null);
      clearOuter.AddCssClasses("clear-button");
      if (title != null)
      {
        clearOuter.Attributes.Add("title", title);
        clearOuter.AddCssClasses("tiptip");
      }
      var clearInner = AddContainer(clearOuter, null, null);
      if (id != null)
        clearInner.ID = id;
      clearInner.AddCssClasses(className);
      return clearOuter;
    }

    public static HtmlContainerControl CreateContainer(string id, string className)
    {
      var divContainer = new HtmlDiv {EnableViewState = false};
      if (id != null)
        divContainer.ID = id;
      if (className != null)
        divContainer.AddCssClasses(className);
      return divContainer;
    }

    public static HtmlContainerControl CreateHeading(
      int level, string id, string className, object content)
    {
      if (level < 1 || level > 6)
        throw new VoteException("Heading level must be 1 through 6");
      var heading =
        new HtmlGenericControl("h" + level.ToString(CultureInfo.InvariantCulture));
      if (id != null)
        heading.ID = id;
      heading.AddCssClasses(className);
      InsertContent(heading, content);
      return heading;
    }

    public static HtmlInputHidden CreateHiddenField(
      string id, string className, string value = null)
    {
      var hidden = new HtmlInputHidden {EnableViewState = false};
      if (id != null)
        hidden.ID = id;
      hidden.AddCssClasses(className);
      if (value != null) hidden.Value = value;
      return hidden;
    }

    public static HtmlContainerControl CreateHorzTab(
      string hRef, object content, string className)
    {
      var tab = CreateTab(hRef, content);
      tab.AddCssClasses("htab");
      tab.AddCssClasses(className);
      return tab;
    }

    public static HtmlContainerControl CreateHorzTabControl(
      string id, string className)
    {
      var tabControl = CreateTabControl(id, className);
      tabControl.AddCssClasses("htab-control");
      return tabControl;
    }

    public static HtmlContainerControl CreateParagraph(string id, string className)
    {
      var paragraph = new HtmlP {EnableViewState = false};
      if (id != null)
        paragraph.ID = id;
      if (className != null)
        paragraph.AddCssClasses(className);
      return paragraph;
    }

    public static HtmlContainerControl CreateSpan(
      string id, string className, string text)
    {
      var span = new HtmlSpan {EnableViewState = false};
      if (id != null)
        span.ID = id;
      if (className != null)
        span.AddCssClasses(className);
      if (text != null)
        span.InnerHtml = text;
      return span;
    }

    public static HtmlContainerControl CreateStarIndicator(
      string id, string className, string title)
    {
      var star = CreateIndicator(id, className, title);
      star.AddCssClasses("tab-star");
      return star;
    }

    public static HtmlContainerControl CreateTab(string hRef, object content)
    {
      var li = new HtmlLi {EnableViewState = false};
      li.AddCssClasses("tab");
      AddTabAnchor(li, hRef, content);
      return li;
    }

    public static HtmlContainerControl CreateTabAnchor(string hRef, object content)
    {
      var a = new HtmlAnchor {EnableViewState = false};
      if (hRef != null)
        a.HRef = hRef;
      InsertContent(a, content);
      a.Attributes.Add("onclick", "this.blur()");
      return a;
    }

    public static HtmlContainerControl CreateTabContainer(
      string id, string className)
    {
      var tabs = new HtmlUl {EnableViewState = false};
      if (id != null)
        tabs.ID = id;
      tabs.AddCssClasses(className);
      return tabs;
    }

    public static HtmlContainerControl CreateTabControl(string id, string className)
    {
      var tabControl = CreateContainer(id, className);
      tabControl.AddCssClasses("tab-control jqueryui-tabs");
      return tabControl;
    }

    public static TextBox CreateTextArea(
      string id, string className, string value, string placeholder, bool spellcheck,
      bool disabled = false)
    {
      var textbox = new TextBoxWithNormalizedLineBreaks
      {
        EnableViewState = false,
        TextMode = TextBoxMode.MultiLine
      };
      if (id != null)
        textbox.ID = id;
      if (disabled)
        textbox.Attributes.Add("disabled", "disabled");
      textbox.AddCssClasses(className);
      if (placeholder != null)
        textbox.Attributes.Add("placeholder", placeholder);
      textbox.Attributes.Add("spellcheck", spellcheck.ToString());
      if (value != null)
        textbox.Text = value;
      return textbox;
    }

    public static HtmlContainerControl CreateTextAreaInputElement(
      string textboxId, string textboxClassName, string value, string placeholder, bool spellcheck,
      string textboxToolTip, bool required, string fieldName, string outerClassName,
      string asteriskId, string asteriskClassName, string asteriskTitle, bool disabled = false,
      string instructions = null)
    {
      var element = CreateContainer(null, outerClassName);
      element.AddCssClasses("input-element");

      var label = AddParagraph(element, null, "fieldlabel");
      var literal = AddLiteral(label, fieldName);
      if (required)
      {
        literal.Text += " ";
        AddSpan(label, null, "reqd", "◄");
      }

      if (!IsNullOrWhiteSpace(instructions))
      {
        var inst = AddParagraph(element, null, "instructions");
        AddLiteral(inst, instructions);
      }
      var databox = AddContainer(element, null, "databox textbox");
      var textbox = AddTextArea(
        databox, textboxId, textboxClassName, value, placeholder, spellcheck, disabled);
      if (!IsNullOrWhiteSpace(textboxToolTip))
      {
        textbox.ToolTip = textboxToolTip;
        textbox.AddCssClasses("tiptip");
      }
      if (asteriskId != null || asteriskClassName != null || asteriskTitle != null)
      {
        var asterisk = AddContainer(databox, asteriskId, asteriskClassName);
        if (!IsNullOrWhiteSpace(asteriskTitle))
        {
          asterisk.Attributes.Add("title", asteriskTitle);
          asterisk.AddCssClasses("tiptip");
        }
      }

      return element;
    }

    public static TextBox CreateTextBox(string id, string className, string value,
      string placeholder,
      bool spellcheck, bool disabled = false)
    {
      var textbox = new TextBoxWithNormalizedLineBreaks {EnableViewState = false};
      if (id != null)
        textbox.ID = id;
      if (disabled)
        textbox.Attributes.Add("disabled", "disabled");
      textbox.AddCssClasses(className);
      if (placeholder != null)
        textbox.Attributes.Add("placeholder", placeholder);
      textbox.Attributes.Add("spellcheck", spellcheck.ToString());
      if (value != null)
        textbox.Text = value;
      return textbox;
    }

    public static HtmlContainerControl CreateTextInputElement(
      string textboxId, string textboxClassName, string value, string textboxToolTip,
      bool required, string fieldName, string outerClassName, string asteriskId,
      string asteriskClassName, string asteriskTitle, bool disabled = false)
    {
      var element = CreateContainer(null, outerClassName);
      element.AddCssClasses("input-element");

      var label = AddParagraph(element, null, "fieldlabel");
      var literal = AddLiteral(label, fieldName);
      if (required)
      {
        literal.Text += " ";
        AddSpan(label, null, "reqd", "◄");
      }
      var databox = AddContainer(element, null, "databox textbox");
      var textbox = AddTextBox(
        databox, textboxId, textboxClassName, value, null, false, disabled);
      if (!IsNullOrWhiteSpace(textboxToolTip))
      {
        textbox.ToolTip = textboxToolTip;
        textbox.AddCssClasses("tiptip");
      }
      if (asteriskId != null || asteriskClassName != null || asteriskTitle != null)
      {
        var asterisk = AddContainer(databox, asteriskId, asteriskClassName);
        if (!IsNullOrWhiteSpace(asteriskTitle))
        {
          asterisk.Attributes.Add("title", asteriskTitle);
          asterisk.AddCssClasses("tiptip");
        }
      }

      return element;
    }

    public static HtmlContainerControl CreateUndoButton(
      string id, string className, string title)
    {
      var undoOuter = CreateContainer(null, null);
      undoOuter.AddCssClasses("undo-button");
      if (title != null)
      {
        undoOuter.Attributes.Add("title", title);
        undoOuter.AddCssClasses("tiptip");
      }
      var undoInner = AddContainer(undoOuter, null, null);
      if (id != null)
        undoInner.ID = id;
      undoInner.AddCssClasses(className);
      return undoOuter;
    }

    public static HtmlContainerControl CreateUpdateButtonInContainer(
      string id, string containerClassName, string buttonClassName, string toolTip,
      EventHandler onClick)
    {
      var buttonContainer = new HtmlDiv {EnableViewState = false};
      buttonContainer.AddCssClasses(containerClassName);
      var button = new Button {EnableViewState = false};
      buttonContainer.Controls.Add(button);
      if (id != null)
        button.ID = id;
      button.Text = "Update";
      button.AddCssClasses(buttonClassName);
      if (toolTip != null)
      {
        button.ToolTip = toolTip;
        button.AddCssClasses("tiptip");
      }
      if (onClick != null)
        button.Click += onClick;
      return buttonContainer;
    }

    public static HtmlContainerControl CreateVertTab(
      string hRef, object content, string className)
    {
      var tab = CreateTab(hRef, content);
      tab.AddCssClasses("vtab");
      tab.AddCssClasses(className);
      return tab;
    }

    public static HtmlContainerControl CreateVertTabControl(
      string id, string className)
    {
      var tabControl = CreateTabControl(id, className);
      tabControl.AddCssClasses("vtab-control");
      return tabControl;
    }

    private static void InsertContent(HtmlContainerControl control, object content)
    {
      if (content is string stringContent)
      {
        control.InnerHtml = stringContent;
        return;
      }
      if (content is Control controlContent)
      {
        control.Controls.Add(controlContent);
        return;
      }
      throw new VoteException("Unsupported content type");
    }

    public static HtmlContainerControl Relative(Control control)
    {
      var relative = new HtmlDiv {EnableViewState = false};
      relative.Style.Add(HtmlTextWriterStyle.Position, "relative");

      var index = control.Parent.Controls.IndexOf(control);
      control.Parent.Controls.AddAt(index, relative);
      control.Parent.Controls.Remove(control);
      relative.Controls.Add(control);

      return relative;
    }


    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }

  public class MonitorInstance
  {
    #region Private

    private readonly string _GroupSuffix;

    private static string CombineClasses(string className, string classes)
    {
      if (className == null)
        return classes;
      className = className.Trim();
      if (className == Empty)
        return classes;
      return className + " " + classes;
    }

    private string GetClass(string type, string subclass)
    {
      if (subclass == null)
        subclass = Empty;
      else
        subclass = '-' + subclass;
      return Prefix + " " + Prefix + "-" + type + " " + Group + subclass;
    }

    #endregion Private

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global

    public MonitorInstance(string prefix, string groupSuffix)
    {
      Prefix = prefix;
      _GroupSuffix = groupSuffix;
    }

    public string GetAsteriskClass(string className)
    {
      return CombineClasses(className, GetClass("ast", null));
    }

    public string GetAsteriskClass(string className, string subclass)
    {
      return CombineClasses(className, GetClass("ast", subclass));
    }

    public string GetButtonClass(string className)
    {
      return CombineClasses(className, GetClass("button", null));
    }

    public string GetClearClass(string className)
    {
      return CombineClasses(className, GetClass("clear", null));
    }

    public string GetClearClass(string className, string subclass)
    {
      return CombineClasses(className, GetClass("clear", subclass));
    }

    public string GetContainerClass(string className)
    {
      return CombineClasses(className, GetClass("container", null));
    }

    public string GetDataClass(string className)
    {
      return CombineClasses(className, GetClass("data", null));
    }

    public string GetDataClass(string className, string subclass)
    {
      return CombineClasses(className, GetClass("data", subclass));
    }

    public string GetDescriptionClass(string className)
    {
      return CombineClasses(className, GetClass("desc", null));
    }

    public string GetDescriptionClass(string className, string subclass)
    {
      return CombineClasses(className, GetClass("desc", subclass));
    }

    public string GetFeedbackClass(string className)
    {
      return CombineClasses(className, GetClass("feedback", null));
    }

    public string GetStarClass(string className)
    {
      return CombineClasses(className, GetClass("star", null));
    }

    public string GetStarClass(string className, string subclass)
    {
      return CombineClasses(className, GetClass("star", subclass));
    }

    public string GetUndoClass(string className)
    {
      return CombineClasses(className, GetClass("undo", null));
    }

    public string GetUndoClass(string className, string subclass)
    {
      return CombineClasses(className, GetClass("undo", subclass));
    }

    public string Group => Prefix + "-group-" + _GroupSuffix;

    public string Prefix { get; }


    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }

  public class MonitorFactory
  {
    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global

    public MonitorFactory(string prefix)
    {
      Prefix = prefix;
    }

    public MonitorInstance GetMonitorInstance(string groupSuffix)
    {
      return new MonitorInstance(Prefix, groupSuffix);
    }

    public string Prefix { get; }


    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }
}