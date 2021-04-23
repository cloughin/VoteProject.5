using System;
using System.Web.UI.HtmlControls;
using DB.VoteCache;

namespace Vote.Master
{
  public partial class DefaultPage : SecurePage
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsSuperUser)
      {
        Column1.Visible = false;
        Column2.AddCssClasses("wide");
      }

      if (!IsPostBack)
      {
        const string title = "Master Administration";
        Page.Title = title;
        H1.InnerHtml = title;

        NoJurisdiction.CreateStateLinks("/admin/?state={StateCode}");
        NoJurisdiction.SetHead("Links to State Administration Pages");

        if (IsSuperUser)
        {
          var sampleBallotDialogEnabled = DB.Vote.Master.GetPresentGetFutureSampleBallotsDialog(false);
          var nagsEnabled = DonationNagsControl.GetIsNaggingEnabled(false);
          if (!sampleBallotDialogEnabled || !nagsEnabled)
          {
            if (!sampleBallotDialogEnabled)
              new HtmlP { InnerText = "Ballot Choices Dialogs are disabled" }.AddTo(
                AlertPlaceHolder, "alert");
            if (!nagsEnabled)
              new HtmlP { InnerText = "Donation Nags are disabled" }.AddTo(
                AlertPlaceHolder, "alert");
            new HtmlAnchor {InnerText = "Change settings", HRef = "/master/nags.aspx"}
              .AddTo(AlertPlaceHolder, "nags-link");
          }
        }
      }
    }
  }
}