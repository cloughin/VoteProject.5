using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB;
using DB.Vote;
using static System.String;

namespace Vote.Reports
{
  public sealed class PoliticianInfoResponsive : ResponsiveReport
  {
    private Control GenerateReport(DataRow politician)
    {
      var imageContainer = new HtmlDiv().AddTo(ReportContainer, "candidate-image");
      CreatePoliticianImageTag(politician.PoliticianKey(), ImageSize300, false, Empty)
        .AddTo(imageContainer);

      var infoContainer = new HtmlDiv().AddTo(ReportContainer, "candidate-info");

      new HtmlH1 {InnerText = Politicians.FormatOfficeAndStatus(politician)}
        .AddTo(infoContainer, "candidate-status");

      if (politician.LiveOfficeStatus() == "InFutureViewableElection")
      {
        var h2 = new HtmlH2();
        h2.AddTo(infoContainer, "candidate-election");
        // Link removed per Mantis 840
        //new HtmlAnchor
        //{
        //  HRef = UrlManager.GetElectionPageUri(politician.LiveElectionKey()).ToString(),
        //  InnerText = politician.ElectionDescription()
        //}.AddTo(h2);
        h2.InnerText = politician.ElectionDescription();
        CreateCompareTheCandidatesAnchor(politician.LiveElectionKey(), politician.LiveOfficeKey())
          .AddTo(infoContainer);
      }

      Control party;
      if (politician.PartyKey() != null)
      {
        if (IsNullOrWhiteSpace(politician.PartyUrl()))
          party = new LiteralControl(politician.PartyName());
        else
        {
          party = new HtmlAnchor
          {
            HRef = VotePage.NormalizeUrl(politician.PartyUrl()),
            Title = politician.PartyName() + " Website",
            Target = "_blank",
            InnerHtml = politician.PartyName()
          };
          ((HtmlAnchor) party).Attributes["rel"] = "nofollow";
        }
      }
      else
        party = new LiteralControl("no party affiliation");
      if (party is LiteralControl)
      {
        var span = new HtmlSpan();
        party.AddTo(span);
        party = span;
      }
      party.AddTo(infoContainer, "candidate-party");

      FormatWebAddress(infoContainer, politician);
      FormatSocialMedia(infoContainer, politician);
      FormatPostalAddress(infoContainer, politician);
      FormatPhone(infoContainer, politician);
      FormatAge(infoContainer, politician);

      return ReportContainer.AddCssClasses("intro-report clearfix");
    }

    public static Control GetReport(DataRow politicianInfo)
    {
      var reportObject = new PoliticianInfoResponsive();
      return reportObject.GenerateReport(politicianInfo);
    }
  }
}