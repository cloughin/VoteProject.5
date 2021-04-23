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
using DB.Vote;

namespace Vote
{
  public partial class FooterFirst : System.Web.UI.UserControl
  {
    //protected void Page_Load(object sender, EventArgs e)
    protected void Page_PreRender(object sender, EventArgs e)
    {
      //Moved from Page_Load because if design is changes Page_Load is performed before
      //the database is updated with the new design. Page_PreRender is performed after
      //the database is updated.
      try
      {
        //if (!IsPostBack)
        //{
        //if (All.Visible)

        if (db.Is_Include_First_Footer_This())
        {
          #region Visible
          All.Visible = true;

          #region HTML Table Attributes
          HtmlTable FooterFirstHTMLTable = new System.Web.UI.HtmlControls.HtmlTable();
          FooterFirstHTMLTable.Attributes["cellSpacing"] = "0";
          FooterFirstHTMLTable.Attributes["padding"] = "0";
          FooterFirstHTMLTable.Attributes["border"] = "0";
          FooterFirstHTMLTable.Attributes["id"] = "FooterFirst";
          if (VotePage.IsPublicPage)
            FooterFirstHTMLTable.Attributes["class"] = "tablePage";
          else
            FooterFirstHTMLTable.Attributes["class"] = "tableAdmin";
          #endregion HTML Table Attributes

          //<tr>
          //HtmlTableRow FooterFirstHTMLTr = db.AddNavbarRow2Table(FooterFirstHTMLTable);
          HtmlTableRow FooterFirstHTMLTr =
            db.Add_Tr_To_Table_Return_Tr(
              FooterFirstHTMLTable
              , "trFooterFirst"
              );

          string Content = string.Empty;
          if (SecurePage.IsMasterPage)
            Content =
              MasterDesign.GetDesignStringWithSubstitutions(
                MasterDesign.Column.FirstFooterAllPages,
                MasterDesign.Column.IsTextFirstFooterAllPages);
          else
            Content =
              DomainDesigns.GetDesignStringWithSubstitutions(
                DomainDesigns.Column.FirstFooterAllPages,
                DomainDesigns.Column.IsTextFirstFooterAllPages);

          db.Add_Td_To_Tr(
            FooterFirstHTMLTr
            , Content
            , "tdFooterFirst");

          All.Text = db.RenderToString(FooterFirstHTMLTable);
          #endregion Visible
        }
        else        //if (db.Is_Page_Element_Visible(
        //  db.DomainDesigns_Bool_This(
        //      "IsIncludedFirstFooterAllPages")
        //      , "IsTextFirstFooterAllPages"
        //      , "FirstFooterAllPages"
        //      ))
        {
          All.Visible = false;
        }
      }
      //}
      catch (Exception ex)
      {
        db.Log_Page_Not_Found_404("FooterFirst.aspx:ex.Message" + ex.Message);
        if (!VotePage.IsDebugging) VotePage.SafeTransferToError500();
      }
    }
  }
}