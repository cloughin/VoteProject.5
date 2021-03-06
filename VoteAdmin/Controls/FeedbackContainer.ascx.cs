using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using static System.String;

namespace Vote.Controls
{
  public partial class FeedbackContainerControl : UserControl, IFeedbackContainerControl
  {
    #region Private

    public const string ErrorClass = "error";
    private HtmlContainerControl _PlaceholderTag;
    private string _CssClass;

    private bool HasPlaceholder => !IsNullOrWhiteSpace(Placeholder);

    #endregion Private

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global

    public int ValidationErrorCount { get; private set; }

    public void Add(string cssClass, string feedback)
    {
      var p = new HtmlP();
      p.Attributes.Add("class", cssClass);
      p.InnerHtml = feedback;
      Feedback.Controls.Add(p);
      FeedbackContainerOuter.Style.Add(HtmlTextWriterStyle.Display, "");
      if (HasPlaceholder)
        _PlaceholderTag.Style.Add(HtmlTextWriterStyle.Display, "none");
    }

    public void AddError(string feedback) => Add("ng", feedback);

    public void AddInfo(string feedback) => Add("ok", feedback);

    public void Clear()
    {
      Feedback.Controls.Clear();
    }

    public static void ClearValidationError(Control control)
    {
      if (control is HtmlControl htmlControl)
        htmlControl.RemoveCssClass(ErrorClass);
      else
      {
        var webControl = control as WebControl;
        webControl?.RemoveCssClass(ErrorClass);
      }
    }

    public static void ClearValidationErrors(params Control[] controls)
    {
      foreach (var control in controls)
        ClearValidationError(control);
    }

    public string CssClass
    {
      get { return _CssClass.SafeString(); }
      set { _CssClass = value; }
    }

    public void HandleException(Exception ex)
    {
      string message;

      try
      {
        message = ex.Message;
        // We don't log routine UI exceptions
        if (!(ex is VoteUIException))
          VotePage.LogException("Feedback", ex);
      }
      catch (Exception ex2)
      {
        // Don't put anything in here that could possibly
        // throw an exception. In the rare event that this block
        // executes, we use ex.ToString() instead of ex.Message
        // to make sure we capture and report the stack trace.
        message = "Unexpected failure in exception handler: " + ex2 +
          Environment.NewLine + "Original exception: " + ex;
      }

      AddError(message);
    }

    public string Placeholder { get; set; }

    public void PostValidationError(
      IEnumerable<Control> controls, string message, params object[] args)
    {
      if (!IsNullOrWhiteSpace(message))
        AddError(Format(message, args));
      foreach (var control in controls)
        control.AddCssClasses(ErrorClass);
      ValidationErrorCount++;
    }

    public void PostValidationError(
        Control control, string message = null, params object[] args)
    {
      PostValidationError(new[] {control}, message, args);
    }

    public string StripHtml(string input)
    {
      var stripped = input.StripHtml();
      if (stripped != input)
        AddInfo("Note: we removed some text that appeared to be html.");
      return stripped;
    }

    #region ValidateDate

    private DateTime ValidateDate(
      TextBox textBox, out bool success, string name, DateTime min, DateTime max,
      bool allowEmpty, DateTime def)
    {
      success = true;
      var text = textBox.Text.Trim();
      var result = def;
      if (text == Empty)
      {
        if (!allowEmpty)
        {
          PostValidationError(textBox, name + " is required");
          success = false;
        }
      }
      else if (!DateTime.TryParse(text, out result) || result < min || result > max)
      {
        PostValidationError(textBox, name + " is not valid");
        success = false;
      }
      return result;
    }

    public DateTime ValidateDate(TextBox textBox, out bool success, string name) =>
      ValidateDate(textBox, out success, name, DateTime.MinValue, DateTime.MaxValue, false,
        VotePage.DefaultDbDate);

    public DateTime ValidateDate(TextBox textBox, out bool success, string name, DateTime min) =>
      ValidateDate(textBox, out success, name, min, DateTime.MaxValue, false,
        VotePage.DefaultDbDate);

    public DateTime ValidateDate(TextBox textBox, out bool success, string name, DateTime min,
        DateTime max) =>
      ValidateDate(textBox, out success, name, min, max, false, VotePage.DefaultDbDate);

    public DateTime ValidateDateOptional(TextBox textBox, out bool success, string name,
        DateTime def) =>
      ValidateDate(textBox, out success, name, DateTime.MinValue, DateTime.MaxValue, true, def);

    public DateTime ValidateDateOptional(TextBox textBox, out bool success, string name,
        DateTime min, DateTime def) =>
      ValidateDate(textBox, out success, name, min, DateTime.MaxValue, true, def);

    public DateTime ValidateDateOptional(TextBox textBox, out bool success, string name,
        DateTime min, DateTime max, DateTime def) =>
      ValidateDate(textBox, out success, name, min, max, true, def);

    #endregion ValidateDate

    public int ValidateInt(
      TextBox textBox, out bool success, string name, int min = int.MinValue,
      int max = int.MaxValue, bool allowEmpty = false, int def = 0)
    {
      success = true;
      var text = textBox.Text.Trim();
      var result = def;
      if (text == Empty)
      {
        if (!allowEmpty)
        {
          PostValidationError(textBox, name + " is required");
          success = false;
        }
      }
      else if (!int.TryParse(text, out result) || result < min || result > max)
      {
        PostValidationError(textBox, name + " is not valid");
        success = false;
      }
      return result;
    }

    public int ValidateIntOptional(TextBox textBox, out bool success, string name, int def,
        int min = int.MinValue, int max = int.MaxValue) =>
      ValidateInt(textBox, out success, name, min, max, true, def);

    public string ValidateLength(
      TextBox textBox, string name, int min, int max, out bool success)
    {
      success = true;
      var text = textBox.Text.Trim();
      if (min == 1)
        ValidateRequired(textBox, name, out success);
      else if (text.Length < min)
      {
        PostValidationError(
          textBox,
          name + " must be at least " + min.ToString(CultureInfo.InvariantCulture) +
          " characters");
        success = false;
      }
      if (text.Length > max)
      {
        PostValidationError(
          textBox,
          name + " cannot exceed " + max.ToString(CultureInfo.InvariantCulture) +
          " characters");
        success = false;
      }
      return text;
    }

    public string ValidateRequired(Control control, string name, out bool success)
    {
      success = true;
      var text = control.GetValue().Trim();
      if (text == Empty)
      {
        PostValidationError(control, name + " is required");
        success = false;
      }
      return text;
    }

    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public

    #region Event handlers and overrides

    protected void Page_Load(object sender, EventArgs e)
    {
      Feedback.Controls.Clear();
      FeedbackContainerOuter.Style.Add(HtmlTextWriterStyle.Display, "none");
      var onclick = "$('#" + FeedbackContainerOuter.ClientID + "').hide(400);";
      if (HasPlaceholder)
      {
        _PlaceholderTag = new HtmlP();
        _PlaceholderTag.AddCssClasses("feedback-placeholder");
        _PlaceholderTag.InnerHtml = Placeholder;
        _PlaceholderTag.ID = "Placeholder";
        FeedbackContainerOuter.AddAfter(_PlaceholderTag);
        onclick += "$('#" + _PlaceholderTag.ClientID + "').show(400);";
      }
      if (!IsNullOrWhiteSpace(CssClass))
        FeedbackContainerOuter.AddCssClasses(CssClass);
      FeedbackHider.Attributes.Add("onclick", onclick);
      ValidationErrorCount = 0;
    }

    #endregion Event handlers and overrides
  }
}