using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB.Vote;
using Vote.Controls;

namespace Vote
{
  // Base class for all data item fields

  #region Internal

  #region ReSharper disable

  // ReSharper disable MemberCanBePrivate.Global
  // ReSharper disable MemberCanBeProtected.Global
  // ReSharper disable UnusedMember.Global
  // ReSharper disable UnusedMethodReturnValue.Global
  // ReSharper disable UnusedAutoPropertyAccessor.Global
  // ReSharper disable UnassignedField.Global
  // ReSharper disable VirtualMemberNeverOverriden.Global

  #endregion ReSharper disable

  internal abstract class DataItemBase
  {
    public Control DataControl { get; private set; }
    public FeedbackContainerControl Feedback { get; protected set; }
    public string Default { get; protected set; }
    public string Description { get; protected set; }
    public string Column { get; protected set; }
    public bool LabelIsHyperLink { get; protected set; }
    public bool LabelIsMailTo { get; protected set; }
    public string HyperlinkToolTip { get; protected set; }
    protected Func<string, object> ConvertFn;
    protected Func<DataItemBase, bool> Validator;
    protected virtual string DataControlToolTip { get; set; }
    protected HtmlControl Label;
    private string _GroupName;
    private HtmlControl _Asterisk;
    private List<string> _Warnings;

    protected DataItemBase(string groupName = null)
    {
      _GroupName = groupName;
    }

    protected void AddWarning(string warning)
    {
      if (_Warnings == null) _Warnings = new List<string>();
      if (!_Warnings.Contains(warning)) _Warnings.Add(warning);
    }

    protected virtual void AfterUpdate(string newValue)
    {
      if (LabelIsHyperLink)
        SetHyperlinkLabel(newValue);
      else if (LabelIsMailTo)
        SetMailToLabel(newValue);
    }

    protected virtual string AsteriskToolTip => "There are unsaved changes to " + Description;

    public static void ClearAllValidationErrors(IEnumerable<DataItemBase> items)
    {
      foreach (var item in items)
        FeedbackContainerControl.ClearValidationError(item.DataControl);
    }

    private object Convert(string value)
    {
      return ConvertFn == null ? value : ConvertFn(value);
    }

    public SecurePage.UpdateStatus DoUpdate(bool reportUnchanged)
    {
      SecurePage.UpdateStatus updateStatus;

      try
      {
        DataControl.AddCssClasses("badupdate"); // in case of exception
        if (!Validate())
          return SecurePage.UpdateStatus.Failure;
        var newValue = GetControlValue();
        newValue = Feedback.StripHtml(newValue);
        newValue = newValue.StripRedundantSpaces();
        var oldValue = GetCurrentValue();
        if (oldValue == newValue)
        {
          DataControl.SetValue(newValue);
          updateStatus = SecurePage.UpdateStatus.Unchanged;
        }
        else
        {
          SecurePage.ThrowRandomException();
          Log(oldValue, newValue);
          SetControlValue(newValue);
          if (Update(Convert(newValue)))
          {
            Feedback.AddInfo("The " + Description + " entry was updated.");
            updateStatus = SecurePage.UpdateStatus.Success;
          }
          else
            updateStatus = SecurePage.UpdateStatus.Unchanged;
        }
        AfterUpdate(newValue);
        if ((updateStatus == SecurePage.UpdateStatus.Unchanged) && reportUnchanged)
          Feedback.AddInfo("The " + Description + " entry was unchanged.");
        DataControl.RemoveCssClass("badupdate"); // ok now
      }
      catch (Exception ex)
      {
        Feedback.AddError("There was an unexpected error updating the " +
          Description);
        Feedback.HandleException(ex);
        updateStatus = SecurePage.UpdateStatus.Failure;
      }

      if (updateStatus == SecurePage.UpdateStatus.Success)
        IncrementUpdateCount();
      return updateStatus;
    }

    public static Control FindControl(TemplateControl templateControl, string id)
    {
      var page = templateControl as SecurePage;
      if (page != null) return page.Master.FindMainContentControl(id);
      var userControl = templateControl as UserControl;
      return userControl?.FindControl(id);
    }

    // ReSharper disable once VirtualMemberNeverOverridden.Global
    protected virtual string GetControlValue()
    {
      var htmlControl = DataControl as HtmlControl;
      if (htmlControl != null)
        if (htmlControl.HasClass("value-in-hidden-field"))
        {
          var valueId = htmlControl.ID + "Value";
          var valueControl = htmlControl.Parent.Controls.Cast<Control>()
            .FirstOrDefault(c => c.ID == valueId) as HtmlInputHidden;
          return valueControl?.Value;
        }

      return DataControl.GetValue();
    }

    protected virtual string GetCurrentValue()
    {
      throw new VoteException("NotImplemented");
    }

    public static string GetCurrentValue(DataItemBase item)
    {
      return item.GetCurrentValue();
    }

    protected virtual string GetDefaultValue()
    {
      return Default.SafeString();
    }

    protected bool HasWarning(string warning)
    {
      return _Warnings?.Contains(warning) == true;
    }

    protected virtual void IncrementUpdateCount()
    {
    }

    protected virtual void InitializeItem(TemplateControl owner,
      bool addMonitorClasses = true, FeedbackContainerControl feedback = null)
    {
      _GroupName = _GroupName.SafeString();
      var monitorClasses = addMonitorClasses
        ? $"mc mc-group-{_GroupName}-{Column}"
          .ToLowerInvariant()
        : null;

      DataControl = FindControl(owner, "Control" + _GroupName + Column);
      //  owner.Master.FindMainContentControl("Control" + _GroupName + Column);
      if (addMonitorClasses)
        DataControl.AddCssClasses(monitorClasses + " mc-data");
      var tooltip = DataControlToolTip;
      if (!string.IsNullOrWhiteSpace(tooltip))
      {
        DataControl.SetToolTip(tooltip);
        DataControl.AddCssClasses("tiptip");
      }

      Feedback = feedback ?? FindControl(owner, "Feedback" + _GroupName) as
        //  owner.Master.FindMainContentControl("Feedback" + _GroupName) as
        FeedbackContainerControl;

      _Asterisk = FindControl(owner, "Asterisk" + _GroupName + Column) as
        //owner.Master.FindMainContentControl("Asterisk" + _GroupName + Column) as
        HtmlControl;

      Label = FindControl(owner, "Label" + _GroupName + Column) as
        //owner.Master.FindMainContentControl("Label" + _GroupName + Column) as
        HtmlControl;

      if (_Asterisk != null)
      {
        if (addMonitorClasses)
          _Asterisk.AddCssClasses(monitorClasses + " mc-ast");
        tooltip = AsteriskToolTip;
        if (!string.IsNullOrWhiteSpace(tooltip))
        {
          _Asterisk.Attributes.Add("title", tooltip);
          _Asterisk.AddCssClasses("tiptip");
        }
      }

      var expandable = FindControl(owner, "Expandable" + _GroupName + Column);
      //owner.Master.FindMainContentControl("Expandable" + _GroupName + Column);
      expandable?.AddCssClasses(monitorClasses + " mc-expandable");
    }

    internal static void InitializeAll(IEnumerable<DataItemBase> items,
      SecurePage page, string groupName)
    {
      foreach (var item in items)
        item.InitializeItem(page);
      InitializeGroup(page, groupName);
    }

    internal static void InitializeGroup(TemplateControl owner, string groupName)
    {
      var monitorClasses = $"mc mc-group-{groupName}"
        .ToLowerInvariant();

      var container = FindControl(owner, "Container" + groupName);
      container?.AddCssClasses(monitorClasses + " mc-container updated");

      var control = FindControl(owner, "Control" + groupName);
      control?.AddCssClasses(monitorClasses + " mc-data");

      var undo = FindControl(owner, "Undo" + groupName);
      undo?.AddCssClasses(monitorClasses + " mc-undo");

      var feedback = FindControl(owner, "Feedback" + groupName);
      feedback?.AddCssClasses(monitorClasses + " mc-feedback");

      var ajaxloader = FindControl(owner, "AjaxLoader" + groupName);
      ajaxloader?.AddCssClasses(monitorClasses + " mc-ajaxloader");

      var button = FindControl(owner, "Button" + groupName);
      button?.AddCssClasses(monitorClasses + " mc-button");

      var description = FindControl(owner, "Description" + groupName);
      description?.AddCssClasses(monitorClasses + " mc-desc");
    }

    internal static void LoadAllControls(IEnumerable<DataItemBase> items)
    {
      foreach (var item in items)
        item.LoadControl();
    }

    public virtual void LoadControl()
    {
      var value = GetCurrentValue();
      DataControl.SetValue(value);
      AfterUpdate(value);
    }

    protected virtual void Log(string oldValue, string newValue)
    {
    }

    internal static void ResetAll(IEnumerable<DataItemBase> items)
    {
      foreach (var item in items)
        item.ResetControl();
    }

    public virtual void ResetControl()
    {
      DataControl.SetValue(GetDefaultValue());
    }

    // ReSharper disable once VirtualMemberNeverOverridden.Global
    protected virtual void SetControlValue(string newValue)
    {
      var htmlControl = DataControl as HtmlControl;
      if (htmlControl != null)
        if (htmlControl.HasClass("value-in-hidden-field"))
        {
          var valueId = htmlControl.ID + "Value";
          var valueControl = htmlControl.Parent.Controls.Cast<Control>()
            .FirstOrDefault(c => c.ID == valueId) as HtmlInputHidden;
          if (valueControl != null)
            valueControl.Value = newValue;
          return;
        }

      DataControl.SetValue(newValue);
    }

    private void SetHyperlinkLabel(string value)
    {
      if (Label != null)
      {
        Label.Controls.Clear();
        Control label;
        if (string.IsNullOrWhiteSpace(value))
          label = new LiteralControl(Description);
        else
        {
          var tooltip = (HyperlinkToolTip ?? "Visit {link}").Replace("{link}",
            value);
          label = new HyperLink
          {
            Text = Description,
            NavigateUrl = VotePage.NormalizeUrl(value),
            Target = "view",
            ToolTip = tooltip
          }.AddCssClasses("tiptip");
        }
        label.AddTo(Label);
      }
    }

    private void SetMailToLabel(string value)
    {
      if (Label != null)
      {
        Label.Controls.Clear();
        Control label;
        if (string.IsNullOrWhiteSpace(value))
          label = new LiteralControl(Description);
        else
        {
          var tooltip = (HyperlinkToolTip ?? "Send mail to {email}").Replace("{email}",
            value);
          label = new HyperLink
          {
            Text = Description,
            NavigateUrl = VotePage.NormalizeEmailHRef(value),
            ToolTip = tooltip
          }.AddCssClasses("tiptip");
        }
        label.AddTo(Label);
      }
    }

    private void StripRedundantWhiteSpace()
    {
      DataControl.SetValue(DataControl.GetValue().StripRedundantWhiteSpace());
    }

    protected virtual bool Update(object newValue)
    {
      throw new VoteException("NotImplemented");
    }

    internal static int UpdateAll(IEnumerable<DataItemBase> items,
      FeedbackContainerControl feedback, bool showSummary = true,
      UpdatePanel updatePanel = null)
    {
      var errorCount = 0;
      try
      {
        var updateCount = 0;
        var changed = false;

        foreach (var item in items)
        {
          var updateStatus = item.DoUpdate(false);
          switch (updateStatus)
          {
            case SecurePage.UpdateStatus.Failure:
              errorCount++;
              changed = true;
              break;

            case SecurePage.UpdateStatus.Success:
              changed = true;
              updateCount++;
              break;
          }
        }
        if (changed) updatePanel?.Update();
        if (showSummary)
        {
          //if (updateCount > 1)
          feedback.AddInfo(updateCount.ToString(CultureInfo.InvariantCulture) +
            updateCount.Plural(" item was", " items were") + " updated.");
          if (errorCount > 0)
            feedback.AddError(errorCount.ToString(CultureInfo.InvariantCulture) +
              errorCount.Plural(" item was", " items were") +
              " not updated due to errors.");
        }
      }
      catch (Exception ex)
      {
        feedback.HandleException(ex);
      }

      return errorCount;
    }

    private bool Validate()
    {
      return Validator == null || Validator(this);
    }

    internal static bool ValidateAll(IEnumerable<DataItemBase> items)
    {
      var isValid = true;
      foreach (var item in items)
        if (!item.Validate())
        {
          isValid = false;
          break;
        }

      return isValid;
    }

    internal static IEnumerable<DataItemBase> WithWarning(IEnumerable<DataItemBase> items,
      string warning)
    {
      return items.Where(item => item.HasWarning(warning)).ToList();
    }

    // Conversions

    protected static object ToBool(string value)
    {
      bool result;
      bool.TryParse(value, out result);
      return result;
    }

    protected static object ToDateTime(string value)
    {
      DateTime result;
      if (!DateTime.TryParse(value, out result))
        result = VotePage.DefaultDbDate;
      return result;
    }

    protected static object ToInt(string value)
    {
      int result;
      int.TryParse(value, out result);
      return result;
    }

    // Validations

    protected static string ToDisplayDate(DateTime date)
    {
      return date == VotePage.DefaultDbDate
        ? string.Empty
        : date.ToShortDateString();
    }

    protected static string ToDisplay(object o)
    {
      if (o is DateTime) return ToDisplayDate((DateTime) o);
      return o.ToString();
    }

    protected static bool ValidateCityStateZip(DataItemBase item)
    {
      item.StripRedundantWhiteSpace();
      item.DataControl.SetValue(
        Validation.FixCityStateZip(item.DataControl.GetValue()));
      return true;
    }

    protected static bool ValidateDateOfBirth(DataItemBase item)
    {
      bool success;
      var now = DateTime.UtcNow;
      var min = now - new TimeSpan(36525, 0, 0, 0); // 100 years
      var max = now - new TimeSpan(7305, 0, 0, 0); // 20 years
      var validated =
        item.Feedback.ValidateDateOptional(item.DataControl as TextBox,
          out success, item.Description, min, max, VotePage.DefaultDbDate);
      if (success)
        item.DataControl.SetValue(
          Politicians.GetDateOfBirthFromDateTime(validated));
      return success;
    }

    protected static bool ValidateDate(DataItemBase item)
    {
      bool success;
      var validated =
        item.Feedback.ValidateDate(item.DataControl as TextBox, out success,
          item.Description, VotePage.DefaultDbDate);
      if (success)
        item.DataControl.SetValue(ToDisplayDate(validated));
      return success;
    }

    protected static bool ValidateDateOptional(DataItemBase item)
    {
      bool success;
      var validated =
        item.Feedback.ValidateDateOptional(item.DataControl as TextBox, out success,
          item.Description, VotePage.DefaultDbDate);
      if (success)
        item.DataControl.SetValue(ToDisplayDate(validated));
      return success;
    }

    protected static bool ValidateEmail(DataItemBase item)
    {
      item.StripRedundantWhiteSpace();
      var value = item.DataControl.GetValue();
      if (value.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase))
        value = value.Substring(7);
      if (!string.IsNullOrWhiteSpace(value) &&
        !Validation.IsValidEmailAddress(value))
      {
        item.Feedback.PostValidationError(item.DataControl,
          "We do not recognize the Email as valid");
        return false;
      }
      item.DataControl.SetValue(Validation.StripWebProtocol(value));
      return true;
    }

    protected static bool ValidateEmailRequired(DataItemBase item)
    {
      if (!ValidateRequired(item)) return false;
      return ValidateEmail(item);
    }

    protected static bool ValidateFirstName(DataItemBase item)
    {
      bool success;
      item.StripRedundantWhiteSpace();
      item.Feedback.ValidateRequired(item.DataControl, item.Description,
        out success);
      if (!success) return false;
      item.DataControl.SetValue(
        Validation.FixGivenName(item.DataControl.GetValue()));
      return true;
    }

    protected static bool ValidateLastName(DataItemBase item)
    {
      bool success;
      item.StripRedundantWhiteSpace();
      item.Feedback.ValidateRequired(item.DataControl, item.Description,
        out success);
      if (!success) return false;
      var value = item.DataControl.GetValue();
      if (!Validation.IsValidLastName(value))
      {
        item.Feedback.PostValidationError(item.DataControl,
          "We do not recognize the Last Name as valid");
        return false;
      }
      var tokens = value.Trim()
        .Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
      if (tokens.Length >= 2)
      {
        if (char.IsLetter(tokens[0][0]) && (tokens[0][1] == '.'))
        {
          item.Feedback.PostValidationError(item.DataControl,
            "Please use the Middle or Initial field for initials");
          return false;
        }
        if (Validation.IsValidNameSuffix(tokens[tokens.Length - 1]))
        {
          item.Feedback.PostValidationError(item.DataControl,
            "Please use the Suffix field for " + tokens[tokens.Length - 1]);
          return false;
        }
      }

      item.DataControl.SetValue(Validation.FixLastName(value));
      return true;
    }

    protected static bool ValidateMiddleName(DataItemBase item)
    {
      item.StripRedundantWhiteSpace();
      item.DataControl.SetValue(
        Validation.FixGivenName(item.DataControl.GetValue()));
      return true;
    }

    protected static bool ValidateNickname(DataItemBase item)
    {
      item.StripRedundantWhiteSpace();
      var value = item.DataControl.GetValue();
      if (!Validation.IsValidNickname(value))
      {
        item.Feedback.PostValidationError(item.DataControl,
          "We do not recognize the Nickname as valid");
        return false;
      }
      item.DataControl.SetValue(Validation.FixNickname(value));
      return true;
    }

    protected static bool ValidateNumeric(DataItemBase item)
    {
      var value = item.DataControl.GetValue().Trim();
      if (!value.IsDigits())
      {
        item.Feedback.PostValidationError(item.DataControl, "A numeric value is required");
        return false;
      }
      item.DataControl.SetValue(value);
      return true;
    }

    protected static bool ValidateSignedNumeric(DataItemBase item)
    {
      var value = item.DataControl.GetValue().Trim();
      if (!Regex.IsMatch(value, @"^(?:\+|-)?[0-9]+$"))
      {
        item.Feedback.PostValidationError(item.DataControl, "A numeric value is required");
        return false;
      }
      item.DataControl.SetValue(value);
      return true;
    }

    public static bool ValidateRequired(DataItemBase item)
    {
      bool success;
      item.StripRedundantWhiteSpace();
      var value = item.Feedback.ValidateRequired(item.DataControl, item.Description,
        out success);
      if (success)
        item.DataControl.SetValue(ToDisplay(value));
      return success;
    }

    private static bool ValidateSentenceCase(DataItemBase item, bool required)
    {
      if (required && !ValidateRequired(item))
        return false;

      var value = item.DataControl.GetValue();
      value = value.StripRedundantSpaces();

      // We only recase if it's all upper to begin with
      if (value.IsAllUpperCase())
      {
        value = value.ToSentenceCase();
        item.AddWarning("recased");
      }

      item.DataControl.SetValue(value);

      return true;
    }

    protected static bool ValidateSentenceCase(DataItemBase item)
    {
      return ValidateSentenceCase(item, false);
    }

    protected static bool ValidateSentenceCaseRequired(DataItemBase item)
    {
      return ValidateSentenceCase(item, true);
    }

    protected static bool ValidateStreet(DataItemBase item)
    {
      item.StripRedundantWhiteSpace();
      item.DataControl.SetValue(Validation.FixAddress(item.DataControl.GetValue()));
      return true;
    }

    protected static bool ValidateSuffix(DataItemBase item)
    {
      item.StripRedundantWhiteSpace();
      var value = item.DataControl.GetValue();
      if (!Validation.IsValidNameSuffix(value))
      {
        item.Feedback.PostValidationError(item.DataControl,
          "We do not recognize the Suffix as valid");
        return false;
      }
      item.DataControl.SetValue(Validation.FixNameSuffix(value));
      return true;
    }

    protected static bool ValidateWhiteSpace(DataItemBase item)
    {
      item.StripRedundantWhiteSpace();
      return true;
    }

    private static bool ValidateTitleCase(DataItemBase item, bool required)
    {
      if (required && !ValidateRequired(item))
        return false;

      // We always strip line breaks
      var value = item.DataControl.GetValue();
      value = value.Replace(" \r\n", " ");
      value = value.Replace("\r\n ", " ");
      value = value.Replace("\r\n", " ");
      value = value.Replace(" \r", " ");
      value = value.Replace("\r ", " ");
      value = value.Replace("\r", " ");
      value = value.Replace(" \n", " ");
      value = value.Replace("\n ", " ");
      value = value.Replace("\n", " ");

      value = value.StripRedundantSpaces();

      // We only recase if it's all upper to begin with
      if (value.IsAllUpperCase())
      {
        value = value.ToTitleCase();
        item.AddWarning("recased");
      }

      item.DataControl.SetValue(value);

      return true;
    }

    protected static bool ValidateTitleCase(DataItemBase item)
    {
      return ValidateTitleCase(item, false);
    }

    protected static bool ValidateTitleCaseRequired(DataItemBase item)
    {
      return ValidateTitleCase(item, true);
    }

    protected static bool ValidateWebAddress(DataItemBase item)
    {
      item.DataControl.SetValue(
        Validation.StripWebProtocol(item.DataControl.GetValue()));
      return true;
    }
  }

  internal static class DataItemBaseExtensions
  {
    internal static void ClearValidationErrors(this IEnumerable<DataItemBase> items)
    {
      DataItemBase.ClearAllValidationErrors(items);
    }

    internal static void Initialize(this IEnumerable<DataItemBase> items,
      SecurePage page, string groupName)
    {
      DataItemBase.InitializeAll(items, page, groupName);
    }

    internal static void LoadControls(this IEnumerable<DataItemBase> items)
    {
      DataItemBase.LoadAllControls(items);
    }

    internal static void Reset(this IEnumerable<DataItemBase> items)
    {
      DataItemBase.ResetAll(items);
    }

    internal static int Update(this IEnumerable<DataItemBase> items,
      FeedbackContainerControl feedback, bool showSummary = true,
      UpdatePanel updatePanel = null)
    {
      return DataItemBase.UpdateAll(items, feedback, showSummary, updatePanel);
    }

    internal static bool Validate(this IEnumerable<DataItemBase> items)
    {
      return DataItemBase.ValidateAll(items);
    }

    internal static IEnumerable<DataItemBase> WithWarning(this IEnumerable<DataItemBase> items,
      string warning)
    {
      return DataItemBase.WithWarning(items, warning);
    }
  }

  #region ReSharper restore

  // ReSharper restore VirtualMemberNeverOverriden.Global
  // ReSharper restore UnassignedField.Global
  // ReSharper restore UnusedAutoPropertyAccessor.Global
  // ReSharper restore UnusedMethodReturnValue.Global
  // ReSharper restore UnusedMember.Global
  // ReSharper restore MemberCanBeProtected.Global
  // ReSharper restore MemberCanBePrivate.Global

  #endregion ReSharper restore

  #endregion Internal
}