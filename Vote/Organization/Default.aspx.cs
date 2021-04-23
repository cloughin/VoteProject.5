using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Vote.Organization
{
  public partial class Default : VotePage
  {
    //protected void LoadData()
    //{
    //  #region TextBoxes
    //  TextBoxOrganization.Text = db.Organizations(Session["UserOrganizationCode"].ToString(), "Organization");
    //  TextBoxOrganizationURL.Text = db.Organizations(Session["UserOrganizationCode"].ToString(), "OrganizationURL");
    //  TextBoxOrganizationEmail.Text = db.Organizations(Session["UserOrganizationCode"].ToString(), "OrganizationEmail");
    //  TextBoxAdressLine1.Text = db.Organizations(Session["UserOrganizationCode"].ToString(), "AddressLine1");
    //  TextBoxAddressLine2.Text = db.Organizations(Session["UserOrganizationCode"].ToString(), "AddressLine2");
    //  TextBoxCityStateZip.Text = db.Organizations(Session["UserOrganizationCode"].ToString(), "CityStateZip");
    //  TextBoxContact.Text = db.Organizations(Session["UserOrganizationCode"].ToString(), "Contact");
    //  TextBoxContactTitle.Text = db.Organizations(Session["UserOrganizationCode"].ToString(), "ContactTitle");
    //  TextBoxContactEmail.Text = db.Organizations(Session["UserOrganizationCode"].ToString(), "ContactEmail");
    //  TextBoxContactPhone.Text = db.Organizations(Session["UserOrganizationCode"].ToString(), "ContactPhone");
    //  TextBoxAltContact.Text = db.Organizations(Session["UserOrganizationCode"].ToString(), "AltContact");
    //  TextBoxAltContactTitle.Text = db.Organizations(Session["UserOrganizationCode"].ToString(), "AltContactTitle");
    //  TextBoxAltContactEMail.Text = db.Organizations(Session["UserOrganizationCode"].ToString(), "AltContactEMail");
    //  TextBoxAltContactPhone.Text = db.Organizations(Session["UserOrganizationCode"].ToString(), "AltContactPhone");
    //  TextBoxNote.Text = db.Organizations(Session["UserOrganizationCode"].ToString(), "Note");
    //  #endregion TextBoxes

    //  #region mailto: and Urls
    //  if (db.Organizations(Session["UserOrganizationCode"].ToString(), "OrganizationEmail") != string.Empty)
    //    HyperLinkOrganizationEmail.NavigateUrl =
    //    db.UrlAddressEmail(db.Organizations(Session["UserOrganizationCode"].ToString(), "OrganizationEmail"));
    //  else
    //    HyperLinkEmailContact.NavigateUrl = string.Empty;

    //  if (db.Organizations(Session["UserOrganizationCode"].ToString(), "ContactEmail") != string.Empty)
    //    HyperLinkEmailContact.NavigateUrl =
    //    db.UrlAddressEmail(db.Organizations(Session["UserOrganizationCode"].ToString(), "ContactEmail"));
    //  else
    //    HyperLinkEmailContact.NavigateUrl = string.Empty;

    //  if (db.Organizations(Session["UserOrganizationCode"].ToString(), "AltContactEMail") != string.Empty)
    //    HyperLinkEmailContact.NavigateUrl =
    //    db.UrlAddressEmail(db.Organizations(Session["UserOrganizationCode"].ToString(), "AltContactEMail"));
    //  else
    //    HyperLinkEmailContact.NavigateUrl = string.Empty;

    //  if (db.Organizations(Session["UserOrganizationCode"].ToString(), "OrganizationURL") != string.Empty)
    //  {
    //    HyperLinkOrganizationURL.NavigateUrl = NormalizeUrl(db.Organizations(Session["UserOrganizationCode"].ToString(), "OrganizationURL"));
    //    HyperLinkOrganizationURL.Target = "view";
    //  }
    //  else
    //    HyperLinkOrganizationURL.NavigateUrl = string.Empty;


    //  #endregion EMails and Urls
    //}
    //protected void RecordData()
    //{
    //  db.OrganizationsUpdate(Session["UserOrganizationCode"].ToString(), "Organization", TextBoxOrganization.Text);
    //  db.OrganizationsUpdate(Session["UserOrganizationCode"].ToString(), "OrganizationURL", TextBoxOrganizationURL.Text);
    //  db.OrganizationsUpdate(Session["UserOrganizationCode"].ToString(), "OrganizationEmail", TextBoxOrganizationEmail.Text);
    //  db.OrganizationsUpdate(Session["UserOrganizationCode"].ToString(), "AddressLine1", TextBoxAdressLine1.Text);
    //  db.OrganizationsUpdate(Session["UserOrganizationCode"].ToString(), "AddressLine2", TextBoxAddressLine2.Text);
    //  db.OrganizationsUpdate(Session["UserOrganizationCode"].ToString(), "CityStateZip", TextBoxCityStateZip.Text);
    //  db.OrganizationsUpdate(Session["UserOrganizationCode"].ToString(), "Contact", TextBoxContact.Text);
    //  db.OrganizationsUpdate(Session["UserOrganizationCode"].ToString(), "ContactTitle", TextBoxContactTitle.Text);
    //  db.OrganizationsUpdate(Session["UserOrganizationCode"].ToString(), "ContactEmail", TextBoxContactEmail.Text);
    //  db.OrganizationsUpdate(Session["UserOrganizationCode"].ToString(), "ContactPhone", TextBoxContactPhone.Text);
    //  db.OrganizationsUpdate(Session["UserOrganizationCode"].ToString(), "AltContact", TextBoxAltContact.Text);
    //  db.OrganizationsUpdate(Session["UserOrganizationCode"].ToString(), "AltContactTitle", TextBoxAltContactTitle.Text);
    //  db.OrganizationsUpdate(Session["UserOrganizationCode"].ToString(), "AltContactEMail", TextBoxAltContactEMail.Text);
    //  db.OrganizationsUpdate(Session["UserOrganizationCode"].ToString(), "AltContactPhone", TextBoxAltContactPhone.Text);
    //  db.OrganizationsUpdate(Session["UserOrganizationCode"].ToString(), "Note", TextBoxNote.Text);
    //}
    //protected void ButtonSubmit_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    #region Check TextBoxes for Illegal Stuff
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxOrganization);
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxOrganizationURL);
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxOrganizationEmail);
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxAdressLine1);
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxAddressLine2);
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxCityStateZip);
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxContact);
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxContactTitle);
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxContactEmail);
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxContactPhone);
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxAltContact);
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxAltContactTitle);
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxAltContactEMail);
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxAltContactPhone);
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxNote);
    //    #endregion Check TextBoxes for Illegal Stuff

    //    RecordData();

    //    LoadData();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }

    //}

    //protected void Page_Load(object sender, EventArgs e)
    //{
    //  if (!IsPostBack)
    //  {
    //    // A MASTER user enters either Admin/Admin.aspx, Design/Default.aspx, or Admin/Organization.aspx from anchors on Master/Default.aspx
    //    //For ADMIN user Session["UserOrganizationCode"] gets set in Login.aspx
    //    if ((db.User() == db.UserType.Master)
    //      && (!string.IsNullOrEmpty(QueryOrganization)))
    //      Session["UserOrganizationCode"] = QueryOrganization;

    //    #region Security Checks
    //    if (Session["UserOrganizationCode"].ToString() == string.Empty)
    //      HandleFatalError("The UserOrganizationCode is missing");
    //    #endregion Security Checks

    //    try
    //    {
    //      LabelDesignCode.Text = Session["UserOrganizationCode"].ToString();

    //      LoadData();
    //    }
    //    catch (Exception ex)
    //    {
    //      Msg.Text = db.Fail(ex.Message);
    //      db.Log_Error_Admin(ex);
    //    }
    //  }
    //}
  }
}
