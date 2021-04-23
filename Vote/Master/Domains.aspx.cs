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

namespace Vote.Master
{
  public partial class Domains1 : VotePage
  {
    //protected void LoadCodes4Domain2Change()
    //{
    //  LabelDomainDesignCode.Text = db.Domains(LabelDomainServerName.Text, "DomainDesignCode");
    //  LabelDomainDataCode.Text = db.Domains(LabelDomainServerName.Text, "DomainDataCode");
    //  LabelDomainOrganizationCode.Text = db.Domains(LabelDomainServerName.Text, "DomainOrganizationCode");
    //}
    //protected void EmptyCodes4Domain2Change()
    //{
    //  LabelDomainServerName.Text = "No Domain has been selected";
    //  LabelDomainDesignCode.Text = string.Empty;
    //  LabelDomainDataCode.Text = string.Empty;
    //  LabelDomainOrganizationCode.Text = string.Empty;
    //}
    //protected void LoadDomains()
    //{
    //  ListBoxDomainServerName.Items.Clear();

    //  DataTable DomainsTable = db.Table(sql.Domains());
    //  foreach (DataRow DomainRow in DomainsTable.Rows)
    //  {
    //    ListItem ListBoxDomain = new ListItem();
    //    ListBoxDomain.Value = DomainRow["DomainServerName"].ToString();
    //    ListBoxDomain.Text = DomainRow["DomainServerName"].ToString();
    //    ListBoxDomainServerName.Items.Add(ListBoxDomain);
    //  }
    //}
    //protected void LoadDomainDesignCodes()
    //{
    //  string SQL = string.Empty;
    //  SQL += " SELECT ";
    //  SQL += " DomainDesigns.DomainDesignCode ";
    //  SQL += " FROM DomainDesigns ";
    //  SQL += " ORDER BY DomainDesigns.DomainDesignCode";

    //  DataTable DomainDesignsTable = db.Table(SQL);
    //  foreach (DataRow DomainDesignRow in DomainDesignsTable.Rows)
    //  {
    //    ListItem ListBoxDomainDesign = new ListItem();
    //    ListBoxDomainDesign.Value = DomainDesignRow["DomainDesignCode"].ToString();
    //    ListBoxDomainDesign.Text = DomainDesignRow["DomainDesignCode"].ToString();
    //    ListBoxDomainDesignCode.Items.Add(ListBoxDomainDesign);
    //  }
    //}
    //protected void LoadDomainDataCodes()
    //{
    //  ListItem ListBoxDomainData = new ListItem();
    //  ListBoxDomainData.Value = "US";
    //  ListBoxDomainData.Text = "US";
    //  ListBoxDomainDataCode.Items.Add(ListBoxDomainData);

    //  DataTable StatesTable = db.Table(sql.States_51());
    //  foreach (DataRow StateRow in StatesTable.Rows)
    //  {
    //    ListItem ListBoxData = new ListItem();
    //    ListBoxData.Value = StateRow["StateCode"].ToString();
    //    ListBoxData.Text = StateRow["StateCode"].ToString();
    //    ListBoxDomainDataCode.Items.Add(ListBoxData);
    //  }
    //}
    //protected void LoadDomainOrganizationCodes()
    //{
    //  string SQL = string.Empty;
    //  SQL += " SELECT ";
    //  SQL += " Organizations.OrganizationCode ";
    //  SQL += " FROM Organizations ";
    //  SQL += " ORDER BY Organizations.OrganizationCode";

    //  DataTable OrganizationsTable = db.Table(SQL);
    //  foreach (DataRow OrganizationRow in OrganizationsTable.Rows)
    //  {
    //    ListItem ListBoxOrganization = new ListItem();
    //    ListBoxOrganization.Value = OrganizationRow["OrganizationCode"].ToString();
    //    ListBoxOrganization.Text = OrganizationRow["OrganizationCode"].ToString();
    //    ListBoxDomainOrganizationCode.Items.Add(ListBoxOrganization);
    //  }
    //}

    //protected void ButtonAddDomain_Click(object sender, EventArgs e)
    //{
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxDomain2Add);

    //  if (db.Rows(sql.Domains(TextBoxDomain2Add.Text.Trim())) > 0)
    //  {
    //    Msg.Text = db.Fail("The domain alread exists.");
    //  }
    //  else
    //  {
    //    string SQL = string.Empty;
    //    SQL += " INSERT INTO Domains ";
    //    SQL += "(";
    //    SQL += "DomainServerName";
    //    SQL += ")";
    //    SQL += " VALUES ";
    //    SQL += "(";
    //    SQL += db.SQLLit(TextBoxDomain2Add.Text.Trim());
    //    SQL += ")";
    //    db.ExecuteSQL(SQL);

    //    LabelDomainServerName.Text = TextBoxDomain2Add.Text.Trim();

    //    LoadDomains();
    //    LoadCodes4Domain2Change();

    //    Msg.Text = db.Ok("The domain has been added. Now select a Design Code, Data Code and Organization Code.");
    //  }
    //}
    //protected void ButtonDeleteDomain_Click(object sender, EventArgs e)
    //{
    //  if (LabelDomainServerName.Text == "No Domain has been selected")
    //  {
    //    Msg.Text = db.Fail("A Domain needs to be selected.");
    //  }
    //  else
    //  {
    //    string SQL = string.Empty;
    //    SQL += "DELETE FROM Domains";
    //    SQL += " WHERE DomainServerName = " + db.SQLLit(LabelDomainServerName.Text);
    //    db.ExecuteSQL(SQL);

    //    LoadDomains();
    //    EmptyCodes4Domain2Change();

    //    Msg.Text = db.Ok("The domain has been deleted.");
    //  }

    //}

    //protected void ListBoxDomainDesignCode_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  if (LabelDomainServerName.Text == "No Domain has been selected")
    //  {
    //    Msg.Text = db.Fail("A Domain needs to be selected.");
    //  }
    //  else
    //  {
    //    db.DomainsUpdate(LabelDomainServerName.Text, "DomainDesignCode", ListBoxDomainDesignCode.SelectedValue);
    //    LoadCodes4Domain2Change();
    //    Msg.Text = db.Ok("The Domain Design Code has been updated. Select another Domain or Data Code or Organization Code to make additional changes.");
    //  }
    //}
    //protected void ListBoxDomainDataCode_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  if (LabelDomainServerName.Text == "No Domain has been selected")
    //  {
    //    Msg.Text = db.Fail("A Domain needs to be selected.");
    //  }
    //  else
    //  {
    //    db.DomainsUpdate(LabelDomainServerName.Text, "DomainDataCode", ListBoxDomainDataCode.SelectedValue);
    //    LoadCodes4Domain2Change();
    //    Msg.Text = db.Ok("The Domain Data Code has been updated. Select another Domain or Design Code or Organization Code to make additional changes.");
    //  }
    //}
    //protected void ListBoxDomainOrganizationCode_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  if (LabelDomainServerName.Text == "No Domain has been selected")
    //  {
    //    Msg.Text = db.Fail("A Domain needs to be selected.");
    //  }
    //  else
    //  {
    //    db.DomainsUpdate(LabelDomainServerName.Text, "DomainOrganizationCode", ListBoxDomainOrganizationCode.SelectedValue);
    //    LoadCodes4Domain2Change();
    //    Msg.Text = db.Ok("The Domain Oranization Code has been updated. Select another Domain or Data Code or Design Code to make additional changes.");
    //  }
    //}

    //protected void ListBoxDomainServerName_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  LabelDomainServerName.Text = ListBoxDomainServerName.SelectedValue;
    //  TextBoxDomain2Add.Text = string.Empty;

    //  LoadCodes4Domain2Change();

    //  Msg.Text = db.Msg("Select a Design Code, Data Code or Organization Code to change either of these codes for this domain."
    //    + " Or select a different domain to edit that domain.");
    //}

    //protected void Page_Load(object sender, EventArgs e)
    //{
    //  Response.Write("The Domains table has changed. This page might need to be modified" +
    //    " before it is used.");
    //  Response.End();
    //  if (!IsPostBack)
    //  {
    //    if (!SecurePage.IsMasterUser)
    //      SecurePage.HandleSecurityException();

    //    try
    //    {
    //      LoadDomains();
    //      LoadDomainDesignCodes();
    //      LoadDomainDataCodes();
    //      LoadDomainOrganizationCodes();

    //      EmptyCodes4Domain2Change();

    //      Msg.Text = db.Msg("Select a Domain to Edit.");
    //    }

    //    catch (Exception ex)
    //    {
    //      #region
    //      Msg.Text = db.Fail(ex.Message);
    //      db.Log_Error_Admin(ex);
    //      #endregion
    //    }
    //  }
    //}
  }
}
