using System;
using System.Net.Mail;
using System.Web.UI;

namespace Vote.Controls
{
  public partial class EmailForm : UserControl
  {
    private bool? _IsHuman;
    private string[] _Texts;
    public string ToEmailAddress { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
      Page.IncludeJs("http://ajax.googleapis.com/ajax/libs/jquery/1.5/jquery.min.js");
      Page.IncludeJs(
        "http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js");
      Page.IncludeJs("~/js/jq/jquery.simpleCombo.js");
      Page.IncludeJs("~/js/EmailForm.js");
      Page.IncludeCss("~/js/jq/jquery-ui.css");
      Page.IncludeCss("~/css/EmailForm.css");

      EmailFormErrorLabel.Visible = false;
      EmailFormGoodLabel.Visible = false;

      // setup client-side input processing
      EmailFormCaptcha.UserInputClientID = EmailFormCaptchaCodeTextBox.ClientID;

      if (!IsPostBack)
        AddItems("Select a subject or type your own", "");

// ReSharper disable InvertIf
      if (IsPostBack)
// ReSharper restore InvertIf
      {
        var subject = Request.Form["EmailForm$EmailFormSubject"];
        AddItems(subject, subject);
        // validate the Captcha to check we're not dealing with a bot
        if (IsHuman)
          try
          {
            if (!Validation.IsValidEmailAddress(EmailFormFromEmailAddress.Text))
              throw new VoteException("The email address you entered is not valid");
            if (string.IsNullOrWhiteSpace(subject))
              throw new VoteException(
                "Please enter or select a subject for the email");
            if (string.IsNullOrWhiteSpace(EmailFormMessage.Text))
              throw new VoteException("The email message body is missing");
            var email = new MailMessage
              {
                IsBodyHtml = false,
                From = new MailAddress(EmailFormFromEmailAddress.Text)
              };
            email.To.Add(ToEmailAddress);
            email.Subject = subject;
            email.Body = EmailFormMessage.Text;

 #if !NoEmail
           //var smtpClient = new SmtpClient("localhost");
            //smtpClient.Send(email);
            EmailUtility.GetConfiguredSmtpClient().Send(email);
#endif
            EmailFormGoodLabel.Text = "Email has been sent";
            EmailFormGoodLabel.Visible = true;
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

    //private void AddItem(string text)
    //{
    //  AddItem(text, text);
    //}

    private void AddItem(string text, string value)
    {
      EmailFormSubject.AddItem(text, value);
    }

    public void SetItems(params string[] texts)
    {
      _Texts = texts;
    }

    private void AddItems(string firstText, string firstValue)
    {
      EmailFormSubject.Items.Clear();
      AddItem(firstText, firstValue);
      if (_Texts != null)
        foreach (var text in _Texts)
          AddItem(text, text);
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
  }
}