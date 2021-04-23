using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;

namespace TestRedirect
{
  public partial class TestSmtp : Form
  {
    public TestSmtp()
    {
      InitializeComponent();
    }

    private void SendButton_Click(object sender, EventArgs e)
    {
      try
      {
        MailMessage mailMessage = new MailMessage();
        mailMessage.IsBodyHtml = false;
        mailMessage.From = new MailAddress(FromTextBox.Text);
        mailMessage.To.Add(ToTextBox.Text);
        mailMessage.Subject = SubjectTextBox.Text;
        mailMessage.Body = BodyTextBox.Text;

        SmtpClient smtpClient = new SmtpClient(ServerTextBox.Text);
        smtpClient.Send(mailMessage);

        MessageBox.Show("Message sent", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
  }
}
