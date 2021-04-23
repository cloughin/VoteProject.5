using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web.UI;
using static System.String;

namespace Vote.Controls
{
  public partial class EmailFormResponsive : UserControl
  {
    public class OptionalItem
    {
      public UserControl UserControl;
      public string Id;
      public string Label;
      public string EmailLabel;
      public string Value;
      public Func<OptionalItem, string> Validator;

      public Control GetControl() => UserControl.FindControl(Id);

      public string GetValue() => Value ?? GetControl().GetValue();

      public static string Required(OptionalItem item)
      {
        if (IsNullOrWhiteSpace(item.GetValue()))
          return item.Label + " is required";
        return null;
      }

      public static void ValidateAll(IEnumerable<OptionalItem> items)
      {
        foreach (var item in items)
        {
          var message = item.Validator?.Invoke(item);
          if (!IsNullOrWhiteSpace(message))
            throw new VoteException(message);
        }
      }

      public static string FormatAll(IEnumerable<OptionalItem> items)
      {
        var results = new List<string>();
        foreach (var item in items)
        {
          var label = item.EmailLabel;
          if (!label.EndsWith(":")) label += ":";
          var value = item.GetValue();
          results.Add($"{label} {value}");
        }
        var result = Join("\n", results);
        if (!IsNullOrWhiteSpace(result))
          result += "\n\n";
        return result;
      }

      public static void AddAllToDictionary(IEnumerable<OptionalItem> items,
        Dictionary<string, string> dict)
      {
        foreach (var item in items)
          dict.Add(item.Id, item.GetValue());
      }

      public static void ClearAll(IEnumerable<OptionalItem> items)
      {
        foreach (var item in items)
          item.GetControl().SetValue(Empty);
      }
    }

    private bool? _IsHuman;
    private string[] _Texts;
    private readonly List<OptionalItem> _OptionalItems = new List<OptionalItem>();
    public string ToEmailAddress { get; set; }
    public string CcEmailAddress { get; set; }
    public bool MessageOptional { get; set; }
    public Action<Dictionary<string, string>> Callback { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
      //Page.IncludeJs("https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js");
      //Page.IncludeJs(
      //  "https://ajax.googleapis.com/ajax/libs/jqueryui/1.11.4/jquery-ui.min.js");
      //Page.IncludeCss(
      //  "https://ajax.googleapis.com/ajax/libs/jqueryui/1.11.4/themes/smoothness/jquery-ui.css");
      //Page.IncludeJs("~/js/vote/publicutil.js");
      //Page.IncludeJs("~/js/EmailFormResponsive.js");

      EmailFormErrorLabel.Visible = false;
      EmailFormGoodLabel.Visible = false;

      // setup client-side input processing
      EmailFormCaptcha.UserInputClientID = EmailFormCaptchaCodeTextBox.ClientID;

      if (!IsPostBack)
      {
        AddSubjectItems();
        if (MessageOptional) EmailFormMessageLabel.InnerText = "Message (optional):";
      }
    }

    private void AddItem(string text, string value) =>
      EmailFormSubject.AddItem(text, value);

    public void AddOptionalItem(Control control, string id, string label,
      string emailLabel = null, Func<OptionalItem, string> validator = null)
    {
      var wrapper = new HtmlDiv().AddTo(EmailFormFieldPlaceHolder,
        id.ToLowerInvariant() + "-wrapper");
      if (IsNullOrWhiteSpace(label)) label = id;
      if (IsNullOrWhiteSpace(emailLabel)) emailLabel = label;
      _OptionalItems.Add(new OptionalItem
      {
        UserControl = this,
        Id = id,
        Label = label,
        EmailLabel = emailLabel,
        Validator = validator
      });
      if (!label.EndsWith(":")) label += ":";
      control.ID = id;
      new HtmlP {InnerText = label}.AddTo(wrapper, "email-form-label");
      control.AddTo(wrapper);
    }

    public void AddOptionalItem(Control control, bool required, string name,
      string label = null, string emailLabel = null)
    {
      if (IsNullOrWhiteSpace(label)) label = name;
      var validator = required ? OptionalItem.Required : (Func<OptionalItem, string>) null;
      AddOptionalItem(control, name, label, emailLabel, validator);
    }

    public void SetItems(params string[] texts) => _Texts = texts;

    public void SetOptionalValue(string id, string value)
    {
      // for modifying display in email
      var item = _OptionalItems.FirstOrDefault(i => i.Id == id);
      if (item != null)
        item.Value = value;
    }

    private void AddSubjectItems()
    {
      EmailFormSubject.Items.Clear();
      AddItem("Select a subject", "");
      if (_Texts != null)
        foreach (var text in _Texts)
          AddItem(text, text);
      AddItem("Other, please specify", "Other");
    }

    private bool IsHuman
    {
      get
      {
        if (_IsHuman == null)
          _IsHuman = EmailFormCaptcha.Validate(EmailFormCaptchaCodeTextBox.Text);
        return _IsHuman.Value;
      }
    }

    protected void SubmitForm(object sender, EventArgs e)
    {
      var subject = EmailFormSubject.Value;
      if (subject == "Other")
        subject = EmailFormOtherSubject.Text;
      // validate the Captcha to check we're not dealing with a bot
      if (IsHuman)
        try
        {
          if (!Validation.IsValidEmailAddress(EmailFormFromEmailAddress.Text))
            throw new VoteException("The email address you entered is not valid");
          if (IsNullOrWhiteSpace(subject))
            throw new VoteException("Please select a subject for the email");
          if (!MessageOptional && IsNullOrWhiteSpace(EmailFormMessage.Text))
            throw new VoteException("The email message body is missing");

          OptionalItem.ValidateAll(_OptionalItems);

          if (Callback != null)
          {
            // create dictionary for callback
            var dict = new Dictionary<string, string>
            {
              // add the fixed items
              {"EmailFormSubject", subject},
              {"EmailFormFromEmailAddress", EmailFormFromEmailAddress.Text},
              {"EmailFormMessage", EmailFormMessage.Text}
            };

            // add any optional items
            OptionalItem.AddAllToDictionary(_OptionalItems, dict);

            Callback(dict);
          }

          var email = new MailMessage
          {
            IsBodyHtml = false,
            //From = new MailAddress(EmailFormFromEmailAddress.Text)
            From = new MailAddress("info@vote-usa.org")
          };
          email.To.Add(ToEmailAddress);
          if (!IsNullOrWhiteSpace(CcEmailAddress))
            email.CC.Add(CcEmailAddress);
          email.Subject = subject;
          email.Body = $"Email: {EmailFormFromEmailAddress.Text}\n\n" +
            $"{OptionalItem.FormatAll(_OptionalItems)}{EmailFormMessage.Text}";

#if !NoEmail
          EmailUtility.GetConfiguredSmtpClient().Send(email);
#endif
          EmailFormGoodLabel.Text = "Email has been sent";
          EmailFormGoodLabel.Visible = true;

          EmailFormSubject.Value = Empty;
          EmailFormOtherSubject.Text = Empty;
          EmailFormFromEmailAddress.Text = Empty;
          EmailFormMessage.Text = Empty;
          OptionalItem.ClearAll(_OptionalItems);
        }
        catch (Exception ex)
        {
          EmailFormErrorLabel.Text = ex.Message;
          EmailFormErrorLabel.Visible = true;
        }
      else
      {
        EmailFormErrorLabel.Text = "The typed characters did not match the picture";
        EmailFormErrorLabel.Visible = true;
      }
      EmailFormCaptchaCodeTextBox.Text = null; // clear previous user input
    }
  }
}