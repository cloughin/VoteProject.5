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
  public partial class FooterSecond : System.Web.UI.UserControl
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
        #region commented out Is Footer Second Visible ?
        //All.Visible = false;
        //if (db.Is_Page_Public())
        //{
        //  if (db.DomainDesigns_Bool_This("IsIncludedSecondFooterAllPages"))
        //    All.Visible = true;
        //}
        //else
        //  All.Visible = true;//MASTER and DESIGN Pages
        #endregion Is Footer Second Visible ?

        //if (All.Visible)
        //if (db.Is_Page_Element_Visible(
        //  db.DomainDesigns_Bool_This("IsIncludedSecondFooterAllPages")
        //  , "IsTextSecondFooterAllPages"
        //  , "SecondFooterAllPages")
        //  )
          if(db.Is_Include_Second_Footer_This())
        {
          #region Visible
          All.Visible = true;

          #region HTML Table Attributes
          HtmlTable FooterSecondHTMLTable = new System.Web.UI.HtmlControls.HtmlTable();
          FooterSecondHTMLTable.Attributes["cellSpacing"] = "0";
          FooterSecondHTMLTable.Attributes["padding"] = "0";
          FooterSecondHTMLTable.Attributes["border"] = "0";
          FooterSecondHTMLTable.Attributes["id"] = "FooterSecond";
          if (VotePage.IsPublicPage)
            FooterSecondHTMLTable.Attributes["class"] = "tablePage";
          else
            FooterSecondHTMLTable.Attributes["class"] = "tableAdmin";
          #endregion HTML Table Attributes

          //<tr>
          //HtmlTableRow FooterSecondHTMLTr = db.AddNavbarRow2Table(FooterSecondHTMLTable);
          HtmlTableRow FooterSecondHTMLTr = db.Add_Tr_To_Table_Return_Tr(FooterSecondHTMLTable, "trFooterFirst");

          string Content = string.Empty;
          if (SecurePage.IsMasterPage)
            Content = MasterDesign.GetDesignStringWithSubstitutions(
              MasterDesign.Column.SecondFooterAllPages,
              MasterDesign.Column.IsTextSecondFooterAllPages);
          else
            Content = DomainDesigns.GetDesignStringWithSubstitutions(
              DomainDesigns.Column.SecondFooterAllPages,
              DomainDesigns.Column.IsTextSecondFooterAllPages);

          db.Add_Td_To_Tr(
            FooterSecondHTMLTr
            , Content
            , "tdFooterSecond");

          All.Text = db.RenderToString(FooterSecondHTMLTable);
          #endregion Visible
        }
        else
        {
          All.Visible = false;
        }
      }
      //}
      catch (Exception ex)
      {
        db.Log_Page_Not_Found_404("FooterSecond.aspx:ex.Message" + ex.Message);
        if (!VotePage.IsDebugging) VotePage.SafeTransferToError500();
      }
    }
  }
}