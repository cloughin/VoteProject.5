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
  public partial class EmailPowered : System.Web.UI.UserControl
  {
    protected void Page_PreRender(object sender, EventArgs e)
    {
      //Moved from Page_Load because if design is changes Page_Load is performed before
      //the database is updated with the new design. Page_PreRender is performed after
      //the database is updated.
      try
      {
          All.Visible = true; //MASTER and DESIGN Pages footer always shows.
          if (VotePage.IsPublicPage)
          {
            if (
              (!db.Is_Include_Email_Us_This())
              && (!db.Is_Include_Powered_By_This())
              )
              All.Visible = false;
            //else if (!db.Is_StateCode_Identified())
            //{
            //  //There is no StateCode identified so if State substitutions exist don't show
            //  if (db.Is_Substitutions_Has_StateCode(
            //    db.Design_Substitutions_This("EmailUsLineAllPages"))
            //    )
            //    All.Visible = false;
            //}
            All.Attributes["Class"] = "tablePage";
          }
          else
          {
            All.Attributes["Class"] = "tableAdmin";
          }

          if (All.Visible)
          {
            #region Email Us
            if (VotePage.IsPublicPage)
            {
              var emailAddress =
                DomainDesigns.GetDesignStringWithSubstitutions(
                  DomainDesigns.Column.EmailUsAddressAllPages);

              if (!db.Is_Include_Email_Us_This() || 
                string.IsNullOrWhiteSpace(emailAddress))
                EmailUs.Visible = false;
              else
              {
                EmailUs.Visible = true;

                LabelEmailUs.Text =
                  DomainDesigns.GetDesignStringWithSubstitutions(
                    DomainDesigns.Column.EmailUsLineAllPages);

                if (string.IsNullOrWhiteSpace(LabelEmailUs.Text))
                  LabelEmailUs.Text = "Email Us:";

                HyperLinkEMailUs.Text = db.Anchor_Mailto_Email(emailAddress);
              }
            }
            else
            {
              EmailUs.Visible = true;

              if (SecurePage.IsMasterPage)
              {
                LabelEmailUs.Text = 
                  MasterDesign.GetDesignStringWithSubstitutions(
                    MasterDesign.Column.EmailUsLineAllPages);
                HyperLinkEMailUs.Text = 
                  db.Anchor_Mailto_Email(
                    MasterDesign.GetDesignStringWithSubstitutions(
                      MasterDesign.Column.EmailUsAddressAllPages));
              }
              else if (SecurePage.IsDesignPage)
              {
                LabelEmailUs.Text =
                  DomainDesigns.GetDesignStringWithSubstitutions(
                    DomainDesigns.Column.EmailUsLineAllPages);
                HyperLinkEMailUs.Text =
                  db.Anchor_Mailto_Email(
                    DomainDesigns.GetDesignStringWithSubstitutions(
                      DomainDesigns.Column.EmailUsAddressAllPages));
              }
            }
            #endregion Email Us

            #region Powered By
            if (VotePage.IsPublicPage && !db.Is_Include_Powered_By_This())//Always show MASTER and DESIGN pages
              PoweredBy.Visible = false;
            else
            {
              PoweredBy.Visible = true;
              HyperLinkVoteUSAImageButton.Visible = true;
              HyperLinkVoteUSAImageButton.ImageUrl = "/images/poweredby.gif";
              //if (db.Is_Localhost)
              //  HyperLinkVoteUSAImageButton.NavigateUrl = "http://localhost" + db.ServerPort;
              //else
              //  HyperLinkVoteUSAImageButton.NavigateUrl = "http://Vote-USA.org";
              HyperLinkVoteUSAImageButton.NavigateUrl = 
                UrlManager.SiteUri.ToString();
            }
            #endregion Powered By
          }
      }
      catch (Exception ex)
      {
        db.Log_Page_Not_Found_404("EmailPowered.aspx:ex.Message" + ex.Message);
        if (!VotePage.IsDebugging) VotePage.SafeTransferToError500();
      }

    }
  }
}